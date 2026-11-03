using System;
using System.Threading;
using System.Threading.Tasks;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端监控调度器：专注于「何时做什么」的调度职责。
/// - 每日定时关闭 / 间隔关闭（1 秒定时器逐秒检查）
/// - 持续状态监控与自动启动（3 秒定时器轮询）
/// 底层进程操作已拆分为 ServerProcessService，指令通讯已拆分为 ServerCommandService，
/// 本类通过组合方式使用它们，仅保留公开门面方法供 UI 层调用，实现高内聚、低耦合。
/// 原实现中的 Thread.Sleep 阻塞调用已改为异步等待（await Task.Delay），
/// 并加入防重入标志，避免定时器回调并发重入导致重复关闭。
/// </summary>
public class ServerMonitor : IDisposable
{
    private readonly ServerConfig _config;
    private readonly ServerProcessService _processService;
    private readonly ServerCommandService _commandService;
    private readonly Action<ServerConfig> _onStatusChanged;
    private readonly LogManager _logManager;
    private readonly object _lock = new object();

    // 定时器：每日任务 1 秒检查一次；状态监控 3 秒检查一次
    private readonly Timer _timerDaily;
    private readonly Timer _timerContinuous;

    // 防重入标志：定时回调已改为异步（async void），
    // 通过 Interlocked 原子交换防止上一次任务未完成时重复执行
    private int _dailyTaskRunning;
    private int _continuousTaskRunning;

    private bool _disposed;

    /// <summary>
    /// UI 消息请求事件（替代 MessageBox，由界面层订阅并显示对话框）。
    /// 底层 ServerProcessService 的消息请求会统一转发到本事件。
    /// </summary>
    public event Action<string, string>? MessageRequested;

    /// <summary>是否在监控中</summary>
    public bool IsMonitoring => _config.IsMonitoring;

    /// <summary>是否为远程管理模式（状态探测与停止均走 RCON 网络通道）</summary>
    public bool IsRemote => _config.Mode == ManageMode.Remote;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="config">要监控的服务端配置</param>
    /// <param name="onStatusChanged">状态变更回调（通常在后台线程触发，由调用方负责切回 UI 线程）</param>
    public ServerMonitor(ServerConfig config, Action<ServerConfig> onStatusChanged)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _onStatusChanged = onStatusChanged ?? throw new ArgumentNullException(nameof(onStatusChanged));
        _logManager = LogManager.GetInstance();

        // 组合职责单一的底层服务，并转发其消息请求事件（如“启动失败”“禁止多开”提示）
        _processService = new ServerProcessService(config);
        _processService.MessageRequested += (title, message) => MessageRequested?.Invoke(title, message);
        _commandService = new ServerCommandService(config);

        // 定时器初始处于暂停状态（Infinite），由 Start() 启动
        _timerDaily = new Timer(TimerDaily_Tick, null, Timeout.Infinite, Timeout.Infinite);
        _timerContinuous = new Timer(TimerContinuous_Tick, null, Timeout.Infinite, Timeout.Infinite);

