using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace SCNET_Restart_Tool.Form.Help
{
    public partial class VersionInfoForm : System.Windows.Forms.Form
    {
        public VersionInfoForm()
        {
            InitializeComponent();
            InitializeHelpContent();
            SetLogoImage();
            this.CenterToParent();
        }

        private void InitializeHelpContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== SCNET_Restart_Tool 版本信息 ===\n");
            sb.AppendLine("【当前版本】 v1.0.0");
            sb.AppendLine("【发布日期】 2023年10月15日");
            sb.AppendLine("【开发团队】 系统维护组");
            sb.AppendLine("【联系方式】 support@example.com\n");
            
            sb.AppendLine("=== 版本更新日志 ===\n");
            sb.AppendLine("【v1.0.0】");
            sb.AppendLine("- 初始版本发布");
            sb.AppendLine("- 支持SCNET服务一键重启功能");
            sb.AppendLine("- 自动配置服务参数");
            sb.AppendLine("- 实时监控服务状态");
            sb.AppendLine("- 详细日志记录功能");
            sb.AppendLine("- 支持常见问题解决方案查询\n");
            
            sb.AppendLine("=== 系统要求 ===\n");
            sb.AppendLine("- 操作系统: Windows 7/8/10/11");
            sb.AppendLine("- .NET Framework 4.5 或更高版本");
            sb.AppendLine("- 内存: 2GB 或更高");
            sb.AppendLine("- 磁盘空间: 50MB 可用空间");
            sb.AppendLine("- 管理员权限");

            txtVersionInfo.Text = sb.ToString();
            FormatRichTextBox(txtVersionInfo);
        }

        private void FormatRichTextBox(RichTextBox richTextBox)
        {
            richTextBox.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            richTextBox.SelectionStart = 0;
            richTextBox.SelectionLength = 0;
            richTextBox.ReadOnly = true;
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
        }

        private void SetLogoImage()
        {
            try
            {
                // 尝试从资源文件加载Logo
                // 如果没有资源文件，创建一个简单的Logo
                Bitmap logo = new Bitmap(48, 48);
                using (Graphics g = Graphics.FromImage(logo))
                {
                    g.Clear(Color.LightBlue);
                    using (Brush brush = new SolidBrush(Color.Blue))
                    {   
                        g.DrawString("SCNET", new Font("微软雅黑", 12F, FontStyle.Bold), brush, 2, 10);
                        g.DrawString("重启", new Font("微软雅黑", 9F, FontStyle.Bold), brush, 8, 26);
                    }
                }
                pictureBoxLogo.Image = logo;
            }
            catch (Exception ex)
            {
                // 如果出现错误，使用默认的简单Logo
                Bitmap defaultLogo = new Bitmap(48, 48);
                using (Graphics g = Graphics.FromImage(defaultLogo))
                {
                    g.Clear(Color.LightGray);
                    using (Brush brush = new SolidBrush(Color.DarkGray))
                    {   
                        g.DrawString("S", new Font("微软雅黑", 16F, FontStyle.Bold), brush, 10, 10);
                    }
                }
                pictureBoxLogo.Image = defaultLogo;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}