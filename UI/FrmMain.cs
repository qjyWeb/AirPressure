using AirPressure.Logic;
using AirPressure.Models;
using AirPressure.Services;
using AirPressure.UI;
using Sunny.UI;
using System.Diagnostics;

namespace AirPressure
{
    /// <summary>
    /// 主窗体 - 气密性测试系统的主界面
    /// 职责：UI 展示、用户交互、数据查询导出、日志显示
    /// </summary>
    public partial class FrmMain : UIForm
    {
        #region 字段和属性

        /// <summary>
        /// 测试站控制器 - 负责测试流程协调和硬件通信
        /// </summary>
        public TestStationController? _controller { get; private set; }

        /// <summary>
        /// 数据仓储层 - 负责数据库查询操作
        /// </summary>
        private ITestRepository _repository;

        /// <summary>
        /// 日志服务 - 负责日志记录和持久化
        /// </summary>
        private ILogService _logService;

        /// <summary>
        /// 测试计时器 - 用于计算测试耗时
        /// </summary>
        private Stopwatch? _stopwatch;

        /// <summary>
        /// 扫描窗体实例 - 供主窗体调用以打开扫描界面
        /// </summary>
        private static FrmScan frmScan = null;

        #endregion

        #region 构造函数

        public FrmMain()
        {
            InitializeComponent();
            SetStyle(UIStyle.Blue);
            EventsBind();

            // 初始化依赖注入（默认实现，可在未来进一步解耦）
            _controller = new TestStationController();
            _repository = new SqliteRepositoryAdapter();
            _logService = new FileLogService();
            _stopwatch = new Stopwatch();

            // 根据上次配置恢复 MES 开关状态
            // 不直接设置控件的内部属性（不同版本的 Sunny.UI 名称可能不同）
            // 根据上次配置自动（异步）启用/停用 MES 功能，并更新指示灯
            _ = _controller.SetMesEnabledAsync(GlobalVar.IsStartMes);
            ledMes.Color = GlobalVar.IsStartMes ? Color.LimeGreen : Color.White;

            // 配置日志流转：控制器 -> 日志服务 -> UI 显示
            ConfigureLoggingPipeline();

            // 配置控制器事件回调
            ConfigureControllerEvents();
        }

        #endregion

        #region 事件配置

        private void EventsBind()
        {
            this.Load += FrmMain_Load;
            this.FormClosing += FrmMain_FormClosed;
            this.btnSearch.Click += BtnSearch;
            btnRefresh.Click += BtnRefresh;
            btnExport.Click += BtnExport;
            timer1.Tick += Timer1Tick;
            timer2.Tick += Timer2Tick;
            lblRes.TextChanged += LblRes_TextChanged;
        }

        /// <summary>
        /// 配置日志管道：将控制器的日志输出转接到 UI 显示
        /// </summary>
        private void ConfigureLoggingPipeline()
        {
            _controller.OnLog += (msg, color) => _logService.Log(msg, color);
            _logService.OnLog += (msg, color) =>
            {
                if (!this.IsHandleCreated) return;
                this.Invoke(new Action(() => AppendTextWithColor(msg, color)));
            };
        }

        /// <summary>
        /// 配置控制器事件：测试完成、计时、MES 心跳等
        /// </summary>
        private void ConfigureControllerEvents()
        {
            // 测试完成事件
            _controller.OnTestFinished += (res) =>
            {
                if (!this.IsHandleCreated) return;
                this.Invoke(new Action(() =>
                {
                    btnRefresh.PerformClick();
                    RefreshDataCal();
                    if (res == "PASS")
                        AppendTextWithColor($"测试完成: {res}", Color.Green);
                    else
                        AppendTextWithColor($"测试完成: {res}", Color.Red);
                }));
            };

            // 测试计时事件：true=开始计时，false=停止计时
            _controller.OnTime += (flag) =>
            {
                if (flag)
                {
                    _stopwatch.Restart();
                    GlobalVar.Res = string.Empty;
                }
                else
                {
                    if (_stopwatch.IsRunning)
                        _stopwatch.Stop();
                }
            };

            // MES 心跳事件：0=无心跳，1=正常，2=异常
            _controller.OnMes += async (flag) =>
            {
                if (flag == 1)
                {
                    ledMes.Color = Color.LimeGreen;
                    await Task.Delay(1500);
                    ledMes.Color = Color.White;
                }
                else
                {
                    ledMes.Color = Color.Red;
                }
            };
        }

