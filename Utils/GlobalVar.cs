namespace AirPressure
{
    /// <summary>
    /// 全局变量容器 - 系统内各模块共享的配置和状态数据
    /// 
    /// 设计意图：
    ///   - 集中管理系统配置（端口、IP、工站等）
    ///   - 存储实时测试状态（产品信息、测试结果、时间等）
    ///   - 支持跨窗体、跨线程的数据共享
    /// 
    /// </summary>
    public static class GlobalVar
    {
        #region 系统基本配置

        /// <summary>
        /// exe 运行路径
        /// </summary>
        public static string? ExePath { get; set; } = Environment.CurrentDirectory;

        /// <summary>
        /// 配置文件路径（config.ini）
        /// </summary>
        public static string configFilePath = Path.Combine(ExePath, "config.ini");

        /// <summary>
        /// 是否启用 MES 通信
        /// </summary>
        public static bool IsStartMes { get; set; } = true;

        /// <summary>
        /// 工站名称
        /// </summary>
        public static string? StationName { get; set; }

        #endregion

        #region 产品和工单信息

        /// <summary>
        /// 工单号
        /// </summary>
        public static string? WorkNum { get; set; }

        /// <summary>
        /// 治具号（车辆/夹具识别号）
        /// </summary>
        public static string? Vehicle { get; set; }

        /// <summary>
        /// 产品条码（序列号）
        /// </summary>
        public static string? Serial { get; set; }

        /// <summary>
        /// 机种名称
        /// </summary>
        public static string? ModelName { get; set; }

        #endregion

        #region 测试结果和状态

        /// <summary>
        /// 测试结果（"PASS" 或 "FAIL"）
        /// </summary>
        public static string? Res { get; set; }

        /// <summary>
        /// 当前系统状态
        /// </summary>
        public static string? Status { get; set; }

        /// <summary>
        /// 标志位（预留）
        /// </summary>
        public static string? Flag { get; set; }

        /// <summary>
        /// 测试是否异常
        /// </summary>
        public static bool? IsError { get; set; } = false;

        /// <summary>
        /// 指令发送成功后是否开始计时监听
        /// 用于控制是否等待测试数据返回
        /// </summary>
        public static bool IsStart = false;

        #endregion

        #region 测试时间

        /// <summary>
        /// 指令发送成功、开始计时监听的时刻
        /// 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string? StartTime { get; set; }

        /// <summary>
        /// 测试数据返回的时刻
        /// 格式：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static string? EndTime { get; set; }

        /// <summary>
        /// 运行时间（秒）
        /// 计算方式：(EndTime - StartTime) 的总秒数
        /// </summary>
        public static string RunTimeSeconds
        {
            get
            {
                if (StartTime == null || EndTime == null)
                    return "0";

                if (DateTime.TryParseExact(StartTime, "yyyy-MM-dd HH:mm:ss", null,
                        System.Globalization.DateTimeStyles.None, out DateTime start) &&
                    DateTime.TryParseExact(EndTime, "yyyy-MM-dd HH:mm:ss", null,
                        System.Globalization.DateTimeStyles.None, out DateTime end))
                {
                    if (end >= start)
                        return (end - start).TotalSeconds.ToString("F2");
                }

                return "0";
            }
        }

        /// <summary>
        /// 运行时间（分钟）
        /// 计算方式：(EndTime - StartTime) 的总分钟数
        /// </summary>
        public static string RunTimeMinutes
        {
            get
            {
                if (StartTime == null || EndTime == null)
                    return "0";

                if (DateTime.TryParseExact(StartTime, "yyyy-MM-dd HH:mm:ss", null,
                        System.Globalization.DateTimeStyles.None, out DateTime start) &&
                    DateTime.TryParseExact(EndTime, "yyyy-MM-dd HH:mm:ss", null,
                        System.Globalization.DateTimeStyles.None, out DateTime end))
                {
                    if (end >= start)
                        return (end - start).TotalMinutes.ToString("F2");
                }

                return "0";
            }
        }

        #endregion

        #region 串口通讯配置

        /// <summary>
        /// 产品最大测试时长（秒）
        /// 用作超时保护，防止测试无限等待
        /// </summary>
        public static string? MaxRunTime { get; set; }

        /// <summary>
        /// 配置参数通道号（0-31）
        /// 用于 Modbus 选通道
        /// </summary>
        public static string? ParamCH { get; set; }

        /// <summary>
        /// 数据口端口号（如 "COM3"）
        /// 用于接收测试仪的测试结果数据
        /// </summary>
        public static string DataPortName = "";

        /// <summary>
        /// 数据口波特率
        /// </summary>
        public static string DataBaudRate = "";

        /// <summary>
        /// 指令口端口号（如 "COM4"）
        /// 用于 Modbus 主站发送控制指令
        /// </summary>
        public static string CmdPortName = "";

        /// <summary>
        /// 指令口波特率
        /// </summary>
        public static string CmdBaudRate = "";

        #endregion

        #region HTTP 通讯配置

        /// <summary>
        /// 本地监听 IP 地址（如 "192.168.1.100"）
        /// 用于监听来自 MES 的 HTTP 请求
        /// </summary>
        public static string listenIp = "";

        /// <summary>
        /// 本地监听端口号（如 "8080"）
        /// </summary>
        public static string listenPort = "";

        /// <summary>
        /// MES 服务器 IP 地址
        /// 用于主动上报测试结果
        /// </summary>
        public static string sendIp = "";

        /// <summary>
        /// MES 服务器端口号
        /// </summary>
        public static string sendPort = "";

        #endregion

        #region 配置加载

        /// <summary>
        /// 从 config.ini 配置文件加载所有参数
        /// 应在系统初始化时调用
        /// </summary>
        public static void LoadData()
        {
            // 加载串口配置
            DataPortName = Ini.ReadToini("Port", "DataPortName", configFilePath);
            DataBaudRate = Ini.ReadToini("Port", "DataBaudRate", configFilePath);
            CmdPortName = Ini.ReadToini("Port", "CmdPortName", configFilePath);
            CmdBaudRate = Ini.ReadToini("Port", "CmdBaudRate", configFilePath);

            // 加载 HTTP 配置
            listenIp = Ini.ReadToini("Http", "listenIp", configFilePath);
            listenPort = Ini.ReadToini("Http", "listenPort", configFilePath);
            sendIp = Ini.ReadToini("Http", "sendIp", configFilePath);
            sendPort = Ini.ReadToini("Http", "sendPort", configFilePath);

            // 读取 MES 开关状态（默认 true）
            {
                bool mesEnabled = Ini.ReadToini("Http", "IsStartMes", GlobalVar.configFilePath) == "1" ? true : false;
                IsStartMes = mesEnabled;
            }

            // 读取是否上报异常到 MES（默认 true）
            {
                bool reportErrors = true;
                Ini.Read("Http", "ReportErrorsToMes", ref reportErrors, configFilePath);
                ReportErrorsToMes = reportErrors;
            }

            // 加载测试配置
            StationName = Ini.ReadToini("TestConfig", "station", configFilePath);
            MaxRunTime = Ini.ReadToini("TestConfig", "testTime", configFilePath);

            // 读取机种通道映射（格式示例：KongPao:0,ModelX:3）
            try
            {
                string mappings = Ini.ReadToini("Model", "ModelChannelMap", configFilePath) ?? string.Empty;
                ModelChannelMap.Clear();
                if (!string.IsNullOrWhiteSpace(mappings))
                {
                    var entries = mappings.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var e in entries)
                    {
                        var kv = e.Split(new[] { ':' }, 2);
                        if (kv.Length == 2)
                        {
                            var name = kv[0].Trim();
                            var ch = kv[1].Trim();
                            if (!string.IsNullOrEmpty(name))
                                ModelChannelMap[name] = ch;
                        }
                    }
                }
            }
            catch { }

            // 读取机种动作映射（格式示例：KongPao:None,ModelX:Start）
            try
            {
                string actions = Ini.ReadToini("Model", "ModelActionMap", configFilePath) ?? string.Empty;
                ModelActionMap.Clear();
                if (!string.IsNullOrWhiteSpace(actions))
                {
                    var entries = actions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var e in entries)
                    {
                        var kv = e.Split(new[] { ':' }, 2);
                        if (kv.Length == 2)
                        {
                            var name = kv[0].Trim();
                            var action = kv[1].Trim();
                            if (!string.IsNullOrEmpty(name))
                                ModelActionMap[name] = action;
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        #region 异常上报配置

        /// <summary>
        /// 是否上报异常到 MES
        /// </summary>
        public static bool ReportErrorsToMes { get; set; } = true;

        #endregion

        /// <summary>
        /// 机种到通道映射（从 config.ini 读取），key=ModelName, value=channel string
        /// </summary>
        public static Dictionary<string, string> ModelChannelMap { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 机种到动作映射（从 config.ini 读取），value 可以是: Start / None (不发送指令)
        /// </summary>
        public static Dictionary<string, string> ModelActionMap { get; private set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
}
