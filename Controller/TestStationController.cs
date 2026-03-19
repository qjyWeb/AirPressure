using AirPressure.Models;
using AirPressure.Utils;

namespace AirPressure.Logic
{
    /// <summary>
    /// 测试站控制器 - 系统核心业务逻辑
    /// 职责：
    ///   1. 硬件协调（串口、Modbus、HTTP 通信）
    ///   2. 测试流程管理（初始化、启动、监控、结束）
    ///   3. 数据采集和处理（解析、存储、上传）
    /// 
    /// 架构说明：
    ///   - 采用事件驱动，向 UI 层发送日志、测试结果、状态变化等
    ///   - 串口分离：CmdPort（指令） + DataPort（数据）
    ///   - HTTP 双向：监听 MES 指令 + 主动上报测试结果
    /// </summary>
    public class TestStationController : IDisposable
    {
        #region 常量定义

        /// <summary>
        /// Modbus 设备地址
        /// </summary>
        private const byte MODBUS_SLAVE_ADDRESS = 10;

        /// <summary>
        /// DI 异常位置（线圈地址），当为 true 时表示测试异常
        /// </summary>
        private const ushort DI_ERROR_COIL = 2;

        /// <summary>
        /// Modbus 心跳监控间隔（毫秒）
        /// </summary>
        private const int HEARTBEAT_CHECK_INTERVAL = 5000;

        /// <summary>
        /// 心跳超时阈值（秒）
        /// </summary>
        private const int HEARTBEAT_TIMEOUT_SECONDS = 20;

        /// <summary>
        /// 通道选择写入点（线圈地址）
        /// </summary>
        private const ushort CHANNEL_SELECT_COIL = 8;

        /// <summary>
        /// 测试启动线圈地址
        /// </summary>
        private const ushort TEST_START_COIL = 0;

        /// <summary>
        /// 测试停止线圈地址
        /// </summary>
        private const ushort TEST_STOP_COIL = 1;

        #endregion

        #region 字段和属性

        /// <summary>
        /// 双串口管理器：处理指令和数据的收发
        /// </summary>
        private DualSerialPortManager? _serialManager;

        /// <summary>
        /// SQLite 数据库助手：管理本地数据持久化
        /// </summary>
        private SqliteHelper? _sqlite;

        /// <summary>
        /// HTTP JSON 工具：处理与 MES 的 HTTP 通信
        /// </summary>
        private HttpJsonTool? _httpTool;

        /// <summary>
        /// MES 心跳监控取消标记
        /// </summary>
        private CancellationTokenSource _heartbeatCts;

        /// <summary>
        /// 上一次收到 MES 心跳的时刻
        /// </summary>
        private DateTime _lastHeartbeatTime = DateTime.MinValue;

        /// <summary>
        /// 当前是否正在进行测试
        /// </summary>
        private bool _isTesting;

        /// <summary>
        /// 超时保护的取消标记
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// 测试状态标记（get/set 时触发 OnTime 事件）
        /// </summary>
        private bool IsTesting
        {
            get { return _isTesting; }
            set
            {
                if (_isTesting != value)
                {
                    _isTesting = value;
                    OnTime?.Invoke(_isTesting);
                }
            }
        }

        #endregion

        #region 事件定义

        /// <summary>
        /// 日志事件：将日志信息发送到 UI 层显示
        /// 参数：(消息内容, 显示颜色)
        /// </summary>
        public event Action<string, Color>? OnLog;

        /// <summary>
        /// 测试完成事件：当测试结束时触发
        /// 参数：测试结果字符串（"PASS" 或 "FAIL"）
        /// </summary>
        public event Action<string>? OnTestFinished;

        /// <summary>
        /// 计时事件：标记测试时间计算的开始/停止
        /// 参数：true=开始计时，false=停止计时
        /// </summary>
        public event Action<bool>? OnTime;

        /// <summary>
        /// MES 心跳状态事件
        /// 参数：0=无心跳，1=正常心跳，2=异常（超时）
        /// </summary>
        public event Action<int>? OnMes;

        #endregion

        #region 初始化

