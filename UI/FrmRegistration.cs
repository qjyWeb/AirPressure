using Sunny.UI;

namespace AirPressure.UI
{
    public partial class FrmRegistration : UIForm
    {
        public FrmRegistration()
        {
            SetStyle(UIStyle.Orange);
            InitializeComponent();
        }

        private async void FrmRegistration_Load(object sender, EventArgs e)
        {
            await Task.Delay(50);
            uiTextBox1.Focus();
        }

        /// <summary>
        /// 确认修改按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            FrmLogin loginForm = new FrmLogin();
            loginForm.FormClosed += (s, args) => Application.Exit(); // 登录窗体关闭时退出应用
            loginForm.Show();
        }
    }
}
