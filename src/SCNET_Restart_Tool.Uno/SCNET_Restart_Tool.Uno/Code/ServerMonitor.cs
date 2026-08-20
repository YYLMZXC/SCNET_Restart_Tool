using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端监控器（Uno 跨平台版）
/// - 定时任务（每日定时关闭 / 间隔关闭）
/// - 持续状态监控与自动启动
/// - 进程启停与 TCP 指令发送
/// </summary>
public class ServerMonitor : IDisposable
{
    private readonly ServerConfig _config;
    private readonly object _lock = new object();
    private Timer _timerDaily;
    private Timer _timerContinuous;
    private readonly Action<ServerConfig> _onStatusChanged;
    private readonly LogManager _logManager;
    private bool _disposed;

    /// <summary>UI 消息请求事件（替代 MessageBox，由界面层订阅并显示对话框）</summary>
    public event Action<string, string>? MessageRequested;

    /// <summary>是否在监控中</summary>
    public bool IsMonitoring => _config.IsMonitoring;

    public ServerMonitor(ServerConfig config, Action<ServerConfig> onStatusChanged)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _onStatusChanged = onStatusChanged ?? throw new ArgumentNullException(nameof(onStatusChanged));
        _logManager = LogManager.GetInstance();

        // 定时器：每日任务 1 秒检查一次；状态监控 3 秒检查一次
        _timerDaily = new Timer(TimerDaily_Tick, null, Timeout.Infinite, Timeout.Infinite);
        _timerContinuous = new Timer(TimerContinuous_Tick, null, Timeout.Infinite, Timeout.Infinite);