        public TestStationController()
        {
            _serialManager = new DualSerialPortManager();
            _httpTool = new HttpJsonTool();
            _sqlite = new SqliteHelper();
            _heartbeatCts = new CancellationTokenSource();
            BindEvents();
        }

        /// <summary>
        /// 绑定各模块的事件处理
        /// </summary>
        private void BindEvents()
        {
            if (_serialManager != null)
            {
                _serialManager.StatusMessage += (s, msg) => Log($"[串口] {msg}", Color.LimeGreen);
                _serialManager.ErrorMessage += (s, msg) => Log($"[串口错误] {msg}", Color.Red);
                _serialManager.ErrorOccurred += (s, ex) => Log($"[异常] {ex.Message}", Color.Red);
                _serialManager.RawDataReceived += OnSerialDataReceived;
            }

            if (_httpTool != null)
            {
                _httpTool.HttpStatusMessage += (s, msg) => Log(msg, Color.LimeGreen);
                _httpTool.HttpErrorMessage += (s, msg) => Log(msg, Color.Red);
            }
        }

        #endregion

        #region 系统初始化和启动

        /// <summary>
        /// 初始化系统：加载配置、启动双串口、启动 HTTP 监听、启动心跳监控
        /// 调用时机：应用启动时在主窗体 Load 事件中调用
        /// </summary>
        public void InitializeSystem()
        {
            try
            {
                // 第一步：从配置文件加载全局参数
                GlobalVar.LoadData();

                //启动心跳
                // 第二步：启动 MES 心跳监控
                StartHeartbeatMonitoring();

                // 获取参数
                // 第三步：启动双串口系统
                InitializeSerialPorts();

                // 第四步：启动 HTTP 监听服务
                InitializeHttpListener();
            }
            catch (Exception ex)
            {
                Log($"系统初始化异常: {ex.Message}", Color.Red);
                // 上报异常到 MES（不阻塞初始化流程的异常处理）
                //_ = ReportErrorToServerAsync();
                throw;
            }
        }

        /// <summary>
        /// 初始化和启动双串口
        /// </summary>
        private void InitializeSerialPorts()
        {
            int cmdBaud = int.TryParse(GlobalVar.CmdBaudRate, out int cb) ? cb : 9600;
            int dataBaud = int.TryParse(GlobalVar.DataBaudRate, out int db) ? db : 9600;

            string cmdPort = GlobalVar.CmdPortName;
            string dataPort = GlobalVar.DataPortName;

            if (!string.IsNullOrEmpty(cmdPort) && !string.IsNullOrEmpty(dataPort))
            {
                bool success = _serialManager.OpenSystem(cmdPort, cmdBaud, dataPort, dataBaud);
                if (!success)
                    throw new Exception("双串口启动失败");
            }
            else
            {
                throw new Exception("配置错误：请在 config.ini 配置 CmdPortName 和 DataPortName");
            }
        }

        /// <summary>
        /// 初始化和启动 HTTP 监听服务
        /// </summary>
        private void InitializeHttpListener()
        {
            if (!GlobalVar.IsStartMes)
            {
                Log("MES 未启用，HTTP 监听服务不启动", Color.Orange);
                return;
            }

            _ = _httpTool.StartListenerAsync<MonitorDataModel>(
                GlobalVar.listenIp,
                GlobalVar.listenPort,
                ProcessHttpPacket
            );
        }

        #endregion

        #region 心跳监控

        /// <summary>
        /// 启动 MES 心跳监控
        /// 定期检查是否收到 MES 的 GET.APP 请求，超时则标记异常
        /// </summary>
        public void StartHeartbeatMonitoring()
        {
            _lastHeartbeatTime = DateTime.Now;
            _heartbeatCts?.Cancel();
            _heartbeatCts = new CancellationTokenSource();
            Task.Run(async () => await MonitorHeartbeatAsync(_heartbeatCts.Token));
        }

        /// <summary>
        /// 心跳监控工作循环
        /// 每 5 秒检查一次，如果 20 秒未收到心跳则触发异常事件
        /// </summary>
        private async Task MonitorHeartbeatAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(HEARTBEAT_CHECK_INTERVAL, cancellationToken);

