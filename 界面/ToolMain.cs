using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SCNET_Restart_Tool
{
    public partial class ToolMain : Form
    {
        // 核心数据与状态
        private List<ServerConfig> _serverConfigs = new List<ServerConfig>();
        private ServerConfig _selectedServer;
        private bool _isEditing = false;
        private bool _isAdding = false;
        private FolderManagerForm _folderManagerForm;
        private readonly LogManager _logManager = LogManager.GetInstance();
        private Dictionary<int, ServerMonitor> _serverMonitors = new Dictionary<int, ServerMonitor>();

        public ToolMain()
        {
            InitializeComponent();
            InitializeUI();
            LoadServerData();
            BindEvents();
        }

        #region 初始化与基础设置
        // 初始化界面组件
        private void InitializeUI()
        {
            // 配置服务器表格
            ConfigureDataGridView();

            // 初始化日志系统
            logTextBox.ReadOnly = true;
            _logManager.LogAdded += (log) =>
            {
                if (logTextBox.InvokeRequired)
                {
                    logTextBox.Invoke(new Action(() => AddLogEntry(log)));
                }
                else
                {
                    AddLogEntry(log);
                }
            };

            // 初始禁用编辑控件
            SetEditControlsEnabled(false);
        }

        // 配置数据表格
        private void ConfigureDataGridView()
        {
            serversGridView.AutoGenerateColumns = false;
            serversGridView.MultiSelect = false;
            serversGridView.BackgroundColor = Color.White;
            serversGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            serversGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 230, 242);
            serversGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

            // 添加列定义
            serversGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", HeaderText = "ID", Width = 50 },
                new DataGridViewTextBoxColumn { Name = "Name", DataPropertyName = "Name", HeaderText = "服务器名称", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "IpPort", HeaderText = "IP:端口", Width = 120 },
                new DataGridViewTextBoxColumn { Name = "ExePath", DataPropertyName = "ExePath", HeaderText = "程序路径", Width = 400 },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "状态", Width = 100 }
            });
        }

        // 加载服务器数据
        private void LoadServerData()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerConfigs.xml");
                if (File.Exists(configPath))
                {
                    using (var fs = new FileStream(configPath, FileMode.Open))
                    {
                        var serializer = new XmlSerializer(typeof(List<ServerConfig>));
                        _serverConfigs = (List<ServerConfig>)serializer.Deserialize(fs);
                    }
                }
                else
                {
                    _serverConfigs = new List<ServerConfig>();
                }

                // 为每个服务器创建监控器
                foreach (var server in _serverConfigs)
                {
                    CreateServerMonitor(server);
                }

                UpdateServerList();
                _logManager.AddLog("系统", "服务器配置加载完成");
            }
            catch (Exception ex)
            {
                _logManager.AddLog("系统", $"加载配置失败: {ex.Message}", "错误");
                MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 绑定事件处理（修正事件名称与方法名一致）
        private void BindEvents()
        {
            // 管理按钮事件
            addBtn.Click += addBtn_Click;
            editBtn.Click += editBtn_Click;
            deleteBtn.Click += deleteBtn_Click;
            folderManagerBtn.Click += folderManagerBtn_Click;

            // 编辑按钮事件
            saveEditBtn.Click += saveEditBtn_Click;
            cancelEditBtn.Click += cancelEditBtn_Click;

            // 控制按钮事件
            startBtn.Click += startBtn_Click;
            stopBtn.Click += stopBtn_Click;
            restartBtn.Click += restartBtn_Click;
            stopAllBtn.Click += stopAllBtn_Click;

            // 监控按钮事件
            startMonitorBtn.Click += startMonitorBtn_Click;
            stopMonitorBtn.Click += stopMonitorBtn_Click;

            // 其他功能按钮
            browseExeBtn.Click += browseExeBtn_Click;
            sendCommandBtn.Click += sendCommandBtn_Click;
            clearLogBtn.Click += clearLogBtn_Click;
        }
        #endregion

        #region 界面状态管理
        // 更新服务器列表
        private void UpdateServerList()
        {
            serversGridView.DataSource = null;
            serversGridView.DataSource = _serverConfigs;

            // 格式化显示自定义列
            foreach (DataGridViewRow row in serversGridView.Rows)
            {
                if (row.DataBoundItem is ServerConfig server)
                {
                    row.Cells["IpPort"].Value = $"{server.Ip}:{server.Port}";
                    row.Cells["Status"].Value = server.Status.ToString();

                    // 根据状态设置颜色
                    switch (server.Status)
                    {
                        case ServerStatus.Running:
                            row.Cells["Status"].Style.ForeColor = Color.Green;
                            break;
                        case ServerStatus.Monitoring:
                            row.Cells["Status"].Style.ForeColor = Color.Blue;
                            break;
                        case ServerStatus.Stopped:
                            row.Cells["Status"].Style.ForeColor = Color.Gray;
                            break;
                        case ServerStatus.Starting:
                            row.Cells["Status"].Style.ForeColor = Color.Orange;
                            break;
                        case ServerStatus.Stopping:
                            row.Cells["Status"].Style.ForeColor = Color.OrangeRed;
                            break;
                    }
                }
            }
        }

        // 更新按钮状态
        private void UpdateButtonStates()
        {
            bool hasSelection = _selectedServer != null;
            bool hasRunningServers = _serverConfigs.Any(s =>
                s.Status == ServerStatus.Running || s.Status == ServerStatus.Monitoring);

            // 管理按钮状态
            editBtn.Enabled = hasSelection && !_isEditing && !_selectedServer.IsMonitoring;
            deleteBtn.Enabled = hasSelection && !_isEditing && !_selectedServer.IsMonitoring;
            folderManagerBtn.Enabled = hasSelection && !_isEditing;

            // 运行控制按钮状态
            startBtn.Enabled = hasSelection && !_isEditing && _selectedServer.Status == ServerStatus.Stopped;
            stopBtn.Enabled = hasSelection && !_isEditing &&
                            (_selectedServer.Status == ServerStatus.Running ||
                              _selectedServer.Status == ServerStatus.Monitoring);
            restartBtn.Enabled = hasSelection && !_isEditing &&
                               (_selectedServer.Status == ServerStatus.Running ||
                                 _selectedServer.Status == ServerStatus.Monitoring);
            stopAllBtn.Enabled = hasRunningServers && !_isEditing;

            // 监控按钮状态
            startMonitorBtn.Enabled = hasSelection && !_isEditing && !_selectedServer.IsMonitoring;
            stopMonitorBtn.Enabled = hasSelection && !_isEditing && _selectedServer.IsMonitoring;

            // 编辑按钮状态
            saveEditBtn.Enabled = _isEditing;
            cancelEditBtn.Enabled = _isEditing;

            // 指令发送按钮状态
            sendCommandBtn.Enabled = hasSelection && !_isEditing &&
                                   _selectedServer.EnableCommands &&
                                   (_selectedServer.Status == ServerStatus.Running ||
                                     _selectedServer.Status == ServerStatus.Monitoring);
        }

        // 设置编辑控件启用状态
        private void SetEditControlsEnabled(bool enabled)
        {
            serverNameInput.Enabled = enabled;
            exePathInput.Enabled = enabled;
            browseExeBtn.Enabled = enabled;
            ipInput.Enabled = enabled;
            portNum.Enabled = enabled;
            passwordInput.Enabled = enabled;
            scheduleTimeInput.Enabled = enabled;
            intervalHoursNum.Enabled = enabled;
            enableCommandsChk.Enabled = enabled;
        }

        // 加载服务器数据到编辑面板
        private void LoadServerToEditPanel(ServerConfig server)
        {
            if (server == null)
            {
                // 清空编辑面板
                serverNameInput.Text = "";
                exePathInput.Text = "";
                ipInput.Text = "127.0.0.1";
                portNum.Value = 5612;
                passwordInput.Text = "";
                scheduleTimeInput.Text = "01:00";
                intervalHoursNum.Value = 0;
                enableCommandsChk.Checked = false;
            }
            else
            {
                // 填充服务器数据
                serverNameInput.Text = server.Name;
                exePathInput.Text = server.ExePath;
                ipInput.Text = server.Ip;
                portNum.Value = server.Port;
                passwordInput.Text = server.Password;
                scheduleTimeInput.Text = server.ScheduleTime;
                intervalHoursNum.Value = (decimal)server.IntervalHours;
                enableCommandsChk.Checked = server.EnableCommands;
            }
        }

        // 添加日志条目
        private void AddLogEntry(string log)
        {
            logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {log}\n");
            logTextBox.ScrollToCaret();
        }
        #endregion

        #region 服务器选择与编辑
        // 服务器选择变更
        private void serversGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (!_isEditing && serversGridView.SelectedRows.Count > 0 &&
                serversGridView.SelectedRows[0].DataBoundItem is ServerConfig server)
            {
                _selectedServer = server;
                LoadServerToEditPanel(server);
            }
            else if (!_isEditing)
            {
                _selectedServer = null;
                LoadServerToEditPanel(null);
            }

            UpdateButtonStates();

            // 更新文件夹管理器
            if (_folderManagerForm != null && !_folderManagerForm.IsDisposed)
            {
                _folderManagerForm.RefreshServer(_selectedServer);
            }
        }

        // 进入新增模式（修正方法名）
        private void addBtn_Click(object sender, EventArgs e)
        {
            _isEditing = true;
            _isAdding = true;
            _selectedServer = null;
            serversGridView.ClearSelection();
            LoadServerToEditPanel(null);
            SetEditControlsEnabled(true);
            UpdateButtonStates();
            serverNameInput.Focus();
            _logManager.AddLog("系统", "进入新增服务器模式");
        }

        // 进入编辑模式（修正方法名）
        private void editBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            _isEditing = true;
            _isAdding = false;
            LoadServerToEditPanel(_selectedServer);
            SetEditControlsEnabled(true);
            UpdateButtonStates();
            _logManager.AddLog("系统", $"进入编辑服务器模式: {_selectedServer.Name}");
        }

        // 保存编辑（修正方法名）
        private void saveEditBtn_Click(object sender, EventArgs e)
        {
            // 验证服务器名称
            if (string.IsNullOrWhiteSpace(serverNameInput.Text))
            {
                MessageBox.Show("请输入服务器名称", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 验证程序路径
            if (!string.IsNullOrWhiteSpace(exePathInput.Text) && !File.Exists(exePathInput.Text))
            {
                var result = MessageBox.Show(
                    "程序路径不存在，是否继续？",
                    "路径验证",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            try
            {
                if (_isAdding)
                {
                    // 创建新服务器
                    var newId = _serverConfigs.Any() ? _serverConfigs.Max(s => s.Id) + 1 : 1;
                    var newServer = new ServerConfig
                    {
                        Id = newId,
                        Name = serverNameInput.Text.Trim(),
                        ExePath = exePathInput.Text.Trim(),
                        ExeName = Path.GetFileName(exePathInput.Text.Trim()),
                        Ip = ipInput.Text.Trim(),
                        Port = (int)portNum.Value,
                        Password = passwordInput.Text,
                        ScheduleTime = scheduleTimeInput.Text.Trim(),
                        IntervalHours = (double)intervalHoursNum.Value,
                        EnableCommands = enableCommandsChk.Checked,
                        Status = ServerStatus.Stopped
                    };

                    _serverConfigs.Add(newServer);
                    CreateServerMonitor(newServer);
                    _logManager.AddLog("系统", $"已添加服务器: {newServer.Name}");
                }
                else if (_selectedServer != null)
                {
                    // 更新现有服务器
                    _selectedServer.Name = serverNameInput.Text.Trim();
                    _selectedServer.ExePath = exePathInput.Text.Trim();
                    _selectedServer.ExeName = Path.GetFileName(exePathInput.Text.Trim());
                    _selectedServer.Ip = ipInput.Text.Trim();
                    _selectedServer.Port = (int)portNum.Value;
                    _selectedServer.Password = passwordInput.Text;
                    _selectedServer.ScheduleTime = scheduleTimeInput.Text.Trim();
                    _selectedServer.IntervalHours = (double)intervalHoursNum.Value;
                    _selectedServer.EnableCommands = enableCommandsChk.Checked;
                    _logManager.AddLog("系统", $"已更新服务器: {_selectedServer.Name}");
                }

                SaveServerConfigs();
                UpdateServerList();
                ExitEditMode();
            }
            catch (Exception ex)
            {
                _logManager.AddLog("系统", $"保存失败: {ex.Message}", "错误");
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 取消编辑（修正方法名）
        private void cancelEditBtn_Click(object sender, EventArgs e)
        {
            ExitEditMode();
            _logManager.AddLog("系统", "已取消编辑");
        }

        // 退出编辑模式
        private void ExitEditMode()
        {
            _isEditing = false;
            _isAdding = false;
            SetEditControlsEnabled(false);
            serversGridView_SelectionChanged(null, EventArgs.Empty);
            UpdateButtonStates();
        }

        // 删除服务器（修正方法名）
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            var confirm = MessageBox.Show(
                $"确定要删除服务器「{_selectedServer.Name}」吗？",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                // 停止并移除监控器
                if (_serverMonitors.ContainsKey(_selectedServer.Id))
                {
                    _serverMonitors[_selectedServer.Id].Stop();
                    _serverMonitors.Remove(_selectedServer.Id);
                }

                _serverConfigs.Remove(_selectedServer);
                SaveServerConfigs();
                UpdateServerList();
                _logManager.AddLog("系统", $"已删除服务器: {_selectedServer.Name}");
                _selectedServer = null;
                UpdateButtonStates();
                LoadServerToEditPanel(null);
            }
        }

        // 浏览程序路径（修正方法名）
        private void browseExeBtn_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Filter = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*",
                Title = "选择服务器程序"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    exePathInput.Text = ofd.FileName;
                    if (string.IsNullOrWhiteSpace(serverNameInput.Text))
                    {
                        serverNameInput.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                    }
                }
            }
        }
        #endregion

        #region 服务器运行控制
        // 创建服务器监控器
        private void CreateServerMonitor(ServerConfig server)
        {
            if (!_serverMonitors.ContainsKey(server.Id))
            {
                var monitor = new ServerMonitor(server, (s) =>
                {
                    if (serversGridView.InvokeRequired)
                    {
                        serversGridView.Invoke(new Action(UpdateServerList));
                    }
                    else
                    {
                        UpdateServerList();
                    }
                });

                _serverMonitors[server.Id] = monitor;

                // 如果配置为监控中，则启动监控
                if (server.IsMonitoring)
                {
                    monitor.Start();
                }
            }
        }

        // 启动服务器（修正方法名）
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            try
            {
                if (string.IsNullOrWhiteSpace(_selectedServer.ExePath) || !File.Exists(_selectedServer.ExePath))
                {
                    _logManager.AddLog(_selectedServer.Name, "程序路径无效，无法启动", "错误");
                    MessageBox.Show("请配置有效的服务器程序路径", "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
                {
                    _selectedServer.Status = ServerStatus.Starting;
                    UpdateServerList();
                    monitor.StartProcess();
                }
            }
            catch (Exception ex)
            {
                _selectedServer.Status = ServerStatus.Stopped;
                _logManager.AddLog(_selectedServer.Name, $"启动失败: {ex.Message}", "错误");
                UpdateServerList();
            }
        }

        // 停止服务器（修正方法名）
        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
            {
                _selectedServer.Status = ServerStatus.Stopping;
                UpdateServerList();
                monitor.KillProcess();
            }
        }

        // 重启服务器（修正方法名）
        private void restartBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            _logManager.AddLog(_selectedServer.Name, "开始重启服务器");
            stopBtn_Click(sender, e);

            // 延迟启动
            var timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (s, args) =>
            {
                timer.Dispose();
                startBtn_Click(sender, e);
            };
            timer.Start();
        }

        // 停止所有服务器（修正方法名）
        private void stopAllBtn_Click(object sender, EventArgs e)
        {
            var runningServers = _serverConfigs.Where(s =>
                s.Status == ServerStatus.Running || s.Status == ServerStatus.Monitoring).ToList();

            if (runningServers.Count == 0)
            {
                MessageBox.Show("没有运行中的服务器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"确定要停止所有 {runningServers.Count} 个运行中的服务器吗？",
                "确认操作",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                foreach (var server in runningServers)
                {
                    server.Status = ServerStatus.Stopping;
                }
                UpdateServerList();

                // 异步停止所有服务器
                Task.Run(() =>
                {
                    foreach (var server in runningServers)
                    {
                        if (_serverMonitors.TryGetValue(server.Id, out var monitor))
                        {
                            monitor.KillProcess();
                            Thread.Sleep(500); // 间隔停止避免冲突
                        }
                    }
                    Thread.Sleep(1000);
                    UpdateServerList();
                });
            }
        }
        #endregion

        #region 监控与指令功能
        // 启动监控（修正方法名）
        private void startMonitorBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
            {
                monitor.Start();
                _selectedServer.IsMonitoring = true;
                UpdateButtonStates();
                _logManager.AddLog(_selectedServer.Name, "已开启监控");
            }
        }

        // 停止监控（修正方法名）
        private void stopMonitorBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
            {
                monitor.Stop();
                _selectedServer.IsMonitoring = false;
                UpdateButtonStates();
                _logManager.AddLog(_selectedServer.Name, "已关闭监控");
            }
        }

        // 发送指令（修正方法名）
        private void sendCommandBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;
            if (string.IsNullOrWhiteSpace(commandInput.Text))
            {
                MessageBox.Show("请输入指令内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_selectedServer.EnableCommands)
            {
                MessageBox.Show("请在编辑模式中启用指令功能", "功能未启用", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
            {
                string result = monitor.SendServiceCommand(commandInput.Text.Trim());
                MessageBox.Show($"指令响应:\n{result}", "发送结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 其他功能
        // 打开文件夹管理器（修正方法名）
        private void folderManagerBtn_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null)
            {
                MessageBox.Show("请先选择服务器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_folderManagerForm == null || _folderManagerForm.IsDisposed)
            {
                _folderManagerForm = new FolderManagerForm(_selectedServer);
                _folderManagerForm.FormClosed += (s, args) => _folderManagerForm = null;
                _folderManagerForm.Show(this);
            }
            else
            {
                _folderManagerForm.RefreshServer(_selectedServer);
                _folderManagerForm.Activate();
            }
        }

        // 清空日志（修正方法名）
        private void clearLogBtn_Click(object sender, EventArgs e)
        {
            logTextBox.Clear();
            _logManager.AddLog("系统", "日志已清空");
        }

        // 保存服务器配置
        private void SaveServerConfigs()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerConfigs.xml");
                using (var fs = new FileStream(configPath, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(List<ServerConfig>));
                    serializer.Serialize(fs, _serverConfigs);
                }
            }
            catch (Exception ex)
            {
                _logManager.AddLog("系统", $"保存配置失败: {ex.Message}", "错误");
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #region 补充缺失的事件处理方法
        /// <summary>
        /// 服务器表格单元格点击事件（Designer绑定，空实现避免报错，可根据需求扩展）
        /// </summary>
        private void serversGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 若需要实现“点击单元格触发操作”（如按钮列），可在此添加逻辑
            // 示例：如果点击第3列（索引2）的单元格，执行某操作
            // if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            // {
            //     var selectedServer = serversGridView.Rows[e.RowIndex].DataBoundItem as ServerConfig;
            //     if (selectedServer != null)
            //     {
            //         // 执行自定义逻辑（如打开详情）
            //     }
            // }
        }

        /// <summary>
        /// 服务器列表面板绘制事件（Designer绑定，空实现避免报错，可根据需求扩展）
        /// </summary>
        private void serverListPanel_Paint(object sender, PaintEventArgs e)
        {
            // 若需要自定义面板绘制（如绘制边框、背景色渐变），可在此添加逻辑
            // 示例：给面板绘制灰色边框
            // var panel = sender as Panel;
            // if (panel != null)
            // {
            //     using (var pen = new Pen(Color.LightGray, 1))
            //     {
            //         e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, panel.Width - 1, panel.Height - 1));
            //     }
            // }
        }
        #endregion
        // 窗口关闭时
        private void ToolMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止所有监控
            foreach (var monitor in _serverMonitors.Values)
            {
                monitor.Stop();
            }
            SaveServerConfigs();
        }
        #endregion
    }
}