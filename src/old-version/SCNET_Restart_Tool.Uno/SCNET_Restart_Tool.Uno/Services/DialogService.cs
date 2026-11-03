using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace SCNET_Restart_Tool.Uno;

/// <summary>
/// 对话框服务：统一封装「信息提示」「确认询问」「文件选择」对话框。
/// 将对话框的创建、平台差异处理从各页面代码后置（code-behind）中剥离，
/// 供 MainPage、FolderManagerPage 及静态工具类（FolderOperations 的委托注入）复用。
/// 页面仅需提供 XamlRoot，无需关心对话框的具体实现细节。
/// </summary>
public static class DialogService
{
    /// <summary>
    /// 显示信息对话框（带“确定”按钮）。
    /// </summary>
    /// <param name="root">XamlRoot，ContentDialog 必须挂载到有效的 XamlRoot 才能显示</param>
    /// <param name="title">对话框标题</param>
    /// <param name="content">对话框内容文本</param>
    public static async Task ShowAsync(XamlRoot root, string title, string content)
    {
        var dialog = CreateDialog(root, title, content, isPrimary: false);
        await dialog.ShowAsync();
    }

    /// <summary>
    /// 显示确认对话框（“是/否”按钮）。
    /// </summary>
    /// <param name="root">XamlRoot，ContentDialog 必须挂载到有效的 XamlRoot 才能显示</param>
    /// <param name="message">确认询问的内容文本</param>
    /// <param name="title">对话框标题</param>
    /// <returns>用户点击“是”返回 true，点击“否”或关闭返回 false</returns>
    public static async Task<bool> ConfirmAsync(XamlRoot root, string message, string title)
    {
        var dialog = CreateDialog(root, title, message, isPrimary: true);
        var result = await dialog.ShowAsync();
        return result == ContentDialogResult.Primary;
    }

    /// <summary>
    /// 选择单个文件（当前仅 Windows 桌面端支持，其他平台返回空字符串）。
    /// Uno 桌面端必须通过窗口句柄初始化文件选择器，否则无法弹出；
    /// 这里统一从 App.MainWindow 获取句柄，页面层无需关心平台差异。
    /// </summary>
    /// <param name="root">XamlRoot（保留参数，保证签名一致）</param>
    /// <param name="title">对话框标题（当前平台的文件选择器标题支持有限，保留以备扩展）</param>
    /// <param name="filterName">文件类型筛选名称（如 "可执行文件"）</param>
    /// <param name="extensions">允许选择的扩展名列表（如 ".exe"）</param>
    /// <returns>选中文件的完整路径；用户取消或平台不支持时返回空字符串</returns>
    public static async Task<string> PickFileAsync(XamlRoot root, string title, string filterName, string[] extensions)
    {
#if WINDOWS
        var picker = new Windows.Storage.Pickers.FileOpenPicker();

        // Uno 桌面端需要初始化窗口句柄，否则选择器不会弹出
        var window = (App.Current as App)?.MainWindow;
        if (window != null)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        }

        picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
        if (extensions != null && extensions.Length > 0)
        {
            foreach (var ext in extensions)
            {
                picker.FileTypeFilter.Add(ext == "*" ? "*" : ext);
            }
        }

        var file = await picker.PickSingleFileAsync();
        return file?.Path ?? "";
#else
        // 非 Windows 平台暂不支持文件选择器，返回空字符串由调用方处理
        return "";
#endif
    }

    /// <summary>
    /// 创建 ContentDialog 的公共构建逻辑。
    /// </summary>
    /// <param name="root">XamlRoot，对话框的宿主</param>
    /// <param name="title">标题</param>
    /// <param name="content">内容文本</param>
    /// <param name="isPrimary">是否为确认对话框（带“是/否”按钮）；false 时仅显示“确定”按钮</param>
    private static ContentDialog CreateDialog(XamlRoot root, string title, string content, bool isPrimary)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = content,
            XamlRoot = root
        };

        if (isPrimary)
        {
            // 确认对话框：主按钮“是” + 关闭按钮“否”
            dialog.PrimaryButtonText = "是";
            dialog.CloseButtonText = "否";
        }
        else
        {
            // 信息对话框：仅关闭按钮“确定”
            dialog.CloseButtonText = "确定";
        }

        return dialog;
    }
}