        // 初始化一次状态显示（创建监控器时同步当前真实状态；远程模式健康探测为异步，不阻塞构造）
        _ = UpdateStatusAsync();
    }

    /// <summary>
    /// 启动监控：将服务器标记为监控中，并启动两个定时器。
    /// 幂等操作，重复调用不会造成多次启动。
    /// </summary>
    public void Start()
    {
        lock (_lock)
        {
            if (!_config.IsMonitoring)
            {
                _config.IsMonitoring = true;
                _timerDaily.Change(0, 1000);          // 立即执行一次，之后每秒检查
                _timerContinuous.Change(0, 3000);     // 立即执行一次，之后每 3 秒检查
                _ = UpdateStatusAsync();
                _logManager.AddLog(_config.Name, "监控已开启（定时器已启动）");
            }
            else
            {
                _logManager.AddLog(_config.Name, "监控已处于开启状态（无需重复启动）", "调试");
            }
        }
    }

    /// <summary>
    /// 停止监控：将服务器标记为非监控中，并暂停两个定时器。
    /// 幂等操作，重复调用不会造成异常。
    /// </summary>
    public void Stop()
    {
        lock (_lock)
        {
            if (_config.IsMonitoring)
            {
                _config.IsMonitoring = false;
                _timerDaily.Change(Timeout.Infinite, Timeout.Infinite);
                _timerContinuous.Change(Timeout.Infinite, Timeout.Infinite);
                _ = UpdateStatusAsync();
                _logManager.AddLog(_config.Name, "监控已关闭（定时器已停止）");
            }
            else
            {
                _logManager.AddLog(_config.Name, "监控已处于关闭状态（无需重复停止）", "调试");
            }
        }
    }

    /// <summary>
    /// 每日任务检查（每秒触发一次）：
    /// 1) 到达设定的定时时间（HH:mm，秒为 0）时，执行每日定时关闭；
    /// 2) 距上次间隔关闭超过 IntervalHours 时，执行间隔关闭。
    /// 关闭前若启用了指令功能，先经 RCON 发送 stop 指令（服务端延迟 N 秒存档退出），
    /// 等待「延迟秒数 + 存档余量」后再兜底强制关闭进程。
    /// </summary>
    private async void TimerDaily_Tick(object? state)
    {
        // 防重入：上一次任务未完成时直接返回，避免并发重复关闭
        if (Interlocked.Exchange(ref _dailyTaskRunning, 1) == 1) return;

        try
        {
            // ---- 每日定时关闭 ----
            if (TimeSpan.TryParse(_config.ScheduleTime, out TimeSpan scheduleTime))
            {
                var now = DateTime.Now;
                if (now.Hour == scheduleTime.Hours && now.Minute == scheduleTime.Minutes && now.Second == 0)
                {
                    _logManager.AddLog(_config.Name, $"触发每日定时关闭（设定时间：{_config.ScheduleTime}）");
                    await GracefulShutdownAsync("例行维护");
                    // 远程模式无本地进程控制权，仅靠 RCON stop 让服务端自行存档退出
                    if (!IsRemote)
                    {
                        _processService.KillProcess();
                    }
                }
            }

            // ---- 间隔关闭 ----
            if (_config.IntervalHours > 0)
            {
                var nextClose = _config.LastIntervalClose.AddHours(_config.IntervalHours);
                if (DateTime.Now >= nextClose)
                {
                    _logManager.AddLog(_config.Name, $"触发间隔关闭（间隔：{_config.IntervalHours}小时）");
                    await GracefulShutdownAsync("间隔维护");
                    if (!IsRemote)
                    {
                        _processService.KillProcess();
                    }
                    // 记录本次间隔关闭时间，作为下次间隔的起点
                    _config.LastIntervalClose = DateTime.Now;
                }
            }
        }
        catch (Exception ex)
        {
            _logManager.AddLog(_config.Name, $"每日定时任务错误：{ex.Message}", "错误");
        }
        finally
        {
            // 释放防重入标志，允许下一次任务执行
            Interlocked.Exchange(ref _dailyTaskRunning, 0);
        }
    }

    /// <summary>
    /// 持续状态检查（每 3 秒触发一次）：
    /// 监控中且服务端未运行时，自动尝试启动（最多重试 3 次，每次间隔 2 秒）。
    /// </summary>
    private async void TimerContinuous_Tick(object? state)
    {
        // 防重入：上一次任务未完成时直接返回
        if (Interlocked.Exchange(ref _continuousTaskRunning, 1) == 1) return;

        try
        {
            await UpdateStatusAsync();

            // 监控中且服务端未运行时，触发自动启动（仅本地模式：远程无启动通道，服务端需在宿主机自行启动）
            if (!IsRemote && _config.IsMonitoring && _config.Status == ServerStatus.Stopped)
            {
                _logManager.AddLog(_config.Name, "监控检测到服务端未运行，尝试自动启动...");

                // 重试机制（防止偶发启动失败），最多尝试 3 次
                int retryCount = 0;
                while (retryCount < 3 && !_processService.IsProcessRunning())
                {
                    retryCount++;
                    _logManager.AddLog(_config.Name, $"自动启动尝试第 {retryCount}/3 次", "调试");
                    _processService.StartProcess();
                    await Task.Delay(2000); // 等待进程启动完成后再检测
                }

                if (_processService.IsProcessRunning())
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
        finally
        {
            // 释放防重入标志，允许下一次任务执行
            Interlocked.Exchange(ref _continuousTaskRunning, 0);
        }
    }

    /// <summary>
    /// 更新服务端运行状态并通知 UI 刷新。
    /// - 本地模式：枚举本地进程判断存活；
    /// - 远程模式：GET /health 网络探测 RCON 端口（静默轮询，不刷调试日志）。
    /// 监控开启时状态显示为 Monitoring/Stopped，否则为 Running/Stopped；
    /// 状态发生变化时输出状态变更日志。
    /// </summary>
    private async Task UpdateStatusAsync()
    {
        var isRunning = IsRemote
            ? await _commandService.CheckHealthAsync(quiet: true).ConfigureAwait(false)
            : _processService.IsProcessRunning();
        var oldStatus = _config.Status;

        if (_config.IsMonitoring)
        {
            _config.Status = isRunning ? ServerStatus.Monitoring : ServerStatus.Stopped;
        }
        else
        {
            _config.Status = isRunning ? ServerStatus.Running : ServerStatus.Stopped;
        }

        if (oldStatus != _config.Status)
        {
            _logManager.AddLog(_config.Name, $"状态变更：{oldStatus} → {_config.Status}");
        }

        // 通知 UI 层刷新（调用方负责切回 UI 线程）
        _onStatusChanged.Invoke(_config);
    }

    // ==================== 公开门面方法（转发到底层服务） ====================

    /// <summary>
    /// 检查服务端是否运行：本地模式查进程存活，远程模式做一次健康探测。
    /// </summary>
    public async Task<bool> IsRunningAsync() => IsRemote
        ? await _commandService.CheckHealthAsync().ConfigureAwait(false)
        : _processService.IsProcessRunning();

    /// <summary>启动服务端进程（仅本地模式可用；远程模式无启动通道，记录警告）</summary>
    public void StartProcess()
    {
        if (IsRemote)
        {
            _logManager.AddLog(_config.Name, "远程管理模式不支持启动：请在服务端宿主机上启动服务端", "警告");
            return;
        }
        _processService.StartProcess();
    }

    /// <summary>关闭服务端进程（仅本地模式可用；远程模式请使用 StopAsync）</summary>
    public void KillProcess()
    {
        if (IsRemote)
        {
            _logManager.AddLog(_config.Name, "远程管理模式无法强制结束进程，请使用停止（RCON stop）", "警告");
            return;
        }
        _processService.KillProcess();
    }

    /// <summary>
    /// 停止服务器（按管理模式自动选择通道）：
    /// - 远程模式：经 RCON 发送 stop 指令（服务端延迟存档退出），无强杀兜底；
    /// - 本地模式：与既有行为一致，直接结束本地进程。
    /// </summary>
    /// <param name="reason">停止原因（透传给服务端公告）</param>
    public async Task StopAsync(string reason = "远程停止")
    {
        if (IsRemote)
        {
            await GracefulShutdownAsync(reason).ConfigureAwait(false);
            await UpdateStatusAsync().ConfigureAwait(false);
        }
        else
        {
            _processService.KillProcess();
        }
    }

    /// <summary>
    /// 经 RCON 标准接口向服务端发送指令并返回展示文本（委托给 ServerCommandService）。
    /// 底层含连接 / 响应超时，不会无限阻塞调用方。
    /// </summary>
    public async Task<string> SendServiceCommandAsync(string command)
    {
        var result = await _commandService.SendCommandAsync(command).ConfigureAwait(false);
        return result.ToDisplayText();
    }

    /// <summary>
    /// 优雅关闭：经 RCON 发送 stop 指令，服务端会在延迟 N 秒内存档并自行退出。
    /// 等待「延迟秒数 + 存档余量（3 秒）」后返回，由调用方决定是否兜底强杀进程。
    /// 本地模式未启用指令功能、或指令失败时直接返回（交由强杀流程处理）；
    /// 远程模式 RCON 是唯一通道，指令失败只能告警（无强杀手段）。
    /// </summary>
    /// <param name="reason">关闭原因（透传给服务端公告）</param>
    private async Task GracefulShutdownAsync(string reason)
    {
        if (!_config.EnableCommands && !IsRemote) return;

        // ScNetworkDEV 标准：stop [秒数] [原因]，延迟 9 秒给玩家留缓冲并完成存档
        const int delaySeconds = 9;
        _logManager.AddLog(_config.Name, $"开始优雅关闭：发送 stop {delaySeconds} {reason}，服务端将延迟存档退出", "信息");
        var result = await _commandService.SendCommandAsync($"stop {delaySeconds} {reason}").ConfigureAwait(false);
        if (!result.Ok)
        {
            _logManager.AddLog(_config.Name, IsRemote
                ? $"远程停止指令未生效（{result.ToDisplayText()}），远程模式无法强制关闭，请检查网络与口令"
                : $"优雅关闭指令未生效（{result.ToDisplayText()}），将直接强制关闭", "警告");
            return;
        }

        // 等待服务端按延迟计划自行退出（含存档余量），不阻塞线程池线程
        _logManager.AddLog(_config.Name, $"优雅关闭指令已接受，等待服务端 {delaySeconds + 3} 秒内自行存档退出", "调试");
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds + 3)).ConfigureAwait(false);
    }

    /// <summary>释放定时器等非托管资源</summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _timerDaily?.Dispose();
        _timerContinuous?.Dispose();
    }
}
