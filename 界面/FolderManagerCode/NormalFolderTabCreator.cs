using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    internal static class NormalFolderTabCreator
    {
        // 创建普通文件夹标签页（支持文件+文件夹显示）
        public static TabPage Create(FolderManagerForm form, string tabName, string folderPath, ToolTip toolTip, Action onBatchBackup)
        {
            var tabPage = new TabPage(tabName);
            tabPage.Tag = folderPath; // 存储根目录路径
            tabPage.Padding = new Padding(10);
            tabPage.BackColor = Color.White;

            // 主布局表格（3行：路径、文件列表、按钮）
            var tablePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(5),
                RowStyles =
                {
                    new RowStyle(SizeType.Absolute, 35F),
                    new RowStyle(SizeType.Percent, 100F),
                    new RowStyle(SizeType.Absolute, 70F)
                }
            };

            // 1. 路径显示面板（支持动态更新当前目录）
            var pathPanel = CreatePathPanel(form, folderPath, tabName);
            pathPanel.Margin = new Padding(0, 5, 0, 5);
            tablePanel.Controls.Add(pathPanel, 0, 0);

            // 2. 文件列表（支持显示文件和文件夹）
            var fileGridView = ControlFactory.CreateFileGridView(form);
            fileGridView.Margin = new Padding(0, 5, 0, 5);
            InitFileGridViewColumns(fileGridView);
            LoadFiles(fileGridView, folderPath, pathPanel); // 加载文件+文件夹
            fileGridView.CellContentClick += (s, e) =>
                OnFolderRowClick(s, e, fileGridView, pathPanel); // 文件夹点击事件
            tablePanel.Controls.Add(fileGridView, 0, 1);

            // 3. 按钮面板（支持删除文件/文件夹、返回上级等）
            var btnPanel = CreateButtonPanel(form, folderPath, tabName, fileGridView, toolTip, onBatchBackup, pathPanel);
            btnPanel.Margin = new Padding(0, 5, 0, 0);
            tablePanel.Controls.Add(btnPanel, 0, 2);

            tabPage.Controls.Add(tablePanel);
            return tabPage;
        }

        // 创建路径显示面板
        private static Panel CreatePathPanel(FolderManagerForm form, string initialFolderPath, string tabName)
        {
            var panel = new Panel
            {
                BackColor = form.LightGray,
                Dock = DockStyle.Fill,
                Padding = new Padding(5),
                Tag = initialFolderPath // 存储当前浏览路径
            };

            var exists = Directory.Exists(initialFolderPath);
            var txtPath = ControlFactory.CreatePathTextBox(form, initialFolderPath, exists);

            if (!exists)
            {
                txtPath.Text += "（目录不存在，点击创建）";
                txtPath.Cursor = Cursors.Hand;
                txtPath.Click += (s, e) =>
                    FolderOperations.CreateFolderIfNotExists(initialFolderPath, tabName);
            }

            txtPath.Dock = DockStyle.Fill;
            txtPath.Padding = new Padding(3);
            panel.Controls.Add(txtPath);
            return panel;
        }

        // 初始化表格列（新增“类型”列）
        private static void InitFileGridViewColumns(DataGridView dgv)
        {
            dgv.Columns.Add("Type", "类型");
            dgv.Columns["Type"].Width = 80;
            dgv.Columns["Type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.Columns.Add("FileName", "文件名");
            dgv.Columns["FileName"].Width = 300;

            dgv.Columns.Add("FileSize", "大小(KB)");
            dgv.Columns["FileSize"].Width = 100;
            dgv.Columns["FileSize"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgv.Columns.Add("LastWriteTime", "修改时间");
            dgv.Columns["LastWriteTime"].Width = 160;
        }

        // 加载文件和文件夹
        private static void LoadFiles(DataGridView dgv, string targetFolderPath, Panel pathPanel)
        {
            dgv.Rows.Clear();
            UpdatePathTextBox(pathPanel, targetFolderPath); // 更新路径显示

            if (!Directory.Exists(targetFolderPath))
            {
                dgv.Rows.Add("", "当前目录不存在", "-", "-");
                return;
            }

            try
            {
                // 加载子文件夹（蓝色粗体标识）
                foreach (var dirPath in Directory.GetDirectories(targetFolderPath))
                {
                    var dirInfo = new DirectoryInfo(dirPath);
                    var rowIndex = dgv.Rows.Add(
                        "文件夹",
                        dirInfo.Name,
                        "-",
                        dirInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                    );
                    dgv.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(52, 152, 219);
                    dgv.Rows[rowIndex].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                    dgv.Rows[rowIndex].Tag = dirInfo.FullName; // 存储完整路径
                }

                // 加载文件（高亮今日修改项）
                foreach (var filePath in Directory.GetFiles(targetFolderPath))
                {
                    var fileInfo = new FileInfo(filePath);
                    var rowIndex = dgv.Rows.Add(
                        "文件",
                        fileInfo.Name,
                        (fileInfo.Length / 1024.0).ToString("F2"),
                        fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                    );
                    dgv.Rows[rowIndex].Tag = fileInfo.FullName;
                    if (fileInfo.LastWriteTime.Date == DateTime.Today)
                    {
                        dgv.Rows[rowIndex].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                dgv.Rows.Add("", $"权限不足：{ex.Message}", "-", "-");
                MessageBox.Show("无权限访问目录", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                dgv.Rows.Add("", $"加载失败：{ex.Message}", "-", "-");
            }
        }

        // 点击文件夹行进入子目录
        private static void OnFolderRowClick(object sender, DataGridViewCellEventArgs e, DataGridView dgv, Panel pathPanel)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgv.Columns["FileName"].Index)
                return;

            var row = dgv.Rows[e.RowIndex];
            if (row.Cells["Type"].Value?.ToString() == "文件夹")
            {
                var subPath = row.Tag?.ToString();
                if (!string.IsNullOrEmpty(subPath))
                {
                    LoadFiles(dgv, subPath, pathPanel);
                }
            }
        }

        // 创建按钮面板（含删除文件夹、返回上级等功能）
        private static Panel CreateButtonPanel(FolderManagerForm form, string rootFolderPath, string tabName,
                                              DataGridView dgv, ToolTip toolTip, Action onBatchBackup, Panel pathPanel)
        {
            // 刷新按钮
            var btnRefresh = ControlFactory.CreateButton(form, "刷新", form.PrimaryColor, 80);
            btnRefresh.Click += (s, e) =>
            {
                var currentPath = pathPanel.Tag?.ToString() ?? rootFolderPath;
                LoadFiles(dgv, currentPath, pathPanel);
            };

            // 返回上级按钮
            var btnBack = ControlFactory.CreateButton(form, "返回上级", form.SecondaryColor, 80);
            btnBack.Click += (s, e) =>
            {
                var currentPath = pathPanel.Tag?.ToString() ?? rootFolderPath;
                var parent = Directory.GetParent(currentPath);
                if (parent != null)
                {
                    LoadFiles(dgv, parent.FullName, pathPanel);
                }
                else
                {
                    MessageBox.Show("已到根目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // 打开目录按钮
            var btnOpen = ControlFactory.CreateButton(form, "打开目录", form.SecondaryColor, 80);
            btnOpen.Click += (s, e) =>
            {
                var currentPath = pathPanel.Tag?.ToString() ?? rootFolderPath;
                FolderOperations.OpenFolder(currentPath, tabName);
            };

            // 添加文件按钮
            var btnAdd = ControlFactory.CreateButton(form, "添加文件", form.SuccessColor, 80);
            btnAdd.Click += (s, e) =>
            {
                var currentPath = pathPanel.Tag?.ToString() ?? rootFolderPath;
                FolderOperations.AddFileToFolder(currentPath, tabName);
                LoadFiles(dgv, currentPath, pathPanel);
            };

            // 删除选中按钮（支持文件和文件夹）
            var btnDelete = ControlFactory.CreateButton(form, "删除选中", form.DangerColor, 80);
            btnDelete.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选中文件或文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var row = dgv.SelectedRows[0];
                var type = row.Cells["Type"].Value?.ToString();
                var name = row.Cells["FileName"].Value?.ToString();
                var currentPath = pathPanel.Tag?.ToString() ?? rootFolderPath;

                if (type == "文件")
                {
                    FolderOperations.DeleteFile(currentPath, name, tabName);
                }
                else if (type == "文件夹")
                {
                    FolderOperations.DeleteFolder(currentPath, name, tabName);
                }

                LoadFiles(dgv, currentPath, pathPanel);
            };

            // 批量备份按钮
            var btnBatchBackup = ControlFactory.CreateButton(form, "批量备份所有目录", form.PrimaryColor, 150);
            btnBatchBackup.Click += (s, e) => onBatchBackup();

            // 按钮容器布局
            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(5),
                BackColor = Color.White
            };

            var buttons = new[] { btnRefresh, btnBack, btnOpen, btnAdd, btnDelete, btnBatchBackup };
            foreach (var btn in buttons)
            {
                btn.Margin = new Padding(0, 8, 12, 8);
                btn.FlatAppearance.BorderSize = 0;
                flowPanel.Controls.Add(btn);
            }

            var outerPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(5) };
            outerPanel.Controls.Add(flowPanel);
            return outerPanel;
        }

        // 更新路径文本框显示
        private static void UpdatePathTextBox(Panel pathPanel, string currentPath)
        {
            var txtPath = pathPanel.Controls.OfType<TextBox>().FirstOrDefault();
            if (txtPath != null)
            {
                txtPath.Text = currentPath;
                pathPanel.Tag = currentPath;
            }
        }
    }
}