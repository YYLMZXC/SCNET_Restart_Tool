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
            // 检查是否以管理员权限运行
            if (!IsRunningAsAdmin())
            {
                // 提示用户需要管理员权限
                var result = MessageBox.Show(
                    "本程序需要管理员权限才能正常工作（如进程管理、WMI操作等），是否以管理员身份重启？",
                    "权限不足",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // 以管理员身份重启程序
                    RestartAsAdmin();
                    return; // 退出当前非管理员进程
                }
                // 如果用户选择"否"，继续运行（但可能部分功能失效）
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
                MessageBox.Show($"重启失败：{ex.Message}\n部分功能可能无法正常使用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}