using System;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public partial class ServerEditForm : Form
    {
        public ServerConfig Server { get; private set; }

        public ServerEditForm(ServerConfig server)
        {
            InitializeComponent();
            Server = server ?? new ServerConfig();
            LoadServerData();
        }

        private void LoadServerData()
        {
            txtServerName.Text = Server.Name;
            txtExePath.Text = Server.ExePath;
            txtIp.Text = Server.Ip;
            nudPort.Value = Server.Port;
            txtPassword.Text = Server.Password;
            txtScheduleTime.Text = Server.ScheduleTime;
            nudIntervalHours.Value = (decimal)Server.IntervalHours;
            chkEnableCommands.Checked = Server.EnableCommands;
        }

        private void btnBrowseExe_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Filter = "可执行文件|*.exe|所有文件|*.*",
                Title = "选择服务端程序"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtExePath.Text = ofd.FileName;
                    if (string.IsNullOrEmpty(txtServerName.Text))
                    {
                        // 自动填充服务端名称（取文件名）
                        txtServerName.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServerName.Text.Trim()))
            {
                MessageBox.Show("请输入服务端名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtExePath.Text.Trim()) || !File.Exists(txtExePath.Text.Trim()))
            {
                var result = MessageBox.Show(
                    "服务端程序路径无效，是否继续？",
                    "路径无效",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            // 保存数据
            Server.Name = txtServerName.Text.Trim();
            Server.ExePath = txtExePath.Text.Trim();
            Server.ExeName = Path.GetFileName(Server.ExePath);
            Server.Ip = txtIp.Text.Trim();
            Server.Port = (int)nudPort.Value;
            Server.Password = txtPassword.Text;
            Server.ScheduleTime = txtScheduleTime.Text.Trim();
            Server.IntervalHours = (double)nudIntervalHours.Value;
            Server.EnableCommands = chkEnableCommands.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}