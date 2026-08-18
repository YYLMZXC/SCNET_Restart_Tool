using System;
using System.Drawing;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public partial class FolderManagerForm : System.Windows.Forms.Form
    {
        private ServerConfig _currentServer;
        private readonly LogManager _logManager;
        private string _activeFolderPath;

        // 配色方案定义
        internal readonly Color PrimaryColor = Color.FromArgb(52, 152, 219);
        internal readonly Color SecondaryColor = Color.FromArgb(41, 128, 185);
        internal readonly Color LightGray = Color.FromArgb(245, 245, 245);
        internal readonly Color DarkGray = Color.FromArgb(70, 70, 70);
        internal readonly Color SuccessColor = Color.FromArgb(46, 204, 113);
        internal readonly Color WarningColor = Color.FromArgb(241, 196, 15);
        internal readonly Color DangerColor = Color.FromArgb(231, 76, 60);

        // 用于按钮提示的ToolTip
        private readonly ToolTip _toolTip = new ToolTip();

        public FolderManagerForm(ServerConfig selectedServer)
        {
            InitializeComponent();
            _currentServer = selectedServer;
            _logManager = LogManager.GetInstance();
            InitStyle();
            InitFolderTabs();
            UpdateWindowTitle();
            StartPosition = FormStartPosition.CenterScreen;
        }

        // 初始化界面样式
        private void InitStyle()
        {
            BackColor = Color.White;
            Font = new Font("微软雅黑", 9F);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(800, 600);

            // 标签页样式
            tabControlFolders.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlFolders.DrawItem += TabControlFolders_DrawItem;
            tabControlFolders.ItemSize = new Size(120, 35);
            tabControlFolders.Padding = new Point(10, 5);
            tabControlFolders.BackColor = LightGray;
        }

        // 自定义标签页绘制
        private void TabControlFolders_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabPage = tabControl?.TabPages[e.Index];
            if (tabPage == null) return;

            // 背景绘制
            using (var brush = new SolidBrush(e.State.HasFlag(DrawItemState.Selected) ? Color.White : LightGray))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            // 选中下划线
            if (e.State.HasFlag(DrawItemState.Selected))
            {
                using (var pen = new Pen(PrimaryColor, 3))
                {
                    e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
                }
            }

            // 文本绘制
            var textColor = e.State.HasFlag(DrawItemState.Selected) ? DarkGray : Color.Gray;
            using (var brush = new SolidBrush(textColor))
            {
                var textRect = new Rectangle(e.Bounds.Left + 5, e.Bounds.Top + 5,
                                            e.Bounds.Width - 10, e.Bounds.Height - 10);
                e.Graphics.DrawString(tabPage.Text, tabControl.Font, brush, textRect);
            }
        }

        // 公共刷新方法
        public void RefreshServer(ServerConfig newServer) => RefreshForm(newServer);
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

        // 初始化标签页（核心入口）
        private void InitFolderTabs()
        {
            tabControlFolders.SelectedIndexChanged -= TabControlFolders_SelectedIndexChanged;
            tabControlFolders.TabPages.Clear();

            if (_currentServer == null)
            {
                tabControlFolders.TabPages.Add(CreateEmptyTab());
                return;
            }

            // 批量备份委托（统一逻辑）
            Action batchBackupAction = () =>
            {
                if (_currentServer != null)
                    FolderOperations.BatchBackupAllFolders(_currentServer);
            };

            // 添加各功能标签页（传递备份按钮事件）
            tabControlFolders.TabPages.Add(
                BugsLogTabCreator.Create(this, _currentServer.BugsLogPath, _toolTip, batchBackupAction)
            );
            tabControlFolders.TabPages.Add(
                NormalFolderTabCreator.Create(this, "CharacterSkins", _currentServer.CharacterSkinsPath, _toolTip, batchBackupAction)
            );
            tabControlFolders.TabPages.Add(
                NormalFolderTabCreator.Create(this, "Configs", _currentServer.ConfigsPath, _toolTip, batchBackupAction)
            );
            tabControlFolders.TabPages.Add(
                NormalFolderTabCreator.Create(this, "TexturePacks", _currentServer.TexturePacksPath, _toolTip, batchBackupAction)
            );
            tabControlFolders.TabPages.Add(
                NormalFolderTabCreator.Create(this, "NetMods", _currentServer.NetModsPath, _toolTip, batchBackupAction)
            );
            tabControlFolders.TabPages.Add(
                NormalFolderTabCreator.Create(this, "Plugins", _currentServer.PluginsPath, _toolTip, batchBackupAction)
            );
            tabControlFolders.TabPages.Add(
                NormalFolderTabCreator.Create(this, "Worlds", _currentServer.WorldsPath, _toolTip, batchBackupAction)
            );

            tabControlFolders.SelectedIndexChanged += TabControlFolders_SelectedIndexChanged;
        }

        // 创建空状态标签页
        private TabPage CreateEmptyTab()
        {
            var emptyTab = new TabPage("提示");
            emptyTab.BackColor = Color.White;
            emptyTab.Controls.Add(new Label
            {
                Text = "请先在主窗口选中服务端！",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("微软雅黑", 12F),
                ForeColor = DarkGray
            });
            return emptyTab;
        }

        // 标签页切换事件
        private void TabControlFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlFolders.SelectedTab?.Tag != null)
            {
                _activeFolderPath = tabControlFolders.SelectedTab.Tag.ToString();
            }
        }
    }
}