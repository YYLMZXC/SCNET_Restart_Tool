using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SCNET_Restart_Tool;

namespace SCNET_Restart_Tool.Avalonia;

/// <summary>
/// 应用入口类：初始化 XAML 资源、创建主窗口，并把「确认 / 提示 / 选择文件」的 UI 能力
/// 注入到与界面无关的静态工具类 FolderOperations 中（与原 Uno 版设计保持一致）。
/// </summary>
public partial class App : Application
{
    /// <summary>当前主窗口（对话框与文件夹选择器需要以它作为宿主/所有者）</summary>
    public static Window? MainWindow { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindow = new MainWindow();
            desktop.MainWindow = MainWindow;
            desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        // 将静态工具类的 UI 交互委托指向 DialogService（统一对话框入口）
        FolderOperations.ConfirmAsync = (message, title) => DialogService.ConfirmAsync(message, title);
        FolderOperations.NotifyAsync = (message, title) => DialogService.ShowAsync(title, message);
        FolderOperations.PickFileAsync = (title, filterName, extensions) => DialogService.PickFileAsync(title, filterName, extensions);

        LogManager.GetInstance().AddLog("系统", "SCNET 服务端管理工具（Avalonia 版）已启动");

        base.OnFrameworkInitializationCompleted();
    }
}
