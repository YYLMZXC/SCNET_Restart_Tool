using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SCNET_Restart_Tool.Form.Help
{
    public partial class ProjectInfoForm : System.Windows.Forms.Form
    {
        public ProjectInfoForm()
        {
            InitializeComponent();
            InitializeHelpContent();
            SetLogoImage();
            CenterToParent();
        }

        private void InitializeHelpContent()
        {
            string projectInfo = $@"项目信息

项目名称：SCNET_Restart_Tool

项目描述：
SCNET客户端重启工具，用于快速重启SCNET客户端，提供自动化的重启流程和错误处理机制。

项目特点：
- 简单易用的操作界面
- 快速的重启响应
- 稳定的错误处理
- 智能的路径识别

开发环境：
- .NET Framework 4.7.2
- C# 8.0
- Windows Forms

适用环境：
- Windows 7 SP1 及以上版本
- 支持32位和64位系统
- 需要管理员权限运行

注意事项：
- 请确保正确安装了SCNET客户端
- 程序需要访问系统进程和文件系统权限
- 如有任何问题，请联系开发团队

本工具仅用于辅助SCNET客户端的管理和维护。";

            txtProjectInfo.Text = projectInfo;
            FormatRichTextBox(txtProjectInfo);
        }

        private void SetLogoImage()
        {
            try
            {
                // 尝试从资源中加载Logo
                Assembly assembly = Assembly.GetExecutingAssembly();
                pictureBoxLogo.Image = Image.FromStream(assembly.GetManifestResourceStream("SCNET_Restart_Tool.Resources.logo.ico"));
            }
            catch
            {
                // 如果加载失败，创建默认Logo
                Bitmap defaultLogo = new Bitmap(48, 48);
                using (Graphics g = Graphics.FromImage(defaultLogo))
                {
                    g.Clear(Color.White);
                    using (Brush brush = new SolidBrush(Color.FromArgb(64, 64, 64))) {
                        g.FillRectangle(brush, 4, 4, 40, 40);
                    }
                    using (Brush brush = new SolidBrush(Color.White))
                    using (Font font = new Font("微软雅黑", 12, FontStyle.Bold))
                    {
                        g.DrawString("S", font, brush, 16, 12);
                    }
                }
                pictureBoxLogo.Image = defaultLogo;
            }
        }

        private void FormatRichTextBox(RichTextBox rtb)
        {
            rtb.Font = new Font("微软雅黑", 9);
            rtb.SelectionStart = 0;
            rtb.SelectionLength = 0;
            rtb.SelectAll();
            rtb.SelectionColor = Color.Black;
            rtb.SelectionLength = 0;
            rtb.ReadOnly = true;
            rtb.ScrollBars = RichTextBoxScrollBars.Vertical;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}