        UpdateStatus();
    }

    /// <summary>启动监控</summary>
    public void Start()
    {
        lock (_lock)
        {
            if (!_config.IsMonitoring)
            {
                _config.IsMonitoring = true;
                _timerDaily.Change(0, 1000);
                _timerContinuous.Change(0, 3000);
                UpdateStatus();
                _logManager.AddLog(_config.Name, "监控已开启（定时器已启动）");
            }
            else
            {
                _logManager.AddLog(_config.Name, "监控已处于开启状态（无需重复启动）", "调试");
            }
        }
    }

    /// <summary>停止监控</summary>
    public void Stop()
    {
        lock (_lock)
        {
            if (_config.IsMonitoring)
            {
                _config.IsMonitoring = false;
                _timerDaily.Change(Timeout.Infinite, Timeout.Infinite);
                _timerContinuous.Change(Timeout.Infinite, Timeout.Infinite);
                UpdateStatus();
                _logManager.AddLog(_config.Name, "监控已关闭（定时器已停止）");
            }
            else
            {
                _logManager.AddLog(_config.Name, "监控已处于关闭状态（无需重复停止）", "调试");
            }
        }
    }

    /// <summary>定时任务检查（每日定时关闭 / 间隔关闭）</summary>
    private void TimerDaily_Tick(object? state)
    {
        try
        {
            // 每日定时关闭
            if (TimeSpan.TryParse(_config.ScheduleTime, out TimeSpan scheduleTime))
            {
                var now = DateTime.Now;
                if (now.Hour == scheduleTime.Hours && now.Minute == scheduleTime.Minutes && now.Second == 0)
                {
                    _logManager.AddLog(_config.Name, $"触发每日定时关闭（设定时间：{_config.ScheduleTime}）");
                    if (_config.EnableCommands)
                    {
                        SendServiceCommand("close 9 例行维护");
                        Thread.Sleep(7000);
                    }
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
                    if (_config.EnableCommands)
                    {
                        SendServiceCommand("close 9 间隔维护");
                        Thread.Sleep(7000);
                    }
                    KillProcess();
                    _config.LastIntervalClose = DateTime.Now;
                }
            }
        }
        catch (Exception ex)
        {
            _logManager.AddLog(_config.Name, $"每日定时任务错误：{ex.Message}", "错误");
        }
    }

    /// <summary>持续检查服务端状态，自动启动</summary>
    private void TimerContinuous_Tick(object? state)
    {
        try
        {
            UpdateStatus();

            // 监控中且服务端未运行时，触发自动启动
            if (_config.IsMonitoring && _config.Status == ServerStatus.Stopped)
            {
                _logManager.AddLog(_config.Name, "监控检测到服务端未运行，尝试自动启动...");

                // 重试机制（防止偶然失败）
                int retryCount = 0;
                while (retryCount < 3 && !IsProcessRunning())
                {
                    StartProcess();
                    Thread.Sleep(2000);
                    retryCount++;
                }

                if (IsProcessRunning())
                {
                    _logManager.AddLog(_config.Name, "监控自动启动成功");
                }
                else
                {
                    _logManager.AddLog(_config.Name, "监控自动启动失败（已重试3次）", "错误");
                }
            }
        }
        catch (Exception ex)
        {
            _logManager.AddLog(_config.Name, $"监控定时器错误：{ex.Message}", "错误");
        }
    }

    /// <summary>更新服务端状态</summary>
    private void UpdateStatus()
    {
        var isProcessRunning = IsProcessRunning();
        var oldStatus = _config.Status;

        if (_config.IsMonitoring)
        {
            _config.Status = isProcessRunning ? ServerStatus.Monitoring : ServerStatus.Stopped;
        }
        else
        {
            _config.Status = isProcessRunning ? ServerStatus.Running : ServerStatus.Stopped;
        }

        if (oldStatus != _config.Status)
        {
            _logManager.AddLog(_config.Name, $"状态变更：{oldStatus} → {_config.Status}");
        }

        _onStatusChanged.Invoke(_config);
    }

    /// <summary>检查进程是否运行（跨平台兼容：优先 Process API）</summary>
    public bool IsProcessRunning()
    {
        if (string.IsNullOrEmpty(_config.ExePath))
        {
            _logManager.AddLog(_config.Name, "状态检测失败：未配置程序路径", "警告");
            return false;
        }

        string targetFileName = Path.GetFileName(_config.ExePath);
        string targetProcessName = Path.GetFileNameWithoutExtension(targetFileName);
        bool isRunning = false;

        try
        {
            foreach (var process in Process.GetProcessesByName(targetProcessName))
            {
                try
                {
                    if (process.MainModule?.FileName.Equals(_config.ExePath, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        isRunning = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    // 捕获权限或位数问题，不中断检测
                    _logManager.AddLog(_config.Name, $"进程路径验证警告：{ex.Message}", "警告");
                }
            }
        }
        catch (Exception ex)
        {
            _logManager.AddLog(_config.Name, $"状态检测失败：{ex.Message}", "错误");
        }

        _logManager.AddLog(_config.Name, $"监控状态检测：{(isRunning ? "运行中" : "已停止")}", "调试");
        return isRunning;
    }

    /// <summary>启动服务端进程</summary>
    public void StartProcess()
    {
        if (IsProcessRunning())
        {
            _logManager.AddLog(_config.Name, "启动失败：服务端已在运行中（禁止多开）", "警告");
            MessageRequested?.Invoke("提示", "服务端已在运行中，禁止多开！");
            return;
        }

        if (!File.Exists(_config.ExePath))
        {
            var error = $"启动失败：程序不存在（{_config.ExePath}）";
            _logManager.AddLog(_config.Name, error, "错误");
            MessageRequested?.Invoke("启动失败", error);
            return;
        }

        try
        {
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
            MessageRequested?.Invoke("启动失败", error);
        }
    }

    /// <summary>关闭服务端进程</summary>
    public void KillProcess()
    {
        if (string.IsNullOrEmpty(_config.ExePath))
        {
            _logManager.AddLog(_config.Name, "关闭失败：未配置程序路径", "错误");
            return;
        }

        string targetPath = _config.ExePath.ToLowerInvariant();
        string targetFileName = Path.GetFileName(_config.ExePath).ToLowerInvariant();
        bool isKilled = false;

        try
        {
            string processName = Path.GetFileNameWithoutExtension(targetFileName);
            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    string processPath = "";
                    try
                    {
                        processPath = process.MainModule?.FileName?.ToLowerInvariant() ?? "";
                    }
                    catch (Exception ex)
                    {
                        _logManager.AddLog(_config.Name, $"获取进程路径失败：{ex.Message}", "警告");
                    }

                    // 路径匹配即可视为目标进程
                    bool isMatch = processPath == targetPath ||
                                   processPath.EndsWith(targetFileName);

                    if (isMatch)
                    {
                        _logManager.AddLog(_config.Name, $"尝试关闭进程（PID：{process.Id}）");

                        if (process.CloseMainWindow())
                        {
                            if (process.WaitForExit(2000))
                            {
                                _logManager.AddLog(_config.Name, $"进程正常关闭（PID：{process.Id}）");
                                isKilled = true;
                                break;
                            }
                        }

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
        }
        catch (Exception ex)
        {
            _logManager.AddLog(_config.Name, $"关闭进程出错：{ex.Message}", "错误");
        }

        if (!isKilled)
        {
            _logManager.AddLog(_config.Name, "关闭失败：可能缺少管理员权限，请尝试以管理员身份运行程序", "错误");
        }
    }

    /// <summary>发送指令到服务端</summary>
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

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _timerDaily?.Dispose();
        _timerContinuous?.Dispose();
    }
}
