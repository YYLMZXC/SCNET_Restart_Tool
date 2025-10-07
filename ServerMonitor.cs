using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    /// <summary>
    /// 服务端监控器
    /// </summary>
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

        /// <summary>
        /// 启动监控
        /// </summary>
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

        /// <summary>
        /// 停止监控
        /// </summary>
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

        /// <summary>
        /// 定时任务检查（每日关闭/间隔关闭）
        /// </summary>
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

        /// <summary>
        /// 持续检查服务端状态，自动启动
        /// </summary>
        private void TimerContinuous_Tick(object sender, EventArgs e)
        {
            UpdateStatus();

            if (_config.IsMonitoring && _config.Status == ServerStatus.Stopped)
            {
                _logManager.AddLog(_config.Name, "检测到服务端未运行，尝试启动...");
                StartProcess();
            }
        }

        /// <summary>
        /// 更新服务端状态
        /// </summary>
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

        /// <summary>
        /// 检查进程是否运行
        /// </summary>
        public bool IsProcessRunning()
        {
            if (string.IsNullOrEmpty(_config.ExePath))
                return false;

            var processName = Path.GetFileNameWithoutExtension(_config.ExePath);
            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    // 路径完全匹配才视为运行中
                    if (process.MainModule.FileName.Equals(_config.ExePath, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
                catch
                {
                    // 忽略无权限的进程
                }
            }
            return false;
        }

        /// <summary>
        /// 启动服务端进程
        /// </summary>
        public void StartProcess()
        {
            if (!File.Exists(_config.ExePath))
            {
                var error = $"启动失败：程序不存在（{_config.ExePath}）";
                _logManager.AddLog(_config.Name, error, "错误");
                MessageBox.Show(error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _config.ExePath,
                    Arguments = "world=1",
                    UseShellExecute = true,
                    WorkingDirectory = Path.GetDirectoryName(_config.ExePath)
                });
                _logManager.AddLog(_config.Name, $"服务端已启动（路径：{_config.ExePath}）");
            }
            catch (Exception ex)
            {
                var error = $"启动失败：{ex.Message}";
                _logManager.AddLog(_config.Name, error, "错误");
                MessageBox.Show(error);
            }
        }

        /// <summary>
        /// 关闭服务端进程（仅当前配置的服务端）
        /// </summary>
        public void KillProcess()
        {
            try
            {
                if (string.IsNullOrEmpty(_config.ExePath))
                {
                    _logManager.AddLog(_config.Name, "关闭失败：未配置程序路径", "错误");
                    return;
                }

                var targetPath = _config.ExePath.ToLowerInvariant();
                var processName = Path.GetFileNameWithoutExtension(_config.ExePath);
                bool isKilled = false;

                foreach (var process in Process.GetProcessesByName(processName))
                {
                    try
                    {
                        if (process.MainModule.FileName.ToLowerInvariant() == targetPath)
                        {
                            process.Kill();
                            process.WaitForExit(2000);
                            _logManager.AddLog(_config.Name, $"服务端已关闭（PID：{process.Id}）");
                            isKilled = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is Win32Exception && ((Win32Exception)ex).ErrorCode == -2147467259)
                            continue;

                        _logManager.AddLog(_config.Name, $"关闭进程出错：{ex.Message}", "警告");
                    }
                }

                if (!isKilled)
                {
                    _logManager.AddLog(_config.Name, "关闭失败：未找到运行中的服务端进程");
                }
            }
            catch (Exception ex)
            {
                var error = $"关闭服务端失败：{ex.Message}";
                _logManager.AddLog(_config.Name, error, "错误");
                MessageBox.Show(error);
            }
        }

        /// <summary>
        /// 发送指令到服务端
        /// </summary>
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