                if ((DateTime.Now - _lastHeartbeatTime).TotalSeconds > HEARTBEAT_TIMEOUT_SECONDS)
                {
                    OnMes?.Invoke(2);  // 2 = 异常状态
                }
            }
        }

        #endregion

        #region HTTP 业务处理

        /// <summary>
        /// 处理来自 MES 的 HTTP 请求
        /// 支持的命令：SET.INIT, GET.APP, SET.RFID, SET.SN, SET.WORK, SET.MODELNAME
        /// </summary>
        private ResponseDataModel ProcessHttpPacket(MonitorDataModel msg)
        {
            // 忽略心跳请求的日志输出，减少日志噪音
            if (msg.cmd != "GET.APP")
                Log($"[HTTP收到] {msg.cmd} {msg.key} {msg.value}", Color.Cyan);

            var response = new ResponseDataModel
            {
                station = GlobalVar.StationName,
                cmd = "OK",
                success = "OK",
                message = "OK"
            };

            switch (msg.cmd?.ToUpper())
            {
                case "SET.INIT":
                    // 初始化命令：清空测试结果
                    GlobalVar.Res = "";
                    break;

                case "GET.APP":
                    // 心跳请求：返回在线状态，更新心跳时间
                    response.success = "OK";
                    response.message = "ON";
                    OnMes?.Invoke(1);  // 1 = 正常状态
                    _lastHeartbeatTime = DateTime.Now;
                    return response;

                case "SET.RFID":
                    // 设置治具码
                    GlobalVar.Vehicle = msg.value;
                    break;

                case "SET.SN":
                    // 设置产品条码
                    GlobalVar.Serial = msg.value;
                    break;

                case "SET.WORK":
                    // 设置工单号
                    GlobalVar.WorkNum = msg.value;
                    break;

                case "SET.MODELNAME":
                    // 设置机种名称并启动测试流程
                    GlobalVar.ModelName = $"电控F线-OP4460-CT407-001-20260309";
                    Task.Run(() => StartTestProcess());
                    break;
            }

            return response;
        }

        #endregion

        #region 测试流程核心

        /// <summary>
        /// 启动测试流程：选通道 -> 发启动命令 -> 等待数据返回
        /// 调用者需确保 GlobalVar.Serial 和 GlobalVar.Vehicle 已填充
        /// </summary>
        public async Task StartTestProcess()
        {
            // 防止测试重复启动
            if (IsTesting)
            {
                Log("测试进行中，忽略重复请求", Color.Coral);
                return;
            }

            // 验证必要的参数
            if (string.IsNullOrEmpty(GlobalVar.Serial) || string.IsNullOrEmpty(GlobalVar.Vehicle))
            {
                Log("治具码或产品条码为空，不允许测试!", Color.Red);
                _ = ReportErrorToServerAsync();
                return;
            }

            // 确保 Modbus 已准备好
            if (_serialManager == null || _serialManager._master == null)
            {
                Log("无法启动测试：Modbus 主站未初始化，请检查串口连接", Color.Red);
                return;
            }

            IsTesting = true;

            try
            {
                if (GlobalVar.IsStartMes)
                {
                    string model = GlobalVar.ModelName?.Trim() ?? string.Empty;

                    // 根据配置映射设置通道（GlobalVar.ModelChannelMap 从 config.ini 加载）
                    if (!string.IsNullOrEmpty(model) && GlobalVar.ModelChannelMap.TryGetValue(model, out var mappedCh))
                    {
                        GlobalVar.ParamCH = mappedCh;
                        Log($"机种 {model} 映射到通道 {mappedCh}", Color.Cyan);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(GlobalVar.ParamCH))
                        {
                            GlobalVar.ParamCH = "0";
                            Log($"机种 {model} 未配置映射，使用默认通道 0", Color.Orange);
                        }
                    }

                    // 根据配置决定是否发送启动指令（GlobalVar.ModelActionMap 从 config.ini 加载）
                    bool skipCommands = false;
                    if (!string.IsNullOrEmpty(model) && GlobalVar.ModelActionMap.TryGetValue(model, out var action))
                    {
                        if (string.Equals(action, "None", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(action, "Skip", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(action, "空跑", StringComparison.OrdinalIgnoreCase))
                        {
                            skipCommands = true;
                        }
                    }

                    if (skipCommands)
                    {
                        // 空跑逻辑：不发送 Modbus 指令，但仍按流程记录开始时间并等待超时
                        Log($"机种 {model} 配置为不发送指令（空跑），进入空跑流程", Color.Cyan);
                        // 等待一小段时间模拟设备响应延迟（可调整或配置）
                        await Task.Delay(10000);
                        GlobalVar.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        GlobalVar.IsStart = true;

                        int.TryParse(GlobalVar.MaxRunTime, out int timeoutNone);
                        await Task.Delay(TimeSpan.FromSeconds(timeoutNone));

                        if (GlobalVar.IsStart)
                        {
                            Log("空跑等待测试数据超时", Color.Red);
                            _ = ReportErrorToServerAsync();
                            GlobalVar.Res = "Fail";
                            GlobalVar.IsStart = false;
                            IsTesting = false;
                        }

                        return;
                    }
                }

                // 步骤 1：选择测试通道
                await SelectTestChannel();

                await Task.Delay(500);

                // 步骤 2：发送测试启动命令
                await SendTestStartCommand();

                // 步骤 3：记录开始时间，标记等待数据
                GlobalVar.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                GlobalVar.IsStart = true;

                // 步骤 4：启动 DI 值监控（异常检测）
                Task.Run(() => Read16DIVal());

                Log("指令发送成功，等待测试数据...", Color.DarkBlue);

                // 步骤 5：设置超时保护
                _cts = new CancellationTokenSource();
                int.TryParse(GlobalVar.MaxRunTime, out int timeout);
                await Task.Delay(TimeSpan.FromSeconds(timeout), _cts.Token);

                if (GlobalVar.IsStart)  // 超时：未收到测试结果
                {
                    Log("等待测试数据超时", Color.Red);
                    _ = ReportErrorToServerAsync();
                    GlobalVar.Res = "Fail";
                    GlobalVar.IsStart = false;
                    IsTesting = false;
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                Log($"测试流程异常: {ex.Message}", Color.Red);
                // 上报错误到 MES（fire-and-forget）
                _ = ReportErrorToServerAsync();
                IsTesting = false;
                GlobalVar.Res = "Fail";
                GlobalVar.IsStart = false;
            }
        }

        /// <summary>
        /// 选择测试通道（通过 Modbus 线圈）
        /// GlobalVar.ParamCH 为通道号（0-31），转换为 5 位二进制写入
        /// </summary>
        private async Task SelectTestChannel()
        {
            // 确保串口管理器和 Modbus 主站已初始化
            var master = _serialManager?._master;
            if (master == null)
            {
                Log("无法选择通道：Modbus 主站未初始化", Color.Red);
                // 终止测试流程的标记
                IsTesting = false;
                GlobalVar.IsStart = false;
                GlobalVar.Res = "Fail";
                return;
            }

            if (!int.TryParse(GlobalVar.ParamCH, out int ch))
                ch = 0;

            // 转换为 5 位二进制
            string binary = Convert.ToString(ch, 2).PadLeft(5, '0');
            bool[] coilValues = new bool[5];
            for (int i = 0; i < 5; i++)
                coilValues[i] = binary[i] == '1';

            await master.WriteMultipleCoilsAsync(MODBUS_SLAVE_ADDRESS, CHANNEL_SELECT_COIL, coilValues);
        }

        /// <summary>
        /// 发送测试启动命令
        /// 先设置为 true，然后立即设置为 false（脉冲）
        /// </summary>
        private async Task SendTestStartCommand()
        {
            var master = _serialManager?._master;
            if (master == null)
            {
                Log("无法发送启动命令：Modbus 主站未初始化", Color.Red);
                IsTesting = false;
                GlobalVar.IsStart = false;
                GlobalVar.Res = "Fail";
                return;
            }

            await master.WriteSingleCoilAsync(MODBUS_SLAVE_ADDRESS, TEST_START_COIL, true);
            await Task.Delay(500);
            await master.WriteSingleCoilAsync(MODBUS_SLAVE_ADDRESS, TEST_START_COIL, false);
        }

        /// <summary>
        /// 读取 16 个 DI 值，监控异常状态
        /// DI[2] 为异常信号：true = 异常，停止测试
        /// </summary>
        private async Task Read16DIVal()
        {
            while (IsTesting)
            {
                try
                {
                    var master = _serialManager?._master;
                    if (master == null)
                    {
                        Log("无法读取 DI：Modbus 主站未初始化，停止监控", Color.Red);
                        IsTesting = false;
                        GlobalVar.IsStart = false;
                        GlobalVar.Res = "Fail";
                        // 停止计时
                        OnTime?.Invoke(false);
                        break;
                    }

                    bool[] diValues = await master.ReadInputsAsync(MODBUS_SLAVE_ADDRESS, 0, 16);

                    if (diValues.Length > DI_ERROR_COIL && diValues[DI_ERROR_COIL])  // DI[2] 异常
                    {
                        Log("测试异常,停止测试!", Color.Red);
                        _ = ReportErrorToServerAsync();
                        GlobalVar.IsError = true;
                        GlobalVar.Res = "Fail";
                        GlobalVar.IsStart = false;
                        _cts.Cancel();  // 取消超时等待
                        IsTesting = false;
                        // 停止计时
                        OnTime?.Invoke(false);
                        break;
                    }

                    await Task.Delay(500);  // 每 500ms 检查一次
                }
                catch (Exception ex)
                {
                    Log($"DI 读取异常: {ex.Message}", Color.Red);
                    // 出现 DI 读取异常时停止测试并停止计时
                    IsTesting = false;
                    GlobalVar.IsStart = false;
                    GlobalVar.Res = "Fail";
                    GlobalVar.IsStart = false;
                    _cts.Cancel();  // 取消超时等待
                    OnTime?.Invoke(false);
                    break;
                }
            }
        }

        #endregion

        #region 数据处理和上报

        /// <summary>
        /// 串口数据接收回调：解析测试结果、保存数据库、上报 MES
        /// </summary>
        private async void OnSerialDataReceived(object sender, string rawData)
        {
            if (!GlobalVar.IsStart) return;

            if (GlobalVar.ModelName == "KongPao")
            {
                TestResultModel test = new TestResultModel();
                test.DetailedData["气密测试测试压力"] = "0";
                test.DetailedData["气密测试测试泄漏值"] = "-1";
                test.DetailedData["气密测试测试压力单位"] = "KPa";
                test.DetailedData["气密测试测试泄漏值单位"] = "ml/min";
                test.DetailedData["气密测试测试压力Max"] = "0";
                test.DetailedData["气密测试测试压力Min"] = "0";

                // 更新全局状态
                GlobalVar.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                GlobalVar.Res = test.Result;
                GlobalVar.IsStart = false;

                // 停止计时
                OnTime?.Invoke(false);

                // 上传数据
                await ExecuteUpload(test);

                // 通知 UI
                OnTestFinished?.Invoke(test.Result);

                return;
            }

            Log($"收到串口原始数据: {rawData}", Color.Black);

            try
            {
                // 解析数据格式（空格分隔）
                string[] parts = rawData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // 需要至少前三项用于判断结果
                if (parts.Length < 3)
                {
                    Log("串口返回数据项不足，忽略该条数据", Color.Orange);
                    return;
                }

                static string GetPart(string[] arr, int idx)
                {
                    if (arr == null) return string.Empty;
                    if (idx < 0 || idx >= arr.Length) return string.Empty;
                    return arr[idx] ?? string.Empty;
                }

                // 创建测试结果模型
                var reportModel = new TestResultModel
                {
                    Result = (GetPart(parts, 2) == "2") ? "PASS" : "FAIL"
                };

                // 填充详细字典（使用安全索引）
                reportModel.DetailedData["气密测试测试压力"] = $"{GetPart(parts, 7)}";
                reportModel.DetailedData["气密测试测试泄漏值"] = $"{GetPart(parts, 3)}";
                reportModel.DetailedData["气密测试测试压力单位"] = $"KPa";
                reportModel.DetailedData["气密测试测试泄漏值单位"] = $"ml/min";
                reportModel.DetailedData["气密测试测试压力Max"] = $"{GetPart(parts, 8)}";
                reportModel.DetailedData["气密测试测试压力Min"] = $"{GetPart(parts, 9)}";

                // 更新全局状态
                GlobalVar.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                GlobalVar.Res = reportModel.Result;
                GlobalVar.IsStart = false;
                _cts.Cancel();  // 取消超时等待

                // 停止计时
                OnTime?.Invoke(false);

                //本地数据库保存
                await SaveToDatabase(parts, reportModel);

                // 上传数据
                await ExecuteUpload(reportModel);

                // 通知 UI
                OnTestFinished?.Invoke(reportModel.Result);
            }
            catch (Exception ex)
            {
                Log($"处理测试结果异常: {ex.Message}", Color.Red);
                // 异常时停止计时
                OnTime?.Invoke(false);
                // 上报异常到 MES（fire-and-forget）
                _ = ReportErrorToServerAsync();
            }
        }

        /// <summary>
        /// 保存测试结果到本地 SQLite 数据库
        /// </summary>
        private async Task SaveToDatabase(string[] parts, TestResultModel reportModel)
        {
            static string GetPart(string[] arr, int idx)
            {
                if (arr == null) return string.Empty;
                if (idx < 0 || idx >= arr.Length) return string.Empty;
                return arr[idx] ?? string.Empty;
            }

            var chPart = GetPart(parts, 10).Trim();
            string ch = chPart.Length >= 2 ? chPart.Substring(0, 2) : chPart;
            string checksum = chPart.Length >= 6 ? chPart.Substring(3, 2) : string.Empty;

            var saveData = new TestResultDbModel()
            {
                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SN = GlobalVar.Serial,
                Vehicle = GlobalVar.Vehicle, // 存入治具码
                InstrumentNumber = GetPart(parts, 0),
                TestMode = GetPart(parts, 1),
                Result = reportModel.Result,
                LeakageRate = GetPart(parts, 3),
                DETUL = GetPart(parts, 4),
                DETLL = GetPart(parts, 5),
                DeltaP = GetPart(parts, 6),
                TestPressure = GetPart(parts, 7),
                TPUL = GetPart(parts, 8),
                TPLL = GetPart(parts, 9),
                CH = ch,
                CheckSum = checksum,
            };
            await _sqlite.InsertAsync(saveData);
        }

        /// <summary>
        /// 上报测试结果到 MES
        /// </summary>
        public async Task ExecuteUpload(TestResultModel model)
        {
            try
            {
                if (!GlobalVar.IsStartMes)
                {
                    Log("MES已禁用,上报失败...", Color.Red);
                }
                Log("正在向 MES 上报数据...", Color.Coral);

                // 构建 HTTP 请求体
                var uploadData = new SendDataModel()
                {
                    station = GlobalVar.StationName,
                    cmd = "End",
                    value = model.ToMesJsonArrayString()
                };

                string targetUrl = $"http://{GlobalVar.sendIp}:{GlobalVar.sendPort}/";
                string response = await _httpTool.SendJsonAsync(targetUrl, uploadData);

                if (response != null)
                {
                    Log($"MES 上报成功: {response}", Color.Lime);
                }
                else
                {
                    Log("MES 上报失败", Color.Red);
                    // 上报失败通知 MES（将错误信息发回给监控端）
                    _ = ReportErrorToServerAsync();
                }

                _isTesting = false;
                OnTime?.Invoke(false);
            }
            catch (Exception ex)
            {
                Log($"MES 上报异常 {ex.Message}", Color.Red);
                OnTime?.Invoke(false);
            }
        }

        #endregion

        #region 手动控制

        /// <summary>
        /// 手动停止测试：发送停止脉冲
        /// </summary>
        public async Task ManualStop()
        {
            try
            {
                await _serialManager._master.WriteSingleCoilAsync(MODBUS_SLAVE_ADDRESS, TEST_STOP_COIL, true);
                await Task.Delay(1000);
                await _serialManager._master.WriteSingleCoilAsync(MODBUS_SLAVE_ADDRESS, TEST_STOP_COIL, false);
                IsTesting = false;
                GlobalVar.IsStart = false;
                Log("测试已手动停止", Color.Orange);
            }
            catch (Exception ex)
            {
                Log($"停止命令发送失败: {ex.Message}", Color.Red);
                _ = ReportErrorToServerAsync();
            }
        }

        /// <summary>
        /// 动态开启或关闭 MES 通信（HTTP 监听/停止），并将开关状态写入 config.ini
        /// 当 enable=true 时尝试启动 HTTP 监听并更新 GlobalVar.IsStartMes
        /// 当 enable=false 时停止 HTTP 监听
        /// </summary>
        public async Task SetMesEnabledAsync(bool enable)
        {
            try
            {
                GlobalVar.IsStartMes = enable;
                // 将状态写入配置文件，供下次启动时恢复
                Ini.Write("Http", "IsStartMes", enable, GlobalVar.configFilePath);

                if (enable)
                {
                    if (_httpTool == null)
                        _httpTool = new HttpJsonTool();

                    // 启动监听器（如果尚未启动）
                    _ = _httpTool.StartListenerAsync<MonitorDataModel>(GlobalVar.listenIp, GlobalVar.listenPort, ProcessHttpPacket);
                    Log("MES 已启用，正在尝试连接并启动监听...", Color.LimeGreen);
                }
                else
                {
                    // 停止监听
                    try
                    {
                        _httpTool?.Stop();
                        Log("MES 已停用，监听服务已停止", Color.Orange);
                    }
                    catch (Exception ex)
                    {
                        Log($"停止 MES 监听失败: {ex.Message}", Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"设置 MES 状态失败: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// 在发生异常时向 MES/服务器上报错误信息。
        /// 构建 ResponseDataModel，字段: station, success="ng", message=errorMessage
        /// 使用 _httpTool.SendJsonAsync 发送到配置的 sendIp:sendPort。
        /// 该方法对外呈现为异步，可 fire-and-forget 调用。
        /// </summary>
        private async Task ReportErrorToServerAsync()
        {
            if (!GlobalVar.ReportErrorsToMes)
            {
                Log("异常上报已禁用，跳过上报。", Color.Orange);
                return;
            }

            try
            {
                if (_httpTool == null)
                {
                    Log("无法上报异常：Http 工具未初始化", Color.Red);
                    return;
                }

                if (string.IsNullOrWhiteSpace(GlobalVar.sendIp) || string.IsNullOrWhiteSpace(GlobalVar.sendPort))
                {
                    Log("无法上报异常：MES 地址未配置", Color.Red);
                    return;
                }

                // 创建测试结果模型
                var reportModel = new TestResultModel
                {
                    Result = "FAIL"
                };

                // 填充详细字典（使用安全索引）
                reportModel.DetailedData["气密测试测试压力"] = $"0";
                reportModel.DetailedData["气密测试测试泄漏值"] = $"-1";
                reportModel.DetailedData["气密测试测试压力单位"] = $"KPa";
                reportModel.DetailedData["气密测试测试泄漏值单位"] = $"ml/min";
                reportModel.DetailedData["气密测试测试压力Max"] = $"0";
                reportModel.DetailedData["气密测试测试压力Min"] = $"0";

                await ExecuteUpload(reportModel);
                await SaveToDatabase(new string[0], reportModel);
                OnTestFinished?.Invoke(reportModel.Result);
            }
            catch (Exception ex)
            {
                // 避免二次异常影响主流程，记录本地日志即可
                Log($"异常上报失败: {ex.Message}", Color.Red);
            }
        }

        #endregion

        #region 日志和清理

        /// <summary>
        /// 发送日志信息到 UI 层
        /// </summary>
        private void Log(string msg, Color color)
        {
            OnLog?.Invoke(msg, color);
        }

        /// <summary>
        /// 清理资源：释放串口、HTTP、数据库连接等
        /// </summary>
        public void Dispose()
        {
            try
            {
                _heartbeatCts?.Cancel();
                _heartbeatCts?.Dispose();
                _serialManager?.Dispose();
                _httpTool?.Dispose();
            }
            catch (Exception ex)
            {
                Log($"清理资源异常: {ex.Message}", Color.Red);
            }
        }

        #endregion
    }
}