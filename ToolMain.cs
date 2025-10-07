using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public partial class ToolMain : Form
    {
        private List<ServerConfig> _servers = new List<ServerConfig>();
        private Dictionary<int, ServerMonitor> _monitors = new Dictionary<int, ServerMonitor>();
        private ServerConfig _selectedServer;
        private LogManager _logManager;
        // 主窗口加载时加载应用设置
        private SettingsModel _appSettings;
        public ToolMain()
        {
            InitializeComponent();
            _logManager = LogManager.GetInstance();
            _logManager.OnLogAdded += LogManager_OnLogAdded;
            // 加载应用设置
            _appSettings = SettingsManager.LoadSettings();
            // 应用默认图标（如果有自定义图标则加载）
            if (!string.IsNullOrEmpty(_appSettings.CustomIconPath))
            {
                try
                {
                    this.Icon = new Icon(_appSettings.CustomIconPath);
                }
                catch
                {
                    _logManager.AddLog("系统", "自定义图标加载失败，使用默认图标", "警告");
                }
            }
            InitializeUI();
            LoadServerConfigs();
        }

        // 设置按钮点击事件
        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(_appSettings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // 更新主窗口的设置引用
                    _appSettings = settingsForm.AppSettings;
                    // 记录设置变更日志
                    _logManager.AddLog("系统", "应用设置已更新", "信息");
                }
            }
        }

        /// 初始化UI控件
        private void InitializeUI()
        {
            // 服务端列表配置
            dgvServers.AutoGenerateColumns = false;
            dgvServers.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    Name = "Name",
                    HeaderText = "服务端名称",
                    DataPropertyName = "Name",
                    Width = 120
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Status",
                    HeaderText = "状态",
                    DataPropertyName = "Status",
                    Width = 80
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "IpPort",
                    HeaderText = "IP:端口",
                    Width = 100
                }
            });

            // 日志表格配置
            dgvLogs.AutoGenerateColumns = false;
            dgvLogs.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    Name = "Time",
                    HeaderText = "时间",
                    DataPropertyName = "Time",
                    Width = 150
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "ServerName",
                    HeaderText = "服务端",
                    DataPropertyName = "ServerName",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Content",
                    HeaderText = "内容",
                    DataPropertyName = "Content",
                    Width = 350
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Level",
                    HeaderText = "级别",
                    DataPropertyName = "Level",
                    Width = 80
                }
            });

            dgvLogs.DataSource = new BindingSource(_logManager.GetAllLogs(), null);

            // 初始化指令控件状态
            UpdateCommandControlsState();
        }

        /// 日志更新事件（实时刷新UI）
        private void LogManager_OnLogAdded(LogItem log)
        {
            if (dgvLogs.InvokeRequired)
            {
                dgvLogs.Invoke(new Action<LogItem>(LogManager_OnLogAdded), log);
                return;
            }

            ((BindingSource)dgvLogs.DataSource).ResetBindings(false);
            if (dgvLogs.Rows.Count > 0)
            {
                dgvLogs.FirstDisplayedScrollingRowIndex = dgvLogs.Rows.Count - 1;
            }
        }

        /// 加载服务端配置
        private void LoadServerConfigs()
        {
            _servers = ServerConfigManager.LoadAll();
            dgvServers.DataSource = new BindingSource(_servers, null);
            InitializeMonitors();
            _logManager.AddLog("系统", "程序启动，加载服务端配置完成");
        }

        /// 初始化所有服务端监控器
        private void InitializeMonitors()
        {
            _monitors.Clear();
            foreach (var server in _servers)
            {
                var monitor = new ServerMonitor(server, OnServerStatusChanged);
                _monitors.Add(server.Id, monitor);
                if (server.IsMonitoring)
                {
                    monitor.Start();
                }
            }
        }

        /// 服务端状态变更回调
        private void OnServerStatusChanged(ServerConfig server)
        {
            if (dgvServers.InvokeRequired)
            {
                dgvServers.Invoke(new Action<ServerConfig>(OnServerStatusChanged), server);
                return;
            }

            dgvServers.Refresh();
            if (_selectedServer != null && _selectedServer.Id == server.Id)
            {
                LoadServerToDetails(server);
            }
        }

        /// 服务端列表选择变更
        private void DgvServers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvServers.SelectedRows.Count == 0)
            {
                _selectedServer = null;
                UpdateCommandControlsState(); // 未选中时更新状态
                return;
            }

            _selectedServer = dgvServers.SelectedRows[0].DataBoundItem as ServerConfig;
            if (_selectedServer != null)
            {
                LoadServerToDetails(_selectedServer);
            }
        }

        /// 加载服务端详情到表单
        private void LoadServerToDetails(ServerConfig server)
        {
            txtServerName.Text = server.Name;
            txtExePath.Text = server.ExePath;
            txtIp.Text = server.Ip;
            txtPort.Text = server.Port.ToString();
            txtPassword.Text = server.Password;
            txtScheduleTime.Text = server.ScheduleTime;
            txtIntervalHours.Text = server.IntervalHours.ToString();
            btnStartMonitor.Text = server.IsMonitoring ? "关闭监控" : "开启监控";
            btnStartMonitor.BackColor = server.IsMonitoring ? System.Drawing.Color.Red : System.Drawing.Color.LimeGreen;

            // 更新指令控件状态
            UpdateCommandControlsState();
        }

        /// 添加服务端按钮
        private void BtnAddServer_Click(object sender, EventArgs e)
        {
            var newServer = new ServerConfig
            {
                Id = ServerConfigManager.GenerateNewId(_servers),
                Name = $"服务端{_servers.Count + 1}",
                Ip = "127.0.0.1",
                Port = 25565
            };

            _servers.Add(newServer);
            _monitors.Add(newServer.Id, new ServerMonitor(newServer, OnServerStatusChanged));
            ServerConfigManager.SaveAll(_servers);
            dgvServers.DataSource = new BindingSource(_servers, null);
            _logManager.AddLog("系统", $"新增服务端：{newServer.Name}");
        }

        /// 删除服务端按钮
        private void BtnDeleteServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null)
            {
                MessageBox.Show("请先选中要删除的服务端");
                return;
            }

            var serverName = _selectedServer.Name;
            _monitors[_selectedServer.Id].Stop();
            _monitors.Remove(_selectedServer.Id);
            _servers.Remove(_selectedServer);
            ServerConfigManager.SaveAll(_servers);
            dgvServers.DataSource = new BindingSource(_servers, null);
            _selectedServer = null;
            _logManager.AddLog("系统", $"删除服务端：{serverName}");
        }

        /// 批量重启按钮
        private void BtnBatchRestart_Click(object sender, EventArgs e)
        {
            _logManager.AddLog("系统", "开始批量重启所有服务端");
            foreach (var server in _servers)
            {
                _monitors[server.Id].KillProcess();
                System.Threading.Thread.Sleep(1000);
                _monitors[server.Id].StartProcess();
            }
            _logManager.AddLog("系统", "批量重启完成");
            MessageBox.Show("批量重启完成");
        }

        /// 选择程序路径按钮
        private void BtnSelectExe_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            using (var ofd = new OpenFileDialog { Filter = "可执行文件|*.exe" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedServer.ExePath = ofd.FileName;
                    txtExePath.Text = ofd.FileName;
                    _logManager.AddLog(_selectedServer.Name, $"已选择程序路径：{ofd.FileName}");
                }
            }
        }

        /// 保存设置按钮
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            _selectedServer.Name = txtServerName.Text;
            _selectedServer.Ip = txtIp.Text;
            _selectedServer.Password = txtPassword.Text;
            _selectedServer.ScheduleTime = txtScheduleTime.Text;

            if (int.TryParse(txtPort.Text, out int port))
                _selectedServer.Port = port;

            if (double.TryParse(txtIntervalHours.Text, out double interval))
                _selectedServer.IntervalHours = interval;

            ServerConfigManager.SaveAll(_servers);
            _logManager.AddLog(_selectedServer.Name, "服务端设置已保存");
            MessageBox.Show("设置已保存");
        }

        /// 开启/关闭监控按钮
        private void BtnStartMonitor_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            if (_selectedServer.IsMonitoring)
            {
                _monitors[_selectedServer.Id].Stop();
                // 强制同步状态（防止状态不一致）
                _selectedServer.IsMonitoring = false;
            }
            else
            {
                _monitors[_selectedServer.Id].Start();
                // 强制同步状态
                _selectedServer.IsMonitoring = true;
            }

            // 更新按钮显示
            btnStartMonitor.Text = _selectedServer.IsMonitoring ? "关闭监控" : "开启监控";
            btnStartMonitor.BackColor = _selectedServer.IsMonitoring ? System.Drawing.Color.Red : System.Drawing.Color.LimeGreen;
            ServerConfigManager.SaveAll(_servers);
        }

        /// 手动关闭当前服务端按钮
        private void BtnStopServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null)
            {
                MessageBox.Show("请先选中要关闭的服务端");
                return;
            }

            _monitors[_selectedServer.Id].KillProcess();
            _logManager.AddLog("系统", $"执行关闭当前服务端：{_selectedServer.Name}");
        }

        /// 手动启动当前服务端按钮
        private void BtnStartServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null)
            {
                MessageBox.Show("请先选中服务端");
                return;
            }

            if (string.IsNullOrEmpty(_selectedServer.ExePath))
            {
                MessageBox.Show("请先选择服务端程序路径");
                return;
            }

            _monitors[_selectedServer.Id].StartProcess();
        }

        /// 关闭所有服务端按钮
        private void BtnStopAllServers_Click(object sender, EventArgs e)
        {
            if (_servers.Count == 0)
            {
                MessageBox.Show("没有可关闭的服务端");
                return;
            }

            if (MessageBox.Show($"确定要关闭所有{_servers.Count}个服务端吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _logManager.AddLog("系统", "开始关闭所有服务端");
                int successCount = 0;

                foreach (var server in _servers)
                {
                    var monitor = _monitors[server.Id];
                    // 先检查是否运行
                    if (monitor.IsProcessRunning())
                    {
                        monitor.KillProcess();
                        // 等待关闭
                        System.Threading.Thread.Sleep(1000);
                        if (!monitor.IsProcessRunning())
                            successCount++;
                    }
                    else
                    {
                        _logManager.AddLog(server.Name, "服务端未在运行，跳过关闭");
                    }
                }

                _logManager.AddLog("系统", $"所有服务端关闭操作完成（成功关闭：{successCount}/{_servers.Count}）");
                MessageBox.Show($"关闭完成，成功关闭 {successCount}/{_servers.Count} 个服务端");
            }
        }

        /// 发送指令按钮
        private void BtnSendCommand_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            var command = txtCommand.Text.Trim();
            if (string.IsNullOrEmpty(command))
            {
                lblCommandStatus.Text = "指令不能为空";
                return;
            }

            var result = _monitors[_selectedServer.Id].SendServiceCommand(command);
            lblCommandStatus.Text = $"响应：{result}";
        }

        /// 清空日志按钮
        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清空所有日志吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _logManager.ClearLogs();
                ((BindingSource)dgvLogs.DataSource).ResetBindings(false);
                _logManager.AddLog("系统", "日志已清空");
            }
        }

        /// 服务端列表单元格格式化（显示IP:端口）
        private void dgvServers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                var server = dgvServers.Rows[e.RowIndex].DataBoundItem as ServerConfig;
                if (server != null)
                {
                    e.Value = $"{server.Ip}:{server.Port}";
                }
            }
        }

        /// 指令启用/禁用切换按钮
        private void BtnToggleCommands_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null)
            {
                MessageBox.Show("请先选中服务端");
                return;
            }

            // 切换状态
            _selectedServer.EnableCommands = !_selectedServer.EnableCommands;
            // 更新按钮显示
            UpdateCommandControlsState();
            // 保存配置
            ServerConfigManager.SaveAll(_servers);
            // 记录日志
            _logManager.AddLog(_selectedServer.Name,
                _selectedServer.EnableCommands ? "已启用指令功能" : "已禁用指令功能");
        }

        /// 更新指令相关控件状态（启用/禁用）
        private void UpdateCommandControlsState()
        {
            if (_selectedServer == null)
            {
                // 未选中服务端时禁用所有控件
                txtCommand.Enabled = false;
                btnSendCommand.Enabled = false;
                btnToggleCommands.Enabled = false;
                btnToggleCommands.Text = "启用指令";
                btnToggleCommands.BackColor = System.Drawing.Color.Gray;
            }
            else
            {
                // 根据配置启用/禁用控件
                txtCommand.Enabled = _selectedServer.EnableCommands;
                btnSendCommand.Enabled = _selectedServer.EnableCommands;
                btnToggleCommands.Enabled = true;

                // 更新按钮文本和颜色
                if (_selectedServer.EnableCommands)
                {
                    btnToggleCommands.Text = "禁用指令";
                    btnToggleCommands.BackColor = System.Drawing.Color.Orange;
                }
                else
                {
                    btnToggleCommands.Text = "启用指令";
                    btnToggleCommands.BackColor = System.Drawing.Color.Gray;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 检查是否以管理员权限运行
            if (!IsRunningAsAdmin())
            {
                _logManager.AddLog("系统", "警告：程序未以管理员权限运行，可能导致部分功能（如进程管理）失效", "警告");
            }
        }

        /// 检查是否以管理员权限运行
        private bool IsRunningAsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}