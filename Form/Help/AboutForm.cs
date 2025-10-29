using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace SCNET_Restart_Tool.Form.Help
{
    public partial class AboutForm : System.Windows.Forms.Form
    {
        public AboutForm()
        {
            InitializeComponent();
            InitializeAboutContent();
            SetLogoImage();
            this.CenterToParent();
        }

        private void InitializeAboutContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== 关于 SCNET_Restart_Tool ===\n");
            sb.AppendLine("SCNET_Restart_Tool 是一款专为系统管理员设计的工具，用于快速重启和管理 SCNET 服务。");
            sb.AppendLine("该工具可以简化服务管理流程，提高系统维护效率，减少人工操作错误。\n");
            
            sb.AppendLine("【主要特点】");
            sb.AppendLine("• 简单直观的用户界面");
            sb.AppendLine("• 一键重启 SCNET 服务");
            sb.AppendLine("• 自动检测和修复常见问题");
            sb.AppendLine("• 详细的操作日志记录");
            sb.AppendLine("• 支持批量操作和定时任务\n");
            
            sb.AppendLine("【项目地址】");
            sb.AppendLine("https://gitee.com/sc-net/SCNET_Restart_Tool");
            sb.AppendLine("欢迎访问项目仓库获取最新版本和提交问题反馈！\n");
            
            sb.AppendLine("【版权信息】");
            sb.AppendLine("© 2023 系统维护组 保留所有权利");
            sb.AppendLine("本软件仅供内部使用，未经授权不得传播或用于商业用途。");

            txtAboutContent.Text = sb.ToString();
            FormatRichTextBox(txtAboutContent);
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
                // 创建一个简单的Logo
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
            catch (Exception)
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
