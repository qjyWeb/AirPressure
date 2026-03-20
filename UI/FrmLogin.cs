using Sunny.UI;

namespace AirPressure.UI
{
    public partial class FrmLogin : UIForm
    {
        public FrmLogin()
        {
            SetStyle(UIStyle.Orange);
            InitializeComponent();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            FrmRegistration frmReg = new FrmRegistration();
            frmReg.FormClosed += (s, args) => Application.Exit(); // 注册窗体关闭时退出应用
            frmReg.Show();
            this.Close();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (txtpassword.Text == "onlycell")//暂时密码，后续可以改为从配置文件读取或者数据库验证
            {
                FrmMain frm = new FrmMain();
                frm.FormClosed += (s, args) => Application.Exit(); // 主窗体关闭时退出应用
                frm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 密码框按回车触发登录
        private void txtpassword_KeyDown(object sender, KeyEventArgs e)
        {
            // 判断是否按下回车键
            if (e.KeyCode == Keys.Enter)
            {
                // 阻止回车键的默认行为（避免光标跳走）
                e.SuppressKeyPress = true;
                // 直接调用登录按钮的点击逻辑
                uiButton2_Click(sender, e);
            }
        }

    }
}
