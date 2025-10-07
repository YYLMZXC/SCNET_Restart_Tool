using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    internal static class BugsLogTabCreator
    {
        private const string GameLogFileName = "Game.log";
        private const int PageLineCount = 1000; // 每页行数
        private static int _currentPage = 1;
        private static int _totalPages = 1;
        private static List<string> _logLinesCache = new List<string>();

        // 创建Bugs日志标签页（接收备份按钮事件）
        public static TabPage Create(FolderManagerForm form, string folderPath, ToolTip toolTip, Action onBatchBackup)
        {
            var tabPage = new TabPage("Bugs");
            tabPage.Tag = folderPath;
            tabPage.Padding = new Padding(10);
            tabPage.BackColor = Color.White;

            // 主容器
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            var panelContainer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };

            // 1. 路径显示区域
            var pathPanel = CreatePathPanel(form, folderPath);
            panelContainer.Controls.Add(pathPanel);

            // 2. 日志预览+文件列表区域
            var splitPanel = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 600,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelContainer.Controls.Add(splitPanel);

            // 左侧：Game.log预览区
            splitPanel.Panel1.Controls.Add(CreateLogPreviewArea(form, folderPath));

            // 右侧：其他文件列表
            var fileGridView = ControlFactory.CreateFileGridView(form);
            InitFileGridViewColumns(fileGridView);
            LoadOtherFiles(fileGridView, folderPath);
            splitPanel.Panel2.Controls.Add(fileGridView);

            // 3. 按钮区域（包含备份按钮）
            var btnPanel = CreateButtonPanel(form, folderPath, fileGridView, toolTip, onBatchBackup);
            panelContainer.Controls.Add(btnPanel);

            // 布局调整
            pathPanel.Dock = DockStyle.Top;
            pathPanel.Height = 40;
            btnPanel.Dock = DockStyle.Bottom;
            splitPanel.Dock = DockStyle.Fill;

            mainPanel.Controls.Add(panelContainer);
            tabPage.Controls.Add(mainPanel);

            return tabPage;
        }

        // 创建路径显示面板
        private static Panel CreatePathPanel(FolderManagerForm form, string folderPath)
        {
            var panel = new Panel { BackColor = form.LightGray };
            var exists = Directory.Exists(folderPath);
            var txtPath = ControlFactory.CreatePathTextBox(form, folderPath, exists);

            if (!exists)
            {
                txtPath.Text += "（目录不存在，点击创建）";
                txtPath.Cursor = Cursors.Hand;
                txtPath.Click += (s, e) => FolderOperations.CreateFolderIfNotExists(folderPath, "Bugs");
            }
            else if (File.Exists(Path.Combine(folderPath, GameLogFileName)))
            {
                txtPath.Text += "（Game.log为只读日志，支持分页预览）";
            }

            panel.Controls.Add(txtPath);
            return panel;
        }

        // 创建日志预览区域
        private static Panel CreateLogPreviewArea(FolderManagerForm form, string folderPath)
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            var logPath = Path.Combine(folderPath, GameLogFileName);

            // 日志显示框
            var rtbLog = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both,
                BorderStyle = BorderStyle.FixedSingle
            };
            panel.Controls.Add(rtbLog);

            // 分页控制栏
            var pagePanel = new Panel { Dock = DockStyle.Bottom, Height = 30, BackColor = form.LightGray };
            var btnPrev = ControlFactory.CreateSmallButton(form, "上一页", form.SecondaryColor, 70);
            var btnNext = ControlFactory.CreateSmallButton(form, "下一页", form.SecondaryColor, 70);
            var lblPage = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "未加载日志"
            };

            pagePanel.Controls.Add(btnPrev);
            pagePanel.Controls.Add(lblPage);
            pagePanel.Controls.Add(btnNext);
            panel.Controls.Add(pagePanel);

            // 加载日志
            if (File.Exists(logPath))
            {
                LoadLogFileAsync(logPath, rtbLog, lblPage);
            }
            else
            {
                rtbLog.Text = $"未找到 {GameLogFileName} 或文件被占用";
                btnPrev.Enabled = false;
                btnNext.Enabled = false;
            }

            // 分页事件
            btnPrev.Click += (s, e) =>
            {
                if (_currentPage > 1) _currentPage--;
                UpdateLogPage(rtbLog, lblPage);
            };
            btnNext.Click += (s, e) =>
            {
                if (_currentPage < _totalPages) _currentPage++;
                UpdateLogPage(rtbLog, lblPage);
            };

            return panel;
        }

        // 异步加载大日志文件
        private static async void LoadLogFileAsync(string path, RichTextBox rtb, Label lblPage)
        {
            try
            {
                rtb.Text = "正在加载日志...";
                _logLinesCache.Clear();

                using (var reader = new StreamReader(path))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        _logLinesCache.Add(line);
                    }
                }

                _totalPages = (int)Math.Ceiling(_logLinesCache.Count / (double)PageLineCount);
                _currentPage = 1;
                UpdateLogPage(rtb, lblPage);
            }
            catch (Exception ex)
            {
                rtb.Text = $"加载失败：{ex.Message}（文件可能被占用）";
            }
        }

        // 更新日志分页显示
        private static void UpdateLogPage(RichTextBox rtb, Label lblPage)
        {
            var start = (_currentPage - 1) * PageLineCount;
            var end = Math.Min(start + PageLineCount, _logLinesCache.Count);
            var pageLines = new string[end - start];
            _logLinesCache.CopyTo(start, pageLines, 0, end - start);

            rtb.Lines = pageLines;
            lblPage.Text = $"第 {_currentPage}/{_totalPages} 页（共 {_logLinesCache.Count} 行）";
        }

        // 初始化文件列表列
        private static void InitFileGridViewColumns(DataGridView dgv)
        {
            dgv.Columns.Add("FileName", "文件名");
            dgv.Columns.Add("FileSize", "大小(KB)");
            dgv.Columns.Add("LastWriteTime", "修改时间");
            dgv.Columns["FileName"].Width = 180;
            dgv.Columns["FileSize"].Width = 80;
            dgv.Columns["LastWriteTime"].Width = 140;
            dgv.Columns["FileSize"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        // 加载除Game.log外的其他文件
        private static void LoadOtherFiles(DataGridView dgv, string folderPath)
        {
            dgv.Rows.Clear();
            if (!Directory.Exists(folderPath)) return;

            foreach (var filePath in Directory.GetFiles(folderPath))
            {
                var fileName = Path.GetFileName(filePath);
                if (fileName.Equals(GameLogFileName, StringComparison.OrdinalIgnoreCase))
                    continue;

                var fileInfo = new FileInfo(filePath);
                dgv.Rows.Add(
                    fileName,
                    (fileInfo.Length / 1024.0).ToString("F2"),
                    fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                );
            }
        }

        // 创建按钮面板（包含备份按钮）
        private static Panel CreateButtonPanel(FolderManagerForm form, string folderPath, DataGridView dgv, ToolTip toolTip, Action onBatchBackup)
        {
            // 刷新按钮
            var btnRefresh = ControlFactory.CreateButton(form, "刷新", form.PrimaryColor, 80);
            btnRefresh.Click += (s, e) => LoadOtherFiles(dgv, folderPath);

            // 打开目录
            var btnOpen = ControlFactory.CreateButton(form, "打开目录", form.SecondaryColor, 80);
            btnOpen.Click += (s, e) => FolderOperations.OpenFolder(folderPath, "Bugs");

            // 添加文件（禁用）
            var btnAdd = ControlFactory.CreateButton(form, "添加文件", form.SuccessColor, 80);
            btnAdd.Enabled = false;
            toolTip.SetToolTip(btnAdd, "Bugs目录为系统日志目录，禁止手动添加");

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
                FolderOperations.DeleteFile(folderPath, fileName, "Bugs");
                LoadOtherFiles(dgv, folderPath);
            };

            // 清理日志按钮
            var btnClean = ControlFactory.CreateButton(form, "清理过期日志", form.WarningColor, 120);
            btnClean.Click += (s, e) =>
            {
                FolderOperations.CleanBugsLog(folderPath, GameLogFileName);
                LoadOtherFiles(dgv, folderPath);
            };

            // 批量备份按钮（整合到按钮组）
            var btnBatchBackup = ControlFactory.CreateButton(form, "批量备份所有目录", form.PrimaryColor, 150);
            btnBatchBackup.Click += (s, e) => onBatchBackup();

            // 所有按钮放入同一面板（自动换行）
            return ControlFactory.CreateButtonPanel(form,
                btnRefresh, btnOpen, btnAdd, btnDelete, btnClean, btnBatchBackup);
        }
    }
}