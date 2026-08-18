using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 检查是否以管理员权限运行，如果不是则自动以管理员权限重启
            if (!IsRunningAsAdmin())
            {
                // 自动以管理员身份重启程序，不询问用户
                RestartAsAdmin();
                return; // 退出当前非管理员进程
            }

            // 正常启动程序
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ToolMain());
        }

        /// <summary>
        /// 检查是否以管理员权限运行
        /// </summary>
        private static bool IsRunningAsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 以管理员身份重启程序
        /// </summary>
        private static void RestartAsAdmin()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath, // 当前程序路径
                    UseShellExecute = true,
                    Verb = "runas" // 请求管理员权限
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法获取管理员权限：{ex.Message}\n程序需要管理员权限才能正常工作。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}