namespace SCNET_Restart_Tool
{
    /// <summary>
    /// 服务端运行状态
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// 未运行
        /// </summary>
        Stopped,

        /// <summary>
        /// 运行中
        /// </summary>
        Running,

        /// <summary>
        /// 监控中
        /// </summary>
        Monitoring
    }
}