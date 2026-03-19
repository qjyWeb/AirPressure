using Modbus.Device;
using System.IO.Ports;
using System.Text;

namespace AirPressure
{
    /// <summary>
    /// 双串口管理器 - 管理气密性测试系统的双串口通信
    /// 
    /// 设计架构：
    ///   - CmdPort（指令口）：使用 Modbus RTU 协议发送控制指令（Modbus 主站）
    ///   - DataPort（数据口）：接收测试仪的 ASCII 文本格式测试结果
    ///   - 两个端口独立工作，互不干扰
    /// 
    /// 工作流程：
    ///   1. 通过 CmdPort 的 Modbus 主站发送选通道、启动测试等控制命令
    ///   2. 同时通过 DataPort 的监听线程等待测试仪的数据反馈
    ///   3. 将接收到的原始数据转换为 ASCII 字符串并触发事件
    /// </summary>
    public class DualSerialPortManager : IDisposable
    {
        #region 常量定义

        /// <summary>
        /// 串口数据读取缓冲区大小（字节）
        /// </summary>
        private const int BUFFER_SIZE = 4096;

        /// <summary>
        /// 分批数据收集等待时间（毫秒）
        /// 用于等待可能分次到达的串口数据
        /// </summary>
        private const int DATA_COLLECTION_DELAY = 1000;

        /// <summary>
        /// 无数据时的轮询间隔（毫秒）
        /// 降低 CPU 占用
        /// </summary>
        private const int IDLE_SLEEP_MS = 50;

        #endregion

        #region 字段定义

        /// <summary>
        /// 指令口串口（用于 Modbus 通信）
        /// </summary>
        private SerialPort? _cmdPort;

        /// <summary>
        /// 数据口串口（接收 ASCII 文本数据）
        /// </summary>
        private SerialPort? _dataPort;

        /// <summary>
        /// Modbus RTU 主站（在 CmdPort 上运行）
        /// 用于发送 Modbus 指令
        /// </summary>
        public IModbusSerialMaster? _master;

        /// <summary>
        /// 系统运行标记
        /// false 时停止所有监听线程
        /// </summary>
        private bool _isRunning = false;

        /// <summary>
        /// 数据监听线程的取消令牌源
        /// </summary>
        private CancellationTokenSource? _token;

        /// <summary>
        /// 数据监听的后台工作线程
        /// </summary>
        private Task? _listenerWork;

        #endregion

        #region 事件定义

        /// <summary>
        /// 原始数据接收事件
        /// 当 DataPort 接收到完整的 ASCII 数据时触发
        /// 参数：(发送者, 转换后的 ASCII 文本)
        /// </summary>
        public event EventHandler<string>? RawDataReceived;

        /// <summary>
        /// 状态消息事件
        /// 用于报告正常的操作状态（如端口打开成功）
        /// </summary>
        public event EventHandler<string>? StatusMessage;

        /// <summary>
        /// 错误消息事件
        /// 用于报告不影响系统运行的错误信息
        /// </summary>
        public event EventHandler<string>? ErrorMessage;

        /// <summary>
        /// 异常事件
        /// 用于报告严重异常（如通信失败）
        /// </summary>
        public event EventHandler<Exception>? ErrorOccurred;

        #endregion

        #region 打开和关闭串口

        /// <summary>
        /// 启动双串口系统
        /// </summary>
        /// <param name="cmdPortName">指令口端口号（如 "COM4"）</param>
        /// <param name="cmdBaud">指令口波特率（如 9600）</param>
        /// <param name="dataPortName">数据口端口号（如 "COM3"）</param>
        /// <param name="dataBaud">数据口波特率</param>
        /// <returns>true=成功，false=失败</returns>
        public bool OpenSystem(string cmdPortName, int cmdBaud, string dataPortName, int dataBaud)
        {
            try
            {
                // 清理旧的连接
                ClosePorts();

                // 步骤 1：初始化指令口（Modbus RTU 主站）
                _cmdPort = new SerialPort(cmdPortName, cmdBaud, Parity.None, 8, StopBits.One);
                _cmdPort.ReadTimeout = 2000;
                _cmdPort.WriteTimeout = 1000;
                _cmdPort.Open();

                // 创建 Modbus RTU 主站（基于 NModbus 库）
                _master = ModbusSerialMaster.CreateRtu(_cmdPort);
                _master.Transport.Retries = 5;
                _master.Transport.WaitToRetryMilliseconds = 200;
                _master.Transport.ReadTimeout = 1000;

                // 步骤 2：初始化数据口（仅用于接收 ASCII 数据）
                _dataPort = new SerialPort(dataPortName, dataBaud, Parity.None, 8, StopBits.One);
                _dataPort.Open();

                _isRunning = true;

                // 步骤 3：启动数据监听线程
                _token = new CancellationTokenSource();
                _listenerWork = Task.Run(() => DataListenerWork(_token));

                SendStatus($"双串口启动成功: 指令[{cmdPortName}] / 数据[{dataPortName}]");
                return true;
            }
            catch (Exception ex)
            {
                SendError(ex);
                return false;
            }
        }

