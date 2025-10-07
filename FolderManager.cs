using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public static class FolderManager
    {
        private static readonly LogManager _logManager = LogManager.GetInstance();

        // 打开文件夹
        public static void OpenFolder(string folderPath, string folderName, string serverName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath))
                {
                    _logManager.AddLog(serverName, $"{folderName}路径无效（服务端程序路径未配置）", "警告");
                    MessageBox.Show("请先配置服务端程序路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Directory.Exists(folderPath))
                {
                    var result = MessageBox.Show(
                        $"{folderName}目录不存在，是否创建？\n路径：{folderPath}",
                        "目录不存在",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(folderPath);
                        _logManager.AddLog(serverName, $"已创建{folderName}目录：{folderPath}");
                    }
                    else
                    {
                        return;
                    }
                }

                System.Diagnostics.Process.Start("explorer.exe", folderPath);
                _logManager.AddLog(serverName, $"已打开{folderName}目录：{folderPath}");
            }
            catch (Exception ex)
            {
                _logManager.AddLog(serverName, $"打开{folderName}失败：{ex.Message}", "错误");
                MessageBox.Show($"打开失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 备份文件夹
        public static void BackupFolder(string folderPath, string folderName, string serverName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                {
                    _logManager.AddLog(serverName, $"{folderName}目录不存在，无法备份", "警告");
                    MessageBox.Show($"{folderName}目录不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var backupRoot = Path.Combine(Path.GetDirectoryName(folderPath), "Backup");
                var serverBackupDir = Path.Combine(backupRoot, serverName);
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var backupZipPath = Path.Combine(serverBackupDir, $"{folderName}_{timestamp}.zip");

                if (!Directory.Exists(serverBackupDir))
                    Directory.CreateDirectory(serverBackupDir);

                if (File.Exists(backupZipPath))
                    File.Delete(backupZipPath);
                ZipFile.CreateFromDirectory(folderPath, backupZipPath, CompressionLevel.Optimal, true);

                _logManager.AddLog(serverName, $"已备份{folderName}：{backupZipPath}");
                MessageBox.Show($"备份成功！\n路径：{backupZipPath}", "备份完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logManager.AddLog(serverName, $"备份{folderName}失败：{ex.Message}", "错误");
                MessageBox.Show($"备份失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 清理Bugs日志
        public static void CleanBugsLog(string bugsLogPath, string serverName, int keepDays = 7)
        {
            try
            {
                if (string.IsNullOrEmpty(bugsLogPath) || !Directory.Exists(bugsLogPath))
                {
                    _logManager.AddLog(serverName, "Bugs日志目录不存在，无需清理", "警告");
                    MessageBox.Show("Bugs日志目录不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"确定清理{bugsLogPath}中{keepDays}天前的日志文件？",
                    "确认清理",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                var cutoffTime = DateTime.Now.AddDays(-keepDays);
                var files = Directory.GetFiles(bugsLogPath, "*.*", SearchOption.TopDirectoryOnly);
                int deletedCount = 0;

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffTime)
                    {
                        fileInfo.Delete();
                        deletedCount++;
                    }
                }

                _logManager.AddLog(serverName, $"清理Bugs日志完成：删除{deletedCount}个文件（保留{keepDays}天内文件）");
                MessageBox.Show($"清理完成！共删除{deletedCount}个过期日志文件", "清理完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logManager.AddLog(serverName, $"清理Bugs日志失败：{ex.Message}", "错误");
                MessageBox.Show($"清理失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 批量备份所有目录
        public static void BatchBackupAllFolders(ServerConfig server)
        {
            if (string.IsNullOrEmpty(server.ServerRootPath))
            {
                _logManager.AddLog(server.Name, "服务端程序路径未配置，无法批量备份", "警告");
                MessageBox.Show("请先配置服务端程序路径！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BackupFolderIfExists(server.BugsLogPath, "Bugs日志", server.Name);
            BackupFolderIfExists(server.CharacterSkinsPath, "CharacterSkins（皮肤）", server.Name);
            BackupFolderIfExists(server.ConfigsPath, "Configs（插件配置）", server.Name);
            BackupFolderIfExists(server.TexturePacksPath, "TexturePacks（材质包）", server.Name);
            BackupFolderIfExists(server.NetModsPath, "NetMods（模组）", server.Name);
            BackupFolderIfExists(server.PluginsPath, "Plugins（服务端插件）", server.Name);
            BackupFolderIfExists(server.WorldsPath, "Worlds（地图存档）", server.Name);

            _logManager.AddLog(server.Name, "批量备份任务已完成（跳过不存在的目录）");
            MessageBox.Show("批量备份完成！（不存在的目录已跳过）", "批量备份完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void BackupFolderIfExists(string folderPath, string folderName, string serverName)
        {
            if (Directory.Exists(folderPath))
                BackupFolder(folderPath, folderName, serverName);
            else
                _logManager.AddLog(serverName, $"{folderName}目录不存在，跳过备份");
        }
    }
}