using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    internal static class NormalFolderTabCreator
    {
        // 创建普通文件夹标签页（接收备份按钮事件）
        public static TabPage Create(FolderManagerForm form, string tabName, string folderPath, ToolTip toolTip, Action onBatchBackup)
        {
            var tabPage = new TabPage(tabName);
            tabPage.Tag = folderPath;
            tabPage.Padding = new Padding(10);
            tabPage.BackColor = Color.White;

            // 用TableLayoutPanel按行划分区域：行1（路径）、行2（文件列表）、行3（按钮）
            var tablePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1, // 仅1列，垂直排列
                RowCount = 3,
                RowStyles =
        {
            new RowStyle(SizeType.Absolute, 30F), // 路径行高度30
            new RowStyle(SizeType.Percent, 100F), // 文件列表占剩余高度
            new RowStyle(SizeType.Absolute, 60F)  // 按钮行高度60
        }
            };

            // 1. 路径显示区域（第1行）
            var pathPanel = CreatePathPanel(form, folderPath, tabName);
            tablePanel.Controls.Add(pathPanel, 0, 0);

            // 2. 文件列表（第2行）
            var fileGridView = ControlFactory.CreateFileGridView(form);
            InitFileGridViewColumns(fileGridView);
            LoadFiles(fileGridView, folderPath);
            tablePanel.Controls.Add(fileGridView, 0, 1);

            // 3. 按钮区域（第3行）
            var btnPanel = CreateButtonPanel(form, folderPath, tabName, fileGridView, toolTip, onBatchBackup);
            tablePanel.Controls.Add(btnPanel, 0, 2);

            tabPage.Controls.Add(tablePanel);
            return tabPage;
        }

        // 创建路径显示面板
        private static Panel CreatePathPanel(FolderManagerForm form, string folderPath, string tabName)
        {
            var panel = new Panel { BackColor = form.LightGray, Dock = DockStyle.Fill }; // 填充所在行
            var exists = Directory.Exists(folderPath);
            var txtPath = ControlFactory.CreatePathTextBox(form, folderPath, exists);

            if (!exists)
            {
                txtPath.Text += "（目录不存在，点击创建）";
                txtPath.Cursor = Cursors.Hand;
                txtPath.Click += (s, e) => FolderOperations.CreateFolderIfNotExists(folderPath, tabName);
            }

            txtPath.Dock = DockStyle.Fill; // 路径文本框填充路径面板
            panel.Controls.Add(txtPath);
            return panel;
        }

        // 初始化文件列表列
        private static void InitFileGridViewColumns(DataGridView dgv)
        {
            dgv.Columns.Add("FileName", "文件名");
            dgv.Columns.Add("FileSize", "大小(KB)");
            dgv.Columns.Add("LastWriteTime", "修改时间");
            dgv.Columns["FileName"].Width = 350;
            dgv.Columns["FileSize"].Width = 100;
            dgv.Columns["LastWriteTime"].Width = 160;
            dgv.Columns["FileSize"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        // 加载文件列表
        private static void LoadFiles(DataGridView dgv, string folderPath)
        {
            dgv.Rows.Clear();
            if (!Directory.Exists(folderPath)) return;

            foreach (var filePath in Directory.GetFiles(folderPath))
            {
                var fileInfo = new FileInfo(filePath);
                var rowIndex = dgv.Rows.Add(
                    fileInfo.Name,
                    (fileInfo.Length / 1024.0).ToString("F2"),
                    fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                );

                // 高亮今天修改的文件
                if (fileInfo.LastWriteTime.Date == DateTime.Today)
                {
                    dgv.Rows[rowIndex].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                }
            }
        }

        // 创建按钮面板（包含备份按钮）
        // 创建按钮面板（包含备份按钮）
        private static Panel CreateButtonPanel(FolderManagerForm form, string folderPath, string tabName, DataGridView dgv, ToolTip toolTip, Action onBatchBackup)
        {
            // 刷新按钮
            var btnRefresh = ControlFactory.CreateButton(form, "刷新", form.PrimaryColor, 80);
            btnRefresh.Click += (s, e) => LoadFiles(dgv, folderPath);

            // 打开目录
            var btnOpen = ControlFactory.CreateButton(form, "打开目录", form.SecondaryColor, 80);
            btnOpen.Click += (s, e) => FolderOperations.OpenFolder(folderPath, tabName);

            // 添加文件
            var btnAdd = ControlFactory.CreateButton(form, "添加文件", form.SuccessColor, 80);
            btnAdd.Click += (s, e) =>
            {
                FolderOperations.AddFileToFolder(folderPath, tabName);
                LoadFiles(dgv, folderPath);
            };

            // 删除文件
            var btnDelete = ControlFactory.CreateButton(form, "删除选中", form.DangerColor, 80);
            btnDelete.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选中要删除的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var fileName = dgv.SelectedRows[0].Cells["FileName"].Value.ToString();
                FolderOperations.DeleteFile(folderPath, fileName, tabName);
                LoadFiles(dgv, folderPath);
            };

            // 批量备份按钮（整合到按钮组）
            var btnBatchBackup = ControlFactory.CreateButton(form, "批量备份所有目录", form.PrimaryColor, 150);
            btnBatchBackup.Click += (s, e) => onBatchBackup();

            // 创建按钮面板容器（使用FlowLayoutPanel自动排列按钮）
            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(5),
                Margin = new Padding(0),
                BackColor = Color.White
            };

            // 添加所有按钮到FlowLayoutPanel
            foreach (var btn in new[] { btnRefresh, btnOpen, btnAdd, btnDelete, btnBatchBackup })
            {
                btn.Margin = new Padding(0, 5, 10, 5); // 按钮间距（上5，右10，下5）
                flowPanel.Controls.Add(btn);
            }

            // 外层面板（用于适配TableLayoutPanel的行）
            var outerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(5)
            };
            outerPanel.Controls.Add(flowPanel);

            return outerPanel;
        }
    }
   }