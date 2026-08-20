using System;
using System.Windows.Forms;

namespace SCNET_Restart_Tool.Form.Help
{
    public partial class AboutForm : System.Windows.Forms.Form
    {
        public AboutForm()
        {
            InitializeComponent();
            // 直接设置地址信息，移除所有多余功能
            txtAboutContent.Text = "项目地址：https://gitee.com/sc-net/SCNET_Restart_Tool";
            txtAboutContent.ReadOnly = true;
            this.CenterToParent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
