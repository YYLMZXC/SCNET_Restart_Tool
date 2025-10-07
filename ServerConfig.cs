using System;
using System.IO;

namespace SCNET_Restart_Tool
{
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

        public string BugsLogPath => Path.Combine(ServerRootPath, "Bugs日志");
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
}