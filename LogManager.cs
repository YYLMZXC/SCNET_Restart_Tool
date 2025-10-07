using System;
using System.Collections.Generic;

namespace SCNET_Restart_Tool
{
    /// 日志管理类（单例模式）
    public class LogManager
    {
        private static LogManager _instance;
        private readonly List<LogItem> _logs = new List<LogItem>();
        /// 日志添加事件（用于UI实时刷新）
        public event Action<LogItem> OnLogAdded;

        private LogManager() { }
        /// 获取单例实例
        public static LogManager GetInstance()
        {
            return _instance ?? (_instance = new LogManager());
        }
        /// 添加日志
        public void AddLog(string serverName, string content, string level = "信息")
        {
            var log = new LogItem(serverName, content, level);
            _logs.Add(log);
            OnLogAdded?.Invoke(log); // 触发日志更新事件
        }
        /// 获取所有日志
        public List<LogItem> GetAllLogs()
        {
            return _logs;
        }
        /// 清空日志
        public void ClearLogs()
        {
            _logs.Clear();
        }
    }
}