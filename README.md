# 项目交接文档 —— AirPressure

版本说明：基于工作区 `AirPressure`（Windows, .NET 8.0 Windows Forms）。

---

## 1. 简要说明
- 项目名称：AirPressure（气密/泄漏测试站控制软件）
- 平台与技术：.NET 8.0, Windows Forms, Modbus(NModbus4), SQLite, Newtonsoft.Json
- 功能概述：负责与测试设备通信（串口/Modbus）、与 MES 进行 HTTP 双向通信、管理测试流程、保存并上报测试结果。
 
 
## 2. 主要文件/目录地图（快速索引）
- Program.cs — 程序入口
- UI/FrmMain.cs、UI/FrmSetting.cs — 主界面与设置界面（与控制器交互并显示日志/状态）
- Controller/TestStationController.cs — 系统核心控制器（测试流程、心跳、HTTP 处理、串口数据解析、结果上报）
- Utils/
  - DualSerialPortManager.cs — 双串口与 Modbus 主站封装（CmdPort + DataPort）
  - HttpJsonTool.cs — HTTP 服务与客户端（监听 MES 指令 & 主动上报）
  - GlobalVar.cs — 配置与运行时全局变量（从 config.ini 读取）
  - Ini.cs — INI 读写工具
  - SqliteHelper.cs — SQLite 数据库操作封装
- Models/
  - MonitorDataModel.cs、ResponseDataModel.cs、SendDataModel.cs — HTTP 请求/响应模型
  - TestResultModel.cs、TestResultDbModel.cs、TestResultShowModel.cs — 测试结果模型
- Services/ — 日志、CSV 导出、仓储适配等

## 3. 关键组件职责
- TestStationController（位于 `Controller/TestStationController.cs`）
  - 协调硬件（串口、Modbus）、HTTP 通信、测试流程管理（初始化、启动、监控、结束）
  - 基于事件驱动向 UI 发送日志/状态/测试结果（事件：`OnLog`, `OnTestFinished`, `OnTime`, `OnMes`）
  - 负责心跳监控、DI 异常检测、超时处理、结果解析与上报。

- DualSerialPortManager（`Utils/DualSerialPortManager.cs`）
  - 管理两个物理串口：指令口（CmdPort）与数据口（DataPort）
  - 创建并暴露 Modbus 主站实例（项目中名为 `_master`，TestStationController 直接访问 `_serialManager._master`）

- HttpJsonTool（`Utils/HttpJsonTool.cs`）
  - 提供本地 HTTP 监听服务，用于接收 MES 的 JSON 包
  - 提供发送 JSON 到 MES 的客户端接口（上报测试结果）

- SqliteHelper（`Utils/SqliteHelper.cs`）
  - 封装本地 SQLite 操作，用于保存测试记录到 `TestData.db`

- GlobalVar（`Utils/GlobalVar.cs`）
  - 负责从 `config.ini` 读取配置项并在运行时保存全局状态（如 Serial、Vehicle、ModelName、IsStartMes、ParamCH 等）

## 4. Controller 深度讲解（TestStationController）
下面对 `Controller/TestStationController.cs` 做深入说明，帮助接手人员快速理解主要流程与扩展点。

概述：`TestStationController` 是项目的核心协调器，负责初始化系统、管理测试主流程、接收并解析串口数据、与 MES 通过 HTTP 交互、处理心跳与异常、以及保存/上报测试结果。它通过事件向 UI 发送日志与状态变化。

主要职责与设计要点：
- 初始化与资源管理：负责调用 `GlobalVar.LoadData()`、打开串口（`DualSerialPortManager.OpenSystem`）、启动 HTTP 监听（`HttpJsonTool`）和心跳监控。
- 测试流程编排：接收 MES 命令（`SET.MODELNAME` 等）后触发 `StartTestProcess()`，负责通道选择、启动脉冲下发、超时保护与 DI 监控。
- 数据接收与解析：通过订阅 `DualSerialPortManager.RawDataReceived` 处理原始串口返回（`OnSerialDataReceived`），解析为 `TestResultModel` 并保存/上报。
- 异常处理与上报：DI 异常、心跳超时或解析失败时负责构造 FAIL 记录并调用 `ReportErrorToServerAsync()`。

