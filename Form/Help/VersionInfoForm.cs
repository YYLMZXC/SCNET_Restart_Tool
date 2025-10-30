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
                // 使用 using 确保 Bitmap 资源释放
                using (Bitmap logo = new Bitmap(48, 48))
                {
                    using (Graphics g = Graphics.FromImage(logo))
                    {
                        g.Clear(Color.LightBlue);
                        // Font 也需释放，嵌套 using
                        using (Font font1 = new Font("微软雅黑", 12F, FontStyle.Bold))
                        using (Font font2 = new Font("微软雅黑", 9F, FontStyle.Bold))
                        using (Brush brush = new SolidBrush(Color.Blue))
                        {
                            g.DrawString("SCNET", font1, brush, 2, 10);
                            g.DrawString("重启", font2, brush, 8, 26);
                        }
                    }
                    // 克隆图像到 pictureBox，避免原 Bitmap 释放后图像失效
                    pictureBoxLogo.Image?.Dispose(); // 释放旧图像
                    pictureBoxLogo.Image = (Bitmap)logo.Clone();
                }
            }
            catch (Exception)
            {
                using (Bitmap defaultLogo = new Bitmap(48, 48))
                {
                    using (Graphics g = Graphics.FromImage(defaultLogo))
                    {
                        g.Clear(Color.LightGray);
                        using (Font font = new Font("微软雅黑", 16F, FontStyle.Bold))
                        using (Brush brush = new SolidBrush(Color.DarkGray))
                        {
                            g.DrawString("S", font, brush, 10, 10);
                        }
                    }
                    pictureBoxLogo.Image?.Dispose(); // 释放旧图像
                    pictureBoxLogo.Image = (Bitmap)defaultLogo.Clone();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}