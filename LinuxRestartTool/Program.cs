using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Timers;

namespace LinuxRestartTool
{
    class Program
    {
        private const string ConfigFileName = "default_program.config";
        private static string targetExecutablePath = string.Empty;
        private static string targetExecutableName = string.Empty;
        private static TimeSpan scheduledCloseTime = new TimeSpan(1, 0, 0); // 默认01:00
        private static double intervalHours = 0;
        private static DateTime lastIntervalCloseTime = DateTime.MinValue;
        private static System.Timers.Timer? monitorTimer;
        private static bool isRunning = true;
        private static readonly ManualResetEvent exitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("生存战争服务端监控程序 - Linux版");
            Console.WriteLine("输入 'exit' 并按回车键退出程序");

            // 设置Ctrl+C处理
            Console.CancelKeyPress += (sender, e) => {
                e.Cancel = true; // 防止立即退出
                ExitApplication();
            };

            LoadConfiguration();
            SetupTimers();

            // 启动控制台输入监听线程
            var inputThread = new Thread(ReadConsoleInput);
            inputThread.IsBackground = true;
            inputThread.Start();

            // 主线程等待退出信号
            exitEvent.WaitOne();

            // 清理资源
            monitorTimer?.Stop();
            monitorTimer?.Dispose();
            Console.WriteLine("程序已安全退出");
        }

        private static void ReadConsoleInput()
        {
            while (isRunning)
            {
                var input = Console.ReadLine();
                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    ExitApplication();
                }
            }
        }

        private static void ExitApplication()
        {
            if (!isRunning) return;

            isRunning = false;
            Console.WriteLine("正在退出程序...");

            // 停止监控
            monitorTimer?.Stop();

            // 发送退出信号
            exitEvent.Set();
        }

        private static void LoadConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFileName))
                {
                    var lines = File.ReadAllLines(ConfigFileName);
                    if (lines.Length >= 5)
                    {
                        targetExecutablePath = lines[0];
                        targetExecutableName = lines[1];

                        if (TimeSpan.TryParse(lines[2], out var time))
                            scheduledCloseTime = time;

                        if (double.TryParse(lines[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var hours))
                            intervalHours = hours;

                        if (DateTime.TryParse(lines[4], out var lastClose))
                            lastIntervalCloseTime = lastClose;
                    }
                }

                // 默认配置
                if (string.IsNullOrEmpty(targetExecutablePath))
                {
                    targetExecutablePath = Path.Combine(Directory.GetCurrentDirectory(), "TerminalEntry.exe");
                    targetExecutableName = "TerminalEntry.exe";
                    SaveConfiguration();
                }

                Console.WriteLine($"监控程序: {targetExecutableName}");
                Console.WriteLine($"路径: {targetExecutablePath}");
                Console.WriteLine($"定时关闭: {scheduledCloseTime:hh\\:mm}");
                Console.WriteLine($"间隔关闭: {intervalHours} 小时");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"配置错误: {ex.Message}");
            }
        }

        private static void SaveConfiguration()
        {
            File.WriteAllLines(ConfigFileName, new[] {
                targetExecutablePath,
                targetExecutableName,
                scheduledCloseTime.ToString("hh\\:mm"),
                intervalHours.ToString(CultureInfo.InvariantCulture),
                lastIntervalCloseTime.ToString("O")
            });
        }

        private static void SetupTimers()
        {
            // 主监控计时器 (每秒检查)
            monitorTimer = new System.Timers.Timer(1000);
            monitorTimer.Elapsed += (sender, e) => {
                if (!isRunning) return;
                CheckProcessStatus();
                CheckScheduledClose();
                CheckIntervalClose();
            };
            monitorTimer.Start();
        }

        private static void CheckProcessStatus()
        {
            try
            {
                if (!IsProcessRunning(Path.GetFileNameWithoutExtension(targetExecutableName)))
                {
                    Console.WriteLine($"{DateTime.Now} - 进程未运行，正在启动...");
                    StartProcess();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} - 监控错误: {ex.Message}");
            }
        }

        private static void CheckScheduledClose()
        {
            var now = DateTime.Now.TimeOfDay;
            if (now.Hours == scheduledCloseTime.Hours &&
                now.Minutes == scheduledCloseTime.Minutes &&
                now.Seconds == 0)
            {
                Console.WriteLine($"{DateTime.Now} - 定时关闭触发");
                KillProcess();
            }
        }

        private static void CheckIntervalClose()
        {
            if (intervalHours > 0 &&
                DateTime.Now >= lastIntervalCloseTime.AddHours(intervalHours))
            {
                Console.WriteLine($"{DateTime.Now} - 间隔关闭触发");
                KillProcess();
                lastIntervalCloseTime = DateTime.Now;
                SaveConfiguration();
            }
        }

        private static bool IsProcessRunning(string processName)
        {
            // Linux系统需要去掉.exe后缀
            processName = processName.Replace(".exe", "");
            var processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        private static void KillProcess()
        {
            try
            {
                var processName = Path.GetFileNameWithoutExtension(targetExecutableName)
                    .Replace(".exe", "");

                foreach (var process in Process.GetProcessesByName(processName))
                {
                    process.Kill();
                    process.WaitForExit();
                    Console.WriteLine($"已终止进程: {processName} (PID: {process.Id})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"终止进程失败: {ex.Message}");
            }
        }

        private static void StartProcess()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = targetExecutablePath,
                    Arguments = "world=1",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                Process.Start(startInfo);
                Console.WriteLine($"已启动进程: {targetExecutablePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"启动失败: {ex.Message}");
            }
        }
    }
}