using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    /// 日志条目模型
    public class LogItem
    {
        /// 日志时间       
        public DateTime Time { get; set; }
        /// 关联的服务端名称
        public string ServerName { get; set; }
        /// 日志内容
        public string Content { get; set; }
        /// 日志级别（信息/警告/错误）
        public string Level { get; set; }

        public LogItem(string serverName, string content, string level = "信息")
        {
            Time = DateTime.Now;
            ServerName = serverName;
            Content = content;
            Level = level;
        }
    }

    public class LogManager
    {
        private static LogManager _instance;
        private string _logPath;

        // 单例模式
        public static LogManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LogManager();
            }
            return _instance;
        }

        // 日志路径属性
        public string LogPath
        {
            get => _logPath;
            private set => _logPath = value;
        }

        // 日志添加事件
        public event Action<string> LogAdded;

        // 构造函数
        private LogManager()
        {
            // 默认使用程序目录
            _logPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        // 添加日志
        public void AddLog(string source, string message, string level = "信息")
        {
            string logEntry = $"[{source}] {level}: {message}";
            LogAdded?.Invoke(logEntry);

            // 保存到文件
            SaveLogToFile(logEntry);
        }

        // 保存日志到文件
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

        // 添加更新日志路径的方法
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

        // 清空日志方法（可选）
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
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
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

    public static class ProcessHelper
    {
        /// 通过WMI获取进程信息（兼容32/64位）
        public static ManagementObjectCollection GetAllProcesses()
        {
            try
            {
                return new ManagementObjectSearcher(
                    "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process").Get();
            }
            catch (Exception ex)
            {
                LogManager.GetInstance().AddLog("系统", $"获取进程列表失败：{ex.Message}", "错误");
                return null;
            }
        }
    }



    [Serializable]
    public class ServerConfig
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "新服务端";
        public string ExePath { get; set; } = "";
        public string ExeName { get; set; } = "";
        public string Ip { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5612;
        public string Password { get; set; } = "";
        public string ScheduleTime { get; set; } = "01:00";
        public double IntervalHours { get; set; } = 0;
        public DateTime LastIntervalClose { get; set; } = DateTime.MinValue;
        public bool IsMonitoring { get; set; } = false;
        [NonSerialized]
        public ServerStatus Status = ServerStatus.Stopped;
        public bool EnableCommands { get; set; } = false;

        // 服务端关联文件夹路径
        [NonSerialized]
        private string _serverRootPath;

        public string ServerRootPath
        {
            get
            {
                if (string.IsNullOrEmpty(ExePath) || !File.Exists(ExePath))
                    return "";
                _serverRootPath = Path.GetDirectoryName(ExePath);
                return _serverRootPath;
            }
        }

        public string BugsLogPath => Path.Combine(ServerRootPath, "Bugs");
        public string CharacterSkinsPath => Path.Combine(ServerRootPath, "CharacterSkins");
        public string ConfigsPath => Path.Combine(ServerRootPath, "Configs");
        public string TexturePacksPath => Path.Combine(ServerRootPath, "TexturePacks");
        public string NetModsPath => Path.Combine(ServerRootPath, "NetMods");
        public string PluginsPath => Path.Combine(ServerRootPath, "Plugins");
        public string WorldsPath => Path.Combine(ServerRootPath, "Worlds");

        public bool IsFolderExists(string folderPath)
        {
            return !string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath);
        }
    }


    public static class ServerConfigManager
    {
        private static readonly string ConfigPath = "servers_config.json";
        private static readonly JsonSerializerSettings JsonOptions = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects
        };

        public static List<ServerConfig> LoadAll()
        {
            if (!File.Exists(ConfigPath))
            {
                return new List<ServerConfig> { new ServerConfig { Id = 1 } };
            }
            try
            {
                string json = File.ReadAllText(ConfigPath);
                var result = JsonConvert.DeserializeObject<List<ServerConfig>>(json, JsonOptions);
                return result ?? new List<ServerConfig>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"配置加载失败：{ex.Message}\n将使用默认配置");
                return new List<ServerConfig> { new ServerConfig { Id = 1 } };
            }
        }

        public static void SaveAll(List<ServerConfig> servers)
        {
            try
            {
                string json = JsonConvert.SerializeObject(servers, JsonOptions);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"配置保存失败：{ex.Message}");
            }
        }

        public static int GenerateNewId(List<ServerConfig> servers)
        {
            int maxId = 0;
            foreach (var server in servers)
            {
                if (server.Id > maxId)
                    maxId = server.Id;
            }
            return maxId + 1;
        }
    }



}
