using System;
using System.Collections.Generic;

namespace SCNET_Restart_Tool
{
    /// <summary>
    /// 日志管理类（单例模式）
    /// </summary>
    public class LogManager
    {
        private static LogManager _instance;
        private readonly List<LogItem> _logs = new List<LogItem>();

        /// <summary>
        /// 日志添加事件（用于UI实时刷新）
        /// </summary>
        public event Action<LogItem> OnLogAdded;

        private LogManager() { }

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static LogManager GetInstance()
        {
            return _instance ?? (_instance = new LogManager());
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        public void AddLog(string serverName, string content, string level = "信息")
        {
            var log = new LogItem(serverName, content, level);
            _logs.Add(log);
            OnLogAdded?.Invoke(log); // 触发日志更新事件
        }

        /// <summary>
        /// 获取所有日志
        /// </summary>
        public List<LogItem> GetAllLogs()
        {
            return _logs;
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        public void ClearLogs()
        {
            _logs.Clear();
        }
    }
}