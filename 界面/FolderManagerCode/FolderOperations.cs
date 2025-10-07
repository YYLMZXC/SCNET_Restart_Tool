using System;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    internal static class FolderOperations
    {
        // 创建目录（通用方法）
        public static void CreateFolderIfNotExists(string folderPath, string folderName)
        {
            if (string.IsNullOrEmpty(folderPath)) return;

            var result = MessageBox.Show(
                $"{folderName}目录不存在，是否创建？\n路径：{folderPath}",
                "目录不存在",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                    LogManager.GetInstance().AddLog(folderName, $"已创建目录：{folderPath}");
                }
                catch (Exception ex)
                {
                    LogManager.GetInstance().AddLog(folderName, $"创建目录失败：{ex.Message}", "错误");
                    MessageBox.Show($"创建失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 打开目录（通用方法）
        public static void OpenFolder(string folderPath, string folderName)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                MessageBox.Show($"{folderName}目录不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
                LogManager.GetInstance().AddLog(folderName, $"打开目录：{folderPath}");
            }
            catch (Exception ex)
            {
                LogManager.GetInstance().AddLog(folderName, $"打开目录失败：{ex.Message}", "错误");
                MessageBox.Show($"打开失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 添加文件到目录（通用方法）
        public static void AddFileToFolder(string folderPath, string folderName)
        {
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"{folderName}目录不存在，无法添加文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var ofd = new OpenFileDialog
            {
                Title = $"选择要添加到{folderName}的文件",
                Filter = "所有文件 (*.*)|*.*",
                RestoreDirectory = true
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var destPath = Path.Combine(folderPath, Path.GetFileName(ofd.FileName));
                        if (File.Exists(destPath))
                        {
                            var overwrite = MessageBox.Show(
                                $"文件{Path.GetFileName(ofd.FileName)}已存在，是否覆盖？",
                                "文件已存在",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);
                            if (overwrite != DialogResult.Yes) return;
                        }

                        File.Copy(ofd.FileName, destPath, true);
                        LogManager.GetInstance().AddLog(folderName, $"已添加文件：{Path.GetFileName(ofd.FileName)}");
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetInstance().AddLog(folderName, $"添加文件失败：{ex.Message}", "错误");
                        MessageBox.Show($"添加失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 删除文件（通用方法）
        public static void DeleteFile(string folderPath, string fileName, string folderName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"文件{fileName}不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show(
                $"确定要删除文件 {fileName} 吗？\n此操作不可恢复！",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    File.Delete(filePath);
                    LogManager.GetInstance().AddLog(folderName, $"已删除文件：{fileName}");
                }
                catch (Exception ex)
                {
                    LogManager.GetInstance().AddLog(folderName, $"删除文件失败：{ex.Message}", "错误");
                    MessageBox.Show($"删除失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 清理Bugs日志（跳过Game.log）
        public static void CleanBugsLog(string folderPath, string excludeFileName)
        {
            if (!Directory.Exists(folderPath)) return;

            try
            {
                foreach (var filePath in Directory.GetFiles(folderPath))
                {
                    var fileName = Path.GetFileName(filePath);
                    if (fileName.Equals(excludeFileName, StringComparison.OrdinalIgnoreCase))
                        continue; // 跳过Game.log

                    File.Delete(filePath);
                }
                LogManager.GetInstance().AddLog("Bugs", "已清理过期日志文件");
                MessageBox.Show("清理完成（保留Game.log）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.GetInstance().AddLog("Bugs", $"清理失败：{ex.Message}", "错误");
                MessageBox.Show($"清理失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 批量备份所有目录
        public static void BatchBackupAllFolders(ServerConfig server)
        {
            // 实际项目中补充备份逻辑
            MessageBox.Show($"已开始批量备份 {server.Name} 的所有目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}