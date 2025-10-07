using System;
using System.Management;

namespace SCNET_Restart_Tool
{
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
}