using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SCNET_Restart_Tool;
using SCNET_Restart_Tool.Avalonia.Controls;

namespace SCNET_Restart_Tool.Avalonia.Views;

/// <summary>
/// 文件夹管理视图（替代原 Uno 版的 FolderManagerPage / WinForms 版 FolderManagerForm）。
/// 每个资源目录对应一个标签页：Bugs 目录使用专用的日志控件，其余目录使用通用文件夹浏览控件。
/// </summary>
public partial class FolderManagerView : UserControl
{
    private readonly LogManager _logManager = LogManager.GetInstance();

    /// <summary>当前管理的服务器</summary>
    public ServerConfig? CurrentServer { get; }

    /// <summary>请求返回主界面（由宿主窗口负责切换视图）</summary>
    public event EventHandler? BackRequested;

    public FolderManagerView(ServerConfig server)
    {
        InitializeComponent();
        CurrentServer = server;
        RefreshServer(server);
    }

    /// <summary>刷新服务端信息并重建标签页</summary>
    public void RefreshServer(ServerConfig server)
    {
        serverNameText.Text = $"当前服务端：{server.Name}";
        InitFolderTabs(server);
    }

    private void InitFolderTabs(ServerConfig server)
    {
        folderTabControl.Items.Clear();

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
        folderTabControl.Items.Add(new TabItem { Header = header, Content = content });
    }

    private async void BatchBackupBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (CurrentServer == null) return;
        await FolderOperations.BatchBackupAllFoldersAsync(CurrentServer);
    }

    private void BackBtn_Click(object? sender, RoutedEventArgs e)
    {
        _logManager.AddLog("系统", "返回主界面");
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
