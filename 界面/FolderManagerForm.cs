using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public partial class FolderManagerForm : Form
    {
        private ServerConfig _currentServer;
        private readonly LogManager _logManager;
        private string _activeFolderPath;

        // 配色方案定义
        private readonly Color _primaryColor = Color.FromArgb(52, 152, 219);
        private readonly Color _secondaryColor = Color.FromArgb(41, 128, 185);
        private readonly Color _lightGray = Color.FromArgb(245, 245, 245);
        private readonly Color _darkGray = Color.FromArgb(70, 70, 70);
        private readonly Color _successColor = Color.FromArgb(46, 204, 113);
        private readonly Color _warningColor = Color.FromArgb(241, 196, 15);
        private readonly Color _dangerColor = Color.FromArgb(231, 76, 60);

        public FolderManagerForm(ServerConfig selectedServer)
        {
            InitializeComponent();
            _currentServer = selectedServer;
            _logManager = LogManager.GetInstance();
            InitStyle(); // 初始化样式
            InitFolderTabs();
            UpdateWindowTitle();
        }

        // 初始化界面样式
        private void InitStyle()
        {
            // 窗口样式
            this.BackColor = Color.White;
            this.Font = new Font("微软雅黑", 9F);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimumSize = new Size(800, 600);

            // 批量备份按钮样式
            btnBatchBackupAll.BackColor = _primaryColor;
            btnBatchBackupAll.ForeColor = Color.White;
            btnBatchBackupAll.FlatStyle = FlatStyle.Flat;
            btnBatchBackupAll.FlatAppearance.BorderSize = 0;
            btnBatchBackupAll.Padding = new Padding(5, 0, 5, 0);
            btnBatchBackupAll.Cursor = Cursors.Hand;
            btnBatchBackupAll.MouseEnter += (s, e) => btnBatchBackupAll.BackColor = _secondaryColor;
            btnBatchBackupAll.MouseLeave += (s, e) => btnBatchBackupAll.BackColor = _primaryColor;

            // 标签页控件样式
            tabControlFolders.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlFolders.DrawItem += TabControlFolders_DrawItem;
            tabControlFolders.ItemSize = new Size(120, 35);
            tabControlFolders.Padding = new Point(10, 5);
            tabControlFolders.BackColor = _lightGray;
        }

        // 自定义绘制标签页标题
        private void TabControlFolders_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabPage = tabControl.TabPages[e.Index];

            // 绘制背景
            using (var brush = new SolidBrush(e.State.HasFlag(DrawItemState.Selected) ? Color.White : _lightGray))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            // 绘制选中状态下划线
            if (e.State.HasFlag(DrawItemState.Selected))
            {
                using (var pen = new Pen(_primaryColor, 3))
                {
                    e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
                }
            }

            // 绘制文本
            var textColor = e.State.HasFlag(DrawItemState.Selected) ? _darkGray : Color.Gray;
            using (var brush = new SolidBrush(textColor))
            {
                var textRect = new Rectangle(e.Bounds.Left + 5, e.Bounds.Top + 5,
                                            e.Bounds.Width - 10, e.Bounds.Height - 10);
                e.Graphics.DrawString(tabPage.Text, tabControl.Font, brush, textRect);
            }
        }
        public void RefreshServer(ServerConfig newServer)
        {
            // 直接调用现有RefreshForm方法，保持功能一致
            RefreshForm(newServer);
        }
        public void RefreshForm(ServerConfig newServer)
        {
            _currentServer = newServer;
            UpdateWindowTitle();
            InitFolderTabs();
        }

        private void UpdateWindowTitle()
        {
            Text = $"文件夹管理 - {(_currentServer?.Name ?? "未选择服务端")}";
        }

        private void InitFolderTabs()
        {
            // 关键：先移除已有的事件绑定，避免重复触发
            tabControlFolders.SelectedIndexChanged -= TabControlFolders_SelectedIndexChanged;

            tabControlFolders.TabPages.Clear();

            if (_currentServer == null)
            {
                var emptyTab = new TabPage("提示");
                emptyTab.BackColor = Color.White;
                var tipLabel = new Label
                {
                    Text = "请先在主窗口选中服务端！",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("微软雅黑", 12F),
                    ForeColor = _darkGray
                };
                emptyTab.Controls.Add(tipLabel);
                tabControlFolders.TabPages.Add(emptyTab);
                return;
            }

            // 添加标签页（原逻辑不变）
            AddFileListTab("Bugs日志", _currentServer.BugsLogPath, true, Properties.Resources.log_icon);
            AddFileListTab("CharacterSkins（皮肤）", _currentServer.CharacterSkinsPath, false, Properties.Resources.skin_icon);
            AddFileListTab("Configs（配置）", _currentServer.ConfigsPath, false, Properties.Resources.config_icon);
            AddFileListTab("TexturePacks（材质）", _currentServer.TexturePacksPath, false, Properties.Resources.texture_icon);
            AddFileListTab("NetMods（模组）", _currentServer.NetModsPath, false, Properties.Resources.mod_icon);
            AddFileListTab("Plugins（插件）", _currentServer.PluginsPath, false, Properties.Resources.plugin_icon);
            AddFileListTab("Worlds（地图）", _currentServer.WorldsPath, false, Properties.Resources.world_icon);

            // 重新绑定事件（使用命名方法）
            tabControlFolders.SelectedIndexChanged += TabControlFolders_SelectedIndexChanged;
        }

        // 新增事件处理方法（判断空值）
        private void TabControlFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlFolders.SelectedTab != null && tabControlFolders.SelectedTab.Tag != null)
            {
                _activeFolderPath = tabControlFolders.SelectedTab.Tag.ToString();
            }
        }

        private void AddFileListTab(string tabName, string folderPath, bool hasCleanButton, Image tabIcon)
        {
            var tabPage = new TabPage(tabName);
            tabPage.Tag = folderPath;
            tabPage.Padding = new Padding(10);
            tabPage.BackColor = Color.White;

            // 路径显示区域
            var pathPanel = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = _lightGray };
            var txtPath = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Text = folderPath,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.None,
                BackColor = _lightGray,
                Font = new Font("微软雅黑", 9F, FontStyle.Regular)
            };

            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                txtPath.ForeColor = _dangerColor;
                txtPath.Text += "（目录不存在，点击创建）";
                txtPath.Cursor = Cursors.Hand;
                txtPath.Click += (s, e) => CreateFolderIfNotExists(folderPath, tabName);
            }
            else
            {
                txtPath.ForeColor = _darkGray;
            }
            pathPanel.Controls.Add(txtPath);

            // 文件列表DataGridView
            var dgvFiles = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.LightGray,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = { BackColor = _lightGray }
            };

            dgvFiles.Columns.Add("FileName", "文件名");
            dgvFiles.Columns.Add("FileSize", "大小(KB)");
            dgvFiles.Columns.Add("LastWriteTime", "修改时间");
            dgvFiles.Columns["FileName"].Width = 350;
            dgvFiles.Columns["FileSize"].Width = 100;
            dgvFiles.Columns["LastWriteTime"].Width = 160;
            dgvFiles.Columns["FileSize"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // 行高和样式
            dgvFiles.RowTemplate.Height = 28;
            dgvFiles.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            dgvFiles.ColumnHeadersDefaultCellStyle.ForeColor = _darkGray;
            dgvFiles.ColumnHeadersDefaultCellStyle.BackColor = _lightGray;
            dgvFiles.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 0, 0);

            // 操作按钮区域
            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.White, Padding = new Padding(10, 5, 10, 5) };

            var btnRefresh = CreateButton("刷新列表", _primaryColor, 80);
            btnRefresh.Click += (s, e) => LoadFilesToGrid(dgvFiles, folderPath);

            var btnOpenFolder = CreateButton("打开目录", _secondaryColor, 80);
            btnOpenFolder.Click += (s, e) => FolderManager.OpenFolder(folderPath, tabName, _currentServer.Name);

            var btnAddFile = CreateButton("添加文件", _successColor, 80);
            btnAddFile.Click += (s, e) => AddFileToFolder(folderPath, tabName);

            var btnDeleteFile = CreateButton("删除选中", _dangerColor, 80);
            btnDeleteFile.Click += (s, e) => DeleteSelectedFile(dgvFiles, folderPath, tabName);

            // 布局按钮
            btnRefresh.Left = 0;
            btnOpenFolder.Left = btnRefresh.Right + 10;
            btnAddFile.Left = btnOpenFolder.Right + 10;
            btnDeleteFile.Left = btnAddFile.Right + 10;

            // 仅Bugs日志添加清理按钮
            Button btnClean = null;
            if (hasCleanButton)
            {
                btnClean = CreateButton("清理过期日志", _warningColor, 120);
                btnClean.Left = btnDeleteFile.Right + 10;
                btnPanel.Controls.Add(btnClean);
                btnClean.Click += (s, e) => FolderManager.CleanBugsLog(folderPath, _currentServer.Name);
            }

            // 添加按钮到面板
            btnPanel.Controls.Add(btnRefresh);
            btnPanel.Controls.Add(btnOpenFolder);
            btnPanel.Controls.Add(btnAddFile);
            btnPanel.Controls.Add(btnDeleteFile);

            // 组装标签页
            var mainPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            mainPanel.Controls.Add(dgvFiles);
            mainPanel.Controls.Add(btnPanel);
            mainPanel.Controls.Add(pathPanel);
            tabPage.Controls.Add(mainPanel);

            // 加载文件列表
            LoadFilesToGrid(dgvFiles, folderPath);

            tabControlFolders.TabPages.Add(tabPage);
        }

        // 创建统一样式的按钮
        private Button CreateButton(string text, Color backColor, int width)
        {
            return new Button
            {
                Text = text,
                Width = width,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = backColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Font = new Font("微软雅黑", 9F)
            };
        }

        private void LoadFilesToGrid(DataGridView dgv, string folderPath)
        {
            dgv.Rows.Clear();
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath)) return;

            try
            {
                foreach (var filePath in Directory.GetFiles(folderPath))
                {
                    var fileInfo = new FileInfo(filePath);
                    var row = dgv.Rows.Add(
                        fileInfo.Name,
                        (fileInfo.Length / 1024.0).ToString("F2"),
                        fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                    );

                    // 高亮显示今天修改的文件
                    if (fileInfo.LastWriteTime.Date == DateTime.Today)
                    {
                        dgv.Rows[row].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                    }
                }
            }
            catch (Exception ex)
            {
                _logManager.AddLog(_currentServer.Name, $"加载文件列表失败：{ex.Message}", "错误");
                MessageBox.Show($"加载失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateFolderIfNotExists(string folderPath, string folderName)
        {
            if (string.IsNullOrEmpty(folderPath)) return;

            var result = MessageBox.Show(
                $"{folderName}目录不存在，是否创建？\n路径：{folderPath}",
                "目录不存在",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                    _logManager.AddLog(_currentServer.Name, $"已创建{folderName}目录：{folderPath}");
                    InitFolderTabs();
                }
                catch (Exception ex)
                {
                    _logManager.AddLog(_currentServer.Name, $"创建目录失败：{ex.Message}", "错误");
                    MessageBox.Show($"创建失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddFileToFolder(string folderPath, string folderName)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                CreateFolderIfNotExists(folderPath, folderName);
                return;
            }

            using (var ofd = new OpenFileDialog
            {
                Title = $"选择要添加到{folderName}的文件",
                Filter = "所有文件 (*.*)|*.*",
                RestoreDirectory = true
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var destPath = Path.Combine(folderPath, Path.GetFileName(ofd.FileName));
                        if (File.Exists(destPath))
                        {
                            var overwrite = MessageBox.Show(
                                $"文件{Path.GetFileName(ofd.FileName)}已存在，是否覆盖？",
                                "文件已存在",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
                            if (overwrite != DialogResult.Yes) return;
                        }

                        File.Copy(ofd.FileName, destPath, true);
                        _logManager.AddLog(_currentServer.Name, $"已添加文件到{folderName}：{Path.GetFileName(ofd.FileName)}");

                        // 刷新当前标签页的文件列表
                        if (tabControlFolders.SelectedTab?.Controls[0] is Panel mainPanel &&
                            mainPanel.Controls[0] is DataGridView dgv)
                        {
                            LoadFilesToGrid(dgv, folderPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logManager.AddLog(_currentServer.Name, $"添加文件失败：{ex.Message}", "错误");
                        MessageBox.Show($"添加失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeleteSelectedFile(DataGridView dgv, string folderPath, string folderName)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中要删除的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var fileName = dgv.SelectedRows[0].Cells["FileName"].Value.ToString();
            var filePath = Path.Combine(folderPath, fileName);

            var result = MessageBox.Show(
                $"确定要删除文件 {fileName} 吗？\n此操作不可恢复！",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    File.Delete(filePath);
                    _logManager.AddLog(_currentServer.Name, $"已从{folderName}删除文件：{fileName}");
                    dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
                }
                catch (Exception ex)
                {
                    _logManager.AddLog(_currentServer.Name, $"删除文件失败：{ex.Message}", "错误");
                    MessageBox.Show($"删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBatchBackupAll_Click(object sender, EventArgs e)
        {
            if (_currentServer != null)
                FolderManager.BatchBackupAllFolders(_currentServer);
        }
    }
}
