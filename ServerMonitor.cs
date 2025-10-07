using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.Sockets;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    /// 服务端监控器
    public class ServerMonitor
    {
        private readonly ServerConfig _config;
        private readonly Timer _timerDaily = new Timer();
        private readonly Timer _timerContinuous = new Timer();
        private readonly Action<ServerConfig> _onStatusChanged;
        private readonly LogManager _logManager;

        public ServerMonitor(ServerConfig config, Action<ServerConfig> onStatusChanged)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _onStatusChanged = onStatusChanged ?? throw new ArgumentNullException(nameof(onStatusChanged));
            _logManager = LogManager.GetInstance();

            // 初始化定时器
            _timerDaily.Interval = 1000; // 1秒检查一次定时任务
            _timerDaily.Tick += TimerDaily_Tick;

            _timerContinuous.Interval = 3000; // 3秒检查一次运行状态
            _timerContinuous.Tick += TimerContinuous_Tick;

            UpdateStatus();
        }

        /// 启动监控
        public void Start()
        {
            if (!_config.IsMonitoring)
            {
                _config.IsMonitoring = true;
                _timerDaily.Start();
                _timerContinuous.Start();
                UpdateStatus();
                _logManager.AddLog(_config.Name, "监控已开启");
            }
        }

        /// 停止监控
        public void Stop()
        {
            if (_config.IsMonitoring)
            {
                _config.IsMonitoring = false;
                _timerDaily.Stop();
                _timerContinuous.Stop();
                UpdateStatus();
                _logManager.AddLog(_config.Name, "监控已关闭");
            }
        }

        /// 定时任务检查（每日关闭/间隔关闭）
        private void TimerDaily_Tick(object sender, EventArgs e)
        {
            // 每日定时关闭
            if (TimeSpan.TryParse(_config.ScheduleTime, out TimeSpan scheduleTime))
            {
                var now = DateTime.Now;
                if (now.Hour == scheduleTime.Hours && now.Minute == scheduleTime.Minutes && now.Second == 0)
                {
                    _logManager.AddLog(_config.Name, $"触发每日定时关闭（设定时间：{_config.ScheduleTime}）");
                    SendServiceCommand("close 9 例行维护");
                    System.Threading.Thread.Sleep(7000);
                    KillProcess();
                }
            }

            // 间隔关闭
            if (_config.IntervalHours > 0)
            {
                var nextClose = _config.LastIntervalClose.AddHours(_config.IntervalHours);
                if (DateTime.Now >= nextClose)
                {
                    _logManager.AddLog(_config.Name, $"触发间隔关闭（间隔：{_config.IntervalHours}小时）");
                    SendServiceCommand("close 9 间隔维护");
                    System.Threading.Thread.Sleep(7000);
                    KillProcess();
                    _config.LastIntervalClose = DateTime.Now;
                }
            }
        }

        /// 持续检查服务端状态，自动启动
        private void TimerContinuous_Tick(object sender, EventArgs e)
        {
            UpdateStatus();

            if (_config.IsMonitoring && _config.Status == ServerStatus.Stopped)
            {
                _logManager.AddLog(_config.Name, "检测到服务端未运行，尝试启动...");
                StartProcess();
            }
        }
        /// 更新服务端状态
        private void UpdateStatus()
        {
            var isRunning = IsProcessRunning();
            var oldStatus = _config.Status;

            _config.Status = isRunning ? ServerStatus.Running : ServerStatus.Stopped;
            if (_config.IsMonitoring)
                _config.Status = ServerStatus.Monitoring;

            // 状态变化时记录日志
            if (oldStatus != _config.Status)
            {
                _logManager.AddLog(_config.Name, $"状态变更：{oldStatus} → {_config.Status}");
            }

            _onStatusChanged.Invoke(_config);
        }
        /// 检查进程是否运行（增强版：兼容权限问题，使用WMI获取路径）
        public bool IsProcessRunning()
        {
            if (string.IsNullOrEmpty(_config.ExePath))
                return false;

            string targetPath = _config.ExePath.ToLowerInvariant();
            string targetFileName = Path.GetFileName(_config.ExePath).ToLowerInvariant();
            string targetArgs = $"world={_config.Id}"; // 唯一标识参数

            try
            {
                // 直接通过WMI查询所有进程（避免Process.GetProcessesByName的局限性）
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        try
                        {
                            // 获取进程关键信息（可能为null，需防御性处理）
                            string processPath = obj["ExecutablePath"]?.ToString()?.ToLowerInvariant() ?? string.Empty;
                            string commandLine = obj["CommandLine"]?.ToString()?.ToLowerInvariant() ?? string.Empty;
                            int processId = Convert.ToInt32(obj["ProcessId"]);

                            // 匹配条件：路径完全一致 或 文件名+唯一参数匹配
                            bool isPathMatch = processPath == targetPath;
                            bool isNameAndArgsMatch = processPath.EndsWith(targetFileName) &&
                                                     commandLine.Contains(targetArgs);

                            if (isPathMatch || isNameAndArgsMatch)
                            {
                                // 二次确认进程是否真的存活（避免WMI缓存）
                                if (Process.GetProcessById(processId) != null)
                                {
                                    return true;
                                }
                            }
                        }
                        catch (ArgumentException)
                        {
                            // 进程已退出，跳过
                            continue;
                        }
                        catch (Exception ex)
                        {
                            _logManager.AddLog(_config.Name, $"WMI检查进程失败：{ex.Message}", "警告");
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                _logManager.AddLog(_config.Name, $"WMI查询失败（可能权限不足）：{ex.Message}", "错误");
                // 权限不足时降级为基于文件名+参数的检查（精度较低但避免崩溃）
                return CheckProcessByFileNameAndArgs(targetFileName, targetArgs);
            }

            return false;
        }
        private bool CheckProcessByFileNameAndArgs(string fileName, string targetArgs)
        {
            string processName = Path.GetFileNameWithoutExtension(fileName);
            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    if (process.StartInfo.Arguments.Contains(targetArgs))
                        return true;
                }
                catch
                {
                    continue;
                }
            }
            return false;
        }

        /// 启动服务端进程
        public void StartProcess()
        {
            // 先检查是否已运行（使用改进后的IsProcessRunning）
            if (IsProcessRunning())
            {
                _logManager.AddLog(_config.Name, "启动失败：服务端已在运行中（禁止多开）", "警告");
                MessageBox.Show("服务端已在运行中，禁止多开！");
                return;
            }

            if (!File.Exists(_config.ExePath))
            {
                var error = $"启动失败：程序不存在（{_config.ExePath}）";
                _logManager.AddLog(_config.Name, error, "错误");
                MessageBox.Show(error);
                return;
            }

            try
            {
                // 启动参数添加唯一标识（服务端ID+随机数防冲突）
                string uniqueArgs = $"world={_config.Id} scnet_tool_id={Guid.NewGuid()}";
                Process.Start(new ProcessStartInfo
                {
                    FileName = _config.ExePath,
                    Arguments = uniqueArgs,
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(_config.ExePath)
                });
                _logManager.AddLog(_config.Name, $"服务端已启动（路径：{_config.ExePath}，参数：{uniqueArgs}）");
            }
            catch (Exception ex)
            {
                var error = $"启动失败：{ex.Message}";
                _logManager.AddLog(_config.Name, error, "错误");
                MessageBox.Show(error);
            }
        }

        /// 关闭服务端进程（仅当前配置的服务端）
        public void KillProcess()
        {
            if (string.IsNullOrEmpty(_config.ExePath))
            {
                _logManager.AddLog(_config.Name, "关闭失败：未配置程序路径", "错误");
                return;
            }

            string targetPath = _config.ExePath.ToLowerInvariant();
            string targetFileName = Path.GetFileName(_config.ExePath).ToLowerInvariant();
            string targetArgs = $"world={_config.Id}";
            bool isKilled = false;

            try
            {
                // 优先使用Process直接关闭（更高效）
                string processName = Path.GetFileNameWithoutExtension(targetFileName);
                foreach (var process in Process.GetProcessesByName(processName))
                {
                    try
                    {
                        // 获取进程路径（处理权限问题）
                        string processPath = "";
                        try
                        {
                            processPath = process.MainModule?.FileName?.ToLowerInvariant() ?? "";
                        }
                        catch (Exception ex)
                        {
                            _logManager.AddLog(_config.Name, $"获取进程路径失败：{ex.Message}", "警告");
                        }

                        // 获取命令行参数（用于精准匹配）
                        string commandLine = "";
                        try
                        {
                            using (var searcher = new ManagementObjectSearcher(
                                $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"))
                            {
                                foreach (var obj in searcher.Get())
                                {
                                    commandLine = obj["CommandLine"]?.ToString()?.ToLowerInvariant() ?? "";
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logManager.AddLog(_config.Name, $"获取命令行失败：{ex.Message}", "警告");
                        }

                        // 匹配目标进程
                        bool isMatch = processPath == targetPath ||
                                      (processPath.EndsWith(targetFileName) && commandLine.Contains(targetArgs));

                        if (isMatch)
                        {
                            _logManager.AddLog(_config.Name, $"尝试关闭进程（PID：{process.Id}）");

                            // 先尝试正常关闭（发送关闭消息）
                            if (process.CloseMainWindow())
                            {
                                // 等待2秒确认关闭
                                if (process.WaitForExit(2000))
                                {
                                    _logManager.AddLog(_config.Name, $"进程正常关闭（PID：{process.Id}）");
                                    isKilled = true;
                                    break;
                                }
                            }

                            // 正常关闭失败，尝试强制终止
                            try
                            {
                                process.Kill();
                                if (process.WaitForExit(2000))
                                {
                                    _logManager.AddLog(_config.Name, $"进程强制关闭（PID：{process.Id}）");
                                    isKilled = true;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logManager.AddLog(_config.Name, $"强制关闭失败：{ex.Message}", "警告");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logManager.AddLog(_config.Name, $"处理进程时出错：{ex.Message}", "警告");
                    }
                }

                // 如果直接关闭失败，尝试WMI方法（兼容更多场景）
                if (!isKilled)
                {
                    using (var searcher = new ManagementObjectSearcher(
                        "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process"))
                    {
                        foreach (ManagementObject obj in searcher.Get())
                        {
                            try
                            {
                                string processPath = obj["ExecutablePath"]?.ToString()?.ToLowerInvariant() ?? "";
                                string commandLine = obj["CommandLine"]?.ToString()?.ToLowerInvariant() ?? "";
                                int processId = Convert.ToInt32(obj["ProcessId"]);

                                bool isMatch = processPath == targetPath ||
                                              (processPath.EndsWith(targetFileName) && commandLine.Contains(targetArgs));

                                if (isMatch)
                                {
                                    _logManager.AddLog(_config.Name, $"尝试WMI关闭进程（PID：{processId}）");
                                    object[] parameters = { 0 };
                                    var result = obj.InvokeMethod("Terminate", parameters);
                                    int exitCode = Convert.ToInt32(result);

                                    if (exitCode == 0)
                                    {
                                        _logManager.AddLog(_config.Name, $"WMI关闭成功（PID：{processId}）");
                                        isKilled = true;
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logManager.AddLog(_config.Name, $"WMI操作出错：{ex.Message}", "警告");
                            }
                            finally
                            {
                                obj.Dispose();
                            }
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                _logManager.AddLog(_config.Name, $"WMI服务错误：{ex.Message}", "错误");
            }

            if (!isKilled)
            {
                _logManager.AddLog(_config.Name, "关闭失败：未找到进程或无操作权限", "错误");
            }
        }

        /// 降级方案：使用Process.Kill()关闭（WMI失败时）
        private bool DegradeKillProcess(string fileName, string targetArgs)
        {
            string processName = Path.GetFileNameWithoutExtension(fileName);
            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    if (process.StartInfo.Arguments.Contains(targetArgs))
                    {
                        process.Kill();
                        process.WaitForExit(2000);
                        return !process.HasExited;
                    }
                }
                catch
                {
                    continue;
                }
            }
            return false;
        }

        /// 发送指令到服务端
        public string SendServiceCommand(string command)
        {
            try
            {
                _logManager.AddLog(_config.Name, $"发送指令：{command}");
                using (var client = new TcpClient())
                {
                    client.Connect(_config.Ip, _config.Port);
                    using (var stream = client.GetStream())
                    using (var writer = new StreamWriter(stream))
                    using (var reader = new StreamReader(stream))
                    {
                        writer.AutoFlush = true;
                        var message = string.IsNullOrEmpty(_config.Password)
                            ? $"command {command}"
                            : $"password {_config.Password} command {command}";

                        writer.WriteLine(message);
                        var response = reader.ReadLine() ?? "无响应";
                        _logManager.AddLog(_config.Name, $"指令响应：{response}");
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                var error = $"指令发送失败：{ex.Message}";
                _logManager.AddLog(_config.Name, error, "错误");
                return error;
            }
        }
    }
}