using System;

namespace SCNET_Restart_Tool
{
    /// <summary>
    /// 日志条目模型
    /// </summary>
    public class LogItem
    {
        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 关联的服务端名称
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 日志级别（信息/警告/错误）
        /// </summary>
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