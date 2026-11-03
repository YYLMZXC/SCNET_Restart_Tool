using Avalonia;

namespace SCNET_Restart_Tool.Avalonia;

/// <summary>
/// 应用程序入口。Avalonia 桌面端使用经典桌面生命周期（StartWithClassicDesktopLifetime）。
/// </summary>
internal static class Program
{
    /// <summary>入口点（GUI 程序不显示控制台窗口）</summary>
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    /// <summary>构建 Avalonia 应用配置（供入口点与设计器共用）</summary>
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}
