using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using SCNET_Restart_Tool;
using SCNET_Restart_Tool.Avalonia.Views;

namespace SCNET_Restart_Tool.Avalonia;

/// <summary>
/// 主窗口：以 ContentControl 承载视图，实现「主界面 ↔ 文件夹管理」的导航
/// （对应原 Uno 版的 Frame.Navigate / Frame.GoBack）。
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainView _mainView = new();

    public MainWindow()
    {
        InitializeComponent();
        TryApplyWindowIcon();

        _mainView.FolderManagerRequested += OnFolderManagerRequested;
        RootContent.Content = _mainView;
    }

    /// <summary>切换到文件夹管理视图</summary>
    private void OnFolderManagerRequested(object? sender, ServerConfig server)
    {
        var view = new FolderManagerView(server);
        view.BackRequested += (_, _) => RootContent.Content = _mainView;
        RootContent.Content = view;
    }

    /// <summary>应用窗口图标（资源缺失时静默忽略，不影响程序运行）</summary>
    private void TryApplyWindowIcon()
    {
        try
        {
            Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://SCNET_Restart_Tool.Avalonia/Assets/app.ico")));
        }
        catch (Exception)
        {
            // 图标仅影响外观，失败无需中断启动
        }
    }
}