关键方法：
- `InitializeSystem()`：系统初始化入口；顺序为加载配置、打开串口、启动 HTTP 监听与心跳监控。确保在异常情况下有清理或重试路径。
- `StartTestProcess()`：测试的主控逻辑，包含并发保护（`IsTesting`），参数校验（SN/RFID），根据 `ModelChannelMap`/`ModelActionMap` 确定是否跳步或走完整流程，调度 `SelectTestChannel()` 与 `SendTestStartCommand()`，设置超时 CancellationToken。
- `SelectTestChannel(string channel)`：将通道号转换为 5 位布尔数组并通过 Modbus `WriteMultipleCoilsAsync` 写入通道选择寄存器。注意异常重试与写失败的补偿逻辑。
- `SendTestStartCommand()`：写 `TEST_START_COIL` 为 true -> 延迟 -> 写 false，以产生启动脉冲；同样需要处理写失败重试。
- `Read16DIVal(CancellationToken ct)`：循环读取 16 路 DI，检测 `DI_ERROR_COIL`（通常是第 2 位）以判断硬件异常；此方法应快速返回并在检测到异常时触发停止测试流程。
- `OnSerialDataReceived(object sender, string raw)`：串口回调处理入口，包含：检测是否处于测试中、按空格分割数据、构造 `TestResultModel`、取消超时 token、调用 `SaveToDatabase()` 与 `ExecuteUpload()` 并触发 `OnTestFinished` 事件。注意防护：字符串校验、索引边界、异常捕获与记录。
- `ProcessHttpPacket(MonitorDataModel data)`：处理 MES 过来的 JSON 命令（如 `GET.APP`, `SET.SN`, `SET.MODELNAME` 等），此处是外部驱动测试的主要入口。
- `ExecuteUpload(TestResultModel result)`：将测试结果封装为 `SendDataModel` 并使用 `HttpJsonTool.SendJsonAsync` 上报 MES；处理上报失败的重试或本地缓冲策略。
- `SaveToDatabase(TestResultDbModel dbModel)`：调用 `SqliteHelper.InsertAsync` 保存到本地 `TestData.db`，需要处理数据库被占用或写入异常。
- `MonitorHeartbeatAsync()`：周期性检查上次心跳时间，触发 `OnMes` 状态变化（无心跳/正常/异常），建议把超时时间与重连策略参数化。
- `ReportErrorToServerAsync(string reason)`：构造一个 FAIL 的 `TestResultModel` 并上报 MES，同时写入本地数据库（当 `GlobalVar.ReportErrorsToMes` 为 true）。

事件与交互：
- `OnLog(string, Color)`：用于 UI 打印调试与运行日志。
- `OnTestFinished(string)`：测试完成后通知 UI 更新结果与界面。
- `OnTime(bool)`：用于计时开始/停止的 UI 反馈。
- `OnMes(int)`：MES 状态事件（0=无、1=正常、2=异常）。

## 5. 启动与初始化流程（调用顺序）
1. 程序入口 `Program.cs` 打开主窗体 `FrmMain`。
2. `FrmMain.Load`（或启动逻辑）调用 `new TestStationController()` 并 `InitializeSystem()`。
3. `TestStationController.InitializeSystem()` 做：
   - `GlobalVar.LoadData()`：加载 `config.ini`
   - `StartHeartbeatMonitoring()`：启动心跳监控循环
   - `InitializeSerialPorts()`：通过 `DualSerialPortManager.OpenSystem(...)` 打开串口并初始化 Modbus 主站
   - `InitializeHttpListener()`：若启用 MES（`GlobalVar.IsStartMes`），启动 HTTP 监听器

## 6. HTTP 协议与 MES 交互（简要）
- 接收命令（由 `ProcessHttpPacket` 处理，关键命令）：
  - `GET.APP` — 心跳（更新 `_lastHeartbeatTime` 并返回 ON）
  - `SET.SN` — 设置产品条码（写入 `GlobalVar.Serial`）
  - `SET.RFID` — 设置治具/治具码（写入 `GlobalVar.Vehicle`）
  - `SET.WORK` — 设置工单号（`GlobalVar.WorkNum`）
  - `SET.MODELNAME` — 设置机种并触发 `StartTestProcess()`（实际代码中当前以固定字符串演示）
  - `SET.INIT` — 清空历史测试状态（示例）
- 上报：`ExecuteUpload(TestResultModel)` 将结果组装为 `SendDataModel` 并 POST 到 `http://{sendIp}:{sendPort}/`。

## 7. 测试主流程（`StartTestProcess`）要点
- 防并发：若 `IsTesting==true` 则忽略重复请求
- 参数校验：需要 `GlobalVar.Serial` 与 `GlobalVar.Vehicle` 非空
- 根据 `GlobalVar.ModelChannelMap` 将机种映射到通道并写入 `GlobalVar.ParamCH`（若配置了）
- 根据 `GlobalVar.ModelActionMap` 判断是否属于“空跑/跳过指令”的机种（Skip/None/空跑），若是则模拟等待并在超时后上报失败
- 选择通道：调用 `SelectTestChannel()` 将通道号转换为 5 位二进制并通过 `WriteMultipleCoilsAsync` 写入 `CHANNEL_SELECT_COIL`
- 发送启动脉冲：`SendTestStartCommand()` 对 `TEST_START_COIL` 写 true -> 延时 -> 写 false
- 标记开始时间 `GlobalVar.StartTime` 并将 `GlobalVar.IsStart = true`，启动 DI 监控 `Read16DIVal()`
- 启用超时保护（基于 `GlobalVar.MaxRunTime`），若超时未收到串口结果则上报失败

