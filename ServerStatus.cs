namespace SCNET_Restart_Tool
{
    /// 服务端运行状态
  
    public enum ServerStatus
    {
        Stopped,       // 已停止
        Running,       // 运行中（未监控）
        Starting,      // 启动中
        Stopping,      // 停止中
        Monitoring     // 监控中（运行状态且处于监控模式）
    }
}