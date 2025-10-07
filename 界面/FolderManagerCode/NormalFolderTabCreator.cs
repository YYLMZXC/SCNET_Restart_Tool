using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    internal static class NormalFolderTabCreator
    {
        // 创建普通文件夹标签页
        public static TabPage Create(FolderManagerForm form, string tabName, string folderPath, ToolTip toolTip)
        {
            var tabPage = new TabPage(tabName);
            tabPage.Tag = folderPath;
            tabPage.Padding = new Padding(10);
            tabPage.BackColor = Color.White;

            // 主容器
            var mainPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };

            // 1. 路径显示区域
            var pathPanel = CreatePathPanel(form, folderPath, tabName);
            mainPanel.Controls.Add(pathPanel);

            // 2. 文件列表
            var fileGridView = ControlFactory.CreateFileGridView(form);
            InitFileGridViewColumns(fileGridView);
            LoadFiles(fileGridView, folderPath);
            mainPanel.Controls.Add(fileGridView);

            // 3. 按钮区域
            var btnPanel = CreateButtonPanel(form, folderPath, tabName, fileGridView, toolTip);
            mainPanel.Controls.Add(btnPanel);

            // 布局调整
            pathPanel.Dock = DockStyle.Top;
            pathPanel.Height = 40;
            btnPanel.Dock = DockStyle.Bottom;
            fileGridView.Dock = DockStyle.Fill;

            tabPage.Controls.Add(mainPanel);
            return tabPage;
        }

        // 创建路径显示面板
        private static Panel CreatePathPanel(FolderManagerForm form, string folderPath, string tabName)
        {
            var panel = new Panel { BackColor = form.LightGray };
            var exists = Directory.Exists(folderPath);
            var txtPath = ControlFactory.CreatePathTextBox(form, folderPath, exists);

            if (!exists)
            {
                txtPath.Text += "（目录不存在，点击创建）";
                txtPath.Cursor = Cursors.Hand;
                txtPath.Click += (s, e) => FolderOperations.CreateFolderIfNotExists(folderPath, tabName);
            }

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

        // 创建按钮面板（支持完整操作）
        private static Panel CreateButtonPanel(FolderManagerForm form, string folderPath, string tabName, DataGridView dgv, ToolTip toolTip)
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

            return ControlFactory.CreateButtonPanel(form, btnRefresh, btnOpen, btnAdd, btnDelete);
        }
    }
}