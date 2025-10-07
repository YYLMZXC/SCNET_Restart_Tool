using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SCNET_Restart_Tool
{
    public partial class ToolMain : Form
    {
        // 服务端配置列表
        private List<ServerConfig> _serverConfigs = new List<ServerConfig>();
        // 当前选中的服务端
        private ServerConfig _selectedServer;
        // 文件夹管理窗口实例
        private FolderManagerForm _folderManagerForm;
        // 日志管理器
        private readonly LogManager _logManager = LogManager.GetInstance();

        public ToolMain()
        {
            InitializeComponent();
            InitServerDataGridView();
            LoadServerConfigs();
            InitLogView();
        }

        #region 初始化与数据加载
        // 初始化服务端列表DataGridView
        private void InitServerDataGridView()
        {
            dgvServers.AutoGenerateColumns = false;
            dgvServers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServers.MultiSelect = false;

            // 添加列
            dgvServers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });
            dgvServers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "服务端名称",
                Width = 150
            });
            dgvServers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "IpPort",
                HeaderText = "IP:端口",
                Width = 120
            });
            dgvServers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ExePath",
                DataPropertyName = "ExePath",
                HeaderText = "程序路径",
                Width = 300
            });
            dgvServers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                Width = 80
            });
        }

        // 初始化日志显示
        private void InitLogView()
        {
            rtbLog.ReadOnly = true;
            _logManager.LogAdded += (log) =>
            {
                if (rtbLog.InvokeRequired)
                {
                    rtbLog.Invoke(new Action(() => AddLogToView(log)));
                }
                else
                {
                    AddLogToView(log);
                }
            };
        }

        // 添加日志到界面
        private void AddLogToView(string log)
        {
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {log}\n");
            rtbLog.ScrollToCaret();
        }

        // 加载服务端配置
        private void LoadServerConfigs()
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
                        RefreshServerList();
                        _logManager.AddLog("系统", "服务端配置加载完成");
                    }
                }
            }
            catch (Exception ex)
            {
                _logManager.AddLog("系统", $"加载配置失败：{ex.Message}", "错误");
                MessageBox.Show($"加载配置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 刷新服务端列表
        private void RefreshServerList()
        {
            dgvServers.DataSource = null;
            dgvServers.DataSource = _serverConfigs;

            // 处理自定义列显示
            foreach (DataGridViewRow row in dgvServers.Rows)
            {
                if (row.DataBoundItem is ServerConfig server)
                {
                    row.Cells["IpPort"].Value = $"{server.Ip}:{server.Port}";
                    row.Cells["Status"].Value = server.Status.ToString();

                    // 状态颜色区分（补充Monitoring的样式）
                    switch (server.Status)
                    {
                        case ServerStatus.Running:
                            row.Cells["Status"].Style.ForeColor = Color.Green;
                            break;
                        case ServerStatus.Monitoring:
                            row.Cells["Status"].Style.ForeColor = Color.Blue; // 监控中用蓝色区分
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
        #endregion

        #region 服务端操作
        // 选中服务端变更
        private void dgvServers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvServers.SelectedRows.Count > 0 && dgvServers.SelectedRows[0].DataBoundItem is ServerConfig server)
            {
                _selectedServer = server;
                UpdateServerControlStatus();
            }
            else
            {
                _selectedServer = null;
                UpdateServerControlStatus();
            }

            // 同步文件夹管理窗口
            if (_folderManagerForm != null && !_folderManagerForm.IsDisposed)
            {
                _folderManagerForm.RefreshForm(_selectedServer);
            }
        }

        // 更新服务端控制按钮状态
        private void UpdateServerControlStatus()
        {
            bool hasSelected = _selectedServer != null;
            btnStartServer.Enabled = hasSelected && _selectedServer.Status == ServerStatus.Stopped;
            btnStopServer.Enabled = hasSelected && _selectedServer.Status == ServerStatus.Running;
            btnRestartServer.Enabled = hasSelected && _selectedServer.Status == ServerStatus.Running;
            btnEditServer.Enabled = hasSelected;
            btnDeleteServer.Enabled = hasSelected;
            btnOpenFolderManager.Enabled = hasSelected;
        }

        // 启动服务端
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            try
            {
                if (string.IsNullOrEmpty(_selectedServer.ExePath) || !File.Exists(_selectedServer.ExePath))
                {
                    _logManager.AddLog(_selectedServer.Name, "服务端程序路径无效，无法启动", "错误");
                    MessageBox.Show("请先配置有效的服务端程序路径！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _selectedServer.Status = ServerStatus.Starting;
                RefreshServerList();

                // 实际启动逻辑（这里仅做示例）
                Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(1000); // 模拟启动延迟
                    _selectedServer.Status = ServerStatus.Running;
                    _logManager.AddLog(_selectedServer.Name, "服务端已启动");
                    RefreshServerList();
                });
            }
            catch (Exception ex)
            {
                _selectedServer.Status = ServerStatus.Stopped;
                _logManager.AddLog(_selectedServer.Name, $"启动失败：{ex.Message}", "错误");
                RefreshServerList();
            }
        }

        // 停止服务端
        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null || _selectedServer.Status != ServerStatus.Running) return;

            _selectedServer.Status = ServerStatus.Stopping;
            RefreshServerList();

            // 实际停止逻辑（这里仅做示例）
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000); // 模拟停止延迟
                _selectedServer.Status = ServerStatus.Stopped;
                _logManager.AddLog(_selectedServer.Name, "服务端已停止");
                RefreshServerList();
            });
        }

        // 重启服务端
        private void btnRestartServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null || _selectedServer.Status != ServerStatus.Running) return;

            _logManager.AddLog(_selectedServer.Name, "开始重启服务端");
            btnStopServer.PerformClick();

            // 延迟启动（等待停止完成）
            var timer = new Timer { Interval = 1500 };
            timer.Tick += (s, args) =>
            {
                timer.Dispose();
                btnStartServer.PerformClick();
            };
            timer.Start();
        }

        // 添加新服务端
        private void btnAddServer_Click(object sender, EventArgs e)
        {
            var newId = _serverConfigs.Any() ? _serverConfigs.Max(s => s.Id) + 1 : 1;
            var newServer = new ServerConfig { Id = newId };

            using (var editForm = new ServerEditForm(newServer))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    _serverConfigs.Add(newServer);
                    SaveServerConfigs();
                    RefreshServerList();
                    _logManager.AddLog("系统", $"已添加新服务端：{newServer.Name}");
                }
            }
        }

        // 编辑服务端
        private void btnEditServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            using (var editForm = new ServerEditForm(_selectedServer))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    SaveServerConfigs();
                    RefreshServerList();
                    _logManager.AddLog("系统", $"已更新服务端：{_selectedServer.Name}");
                }
            }
        }

        // 删除服务端
        private void btnDeleteServer_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null) return;

            if (MessageBox.Show(
                $"确定要删除服务端「{_selectedServer.Name}」吗？",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _serverConfigs.Remove(_selectedServer);
                SaveServerConfigs();
                RefreshServerList();
                _logManager.AddLog("系统", $"已删除服务端：{_selectedServer.Name}");
                _selectedServer = null;
                UpdateServerControlStatus();
            }
        }

        // 保存服务端配置
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
                _logManager.AddLog("系统", $"保存配置失败：{ex.Message}", "错误");
                MessageBox.Show($"保存配置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 文件夹管理窗口
        // 打开文件夹管理窗口
        private void btnOpenFolderManager_Click(object sender, EventArgs e)
        {
            if (_selectedServer == null)
            {
                MessageBox.Show("请先选中服务端！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_folderManagerForm == null || _folderManagerForm.IsDisposed)
            {
                _folderManagerForm = new FolderManagerForm(_selectedServer);
                // 使用args避免与外部e冲突
                _folderManagerForm.FormClosed += (s, args) => _folderManagerForm = null;
                _folderManagerForm.Show(this);
            }
            else
            {
                _folderManagerForm.RefreshForm(_selectedServer);
                _folderManagerForm.Activate();
            }
        }
        #endregion

        #region 其他事件
        // 窗口关闭时保存配置
        private void ToolMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveServerConfigs();
        }

        // 清除日志
        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        // 停止所有服务端
        private void btnStopAllServers_Click(object sender, EventArgs e)
        {
            var runningServers = _serverConfigs.Where(s => s.Status == ServerStatus.Running).ToList();
            if (runningServers.Count == 0)
            {
                MessageBox.Show("没有正在运行的服务端", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(
                $"确定要停止所有{runningServers.Count}个服务端吗？",
                "确认停止全部",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                foreach (var server in runningServers)
                {
                    server.Status = ServerStatus.Stopping;
                }
                RefreshServerList();

                // 模拟批量停止
                Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(1500);
                    foreach (var server in runningServers)
                    {
                        server.Status = ServerStatus.Stopped;
                        _logManager.AddLog(server.Name, "服务端已停止（批量操作）");
                    }
                    RefreshServerList();
                });
            }
        }
        #endregion
    }

    // 服务端状态枚举
    
}