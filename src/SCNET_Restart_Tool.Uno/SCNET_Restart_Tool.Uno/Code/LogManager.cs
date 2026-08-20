using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SCNET_Restart_Tool;

/// <summary>
/// 日志管理器（单例）：负责向界面推送日志并写入日志文件
/// </summary>
public class LogManager
{
    private static LogManager? _instance;
    private string _logPath = "";

    /// <summary>单例模式</summary>
    public static LogManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LogManager();
        }
        return _instance;
    }

    /// <summary>日志路径属性</summary>
    public string LogPath
    {
        get => _logPath;
        private set => _logPath = value;
    }

    /// <summary>日志添加事件（UI 订阅刷新）</summary>
    public event Action<string>? LogAdded;

    private LogManager()
    {
        // 默认使用程序目录
        _logPath = AppContext.BaseDirectory;
    }

    /// <summary>添加日志</summary>
    public void AddLog(string source, string message, string level = "信息")
    {
        string logEntry = $"[{source}] {level}: {message}";
        LogAdded?.Invoke(logEntry);

        // 保存到文件
        SaveLogToFile(logEntry);
    }

    private void SaveLogToFile(string logEntry)
    {
        try
        {
            string logFile = Path.Combine(_logPath, $"log_{DateTime.Now:yyyyMMdd}.txt");
            Directory.CreateDirectory(_logPath); // 确保目录存在
            File.AppendAllText(logFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {logEntry}\r\n");
        }
        catch (Exception ex)
        {
            LogAdded?.Invoke($"日志保存失败: {ex.Message}");
        }
    }

    /// <summary>更新日志路径</summary>
    public void UpdateLogPath(string newPath)
    {
        if (!string.IsNullOrEmpty(newPath) && Directory.Exists(newPath))
        {
            _logPath = newPath;
            AddLog("系统", $"日志路径已更新为: {newPath}");
        }
        else
        {
            AddLog("系统", $"日志路径更新失败，无效路径: {newPath}", "错误");
        }
    }

    /// <summary>清理过期日志</summary>
    public void ClearLogs(int daysToKeep)
    {
        try
        {
            var logFiles = Directory.GetFiles(_logPath, "log_*.txt")
                .Where(f => Path.GetFileName(f).StartsWith("log_"))
                .ToList();

            foreach (var file in logFiles)
            {
                if (DateTime.TryParseExact(
                    Path.GetFileNameWithoutExtension(file).Replace("log_", ""),
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime fileDate))
                {
                    if (fileDate < DateTime.Now.AddDays(-daysToKeep))
                    {
                        File.Delete(file);
                        AddLog("系统", $"已清理过期日志: {file}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            AddLog("系统", $"日志清理失败: {ex.Message}", "错误");
        }
    }
}
