using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SCNET_Restart_Tool;

/// <summary>
/// 文件夹操作工具类（Uno 版）
/// 确认/提示/文件选择通过委托回调，由 UI 层注入实现
/// </summary>
public static class FolderOperations
{
    /// <summary>确认对话框（message, title）→ 返回是否确认</summary>
    public static Func<string, string, Task<bool>>? ConfirmAsync { get; set; }

    /// <summary>信息提示对话框（message, title）</summary>
    public static Func<string, string, Task>? NotifyAsync { get; set; }

    /// <summary>选择单个文件（title, filterName, extensions）→ 返回文件路径或 null</summary>
    public static Func<string, string, string[], Task<string>>? PickFileAsync { get; set; }

    private static async Task<bool> Confirm(string message, string title)
    {
        if (ConfirmAsync == null) return false;
        return await ConfirmAsync(message, title);
    }

    private static async Task Notify(string message, string title)
    {
        if (NotifyAsync != null) await NotifyAsync(message, title);
    }

    private static async Task<string> PickFile(string title, string filterName, string[] extensions)
    {
        if (PickFileAsync == null) return "";
        return await PickFileAsync(title, filterName, extensions);
    }

    /// <summary>创建目录（不存在时经确认后创建）</summary>
    public static async Task CreateFolderIfNotExistsAsync(string folderPath, string folderName)
    {
        if (string.IsNullOrEmpty(folderPath) || Directory.Exists(folderPath)) return;

        var confirmed = await Confirm($"{folderName}目录不存在，是否创建？\n路径：{folderPath}", "目录不存在");
        if (!confirmed) return;

        try
        {
            Directory.CreateDirectory(folderPath);
            LogManager.GetInstance().AddLog(folderName, $"已创建目录：{folderPath}");
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog(folderName, $"创建目录失败：{ex.Message}", "错误");
            await Notify($"创建失败：{ex.Message}", "错误");
        }
    }

    /// <summary>打开目录（使用系统文件管理器）</summary>
    public static async Task OpenFolderAsync(string folderPath, string folderName)
    {
        if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
        {
            await Notify($"{folderName}目录不存在！", "错误");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = folderPath,
                UseShellExecute = true
            });
            LogManager.GetInstance().AddLog(folderName, $"打开目录：{folderPath}");
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog(folderName, $"打开目录失败：{ex.Message}", "错误");
            await Notify($"打开失败：{ex.Message}", "错误");
        }
    }

    /// <summary>添加文件到目录（通过文件选择器）</summary>
    public static async Task AddFileToFolderAsync(string folderPath, string folderName)
    {
        if (!Directory.Exists(folderPath))
        {
            await Notify($"{folderName}目录不存在，无法添加文件！", "错误");
            return;
        }

        var sourceFile = await PickFile($"选择要添加到{folderName}的文件", "所有文件", new[] { "*" });
        if (string.IsNullOrEmpty(sourceFile)) return;

        try
        {
            var destPath = Path.Combine(folderPath, Path.GetFileName(sourceFile));
            if (File.Exists(destPath))
            {
                var overwrite = await Confirm($"{Path.GetFileName(sourceFile)} 已存在，是否覆盖？", "文件已存在");
                if (!overwrite) return;
            }

            File.Copy(sourceFile, destPath, true);
            LogManager.GetInstance().AddLog(folderName, $"已添加文件：{Path.GetFileName(sourceFile)}");
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog(folderName, $"添加文件失败：{ex.Message}", "错误");
            await Notify($"添加失败：{ex.Message}", "错误");
        }
    }

    /// <summary>删除文件（确认后删除）</summary>
    public static async Task DeleteFileAsync(string folderPath, string fileName, string folderName)
    {
        var filePath = Path.Combine(folderPath, fileName);
        if (!File.Exists(filePath))
        {
            await Notify($"文件{fileName}不存在！", "错误");
            return;
        }

        var confirmed = await Confirm($"确定要删除文件 {fileName} 吗？\n此操作不可恢复！", "确认删除");
        if (!confirmed) return;

        try
        {
            File.Delete(filePath);
            LogManager.GetInstance().AddLog(folderName, $"已删除文件：{fileName}");
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog(folderName, $"删除文件失败：{ex.Message}", "错误");
            await Notify($"删除失败：{ex.Message}", "错误");
        }
    }

    /// <summary>清理 Bugs 日志（保留 Game.log）</summary>
    public static async Task CleanBugsLogAsync(string folderPath, string excludeFileName)
    {
        if (!Directory.Exists(folderPath)) return;

        try
        {
            foreach (var filePath in Directory.GetFiles(folderPath))
            {
                var fileName = Path.GetFileName(filePath);
                if (fileName.Equals(excludeFileName, StringComparison.OrdinalIgnoreCase))
                    continue;

                File.Delete(filePath);
            }
            LogManager.GetInstance().AddLog("Bugs", "已清理过期日志文件");
            await Notify("清理完成（保留Game.log）", "提示");
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog("Bugs", $"清理失败：{ex.Message}", "错误");
            await Notify($"清理失败：{ex.Message}", "错误");
        }
    }

    /// <summary>删除文件夹（递归，含子文件/子文件夹）</summary>
    public static async Task DeleteFolderAsync(string parentFolderPath, string folderName, string folderGroupName)
    {
        var fullPath = Path.Combine(parentFolderPath, folderName);
        if (!Directory.Exists(fullPath))
        {
            await Notify($"文件夹「{folderName}」不存在", "错误");
            return;
        }

        var confirmed = await Confirm($"确定删除「{folderName}」？会删除所有子文件和文件夹！", "确认删除");
        if (!confirmed) return;

        try
        {
            Directory.Delete(fullPath, recursive: true);
            LogManager.GetInstance().AddLog(folderGroupName, $"删除文件夹：{fullPath}");
            await Notify("删除成功", "提示");
        }
        catch (UnauthorizedAccessException ex)
        {
            LogManager.GetInstance().AddLog(folderGroupName, $"删除失败：无权限 {ex.Message}", "错误");
            await Notify("无权限删除", "错误");
        }
        catch (IOException ex)
        {
            LogManager.GetInstance().AddLog(folderGroupName, $"删除失败：文件被占用 {ex.Message}", "错误");
            await Notify("文件被占用，无法删除", "错误");
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog(folderGroupName, $"删除失败：{ex.Message}", "错误");
            await Notify($"删除失败：{ex.Message}", "错误");
        }
    }

    /// <summary>批量备份所有目录</summary>
    public static async Task BatchBackupAllFoldersAsync(ServerConfig server)
    {
        // TODO: 实际项目中补充备份逻辑
        await Notify($"已开始批量备份 {server.Name} 的所有目录", "提示");
    }
}
