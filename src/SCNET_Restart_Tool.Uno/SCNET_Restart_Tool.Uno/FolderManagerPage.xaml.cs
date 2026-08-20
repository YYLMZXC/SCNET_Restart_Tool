using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SCNET_Restart_Tool;
using SCNET_Restart_Tool.Uno.Controls;

namespace SCNET_Restart_Tool.Uno;

/// <summary>
/// 文件夹管理器页面（替代 FolderManagerForm）
/// </summary>
public sealed partial class FolderManagerPage : Page
{
    private ServerConfig? _currentServer;
    private readonly LogManager _logManager = LogManager.GetInstance();

    public FolderManagerPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is ServerConfig server)
        {
            _currentServer = server;
            RefreshServer(server);
        }
    }

    /// <summary>刷新服务端并重建标签页</summary>
    public void RefreshServer(ServerConfig server)
    {
        _currentServer = server;
        serverNameText.Text = $"当前服务端：{(_currentServer?.Name ?? "未选择服务端")}";
        InitFolderTabs();
    }

    private void InitFolderTabs()
    {
        folderTabView.TabItems.Clear();

        var server = _currentServer;
        if (server == null)
        {
            var emptyTab = new TabViewItem { Header = "提示" };
            emptyTab.Content = new TextBlock
            {
                Text = "请先在主窗口选中服务端！",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray)
            };
            folderTabView.TabItems.Add(emptyTab);
            return;
        }

        AddTab("Bugs", new BugsLogControl(server, server.BugsLogPath));
        AddTab("CharacterSkins", new FolderBrowserControl("CharacterSkins", server.CharacterSkinsPath));
        AddTab("Configs", new FolderBrowserControl("Configs", server.ConfigsPath));
        AddTab("TexturePacks", new FolderBrowserControl("TexturePacks", server.TexturePacksPath));
        AddTab("NetMods", new FolderBrowserControl("NetMods", server.NetModsPath));
        AddTab("Plugins", new FolderBrowserControl("Plugins", server.PluginsPath));
        AddTab("Worlds", new FolderBrowserControl("Worlds", server.WorldsPath));
    }

    private void AddTab(string header, object content)
    {
        var tab = new TabViewItem { Header = header, Content = content };
        folderTabView.TabItems.Add(tab);
    }

    private async void BatchBackupBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_currentServer == null) return;
        await FolderOperations.BatchBackupAllFoldersAsync(_currentServer);
    }

    private void BackBtn_Click(object sender, RoutedEventArgs e)
    {
        if (Frame.CanGoBack)
        {
            Frame.GoBack();
        }
        else
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
