namespace AirPressure
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            lblLog = new Sunny.UI.UILabel();
            uiLabel1 = new Sunny.UI.UILabel();
            uiLabel2 = new Sunny.UI.UILabel();
            uiLabel3 = new Sunny.UI.UILabel();
            uiLabel4 = new Sunny.UI.UILabel();
            txtStation = new Sunny.UI.UITextBox();
            dataGridView1 = new Sunny.UI.UIDataGridView();
            CreateTime = new DataGridViewTextBoxColumn();
            SN = new DataGridViewTextBoxColumn();
            TestMode = new DataGridViewTextBoxColumn();
            LeakageRate = new DataGridViewTextBoxColumn();
            TestPressure = new DataGridViewTextBoxColumn();
            TPUL = new DataGridViewTextBoxColumn();
            TPLL = new DataGridViewTextBoxColumn();
            Result = new DataGridViewTextBoxColumn();
            datePickerEnd = new Sunny.UI.UIDatePicker();
            txtSearchSN = new Sunny.UI.UITextBox();
            cmbResult = new Sunny.UI.UIComboBox();
            btnSearch = new Sunny.UI.UIButton();
            uiLabel5 = new Sunny.UI.UILabel();
            uiLabel6 = new Sunny.UI.UILabel();
            uiLabel7 = new Sunny.UI.UILabel();
            uiLabel8 = new Sunny.UI.UILabel();
            btnExport = new Sunny.UI.UIButton();
            uiLine2 = new Sunny.UI.UILine();
            uiLabel9 = new Sunny.UI.UILabel();
            lblTotalCount = new Sunny.UI.UILabel();
            lblTotalRate = new Sunny.UI.UILabel();
            uiLabel11 = new Sunny.UI.UILabel();
            lblTodayCount = new Sunny.UI.UILabel();
            uiLabel13 = new Sunny.UI.UILabel();
            timer1 = new System.Windows.Forms.Timer(components);
            btnRefresh = new Sunny.UI.UIButton();
            richTxtLog = new RichTextBox();
            txtSN = new Sunny.UI.UITextBox();
            lblRes = new Label();
            timer2 = new System.Windows.Forms.Timer(components);
            txtVehicle = new Sunny.UI.UITextBox();
            uiLabel14 = new Sunny.UI.UILabel();
            datePickerStart = new Sunny.UI.UIDatePicker();
            uiLabel15 = new Sunny.UI.UILabel();
            uiLabel17 = new Sunny.UI.UILabel();
            uiLabel16 = new Sunny.UI.UILabel();
            lblTodayRate = new Sunny.UI.UILabel();
            lblTodayFailCount = new Sunny.UI.UILabel();
            uiLabel20 = new Sunny.UI.UILabel();
            ledMes = new Sunny.UI.UILedBulb();
            uiLabel12 = new Sunny.UI.UILabel();
            btnScanFrm = new Sunny.UI.UIButton();
            uiLabel10 = new Sunny.UI.UILabel();
            cmbCH = new Sunny.UI.UIComboBox();
            uiLabel18 = new Sunny.UI.UILabel();
            txtTime = new Sunny.UI.UITextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // lblLog
            // 
            lblLog.BorderStyle = BorderStyle.FixedSingle;
            lblLog.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblLog.ForeColor = Color.FromArgb(48, 48, 48);
            lblLog.Location = new Point(1578, 48);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(339, 23);
            lblLog.Style = Sunny.UI.UIStyle.Custom;
            lblLog.TabIndex = 5;
            lblLog.Text = "日志信息";
            lblLog.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel1
            // 
            uiLabel1.BorderStyle = BorderStyle.FixedSingle;
            uiLabel1.Font = new Font("微软雅黑", 18F, FontStyle.Bold);
            uiLabel1.ForeColor = Color.DeepSkyBlue;
            uiLabel1.Location = new Point(8, 154);
            uiLabel1.Name = "uiLabel1";
            uiLabel1.Size = new Size(136, 75);
            uiLabel1.Style = Sunny.UI.UIStyle.Custom;
            uiLabel1.TabIndex = 7;
            uiLabel1.Text = "SN:";
            uiLabel1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel2
            // 
            uiLabel2.BorderStyle = BorderStyle.FixedSingle;
            uiLabel2.Font = new Font("微软雅黑", 18F, FontStyle.Bold);
            uiLabel2.ForeColor = Color.DeepSkyBlue;
            uiLabel2.Location = new Point(8, 242);
            uiLabel2.Name = "uiLabel2";
            uiLabel2.Size = new Size(136, 75);
            uiLabel2.Style = Sunny.UI.UIStyle.Custom;
            uiLabel2.TabIndex = 9;
            uiLabel2.Text = "程序:";
            uiLabel2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel3
            // 
            uiLabel3.BorderStyle = BorderStyle.FixedSingle;
            uiLabel3.Font = new Font("微软雅黑", 18F, FontStyle.Bold);
            uiLabel3.ForeColor = Color.DeepSkyBlue;
            uiLabel3.Location = new Point(8, 330);
            uiLabel3.Name = "uiLabel3";
            uiLabel3.Size = new Size(136, 75);
            uiLabel3.Style = Sunny.UI.UIStyle.Custom;
            uiLabel3.TabIndex = 11;
            uiLabel3.Text = "结果:";
            uiLabel3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel4
            // 
            uiLabel4.BorderStyle = BorderStyle.FixedSingle;
            uiLabel4.Font = new Font("微软雅黑", 18F, FontStyle.Bold);
            uiLabel4.ForeColor = Color.DeepSkyBlue;
            uiLabel4.Location = new Point(8, 418);
            uiLabel4.Name = "uiLabel4";
            uiLabel4.Size = new Size(136, 75);
            uiLabel4.Style = Sunny.UI.UIStyle.Custom;
            uiLabel4.TabIndex = 13;
            uiLabel4.Text = "时间:";
            uiLabel4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtStation
            // 
            txtStation.Enabled = false;
            txtStation.Font = new Font("微软雅黑", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            txtStation.Location = new Point(152, 242);
            txtStation.Margin = new Padding(4, 5, 4, 5);
            txtStation.MinimumSize = new Size(1, 16);
            txtStation.Name = "txtStation";
            txtStation.Padding = new Padding(5);
            txtStation.ShowText = false;
            txtStation.Size = new Size(1420, 75);
            txtStation.TabIndex = 14;
            txtStation.Text = "电控F线-OP4460-CT407-001-2026";
            txtStation.TextAlignment = ContentAlignment.MiddleCenter;
            txtStation.Watermark = "";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 134);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeight = 32;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { CreateTime, SN, TestMode, LeakageRate, TestPressure, TPUL, TPLL, Result });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridView1.GridColor = Color.FromArgb(80, 160, 255);
            dataGridView1.Location = new Point(6, 615);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle4.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 134);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.SelectedIndex = -1;
            dataGridView1.Size = new Size(1566, 358);
            dataGridView1.StripeOddColor = Color.FromArgb(235, 243, 255);
            dataGridView1.TabIndex = 16;
            // 
            // CreateTime
            // 
            CreateTime.DataPropertyName = "CreateTime";
            CreateTime.Frozen = true;
            CreateTime.HeaderText = "日期";
            CreateTime.Name = "CreateTime";
            CreateTime.ReadOnly = true;
            CreateTime.Width = 200;
            // 
            // SN
            // 
            SN.DataPropertyName = "SN";
            SN.Frozen = true;
            SN.HeaderText = "SN";
            SN.Name = "SN";
            SN.ReadOnly = true;
            SN.Width = 430;
            // 
            // TestMode
            // 
            TestMode.DataPropertyName = "TestMode";
            TestMode.Frozen = true;
            TestMode.HeaderText = "测试模式";
            TestMode.Name = "TestMode";
            TestMode.ReadOnly = true;
            // 
            // LeakageRate
            // 
            LeakageRate.DataPropertyName = "LeakageRate";
            LeakageRate.Frozen = true;
            LeakageRate.HeaderText = "泄漏量";
            LeakageRate.Name = "LeakageRate";
            LeakageRate.ReadOnly = true;
            LeakageRate.Width = 150;
            // 
            // TestPressure
            // 
            TestPressure.DataPropertyName = "TestPressure";
            TestPressure.Frozen = true;
            TestPressure.HeaderText = "测试压";
            TestPressure.Name = "TestPressure";
            TestPressure.ReadOnly = true;
            TestPressure.Width = 150;
            // 
            // TPUL
            // 
            TPUL.DataPropertyName = "TPUL";
            TPUL.Frozen = true;
            TPUL.HeaderText = "测试压最大值";
            TPUL.Name = "TPUL";
            TPUL.ReadOnly = true;
            TPUL.Width = 200;
            // 
            // TPLL
            // 
            TPLL.DataPropertyName = "TPLL";
            TPLL.Frozen = true;
            TPLL.HeaderText = "测试压最小值";
            TPLL.Name = "TPLL";
            TPLL.ReadOnly = true;
            TPLL.Width = 200;
            // 
            // Result
            // 
            Result.DataPropertyName = "Result";
            Result.Frozen = true;
            Result.HeaderText = "结果";
            Result.Name = "Result";
            Result.ReadOnly = true;
            Result.Width = 135;
            // 
            // datePickerEnd
            // 
            datePickerEnd.DateCultureInfo = new System.Globalization.CultureInfo("");
            datePickerEnd.FillColor = Color.White;
            datePickerEnd.Font = new Font("微软雅黑", 12F);
            datePickerEnd.Location = new Point(167, 578);
            datePickerEnd.Margin = new Padding(4, 5, 4, 5);
            datePickerEnd.MaxLength = 10;
            datePickerEnd.MinimumSize = new Size(63, 0);
            datePickerEnd.Name = "datePickerEnd";
            datePickerEnd.Padding = new Padding(0, 0, 30, 2);
            datePickerEnd.Size = new Size(150, 29);
            datePickerEnd.SymbolDropDown = 61555;
            datePickerEnd.SymbolNormal = 61555;
            datePickerEnd.SymbolSize = 24;
            datePickerEnd.TabIndex = 18;
            datePickerEnd.Text = "2026-02-02";
            datePickerEnd.TextAlignment = ContentAlignment.MiddleLeft;
            datePickerEnd.Value = new DateTime(2026, 2, 2, 1, 21, 31, 44);
            datePickerEnd.Watermark = "";
            // 
            // txtSearchSN
            // 
            txtSearchSN.Font = new Font("微软雅黑", 12F);
            txtSearchSN.Location = new Point(325, 578);
            txtSearchSN.Margin = new Padding(4, 5, 4, 5);
            txtSearchSN.MinimumSize = new Size(1, 16);
            txtSearchSN.Name = "txtSearchSN";
            txtSearchSN.Padding = new Padding(5);
            txtSearchSN.ShowText = false;
            txtSearchSN.Size = new Size(238, 29);
            txtSearchSN.TabIndex = 19;
            txtSearchSN.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearchSN.Watermark = "";
            // 
            // cmbResult
            // 
            cmbResult.AllowDrop = true;
            cmbResult.DataSource = null;
            cmbResult.DisplayMember = "PASS";
            cmbResult.FillColor = Color.White;
            cmbResult.Font = new Font("微软雅黑", 12F);
            cmbResult.ItemHoverColor = Color.FromArgb(155, 200, 255);
            cmbResult.Items.AddRange(new object[] { "", "PASS", "FAIL" });
            cmbResult.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            cmbResult.Location = new Point(571, 578);
            cmbResult.Margin = new Padding(4, 5, 4, 5);
            cmbResult.MinimumSize = new Size(63, 0);
            cmbResult.Name = "cmbResult";
            cmbResult.Padding = new Padding(0, 0, 30, 2);
            cmbResult.Size = new Size(150, 29);
            cmbResult.SymbolSize = 24;
            cmbResult.TabIndex = 20;
            cmbResult.Text = "PASS";
            cmbResult.TextAlignment = ContentAlignment.MiddleLeft;
            cmbResult.Watermark = "";
            // 
            // btnSearch
            // 
            btnSearch.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            btnSearch.Location = new Point(728, 553);
            btnSearch.MinimumSize = new Size(1, 1);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(77, 54);
            btnSearch.TabIndex = 21;
            btnSearch.Text = "查询";
            btnSearch.TipsFont = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            // 
            // uiLabel5
            // 
            uiLabel5.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            uiLabel5.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel5.Location = new Point(9, 539);
            uiLabel5.Name = "uiLabel5";
            uiLabel5.Size = new Size(100, 34);
            uiLabel5.TabIndex = 22;
            uiLabel5.Text = "开始日期:";
            uiLabel5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel6
            // 
            uiLabel6.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            uiLabel6.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel6.Location = new Point(167, 539);
            uiLabel6.Name = "uiLabel6";
            uiLabel6.Size = new Size(100, 34);
            uiLabel6.TabIndex = 23;
            uiLabel6.Text = "结束日期:";
            uiLabel6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel7
            // 
            uiLabel7.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            uiLabel7.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel7.Location = new Point(325, 539);
            uiLabel7.Name = "uiLabel7";
            uiLabel7.Size = new Size(100, 34);
            uiLabel7.TabIndex = 24;
            uiLabel7.Text = "SN:";
            uiLabel7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiLabel8
            // 
            uiLabel8.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            uiLabel8.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel8.Location = new Point(571, 539);
            uiLabel8.Name = "uiLabel8";
            uiLabel8.Size = new Size(100, 34);
            uiLabel8.TabIndex = 25;
            uiLabel8.Text = "结果:";
            uiLabel8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnExport
            // 
            btnExport.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            btnExport.Location = new Point(811, 553);
            btnExport.MinimumSize = new Size(1, 1);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(76, 54);
            btnExport.TabIndex = 26;
            btnExport.Text = "导出";
            btnExport.TipsFont = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            // 
            // uiLine2
            // 
            uiLine2.BackColor = Color.Transparent;
            uiLine2.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiLine2.ForeColor = Color.FromArgb(48, 48, 48);
            uiLine2.Location = new Point(3, 520);
            uiLine2.MinimumSize = new Size(1, 1);
            uiLine2.Name = "uiLine2";
            uiLine2.Size = new Size(1568, 16);
            uiLine2.TabIndex = 27;
            // 
            // uiLabel9
            // 
            uiLabel9.BorderStyle = BorderStyle.FixedSingle;
            uiLabel9.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabel9.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel9.Location = new Point(6, 976);
            uiLabel9.Name = "uiLabel9";
            uiLabel9.Size = new Size(121, 41);
            uiLabel9.Style = Sunny.UI.UIStyle.Custom;
            uiLabel9.TabIndex = 28;
            uiLabel9.Text = "总测试数:";
            uiLabel9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalCount
            // 
            lblTotalCount.BorderStyle = BorderStyle.FixedSingle;
            lblTotalCount.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            lblTotalCount.ForeColor = Color.Tomato;
            lblTotalCount.Location = new Point(133, 976);
            lblTotalCount.Name = "lblTotalCount";
            lblTotalCount.Size = new Size(111, 41);
            lblTotalCount.Style = Sunny.UI.UIStyle.Custom;
            lblTotalCount.TabIndex = 29;
            lblTotalCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTotalRate
            // 
            lblTotalRate.BorderStyle = BorderStyle.FixedSingle;
            lblTotalRate.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            lblTotalRate.ForeColor = Color.LimeGreen;
            lblTotalRate.Location = new Point(390, 976);
            lblTotalRate.Name = "lblTotalRate";
            lblTotalRate.Size = new Size(97, 41);
            lblTotalRate.Style = Sunny.UI.UIStyle.Custom;
            lblTotalRate.TabIndex = 31;
            lblTotalRate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel11
            // 
            uiLabel11.BorderStyle = BorderStyle.FixedSingle;
            uiLabel11.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            uiLabel11.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel11.Location = new Point(263, 976);
            uiLabel11.Name = "uiLabel11";
            uiLabel11.Size = new Size(121, 41);
            uiLabel11.Style = Sunny.UI.UIStyle.Custom;
            uiLabel11.TabIndex = 30;
            uiLabel11.Text = "总良率:";
            uiLabel11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTodayCount
            // 
            lblTodayCount.BorderStyle = BorderStyle.FixedSingle;
            lblTodayCount.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            lblTodayCount.ForeColor = Color.FromArgb(0, 192, 192);
            lblTodayCount.Location = new Point(910, 976);
            lblTodayCount.Name = "lblTodayCount";
            lblTodayCount.Size = new Size(111, 41);
            lblTodayCount.Style = Sunny.UI.UIStyle.Custom;
            lblTodayCount.TabIndex = 33;
            lblTodayCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel13
            // 
            uiLabel13.BorderStyle = BorderStyle.FixedSingle;
            uiLabel13.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            uiLabel13.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel13.Location = new Point(772, 976);
            uiLabel13.Name = "uiLabel13";
            uiLabel13.Size = new Size(132, 41);
            uiLabel13.Style = Sunny.UI.UIStyle.Custom;
            uiLabel13.TabIndex = 32;
            uiLabel13.Text = "今日测试数:";
            uiLabel13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            btnRefresh.Location = new Point(893, 553);
            btnRefresh.MinimumSize = new Size(1, 1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(76, 54);
            btnRefresh.TabIndex = 35;
            btnRefresh.Text = "刷新";
            btnRefresh.TipsFont = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            // 
            // richTxtLog
            // 
            richTxtLog.BackColor = Color.White;
            richTxtLog.BorderStyle = BorderStyle.FixedSingle;
            richTxtLog.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            richTxtLog.Location = new Point(1578, 72);
            richTxtLog.Name = "richTxtLog";
            richTxtLog.Size = new Size(339, 949);
            richTxtLog.TabIndex = 36;
            richTxtLog.Text = "";
            // 
            // txtSN
            // 
            txtSN.Enabled = false;
            txtSN.Font = new Font("微软雅黑", 21.75F, FontStyle.Bold);
            txtSN.Location = new Point(151, 154);
            txtSN.Margin = new Padding(4, 5, 4, 5);
            txtSN.MinimumSize = new Size(1, 16);
            txtSN.Name = "txtSN";
            txtSN.Padding = new Padding(5);
            txtSN.ShowText = false;
            txtSN.Size = new Size(1420, 75);
            txtSN.TabIndex = 8;
            txtSN.TextAlignment = ContentAlignment.MiddleCenter;
            txtSN.Watermark = "";
            // 
            // lblRes
            // 
            lblRes.BorderStyle = BorderStyle.FixedSingle;
            lblRes.Font = new Font("微软雅黑", 18F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblRes.Location = new Point(151, 330);
            lblRes.Name = "lblRes";
            lblRes.Size = new Size(1420, 75);
            lblRes.TabIndex = 37;
            lblRes.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // timer2
            // 
            timer2.Interval = 50;
            // 
            // txtVehicle
            // 
            txtVehicle.Enabled = false;
            txtVehicle.Font = new Font("微软雅黑", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            txtVehicle.Location = new Point(151, 66);
            txtVehicle.Margin = new Padding(4, 5, 4, 5);
            txtVehicle.MinimumSize = new Size(1, 16);
            txtVehicle.Name = "txtVehicle";
            txtVehicle.Padding = new Padding(5);
            txtVehicle.ShowText = false;
            txtVehicle.Size = new Size(1420, 75);
            txtVehicle.TabIndex = 42;
            txtVehicle.TextAlignment = ContentAlignment.MiddleCenter;
            txtVehicle.Watermark = "";
            // 
            // uiLabel14
            // 
            uiLabel14.BorderStyle = BorderStyle.FixedSingle;
            uiLabel14.Font = new Font("微软雅黑", 18F, FontStyle.Bold);
            uiLabel14.ForeColor = Color.DeepSkyBlue;
            uiLabel14.Location = new Point(8, 66);
            uiLabel14.Name = "uiLabel14";
            uiLabel14.Size = new Size(136, 75);
            uiLabel14.Style = Sunny.UI.UIStyle.Custom;
            uiLabel14.TabIndex = 41;
            uiLabel14.Text = "治具:";
            uiLabel14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // datePickerStart
            // 
            datePickerStart.CanEmpty = true;
            datePickerStart.DateCultureInfo = new System.Globalization.CultureInfo("");
            datePickerStart.FillColor = Color.White;
            datePickerStart.Font = new Font("微软雅黑", 12F);
            datePickerStart.Location = new Point(9, 578);
            datePickerStart.Margin = new Padding(4, 5, 4, 5);
            datePickerStart.MaxLength = 10;
            datePickerStart.MinimumSize = new Size(63, 0);
            datePickerStart.Name = "datePickerStart";
            datePickerStart.Padding = new Padding(0, 0, 30, 2);
            datePickerStart.ShowToday = true;
            datePickerStart.Size = new Size(150, 29);
            datePickerStart.SymbolDropDown = 61555;
            datePickerStart.SymbolNormal = 61555;
            datePickerStart.SymbolSize = 24;
            datePickerStart.TabIndex = 17;
            datePickerStart.Text = "2026-02-02";
            datePickerStart.TextAlignment = ContentAlignment.MiddleCenter;
            datePickerStart.Value = new DateTime(2026, 2, 2, 1, 21, 31, 44);
            datePickerStart.Watermark = "";
            // 
            // uiLabel15
            // 
            uiLabel15.BorderStyle = BorderStyle.FixedSingle;
            uiLabel15.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            uiLabel15.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel15.Location = new Point(1299, 976);
            uiLabel15.Name = "uiLabel15";
            uiLabel15.Size = new Size(132, 41);
            uiLabel15.Style = Sunny.UI.UIStyle.Custom;
            uiLabel15.TabIndex = 43;
            uiLabel15.Text = "今日良率:";
            uiLabel15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel17
            // 
            uiLabel17.Font = new Font("微软雅黑", 18F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabel17.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel17.Location = new Point(493, 976);
            uiLabel17.Name = "uiLabel17";
            uiLabel17.Size = new Size(33, 41);
            uiLabel17.Style = Sunny.UI.UIStyle.Custom;
            uiLabel17.TabIndex = 45;
            uiLabel17.Text = "%";
            uiLabel17.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel16
            // 
            uiLabel16.Font = new Font("微软雅黑", 18F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabel16.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel16.Location = new Point(1540, 976);
            uiLabel16.Name = "uiLabel16";
            uiLabel16.Size = new Size(33, 41);
            uiLabel16.Style = Sunny.UI.UIStyle.Custom;
            uiLabel16.TabIndex = 47;
            uiLabel16.Text = "%";
            uiLabel16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTodayRate
            // 
            lblTodayRate.BorderStyle = BorderStyle.FixedSingle;
            lblTodayRate.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            lblTodayRate.ForeColor = Color.LimeGreen;
            lblTodayRate.Location = new Point(1437, 976);
            lblTodayRate.Name = "lblTodayRate";
            lblTodayRate.Size = new Size(97, 41);
            lblTodayRate.Style = Sunny.UI.UIStyle.Custom;
            lblTodayRate.TabIndex = 46;
            lblTodayRate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTodayFailCount
            // 
            lblTodayFailCount.BorderStyle = BorderStyle.FixedSingle;
            lblTodayFailCount.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            lblTodayFailCount.ForeColor = Color.Red;
            lblTodayFailCount.Location = new Point(1182, 976);
            lblTodayFailCount.Name = "lblTodayFailCount";
            lblTodayFailCount.Size = new Size(111, 41);
            lblTodayFailCount.Style = Sunny.UI.UIStyle.Custom;
            lblTodayFailCount.TabIndex = 49;
            lblTodayFailCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiLabel20
            // 
            uiLabel20.BorderStyle = BorderStyle.FixedSingle;
            uiLabel20.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold);
            uiLabel20.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel20.Location = new Point(1027, 976);
            uiLabel20.Name = "uiLabel20";
            uiLabel20.Size = new Size(149, 41);
            uiLabel20.Style = Sunny.UI.UIStyle.Custom;
            uiLabel20.TabIndex = 48;
            uiLabel20.Text = "今日不良品数:";
            uiLabel20.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ledMes
            // 
            ledMes.Color = Color.White;
            ledMes.Location = new Point(1540, 565);
            ledMes.Name = "ledMes";
            ledMes.Size = new Size(32, 32);
            ledMes.TabIndex = 50;
            ledMes.Text = "MES心跳";
            // 
            // uiLabel12
            // 
            uiLabel12.AutoSize = true;
            uiLabel12.Font = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabel12.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel12.Location = new Point(1485, 572);
            uiLabel12.Name = "uiLabel12";
            uiLabel12.Size = new Size(50, 22);
            uiLabel12.TabIndex = 51;
            uiLabel12.Text = "MES:";
            // 
            // btnScanFrm
            // 
            btnScanFrm.Cursor = Cursors.Hand;
            btnScanFrm.Font = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnScanFrm.Location = new Point(1027, 553);
            btnScanFrm.MinimumSize = new Size(1, 1);
            btnScanFrm.Name = "btnScanFrm";
            btnScanFrm.RectColor = Color.White;
            btnScanFrm.Size = new Size(121, 54);
            btnScanFrm.TabIndex = 52;
            btnScanFrm.Text = "设置";
            btnScanFrm.TipsFont = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            btnScanFrm.Click += btnScanFrm_Click;
            // 
            // uiLabel10
            // 
            uiLabel10.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiLabel10.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel10.Location = new Point(1211, 544);
            uiLabel10.Name = "uiLabel10";
            uiLabel10.Size = new Size(161, 29);
            uiLabel10.TabIndex = 55;
            uiLabel10.Text = "气密仪参数通道:";
            uiLabel10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbCH
            // 
            cmbCH.DataSource = null;
            cmbCH.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            cmbCH.FillColor = Color.White;
            cmbCH.Font = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            cmbCH.ItemHoverColor = Color.FromArgb(155, 200, 255);
            cmbCH.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" });
            cmbCH.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            cmbCH.Location = new Point(1328, 565);
            cmbCH.Margin = new Padding(4, 5, 4, 5);
            cmbCH.MinimumSize = new Size(63, 0);
            cmbCH.Name = "cmbCH";
            cmbCH.Padding = new Padding(0, 0, 30, 2);
            cmbCH.Size = new Size(83, 29);
            cmbCH.SymbolSize = 24;
            cmbCH.TabIndex = 53;
            cmbCH.Text = "0";
            cmbCH.TextAlignment = ContentAlignment.MiddleLeft;
            cmbCH.Watermark = "";
            cmbCH.TextChanged += cmbCH_TextChanged;
            // 
            // uiLabel18
            // 
            uiLabel18.Font = new Font("微软雅黑", 14.25F, FontStyle.Bold);
            uiLabel18.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabel18.Location = new Point(1182, 563);
            uiLabel18.Name = "uiLabel18";
            uiLabel18.Size = new Size(138, 34);
            uiLabel18.TabIndex = 54;
            uiLabel18.Text = "气密参数通道:";
            uiLabel18.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTime
            // 
            txtTime.Enabled = false;
            txtTime.Font = new Font("微软雅黑", 21.75F, FontStyle.Bold);
            txtTime.Location = new Point(152, 418);
            txtTime.Margin = new Padding(4, 5, 4, 5);
            txtTime.MinimumSize = new Size(1, 16);
            txtTime.Name = "txtTime";
            txtTime.Padding = new Padding(5);
            txtTime.ShowText = false;
            txtTime.Size = new Size(1420, 75);
            txtTime.TabIndex = 55;
            txtTime.TextAlignment = ContentAlignment.MiddleCenter;
            txtTime.Watermark = "";
            // 
            // FrmMain
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1920, 1024);
            Controls.Add(txtTime);
            Controls.Add(uiLabel18);
            Controls.Add(cmbCH);
            Controls.Add(btnScanFrm);
            Controls.Add(uiLabel12);
            Controls.Add(ledMes);
            Controls.Add(lblTodayFailCount);
            Controls.Add(uiLabel20);
            Controls.Add(uiLabel16);
            Controls.Add(lblTodayRate);
            Controls.Add(uiLabel17);
            Controls.Add(uiLabel15);
            Controls.Add(txtVehicle);
            Controls.Add(uiLabel14);
            Controls.Add(lblRes);
            Controls.Add(richTxtLog);
            Controls.Add(btnRefresh);
            Controls.Add(lblTodayCount);
            Controls.Add(uiLabel13);
            Controls.Add(lblTotalRate);
            Controls.Add(uiLabel11);
            Controls.Add(lblTotalCount);
            Controls.Add(uiLabel9);
            Controls.Add(uiLine2);
            Controls.Add(btnExport);
            Controls.Add(uiLabel8);
            Controls.Add(uiLabel7);
            Controls.Add(uiLabel6);
            Controls.Add(uiLabel5);
            Controls.Add(btnSearch);
            Controls.Add(cmbResult);
            Controls.Add(txtSearchSN);
            Controls.Add(datePickerEnd);
            Controls.Add(datePickerStart);
            Controls.Add(dataGridView1);
            Controls.Add(txtStation);
            Controls.Add(uiLabel4);
            Controls.Add(uiLabel3);
            Controls.Add(uiLabel2);
            Controls.Add(txtSN);
            Controls.Add(uiLabel1);
            Controls.Add(lblLog);
            Font = new Font("微软雅黑", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FrmMain";
            Padding = new Padding(0, 45, 0, 0);
            Text = "气密测试 - 星驱";
            TitleFont = new Font("微软雅黑", 15F, FontStyle.Bold, GraphicsUnit.Point, 134);
            TitleHeight = 45;
            ZoomScaleRect = new Rectangle(15, 15, 800, 450);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Sunny.UI.UILine uiLine1;
        private Sunny.UI.UIButton btnSetting;
        private Sunny.UI.UIButton btnHistory;
        private Sunny.UI.UILabel lblLog;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UITextBox txtStation;
        private Sunny.UI.UIDataGridView dataGridView1;
        private Sunny.UI.UIDatePicker datePickerEnd;
        private Sunny.UI.UITextBox txtSearchSN;
        private Sunny.UI.UIComboBox cmbResult;
        private Sunny.UI.UIButton btnSearch;
        private Sunny.UI.UILabel uiLabel5;
        private Sunny.UI.UILabel uiLabel6;
        private Sunny.UI.UILabel uiLabel7;
        private Sunny.UI.UILabel uiLabel8;
        private Sunny.UI.UIButton btnExport;
        private Sunny.UI.UILine uiLine2;
        private Sunny.UI.UILabel uiLabel9;
        private Sunny.UI.UILabel lblTotalCount;
        private Sunny.UI.UILabel lblTotalRate;
        private Sunny.UI.UILabel uiLabel11;
        private Sunny.UI.UILabel lblTodayCount;
        private Sunny.UI.UILabel uiLabel13;
        private System.Windows.Forms.Timer timer1;
        private Sunny.UI.UIButton btnRefresh;
        private RichTextBox richTxtLog;
        private Sunny.UI.UITextBox txtSN;
        private Label lblRes;
        private System.Windows.Forms.Timer timer2;
        private Sunny.UI.UITextBox txtVehicle;
        private Sunny.UI.UILabel uiLabel14;
        private Sunny.UI.UIDatePicker datePickerStart;
        private DataGridViewTextBoxColumn CreateTime;
        private DataGridViewTextBoxColumn SN;
        private DataGridViewTextBoxColumn TestMode;
        private DataGridViewTextBoxColumn LeakageRate;
        private DataGridViewTextBoxColumn TestPressure;
        private DataGridViewTextBoxColumn TPUL;
        private DataGridViewTextBoxColumn TPLL;
        private DataGridViewTextBoxColumn Result;
        private Sunny.UI.UILabel uiLabel15;
        private Sunny.UI.UILabel uiLabel17;
        private Sunny.UI.UILabel uiLabel16;
        private Sunny.UI.UILabel lblTodayRate;
        private Sunny.UI.UILabel lblTodayFailCount;
        private Sunny.UI.UILabel uiLabel20;
        private Sunny.UI.UILedBulb ledMes;
        private Sunny.UI.UILabel uiLabel12;
        private Sunny.UI.UIButton btnScanFrm;
        private Sunny.UI.UILabel uiLabel10;
        private Sunny.UI.UIComboBox cmbCH;
        private Sunny.UI.UILabel uiLabel18;
        private Sunny.UI.UITextBox txtTime;
    }
}
