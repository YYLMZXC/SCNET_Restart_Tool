using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using SCNET_Restart_Tool;

namespace SCNET_Restart_Tool.Avalonia;

/// <summary>
/// 对话框服务：统一封装「信息提示」「确认询问」「文件选择」三类 UI 交互。
/// 页面与自定义控件只调用本服务，无需关心 Avalonia 窗口/存储提供者的具体细节。
/// （对应原 Uno 版的 ContentDialog + FileOpenPicker。）
/// </summary>
public static class DialogService
{
    /// <summary>对话框所有者窗口（主窗口）；无窗口时返回 null</summary>
    private static Window? Owner => App.MainWindow;

    /// <summary>显示信息提示对话框（仅「确定」按钮）</summary>
    public static async Task ShowAsync(string title, string content)
    {
        var dialog = new DialogWindow(title, content, isConfirm: false);
        await ShowCoreAsync(dialog);
    }

    /// <summary>显示确认对话框（「是 / 否」按钮），点击「是」返回 true</summary>
    public static async Task<bool> ConfirmAsync(string message, string title)
    {
        var dialog = new DialogWindow(title, message, isConfirm: true);
        return await ShowCoreAsync(dialog);
    }

    /// <summary>
    /// 选择单个文件，返回文件完整路径；用户取消时返回空字符串。
    /// 使用 Avalonia 11 的 StorageProvider，Windows / macOS / Linux 通用。
    /// </summary>
    public static async Task<string> PickFileAsync(string title, string filterName, string[] extensions)
    {
        var owner = Owner;
        if (owner is null) return "";

        try
        {
            var patterns = (extensions ?? Array.Empty<string>())
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => e == "*" ? "*" : e.StartsWith('.') ? "*" + e : e)
                .ToArray();

            var options = new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = patterns.Length == 0
                    ? null
                    : new[] { new FilePickerFileType(filterName) { Patterns = patterns } }
            };

            var files = await owner.StorageProvider.OpenFilePickerAsync(options);
            var file = files.FirstOrDefault();
            if (file is null) return "";

            return file.TryGetLocalPath() ?? file.Path.LocalPath;
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog("系统", $"选择文件失败：{ex.Message}", "错误");
            return "";
        }
    }

    /// <summary>显示对话框并等待关闭；有主窗口时使用模态对话框</summary>
    private static async Task<bool> ShowCoreAsync(DialogWindow dialog)
    {
        var owner = Owner;
        if (owner is not null && owner.IsVisible)
        {
            return await dialog.ShowDialog<bool>(owner);
        }

        // 兜底：主窗口尚未就绪时以非模态方式显示，避免抛异常
        dialog.Show();
        return false;
    }
}