## 8. 串口数据处理（`OnSerialDataReceived`）
- 入口：`DualSerialPortManager` 在接收到数据时触发 `RawDataReceived` 事件，`TestStationController.OnSerialDataReceived` 处理
- 主要逻辑：
  - 若 `GlobalVar.IsStart==false` 则忽略
  - 特殊机种分支（示例中 `ModelName == "KongPao"`）有特殊处理
  - 常规：按空格将原始字符串拆分为各个字段（`parts`），至少需 3 个字段
  - 构造 `TestResultModel`：根据 `parts[2]` 来判断 PASS/FAIL（等于 "2" 则 PASS）
  - 填充 `DetailedData` 中若干条目（压力、泄漏值、单位、最大/最小压力等）
  - 更新全局状态 `GlobalVar.EndTime`、`GlobalVar.Res`、`GlobalVar.IsStart=false` 并取消超时等待 `_cts.Cancel()`
  - 保存到本地 SQLite（`SaveToDatabase`）并调用 `ExecuteUpload` 上报 MES
  - 触发 `OnTestFinished` 通知 UI

注意：当前对串口返回数据的解析基于固定索引（例如 `parts[7]`、`parts[3]` 等），若设备数据格式变化会导致解析错误或索引越界，建议在接手时确认设备输出格式或改用更健壮的解析方法（例如正则或以标签/JSON 的方式回传）。

## 9. 异常与 DI 检测
- DI 监控（`Read16DIVal`）每 500ms 读取 16 个输入，若 DI[2]（`DI_ERROR_COIL`）为 true 则视为测试异常，停止测试并上报
- 心跳监控：`MonitorHeartbeatAsync` 每 5 秒检查一次，若超过 20 秒未收到 `GET.APP` 则触发 `OnMes(2)` 异常事件
- 异常上报：`ReportErrorToServerAsync` 会在 `GlobalVar.ReportErrorsToMes` 为 true 时向 MES 上报一个 FAIL 的测试结果并保存到本地数据库

## 10. 配置项（来自 `GlobalVar` / config.ini）——重要项
- 串口：`CmdPortName`, `DataPortName`, `CmdBaudRate`, `DataBaudRate`
- MES / HTTP：`IsStartMes`, `listenIp`, `listenPort`, `sendIp`, `sendPort`, `ReportErrorsToMes`
- 测试：`ModelChannelMap`, `ModelActionMap`, `ParamCH`, `MaxRunTime`
- 运行时变量（GlobalVar）：`Serial`, `Vehicle`, `StartTime`, `EndTime`, `Res`, `IsStart`, `IsError` 等

示例 config.ini 片段（以 `GlobalVar.LoadData()` 实际实现为准）：

```ini
[Serial]
CmdPortName = COM3
DataPortName = COM4
CmdBaudRate = 9600
DataBaudRate = 9600

[Http]
IsStartMes = true
listenIp = 0.0.0.0
listenPort = 5000
sendIp = 192.168.1.100
sendPort = 8080

[Test]
MaxRunTime = 30
ModelChannelMap = ModelA:1;ModelB:3
ModelActionMap = ModelA:Run;ModelB:Skip

[General]
StationName = Station01
ReportErrorsToMes = true
```

## 11. 本地持久化
- 数据库文件：`TestData.db`（位于运行目录 `bin\...`）
- 插入操作：`TestStationController.SaveToDatabase` 调用 `SqliteHelper.InsertAsync`
- 导出：`Services/CsvExportService.cs` 提供 CSV 导出功能

## 12. 日志与 UI 事件
- 事件：
  - `OnLog(string, Color)` — 日志
  - `OnTestFinished(string)` — 测试完成（"PASS"/"FAIL"）
  - `OnTime(bool)` — 计时开始/停止
  - `OnMes(int)` — MES 心跳状态（0/1/2）
- `FrmMain` 订阅这些事件并在界面上显示日志、状态和测试结果

## 13. 常见问题与排查建议
1. 串口/Modbus 无响应：
   - 检查 `config.ini` 中的 `CmdPortName`/`DataPortName` 是否正确
   - 检查设备物理连接与端口占用
   - 查看 UI 日志中串口错误信息
2. `_master` 为 null：
   - `DualSerialPortManager.OpenSystem` 未成功或未被调用
   - 排查 `DualSerialPortManager` 的 `StatusMessage` / `ErrorMessage`
3. MES 心跳超时：
   - 检查监听地址与端口，确认 MES 能到达该服务
   - 检查 `ProcessHttpPacket` 是否正确处理 `GET.APP`
4. 上报失败：
   - 检查 `GlobalVar.sendIp`/`sendPort` 与网络连通性
   - 查看 `HttpJsonTool` 的错误日志或异常
5. 数据库写入失败：
   - 检查 `TestData.db` 文件权限或是否被占用


