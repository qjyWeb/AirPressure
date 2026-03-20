namespace AirPressure.UI
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            txtuser = new Sunny.UI.UIComboBox();
            Lbluser = new Sunny.UI.UILabel();
            LblPsd = new Sunny.UI.UILabel();
            txtpassword = new Sunny.UI.UITextBox();
            uiButton1 = new Sunny.UI.UIButton();
            uiButton2 = new Sunny.UI.UIButton();
            SuspendLayout();
            // 
            // txtuser
            // 
            txtuser.DataSource = null;
            txtuser.FillColor = Color.White;
            txtuser.Font = new Font("宋体", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtuser.ItemHoverColor = Color.FromArgb(155, 200, 255);
            txtuser.Items.AddRange(new object[] { "超级管理员", "操作工程师" });
            txtuser.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            txtuser.Location = new Point(395, 153);
            txtuser.Margin = new Padding(4, 5, 4, 5);
            txtuser.MinimumSize = new Size(63, 0);
            txtuser.Name = "txtuser";
            txtuser.Padding = new Padding(0, 0, 30, 2);
            txtuser.Size = new Size(216, 37);
            txtuser.SymbolSize = 24;
            txtuser.TabIndex = 1;
            txtuser.TextAlignment = ContentAlignment.MiddleLeft;
            txtuser.Watermark = "";
            // 
            // Lbluser
            // 
            Lbluser.AutoSize = true;
            Lbluser.Font = new Font("宋体", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            Lbluser.ForeColor = Color.FromArgb(48, 48, 48);
            Lbluser.Location = new Point(243, 161);
            Lbluser.Name = "Lbluser";
            Lbluser.Size = new Size(66, 21);
            Lbluser.TabIndex = 2;
            Lbluser.Text = "用 户";
            // 
            // LblPsd
            // 
            LblPsd.AutoSize = true;
            LblPsd.Font = new Font("宋体", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            LblPsd.ForeColor = Color.FromArgb(48, 48, 48);
            LblPsd.Location = new Point(243, 239);
            LblPsd.Name = "LblPsd";
            LblPsd.Size = new Size(66, 21);
            LblPsd.TabIndex = 2;
            LblPsd.Text = "密 码";
            // 
            // txtpassword
            // 
            txtpassword.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            txtpassword.Location = new Point(395, 238);
            txtpassword.Margin = new Padding(4, 5, 4, 5);
            txtpassword.MinimumSize = new Size(1, 16);
            txtpassword.Name = "txtpassword";
            txtpassword.Padding = new Padding(5);
            txtpassword.PasswordChar = '*';
            txtpassword.ShowText = false;
            txtpassword.Size = new Size(216, 37);
            txtpassword.TabIndex = 3;
            txtpassword.TextAlignment = ContentAlignment.MiddleLeft;
            txtpassword.Watermark = "";
            txtpassword.KeyDown += txtpassword_KeyDown;
            // 
            // uiButton1
            // 
            uiButton1.FillColor = Color.Khaki;
            uiButton1.Font = new Font("宋体", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton1.ForeColor = Color.Black;
            uiButton1.Location = new Point(197, 380);
            uiButton1.MinimumSize = new Size(1, 1);
            uiButton1.Name = "uiButton1";
            uiButton1.Size = new Size(152, 54);
            uiButton1.TabIndex = 4;
            uiButton1.Text = "注 册";
            uiButton1.TipsFont = new Font("宋体", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            uiButton1.Click += uiButton1_Click;
            // 
            // uiButton2
            // 
            uiButton2.FillColor = Color.Khaki;
            uiButton2.Font = new Font("宋体", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton2.ForeColor = Color.Black;
            uiButton2.Location = new Point(519, 380);
            uiButton2.MinimumSize = new Size(1, 1);
            uiButton2.Name = "uiButton2";
            uiButton2.Size = new Size(152, 54);
            uiButton2.TabIndex = 4;
            uiButton2.Text = "登 录";
            uiButton2.TipsFont = new Font("宋体", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            uiButton2.Click += uiButton2_Click;
            // 
            // FrmLogin
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(913, 491);
            Controls.Add(uiButton2);
            Controls.Add(uiButton1);
            Controls.Add(txtpassword);
            Controls.Add(LblPsd);
            Controls.Add(Lbluser);
            Controls.Add(txtuser);
            Name = "FrmLogin";
            Text = "登录页面";
            ZoomScaleRect = new Rectangle(15, 15, 729, 435);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Sunny.UI.UIComboBox txtuser;
        private Sunny.UI.UILabel Lbluser;
        private Sunny.UI.UILabel LblPsd;
        private Sunny.UI.UITextBox txtpassword;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIButton uiButton2;
    }
}