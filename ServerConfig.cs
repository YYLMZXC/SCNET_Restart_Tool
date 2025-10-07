using System;

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
    }
}