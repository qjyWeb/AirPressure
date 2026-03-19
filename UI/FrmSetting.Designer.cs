namespace AirPressure.UI
{
    partial class FrmScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmScan));
            txtVehicle = new Sunny.UI.UITextBox();
            uiLabel1 = new Sunny.UI.UILabel();
            uiLabel2 = new Sunny.UI.UILabel();
            txtSN = new Sunny.UI.UITextBox();
            btnConfirm = new Sunny.UI.UIButton();
            btnClear = new Sunny.UI.UIButton();
            btnReturnRes = new Sunny.UI.UIButton();
            btnStartTest = new Sunny.UI.UIButton();
            btnManualStop = new Sunny.UI.UIButton();
            uiLabelMes = new Sunny.UI.UILabel();
            swMesLocal = new Sunny.UI.UISwitch();
            uiLabelReport = new Sunny.UI.UILabel();
            swReportLocal = new Sunny.UI.UISwitch();
            SuspendLayout();
            // 
            // txtVehicle
            // 
            txtVehicle.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            txtVehicle.Location = new Point(137, 49);
            txtVehicle.Margin = new Padding(4, 5, 4, 5);
            txtVehicle.MinimumSize = new Size(1, 16);
            txtVehicle.Name = "txtVehicle";
            txtVehicle.Padding = new Padding(5);
            txtVehicle.ShowText = false;
            txtVehicle.Size = new Size(405, 37);
            txtVehicle.TabIndex = 0;
            txtVehicle.TextAlignment = ContentAlignment.MiddleCenter;
            txtVehicle.Watermark = "";
            txtVehicle.KeyDown += txtVehicle_KeyDown;
            // 
            // uiLabel1
            // 
            uiLabel1.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel1.Location = new Point(34, 49);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(96, 34);
            uiLabel1.TabIndex = 1;
            uiLabel1.Text = "治具码：";
            uiLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            uiLabel2.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel2.Location = new Point(34, 111);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(96, 34);
            uiLabel2.TabIndex = 3;
            uiLabel2.Text = "SN：";
            uiLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtSN
            // 
            txtSN.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            txtSN.Location = new Point(137, 111);
            txtSN.Margin = new Padding(4, 5, 4, 5);
            txtSN.MinimumSize = new Size(1, 16);
            txtSN.Name = "txtSN";
            txtSN.Padding = new Padding(5);
            txtSN.ShowText = false;
            txtSN.Size = new Size(405, 37);
            txtSN.TabIndex = 2;
            txtSN.TextAlignment = ContentAlignment.MiddleCenter;
            txtSN.Watermark = "";
            txtSN.KeyDown += txtSN_KeyDown;
            // 
            // btnConfirm
            // 
            btnConfirm.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnConfirm.Location = new Point(137, 168);
            btnConfirm.MinimumSize = new Size(1, 1);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(145, 46);
            btnConfirm.TabIndex = 4;
            btnConfirm.Text = "确认";
            btnConfirm.TipsFont = new Font("SimSun", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnClear
            // 
            btnClear.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnClear.Location = new Point(397, 168);
            btnClear.MinimumSize = new Size(1, 1);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(145, 46);
            btnClear.TabIndex = 5;
            btnClear.Text = "清除条码";
            btnClear.TipsFont = new Font("SimSun", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnClear.Click += btnClear_Click;
            // 
            // btnReturnRes
            // 
            btnReturnRes.Font = new Font("Microsoft YaHei", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnReturnRes.Location = new Point(385, 326);
            btnReturnRes.MinimumSize = new Size(1, 1);
            btnReturnRes.Name = "btnReturnRes";
            btnReturnRes.Size = new Size(145, 46);
            btnReturnRes.TabIndex = 41;
            btnReturnRes.Text = "返回数据(调试)";
            btnReturnRes.TipsFont = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnReturnRes.Click += btnReturnRes_Click;
            // 
            // btnStartTest
            // 
            btnStartTest.Font = new Font("Microsoft YaHei", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnStartTest.Location = new Point(204, 326);
            btnStartTest.MinimumSize = new Size(1, 1);
            btnStartTest.Name = "btnStartTest";
            btnStartTest.Size = new Size(145, 46);
            btnStartTest.TabIndex = 42;
            btnStartTest.Text = "开始测试(调试)";
            btnStartTest.TipsFont = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnStartTest.Click += btnStartTest_Click;
            // 
            // btnManualStop
            // 
            btnManualStop.Font = new Font("Microsoft YaHei", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnManualStop.Location = new Point(34, 326);
            btnManualStop.MinimumSize = new Size(1, 1);
            btnManualStop.Name = "btnManualStop";
            btnManualStop.Size = new Size(145, 46);
            btnManualStop.TabIndex = 43;
            btnManualStop.Text = "手动停止";
            btnManualStop.TipsFont = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnManualStop.Click += btnManualStop_Click;
            // 
            // uiLabelMes
            // 
            uiLabelMes.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabelMes.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabelMes.Location = new Point(3, 263);
            uiLabelMes.Name = "uiLabelMes";
            uiLabelMes.Size = new Size(132, 34);
            uiLabelMes.TabIndex = 44;
            uiLabelMes.Text = "MES开关：";
            uiLabelMes.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // swMesLocal
            // 
            swMesLocal.Active = true;
            swMesLocal.ActiveText = "ON";
            swMesLocal.Font = new Font("Microsoft YaHei", 14.25F, FontStyle.Bold);
            swMesLocal.InActiveText = "OFF";
            swMesLocal.Location = new Point(137, 268);
            swMesLocal.MinimumSize = new Size(1, 1);
            swMesLocal.Name = "swMesLocal";
            swMesLocal.Size = new Size(81, 29);
            swMesLocal.TabIndex = 45;
            swMesLocal.ValueChanged += swMesLocal_ValueChanged;
            // 
            // uiLabelReport
            // 
            uiLabelReport.Font = new Font("Microsoft YaHei", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabelReport.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabelReport.Location = new Point(290, 263);
            uiLabelReport.Name = "uiLabelReport";
            uiLabelReport.Size = new Size(160, 34);
            uiLabelReport.TabIndex = 46;
            uiLabelReport.Text = "异常上报开关：";
            uiLabelReport.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // swReportLocal
            // 
            swReportLocal.Active = true;
            swReportLocal.ActiveText = "ON";
            swReportLocal.Font = new Font("Microsoft YaHei", 14.25F, FontStyle.Bold);
            swReportLocal.InActiveText = "OFF";
            swReportLocal.Location = new Point(461, 268);
            swReportLocal.MinimumSize = new Size(1, 1);
            swReportLocal.Name = "swReportLocal";
            swReportLocal.Size = new Size(81, 29);
            swReportLocal.TabIndex = 47;
            swReportLocal.ValueChanged += swReportLocal_ValueChanged;
            // 
            // FrmScan
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(629, 405);
            Controls.Add(btnManualStop);
            Controls.Add(btnStartTest);
            Controls.Add(btnReturnRes);
            Controls.Add(btnClear);
            Controls.Add(btnConfirm);
            Controls.Add(uiLabel2);
            Controls.Add(txtSN);
            Controls.Add(uiLabel1);
            Controls.Add(txtVehicle);
            Controls.Add(uiLabelMes);
            Controls.Add(swMesLocal);
            Controls.Add(uiLabelReport);
            Controls.Add(swReportLocal);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmScan";
            Text = "设置";
            TitleFont = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            Load += FrmScan_Load;
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UITextBox txtVehicle;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UITextBox txtSN;
        private Sunny.UI.UIButton btnConfirm;
        private Sunny.UI.UIButton btnClear;
        private Sunny.UI.UIButton btnReturnRes;
        private Sunny.UI.UIButton btnStartTest;
        private Sunny.UI.UIButton btnManualStop;
        private Sunny.UI.UILabel uiLabelMes;
        private Sunny.UI.UISwitch swMesLocal;
        private Sunny.UI.UILabel uiLabelReport;
        private Sunny.UI.UISwitch swReportLocal;
    }
}