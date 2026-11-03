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

        // 初始化一次状态显示（创建监控器时同步当前真实状态）
        UpdateStatus();
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
                UpdateStatus();
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
                UpdateStatus();
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
    /// 关闭前若启用了指令功能，先发送 close 指令并异步等待服务端保存存档，再强制关闭进程。
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
                    if (_config.EnableCommands)
                    {
                        // 先发送 close 指令，等待服务端存档退出后再强制关闭（异步等待，不阻塞线程池线程）
                        _commandService.SendServiceCommand("close 9 例行维护");
                        await Task.Delay(7000);
                    }
                    _processService.KillProcess();
                }
            }

            // ---- 间隔关闭 ----
            if (_config.IntervalHours > 0)
            {
                var nextClose = _config.LastIntervalClose.AddHours(_config.IntervalHours);
                if (DateTime.Now >= nextClose)
                {
                    _logManager.AddLog(_config.Name, $"触发间隔关闭（间隔：{_config.IntervalHours}小时）");
                    if (_config.EnableCommands)
                    {
                        _commandService.SendServiceCommand("close 9 间隔维护");
                        await Task.Delay(7000);
                    }
                    _processService.KillProcess();
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
            UpdateStatus();

            // 监控中且服务端未运行时，触发自动启动
            if (_config.IsMonitoring && _config.Status == ServerStatus.Stopped)
            {
                _logManager.AddLog(_config.Name, "监控检测到服务端未运行，尝试自动启动...");

                // 重试机制（防止偶发启动失败），最多尝试 3 次
                int retryCount = 0;
                while (retryCount < 3 && !_processService.IsProcessRunning())
                {
                    _processService.StartProcess();
                    await Task.Delay(2000); // 等待进程启动完成后再检测
                    retryCount++;
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
    /// 监控开启时状态显示为 Monitoring/Stopped，否则为 Running/Stopped；
    /// 状态发生变化时输出状态变更日志。
    /// </summary>
    private void UpdateStatus()
    {
        var isProcessRunning = _processService.IsProcessRunning();
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

        // 通知 UI 层刷新（调用方负责切回 UI 线程）
        _onStatusChanged.Invoke(_config);
    }

    // ==================== 公开门面方法（转发到底层服务） ====================

    /// <summary>检查服务端进程是否运行（委托给 ServerProcessService）</summary>
    public bool IsProcessRunning() => _processService.IsProcessRunning();

    /// <summary>启动服务端进程（委托给 ServerProcessService）</summary>
    public void StartProcess() => _processService.StartProcess();

    /// <summary>关闭服务端进程（委托给 ServerProcessService）</summary>
    public void KillProcess() => _processService.KillProcess();

    /// <summary>发送指令到服务端（委托给 ServerCommandService）</summary>
    public string SendServiceCommand(string command) => _commandService.SendServiceCommand(command);

    /// <summary>释放定时器等非托管资源</summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _timerDaily?.Dispose();
        _timerContinuous?.Dispose();
    }
}
