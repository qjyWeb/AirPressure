using Sunny.UI;

namespace AirPressure.UI
{
    /// <summary>
    /// 扫描窗体 - 用于扫描治具号、产品条码、启动/停止测试
    /// 
    /// 职责：
    ///   - 接收条码扫描枪的输入（治具号、产品条码）
    ///   - 提供测试启动、停止、结果上报等控制按钮
    ///   - 与主窗体交互，触发测试流程
    /// </summary>
    public partial class FrmScan : UIForm
    {
        #region 字段

        /// <summary>
        /// 对主窗体的引用，用于调用测试相关方法
        /// </summary>
        private FrmMain _frmMain;

        #endregion

        #region 构造函数

        public FrmScan(FrmMain frmMain)
        {
            InitializeComponent();
            SetStyle(UIStyle.Blue);
            _frmMain = frmMain;
        }

        #endregion

        #region 输入控制（Enter 键导航和确认）

        /// <summary>
        /// 治具号输入框的 KeyDown 事件
        /// 用户输入治具号后按 Enter 键，自动将焦点移到条码输入框
        /// </summary>
        private void txtVehicle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtVehicle.Text))
            {
                txtSN.Focus();
            }
        }

        /// <summary>
        /// 产品条码输入框的 KeyDown 事件
        /// 用户输入条码后按 Enter 键，自动点击确认按钮
        /// </summary>
        private void txtSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtSN.Text))
            {
                btnConfirm.PerformClick();
            }
        }

        #endregion

        #region 按钮事件处理

        /// <summary>
        /// 确认按钮：保存治具号和条码到全局变量，清空输入框
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSN.Text) && !string.IsNullOrEmpty(txtVehicle.Text))
            {
                // 将扫描数据保存到全局变量（供测试流程使用）
                GlobalVar.Vehicle = txtVehicle.Text.Trim();
                GlobalVar.Serial = txtSN.Text.Trim();

                // 清空输入框，准备下一次扫描
                txtVehicle.Text = string.Empty;
                txtSN.Text = string.Empty;

                // 将焦点移回治具号输入框
                txtVehicle.Focus();
            }
        }

        /// <summary>
        /// 清空按钮：清空全局的产品信息和输入框
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            GlobalVar.Vehicle = string.Empty;
            GlobalVar.Serial = string.Empty;
            txtVehicle.Text = string.Empty;
            txtSN.Text = string.Empty;
        }

        /// <summary>
        /// 上报结果按钮：调用主窗体的上报方法
        /// </summary>
        private void btnReturnRes_Click(object sender, EventArgs e)
        {
            _ = _frmMain.BtnReturnRes();
        }

        /// <summary>
        /// 开始测试按钮：调用主窗体的测试启动方法
        /// </summary>
        private void btnStartTest_Click(object sender, EventArgs e)
        {
            if (GlobalVar.IsStart)
                return;
            _ = _frmMain.BtnStartTest();
        }

        /// <summary>
        /// 手动停止按钮：调用主窗体的测试停止方法
        /// </summary>
        private void btnManualStop_Click(object sender, EventArgs e)
        {
            _frmMain.BtnStopTest();
        }

        #endregion

        private void swMesLocal_ValueChanged(object sender, bool value)
        {
            bool iniValue = Ini.ReadToini("Http", "IsStartMes", GlobalVar.configFilePath) == "1" ? true : false;
            if (value == iniValue)
                return;

            // Update global var and persist config
            GlobalVar.IsStartMes = value;
            Ini.Write("Http", "IsStartMes", value, GlobalVar.configFilePath);

            // Start/stop listener via main form controller
            if (_frmMain._controller != null)
            {
                _ = _frmMain._controller.SetMesEnabledAsync(value);
            }
        }

        private void swReportLocal_ValueChanged(object sender, bool value)
        {
            GlobalVar.ReportErrorsToMes = value;
            Ini.Write("Http", "ReportErrorsToMes", value, GlobalVar.configFilePath);
        }

        private void FrmScan_Load(object sender, EventArgs e)
        {
            swMesLocal.Active = GlobalVar.IsStartMes;
            swReportLocal.Active = GlobalVar.ReportErrorsToMes;
        }
    }
}