        private void LblRes_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblRes.Text))
            {
                lblRes.BackColor = lblRes.Text == "PASS" ? Color.LimeGreen : Color.Red;
            }
            else
                lblRes.BackColor = Color.White;
        }

        private void cmbCH_TextChanged(object sender, EventArgs e)
        {
            if (GlobalVar.IsStart)
                return;
            GlobalVar.ParamCH = cmbCH.Text.Trim();
        }

        #endregion

        #region 窗体生命周期

        private void FrmMain_Load(object? sender, EventArgs e)
        {
            try
            {
                timer1.Start();
                timer2.Start();
                RefreshDataCal();
                btnRefresh.PerformClick();

                GlobalVar.ParamCH = cmbCH.Text.Trim();

                // 初始化系统和硬件
                _controller?.InitializeSystem();
                AppendTextWithColor("系统初始化完成，服务已启动", Color.LimeGreen);
            }
            catch (Exception ex)
            {
                AppendTextWithColor($"系统初始化失败: {ex.Message}", Color.Red);
            }
        }

        private void FrmMain_FormClosed(object? sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "确定要关闭程序吗？",
                "提示",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                _controller?.Dispose();
                _ = _logService.SaveLogAsync(richTxtLog.Lines);
                //FrmLogin.Close(); // 关闭登录窗体
                //FrmLogin.Dispose(); // 释放登录窗体资源
            }
        }

        #endregion

        #region 数据查询和导出

        /// <summary>
        /// 查询按钮事件：按日期范围、SN、结果筛选数据
        /// </summary>
        private async void BtnSearch(object? sender, EventArgs e)
        {
            // 获取筛选条件
            string start = datePickerStart.Value.Date.ToString("yyyy-MM-dd 00:00:00");
            string end = datePickerEnd.Value.Date.ToString("yyyy-MM-dd 23:59:59");
            string sn = txtSearchSN.Text.Trim();
            string result = cmbResult.Text;

            btnSearch.Enabled = false;
            btnSearch.Text = "查询中...";

            try
            {
                // 调用仓储层查询数据
                List<TestResultShowModel> data = await _repository.QueryDataAsync(start, end, sn, result);

                // 绑定到 DataGridView
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = data;

                // 根据结果改变行颜色
                UpdateRowColors();
            }
            catch (Exception ex)
            {
                AppendTextWithColor($"查询出错:{ex.Message}", Color.Red);
            }
            finally
            {
                btnSearch.Enabled = true;
                btnSearch.Text = "查询";
            }
        }

        /// <summary>
        /// 刷新按钮事件：获取所有数据，按日期从新到旧排序
        /// </summary>
        private async void BtnRefresh(object? sender, EventArgs e)
        {
            try
            {
                RefreshDataCal();
                dataGridView1.AutoGenerateColumns = false;
                List<TestResultShowModel> data = await _repository.GetAllAsync();
                dataGridView1.DataSource = data;
                UpdateRowColors();
            }
            catch (Exception ex)
            {
                AppendTextWithColor($"查询出错:{ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// 导出按钮事件：将当前数据导出为 CSV 文件
        /// </summary>
        private async void BtnExport(object? sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("当前没有数据可导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV 文件 (*.csv)|*.csv|所有文件 (*.*)|*.*",
                FileName = $"DataExport_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "导出数据"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await CsvExporter.ExportAsync(dataGridView1, sfd.FileName);
                    MessageBox.Show("导出成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("导出失败：文件可能正在被其他程序打开。\n\n" + ex.Message, "文件占用", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 更新表格行颜色：FAIL 结果显示为红色
        /// </summary>
        private void UpdateRowColors()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var item = row.DataBoundItem as TestResultShowModel;
                if (item != null && item.Result == "FAIL")
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                }
            }
        }

        #endregion

        #region 定时器事件

        /// <summary>
        /// Timer1 Tick：更新 UI 显示的实时数据（治具、条码、工站、结果等）
        /// 执行频率：较高频（通常 100-500ms）
        /// </summary>
        private void Timer1Tick(object? sender, EventArgs e)
        {
            txtVehicle.Text = GlobalVar.Vehicle;
            txtSN.Text = GlobalVar.Serial;
            txtStation.Text = GlobalVar.ModelName;
            lblRes.Text = GlobalVar.Res;
            cmbCH.Text = GlobalVar.ParamCH;
        }

        /// <summary>
        /// Timer2 Tick：更新测试计时显示
        /// 执行频率：1000ms（每秒更新一次）
        /// </summary>
        private void Timer2Tick(object? sender, EventArgs e)
        {
            txtTime.Text = _stopwatch?.Elapsed.ToString(@"hh\:mm\:ss");
        }

        #endregion

        #region 数据统计和计算

        /// <summary>
        /// 刷新并计算今日和历史良率数据
        /// </summary>
        private async void RefreshDataCal()
        {
            // 今日数据
            int todayCount = await _repository.GetTodayTestCountAsync();
            lblTodayCount.Text = todayCount.ToString();

            int todayFailCount = await _repository.GetTodayDefectiveCountAsync();
            lblTodayFailCount.Text = todayFailCount.ToString();

            if (todayCount != 0)
            {
                double todaySuccessRate = (double)(todayCount - todayFailCount) / todayCount;
                lblTodayRate.Text = (todaySuccessRate * 100).ToString("0.00");
            }
            else
                lblTodayRate.Text = "0.00";

            // 历史数据
            int totalCount = await _repository.GetTotalTestCountAsync();
            lblTotalCount.Text = totalCount.ToString();

            if (totalCount != 0)
            {
                int totalFailCount = await _repository.GetTotalDefectiveCountAsync();
                double totalSuccessRate = (double)(totalCount - totalFailCount) / totalCount;
                lblTotalRate.Text = (totalSuccessRate * 100.0).ToString("0.00");
            }
            else
                lblTotalRate.Text = "0.00";
        }

        #endregion

        #region 日志和辅助方法

        /// <summary>
        /// 以指定颜色将日志文本追加到日志区域，带时间戳
        /// </summary>
        public void AppendTextWithColor(string text, Color color, bool addNewLine = true)
        {
            if (richTxtLog.InvokeRequired)
            {
                richTxtLog.Invoke(new Action<string, Color, bool>(AppendTextWithColor), text, color, addNewLine);
                return;
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{timestamp}] {text}";
            richTxtLog.SelectionStart = richTxtLog.TextLength;
            richTxtLog.SelectionLength = 0;
            richTxtLog.SelectionColor = color;
            richTxtLog.AppendText(addNewLine ? logEntry + Environment.NewLine : logEntry);
            richTxtLog.ScrollToCaret();
        }

        /// <summary>
        /// 打开扫描窗体
        /// </summary>
        private void btnScanFrm_Click(object sender, EventArgs e)
        {
            if (frmScan == null || frmScan.IsDisposed)
            {
                frmScan = new FrmScan(this);
                frmScan.FormClosed += (s, args) =>
                {
                    frmScan = null; // 窗体关闭后重置
                };
            }
            frmScan.Show();
            frmScan.BringToFront();
            frmScan.WindowState = FormWindowState.Normal;
        }

        #endregion

        #region 子窗体调用的接口

        /// <summary>
        /// 开始测试流程
        /// </summary>
        public async Task BtnStartTest()
        {
            if (_controller != null)
                await _controller.StartTestProcess();
            else
                AppendTextWithColor("控制器未初始化，无法开始测试。", Color.Red);
        }

        /// <summary>
        /// 上报测试结果到 MES
        /// </summary>
        public async Task BtnReturnRes()
        {
            if (_controller == null)
            {
                AppendTextWithColor("控制器未初始化，无法上传结果。", Color.Red);
                return;
            }

            // 构造测试结果模型
            TestResultModel test = new TestResultModel();
            test.DetailedData["气密测试测试压力"] = "气密测试测试压力,pa,100,200,150,PASS";
            test.DetailedData["气密测试测试泄漏值"] = "气密测试测试泄漏值,pa,100,200,150,PASS";
            test.DetailedData["气密测试测试压力单位"] = "气密测试测试压力单位,pa,100,200,150,PASS";
            test.DetailedData["气密测试测试泄漏值单位"] = "气密测试测试泄漏值单位,pa,100,200,150,PASS";
            test.DetailedData["气密测试测试压力Max"] = "气密测试测试压力Max,pa,100,200,150,PASS";
            test.DetailedData["气密测试测试压力Min"] = "气密测试测试压力Min,pa,100,200,150,PASS";

            await _controller.ExecuteUpload(test);
        }

        /// <summary>
        /// 手动停止测试
        /// </summary>
        public void BtnStopTest()
        {
            _controller?.ManualStop();
        }

        #endregion


    }
}
