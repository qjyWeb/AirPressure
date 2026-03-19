using AirPressure.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AirPressure.Utils
{
    /// <summary>
    /// HTTP JSON 工具 - 处理与 MES（制造执行系统）的 HTTP 通信
    /// 
    /// 双向通信架构：
    ///   - 服务器模式：监听本地端口，接收来自 MES 的指令（SET.SN, SET.RFID 等）
    ///   - 客户端模式：主动向 MES 服务器发送测试结果（POST 请求）
    /// 
    /// 用途：
    ///   1. 接收 MES 的产品信息、工单、测试指令等
    ///   2. 上报测试结果、设备状态等
    ///   3. 支持心跳监控（GET.APP 命令）
    /// </summary>
    public class HttpJsonTool : IDisposable
    {
        #region 字段定义

        /// <summary>
        /// HTTP 服务器监听器（用于接收 MES 指令）
        /// </summary>
        private readonly HttpListener _listener;

        /// <summary>
        /// HTTP 客户端（用于向 MES 发送数据）
        /// 使用单例避免频繁创建/销毁导致的 Socket 耗尽
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// 监听循环的取消令牌源
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// 监听器运行状态标记
        /// </summary>
        private bool _isListening = false;

        #endregion

        #region 事件定义

        /// <summary>
        /// HTTP 状态消息事件
        /// 用于报告监听启动、请求接收等正常状态信息
        /// </summary>
        public event EventHandler<string>? HttpStatusMessage;

        /// <summary>
        /// HTTP 错误消息事件
        /// 用于报告通信错误、异常等异常信息
        /// </summary>
        public event EventHandler<string>? HttpErrorMessage;

        #endregion

        #region 构造函数

        public HttpJsonTool()
        {
            _client = new HttpClient();
            _listener = new HttpListener();
            _cts = new CancellationTokenSource();
        }

        #endregion

        #region 服务器模式：监听 MES 指令

        /// <summary>
        /// 启动 HTTP 监听服务器
        /// 
        /// 参数说明：
        ///   - ip: 本地监听 IP（如 "192.168.1.100"）
        ///   - port: 监听端口号（如 "8080"）
        ///   - onReceived: 业务层回调函数，用于处理接收到的数据
        /// 
        /// 工作流程：
        ///   1. 根据 ip:port 创建监听前缀（如 "http://192.168.1.100:8080/"）
        ///   2. 启动 HTTP 监听器
        ///   3. 进入循环等待 HTTP 请求
        ///   4. 每个请求异步交给 ProcessRequestAsync 处理
        /// </summary>
        public async Task StartListenerAsync<T>(string ip, string port, Func<MonitorDataModel, ResponseDataModel> onReceived)
        {
            // 防止重复启动
            if (_isListening) return;

            // 如果 MES 未启用，则不启动监听
            if (!GlobalVar.IsStartMes) return;

            string prefix = $"http://{ip}:{port}/";

            try
            {
                // 每次启动监听器时重新初始化 _cts
                _cts = new CancellationTokenSource();

                // 清理旧的前缀并添加新的
                _listener.Prefixes.Clear();
                _listener.Prefixes.Add(prefix);
                _listener.Start();
                _isListening = true;

                HttpStatusMessage?.Invoke(this, $"[HTTP] 监听启动: {prefix}");

                // 主监听循环
                while (_isListening && !_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        // 等待来自客户端的 HTTP 请求
                        var context = await _listener.GetContextAsync();

                        // 异步处理请求，不阻塞主循环
                        _ = ProcessRequestAsync(context, onReceived);
                    }
                    catch (HttpListenerException) when (!_isListening)
                    {
                        // 监听器已关闭，正常退出
                    }
                }
            }
            catch (Exception ex)
            {
                HttpErrorMessage?.Invoke(this, $"[HTTP 监听启动失败] {prefix}异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理单个 HTTP 请求
        /// 
        /// 工作流程：
        ///   1. 从请求体读取 JSON 数据
        ///   2. 反序列化为 MonitorDataModel
        ///   3. 调用业务层回调进行处理
        ///   4. 序列化响应数据为 JSON
        ///   5. 返回给客户端
        /// </summary>
        private async Task ProcessRequestAsync(HttpListenerContext context, Func<MonitorDataModel, ResponseDataModel> onReceived)
        {
            try
            {
                ResponseDataModel responseData = null;

                // 读取请求体中的 JSON 数据
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    string jsonBody = await reader.ReadToEndAsync();
                    MonitorDataModel? data = JsonConvert.DeserializeObject<MonitorDataModel>(jsonBody);

                    // 调用业务层回调处理数据
                    if (onReceived != null)
                    {
                        responseData = onReceived(data);
                    }
                }

                // 如果业务层没有返回响应，提供默认值
                if (responseData == null)
                {
                    responseData = new ResponseDataModel
                    {
                        success = "OK",
                        message = "OK"
                    };
                }

                // 序列化响应数据为 JSON
                string json = JsonConvert.SerializeObject(responseData);
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                // 返回响应给客户端
                context.Response.StatusCode = 200;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                HttpErrorMessage?.Invoke(this, $"[HTTP 请求处理错误]: {ex.Message}");
                context.Response.StatusCode = 500;
            }
            finally
            {
                context.Response.Close();
            }
        }

        #endregion

        #region 客户端模式：向 MES 发送数据

        /// <summary>
        /// 主动向 MES 服务器发送 JSON 数据
        /// 
        /// 用途：上报测试结果、设备状态等
        /// 
        /// 参数说明：
        ///   - url: 目标服务器完整 URL（如 "http://192.168.1.1:8080/"）
        ///   - data: 要发送的对象（会被序列化为 JSON）
        ///   - timeoutSeconds: 请求超时时间（秒）
        /// 
        /// 返回：服务器响应内容，失败返回 null
        /// 
        /// 超时保护：
        ///   - 使用 CancellationTokenSource 实现超时控制
        ///   - 防止请求无限期等待
        /// </summary>
        public async Task<string> SendJsonAsync<T>(string url, T data, int timeoutSeconds = 100)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)))
            {
                try
                {
                    // 步骤 1：将对象序列化为 JSON 字符串
                    string json = JsonConvert.SerializeObject(data);

                    // 步骤 2：封装为 HttpContent
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // 步骤 3：发送 POST 请求
                    // 使用类持有的单例 HttpClient，避免 Socket 耗尽问题
                    HttpResponseMessage response = await _client.PostAsync(url, content, cts.Token);

                    // 步骤 4：确保响应状态码为 2xx，否则抛出异常
                    response.EnsureSuccessStatusCode();

                    // 步骤 5：读取并返回响应体
                    string responseBody = await response.Content.ReadAsStringAsync();
                    HttpStatusMessage?.Invoke(this, $"[HTTP 发送成功] 目标: {url}");
                    return responseBody;
                }
                catch (OperationCanceledException)
                {
                    // 请求超时
                    HttpErrorMessage?.Invoke(this, $"[HTTP 超时] 请求 {url} 超过 {timeoutSeconds} 秒未响应");
                    return null;
                }
                catch (Exception ex)
                {
                    // 其他异常（网络错误、序列化错误等）
                    HttpErrorMessage?.Invoke(this, $"[HTTP 发送异常] 目标: {url}, 原因: {ex.Message}");
                    return null;
                }
            }
        }

        #endregion

        #region 资源清理

        /// <summary>
        /// 停止监听服务
        /// </summary>
        public void Stop()
        {
            _isListening = false;
            _cts.Cancel();
            if (_listener.IsListening)
                _listener.Stop();
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void Dispose()
        {
            Stop();
            _listener.Close();
            _client.Dispose();
        }

        #endregion
    }
}