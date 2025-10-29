using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace SCNET_Restart_Tool.Form.Help
{
    public partial class HelpForm : System.Windows.Forms.Form
    {
        public HelpForm()
        {
            InitializeComponent();
            SetLogoImage();
            CenterToParent();
        }

        /// <summary>
        /// 功能说明按钮点击事件
        /// </summary>
        private void btnFunctionDescription_Click(object sender, EventArgs e)
        {
            using (FunctionDescriptionForm functionForm = new FunctionDescriptionForm())
            {
                functionForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// 项目信息按钮点击事件
        /// </summary>
        private void btnProjectInfo_Click(object sender, EventArgs e)
        {
            using (ProjectInfoForm projectForm = new ProjectInfoForm())
            {
                projectForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// 版本信息按钮点击事件
        /// </summary>
        private void btnVersionInfo_Click(object sender, EventArgs e)
        {
            using (VersionInfoForm versionForm = new VersionInfoForm())
            {
                versionForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// 设置Logo图片
        /// </summary>
        private void SetLogoImage()
        {
            try
            {
                // 尝试加载应用程序目录中的图标文件
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ico.ico");
                if (File.Exists(iconPath))
                {
                    pictureBoxLogo.Image = new Icon(iconPath).ToBitmap();
                }
                else
                {
                    // 尝试从Res文件夹加载
                    string resIconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Res", "ico.ico");
                    if (File.Exists(resIconPath))
                    {
                        pictureBoxLogo.Image = new Icon(resIconPath).ToBitmap();
                    }
                    else
                    {
                        // 如果没有找到图标，使用默认图像
                        using (Bitmap defaultLogo = new Bitmap(48, 48))
                        {
                            using (Graphics g = Graphics.FromImage(defaultLogo))
                            {
                                // 绘制一个简单的默认图标
                                g.Clear(Color.FromArgb(64, 64, 64));
                                using (Font font = new Font("Arial", 16, FontStyle.Bold))
                                {
                                    using (Brush brush = new SolidBrush(Color.White))
                                    {
                                        g.DrawString("S", font, brush, 12, 8);
                                    }
                                }
                            }
                            pictureBoxLogo.Image = defaultLogo.Clone() as Image;
                        }
                    }
                }
            }
            catch
            {
                // 如果出现错误，忽略并继续使用空的Logo
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}