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

        // 当前激活的文件夹路径（用于添加文件时定位）
        private string _activeFolderPath;

        public FolderManagerForm(ServerConfig selectedServer)
        {
            InitializeComponent();
            _currentServer = selectedServer;
            _logManager = LogManager.GetInstance();
            InitFolderTabs();
            UpdateWindowTitle();
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
            tabControlFolders.TabPages.Clear();

            if (_currentServer == null)
            {
                var emptyTab = new TabPage("提示");
                emptyTab.Controls.Add(new Label
                {
                    Text = "请先在主窗口选中服务端！",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("宋体", 12F)
                });
                tabControlFolders.TabPages.Add(emptyTab);
                return;
            }

            // 为每个文件夹类型创建带文件列表的标签页
            AddFileListTab("Bugs日志", _currentServer.BugsLogPath, true);
            AddFileListTab("CharacterSkins（皮肤）", _currentServer.CharacterSkinsPath, false);
            AddFileListTab("Configs（插件配置）", _currentServer.ConfigsPath, false);
            AddFileListTab("TexturePacks（材质包）", _currentServer.TexturePacksPath, false);
            AddFileListTab("NetMods（模组）", _currentServer.NetModsPath, false);
            AddFileListTab("Plugins（服务端插件）", _currentServer.PluginsPath, false);
            AddFileListTab("Worlds（地图存档）", _currentServer.WorldsPath, false);

            // 绑定标签页切换事件（记录当前激活的文件夹路径）
            tabControlFolders.SelectedIndexChanged += (s, e) =>
            {
                if (tabControlFolders.SelectedTab.Tag != null)
                    _activeFolderPath = tabControlFolders.SelectedTab.Tag.ToString();
            };
        }

        /// 创建带文件列表的标签页
        private void AddFileListTab(string tabName, string folderPath, bool hasCleanButton)
        {
            var tabPage = new TabPage(tabName);
            tabPage.Tag = folderPath; // 存储文件夹路径到Tag属性
            tabPage.Padding = new Padding(10);

            // 1. 路径显示区域
            var pathPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
            var txtPath = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Text = folderPath,
                Margin = new Padding(0, 5, 0, 5)
            };
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                txtPath.ForeColor = Color.Red;
                txtPath.Text += "（目录不存在，点击创建）";
                txtPath.Click += (s, e) => CreateFolderIfNotExists(folderPath, tabName);
            }
            pathPanel.Controls.Add(txtPath);

            // 2. 文件列表DataGridView
            var dgvFiles = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };
            dgvFiles.Columns.Add("FileName", "文件名");
            dgvFiles.Columns.Add("FileSize", "大小(KB)");
            dgvFiles.Columns.Add("LastWriteTime", "修改时间");
            dgvFiles.Columns["FileSize"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // 3. 操作按钮区域
            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };

            var btnRefresh = new Button { Text = "刷新列表", Width = 80, Left = 10, Top = 5 };
            btnRefresh.Click += (s, e) => LoadFilesToGrid(dgvFiles, folderPath);

            var btnOpenFolder = new Button { Text = "打开目录", Width = 80, Left = 100, Top = 5 };
            btnOpenFolder.Click += (s, e) => FolderManager.OpenFolder(folderPath, tabName, _currentServer.Name);

            var btnAddFile = new Button { Text = "添加文件", Width = 80, Left = 190, Top = 5 };
            btnAddFile.Click += (s, e) => AddFileToFolder(folderPath, tabName);

            var btnDeleteFile = new Button { Text = "删除选中", Width = 80, Left = 280, Top = 5, ForeColor = Color.Red };
            btnDeleteFile.Click += (s, e) => DeleteSelectedFile(dgvFiles, folderPath, tabName);

            // 仅Bugs日志添加清理按钮
            Button btnClean = null;
            if (hasCleanButton)
            {
                btnClean = new Button { Text = "清理过期日志", Width = 100, Left = 370, Top = 5, BackColor = Color.Orange, ForeColor = Color.White };
                btnClean.Click += (s, e) => FolderManager.CleanBugsLog(folderPath, _currentServer.Name);
            }

            // 添加按钮到面板
            btnPanel.Controls.Add(btnRefresh);
            btnPanel.Controls.Add(btnOpenFolder);
            btnPanel.Controls.Add(btnAddFile);
            btnPanel.Controls.Add(btnDeleteFile);
            if (btnClean != null) btnPanel.Controls.Add(btnClean);

            // 4. 组装标签页
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            mainPanel.Controls.Add(dgvFiles);
            mainPanel.Controls.Add(btnPanel);
            mainPanel.Controls.Add(pathPanel);
            tabPage.Controls.Add(mainPanel);

            // 5. 加载文件列表
            LoadFilesToGrid(dgvFiles, folderPath);

            tabControlFolders.TabPages.Add(tabPage);
        }

        /// 加载文件列表到DataGridView
        private void LoadFilesToGrid(DataGridView dgv, string folderPath)
        {
            dgv.Rows.Clear();
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath)) return;

            try
            {
                foreach (var filePath in Directory.GetFiles(folderPath))
                {
                    var fileInfo = new FileInfo(filePath);
                    dgv.Rows.Add(
                        fileInfo.Name,
                        (fileInfo.Length / 1024.0).ToString("F2"),
                        fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                    );
                }
            }
            catch (Exception ex)
            {
                _logManager.AddLog(_currentServer.Name, $"加载文件列表失败：{ex.Message}", "错误");
                MessageBox.Show($"加载失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// 创建不存在的文件夹
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
                    InitFolderTabs(); // 重新初始化标签页刷新状态
                }
                catch (Exception ex)
                {
                    _logManager.AddLog(_currentServer.Name, $"创建目录失败：{ex.Message}", "错误");
                    MessageBox.Show($"创建失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// 添加文件到文件夹
        private void AddFileToFolder(string folderPath, string folderName)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                CreateFolderIfNotExists(folderPath, folderName);
                return;
            }

            using (var ofd = new OpenFileDialog { Title = $"选择要添加到{folderName}的文件" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var destPath = Path.Combine(folderPath, Path.GetFileName(ofd.FileName));
                        // 处理文件已存在的情况
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
                        var currentTab = tabControlFolders.SelectedTab;
                        if (currentTab?.Controls[0] is Panel mainPanel &&
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

        /// 删除选中的文件
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

        // 批量备份所有目录（复用之前的逻辑）
        private void btnBatchBackupAll_Click(object sender, EventArgs e)
        {
            if (_currentServer != null)
                FolderManager.BatchBackupAllFolders(_currentServer);
        }
    }
}