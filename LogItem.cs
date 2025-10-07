using System;

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
}