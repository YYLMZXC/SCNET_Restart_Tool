using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SCNET_Restart_Tool.Form.Help
{
    public partial class FunctionDescriptionForm : System.Windows.Forms.Form
    {
        public FunctionDescriptionForm()
        {
            InitializeComponent();
            InitializeHelpContent();
            SetLogoImage();
            CenterToParent();
        }

        private void InitializeHelpContent()
        {
            string helpContent = @"功能说明 

 【重启功能】 
 - 提供SCNET客户端的快速重启功能 
 - 自动检测正在运行的SCNET进程 
 - 优雅关闭当前进程后启动新实例 

 【自动配置】 
 - 自动识别安装路径 
 - 智能检测客户端版本 
 - 支持多版本SCNET客户端 

 【错误处理】 
 - 提供详细的错误日志记录 
 - 异常情况的友好提示 
 - 自动恢复机制 

 【界面说明】 
 - 简洁的操作界面 
 - 一键式操作流程 
 - 实时状态反馈 

 【使用方法】 
 1. 确保SCNET客户端已正确安装 
 2. 点击主界面的“重启”按钮 
 3. 等待重启完成 

 如有任何问题，请联系技术支持。";

            txtHelpContent.Text = helpContent;
            FormatRichTextBox(txtHelpContent);
        }

        private void SetLogoImage()
        {
            try
            {
                // 从嵌入资源加载 logo.ico
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "SCNET_Restart_Tool.Resources.logo.ico"; // 请确认资源路径正确

                using Stream? stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    pictureBoxLogo.Image = new Icon(stream).ToBitmap();
                    return;
                }
            }
            catch
            {
                // 忽略错误，使用默认图标
            }

            // 创建默认Logo
            Bitmap defaultLogo = new Bitmap(48, 48);
            using (Graphics g = Graphics.FromImage(defaultLogo))
            {
                g.Clear(Color.White);
                using (Brush brush = new SolidBrush(Color.FromArgb(64, 64, 64)))
                    g.FillRectangle(brush, 4, 4, 40, 40);

                using (Brush brush = new SolidBrush(Color.White))
                using (Font font = new Font("微软雅黑", 12, FontStyle.Bold))
                    g.DrawString("S", font, brush, 12, 10);
            }
            pictureBoxLogo.Image = defaultLogo;
        }

        private void FormatRichTextBox(RichTextBox rtb)
        {
            rtb.Font = new Font("微软雅黑", 9);
            rtb.ReadOnly = true;
            rtb.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtb.BackColor = Color.White;
            rtb.ForeColor = Color.Black;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtHelpContent_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            OpenLinkInBrowser(e.LinkText);
        }

        private void OpenLinkInBrowser(string url)
        {
            try
            {
                // 新式写法，兼容 .NET 6–9
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开链接: " + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