        /// <summary>
        /// 关闭双串口系统并清理资源
        /// </summary>
        public void ClosePorts()
        {
            _isRunning = false;

            // 关闭并释放指令口
            if (_cmdPort != null)
            {
                try
                {
                    if (_cmdPort.IsOpen)
                        _cmdPort.Close();
                }
                catch { }
                _cmdPort.Dispose();
                _cmdPort = null;
            }

            // 关闭并释放数据口
            if (_dataPort != null)
            {
                try
                {
                    if (_dataPort.IsOpen)
                        _dataPort.Close();
                }
                catch { }
                _dataPort.Dispose();
                _dataPort = null;
            }

            // 取消监听线程
            _token?.Cancel();
            _token?.Dispose();
        }

        #endregion

        #region 数据监听工作线程

        /// <summary>
        /// 数据监听工作线程（DataPort 专用）
        /// 
        /// 工作流程：
        ///   1. 轮询检查 DataPort 是否有可读数据
        ///   2. 读取一次数据后，等待 1 秒以收集可能分批到达的数据
        ///   3. 合并所有读取的数据并转换为 ASCII 字符串
        ///   4. 触发 RawDataReceived 事件
        /// 
        /// 为什么要等待 1 秒？
        ///   - 某些设备可能分多个数据包发送完整的结果
        ///   - 等待可以确保收集到完整的测试结果数据
        /// </summary>
        private async Task DataListenerWork(CancellationTokenSource token)
        {
            byte[] buffer = new byte[BUFFER_SIZE];

            while (_isRunning && _dataPort != null && _dataPort.IsOpen && !token.IsCancellationRequested)
            {
                try
                {
                    int bytesToRead = _dataPort.BytesToRead;

                    if (bytesToRead > 0)
                    {
                        // 读取第一批数据
                        int bytesRead = _dataPort.Read(buffer, 0, Math.Min(buffer.Length, bytesToRead));
                        byte[] receivedBytes = new byte[bytesRead];
                        Array.Copy(buffer, receivedBytes, bytesRead);

                        // 等待 1 秒，看是否有后续数据到达
                        await Task.Delay(DATA_COLLECTION_DELAY);

                        // 读取剩余的数据
                        int remainingBytes = _dataPort.BytesToRead;
                        if (remainingBytes > 0)
                        {
                            byte[] remainingData = new byte[remainingBytes];
                            _dataPort.Read(remainingData, 0, remainingBytes);

                            // 合并所有读取的数据
                            byte[] combinedBytes = new byte[receivedBytes.Length + remainingData.Length];
                            Array.Copy(receivedBytes, 0, combinedBytes, 0, receivedBytes.Length);
                            Array.Copy(remainingData, 0, combinedBytes, receivedBytes.Length, remainingData.Length);
                            receivedBytes = combinedBytes;
                        }

                        // 转换为 ASCII 字符串
                        string text = Encoding.ASCII.GetString(receivedBytes);

                        // 触发事件，将数据转发给 Controller
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            RawDataReceived?.Invoke(this, text);
                        }
                    }
                    else
                    {
                        // 无数据时降低 CPU 占用
                        Thread.Sleep(IDLE_SLEEP_MS);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 正常取消，无需处理
                }
                catch (Exception ex)
                {
                    if (_isRunning)
                        SendError(ex);
                }
            }
        }

        #endregion

        #region 事件触发辅助方法

        /// <summary>
        /// 触发状态消息事件
        /// </summary>
        private void SendStatus(string msg) => StatusMessage?.Invoke(this, msg);

        /// <summary>
        /// 触发异常事件
        /// </summary>
        private void SendError(Exception ex) => ErrorOccurred?.Invoke(this, ex);

        #endregion

        #region 资源清理

        /// <summary>
        /// 实现 IDisposable 接口
        /// </summary>
        public void Dispose() { ClosePorts(); }

        #endregion
    }
}