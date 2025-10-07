using System;

namespace SCNET_Restart_Tool
{
    public delegate void LogAddedEventHandler(string log);

    public class LogManager
    {
        // 单例实例
        private static readonly Lazy<LogManager> _instance = new Lazy<LogManager>(() => new LogManager());
        public static LogManager GetInstance() => _instance.Value;

        // 日志添加事件（供UI订阅）
        public event LogAddedEventHandler LogAdded;

        private LogManager() { }

        /// 添加普通日志
        public void AddLog(string source, string message)
        {
            var log = $"[{source}] {message}";
            LogAdded?.Invoke(log);
        }

        /// 添加带类型的日志（警告/错误）
        public void AddLog(string source, string message, string type)
        {
            var log = $"[{source}] [{type}] {message}";
            LogAdded?.Invoke(log);
        }
    }
}