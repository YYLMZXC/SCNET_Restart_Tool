using System;
using System.Diagnostics;
using System.IO;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端进程操作服务：负责服务端进程的启动、关闭与运行状态检测。
/// 该服务从 ServerMonitor 中独立出来，使「进程管理」与「监控调度」职责分离：
/// - ServerProcessService：专注进程相关的具体操作；
/// - ServerMonitor：专注「何时执行什么操作」的调度逻辑。
/// 通过这种拆分实现高内聚、低耦合，也便于将来对进程逻辑进行独立测试与复用。
/// </summary>
public sealed class ServerProcessService
{
    private readonly ServerConfig _config;
    private readonly LogManager _logManager;

    /// <summary>
    /// UI 消息请求事件（如“启动失败”“禁止多开”等提示）。
    /// 本服务不直接依赖任何 UI 框架，仅通过事件上抛，由上层（ServerMonitor → MainPage）订阅并显示对话框。
    /// </summary>
    public event Action<string, string>? MessageRequested;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="config">要管理的服务端配置（仅读取其路径、端口等字段，不修改其状态）</param>
    public ServerProcessService(ServerConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logManager = LogManager.GetInstance();
    }

    /// <summary>
    /// 检查服务端进程是否正在运行。
    /// 按「进程名 + 可执行文件完整路径」双重匹配，避免误判系统中其他同名进程；
    /// 跨平台兼容：优先使用 Process API，路径验证失败时仅记录警告而不中断检测。
    /// </summary>
    public bool IsProcessRunning()
    {
        // 前置校验：未配置程序路径时无法检测
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
                    // 路径完全一致才算匹配，防止误判同名进程（如 java.exe）
                    if (process.MainModule?.FileName.Equals(_config.ExePath, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        isRunning = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    // 权限不足或位数差异可能导致无法读取 MainModule，仅警告，不中断检测
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

    /// <summary>
    /// 启动服务端进程。
    /// 前置校验：禁止多开、程序文件必须存在；
    /// 启动参数附带世界标识（world=Id）与唯一令牌（scnet_tool_id），便于区分多开实例。
    /// </summary>
    public void StartProcess()
    {
        // 校验一：禁止多开
        if (IsProcessRunning())
        {
            _logManager.AddLog(_config.Name, "启动失败：服务端已在运行中（禁止多开）", "警告");
            MessageRequested?.Invoke("提示", "服务端已在运行中，禁止多开！");
            return;
        }

        // 校验二：程序文件必须存在
        if (!File.Exists(_config.ExePath))
        {
            var error = $"启动失败：程序不存在（{_config.ExePath}）";
            _logManager.AddLog(_config.Name, error, "错误");
            MessageRequested?.Invoke("启动失败", error);
            return;
        }

        try
        {
            // 附带唯一参数，避免同一世界被重复启动
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

    /// <summary>
    /// 关闭服务端进程。
    /// 先尝试优雅关闭（CloseMainWindow，等待 2 秒），失败或超时后强制结束（Kill）；
    /// 全程按进程路径匹配目标，避免误杀其他同名进程。
    /// </summary>
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
                    // 读取进程路径（可能因权限失败，仅警告）
                    string processPath = "";
                    try
                    {
                        processPath = process.MainModule?.FileName?.ToLowerInvariant() ?? "";
                    }
                    catch (Exception ex)
                    {
                        _logManager.AddLog(_config.Name, $"获取进程路径失败：{ex.Message}", "警告");
                    }

                    // 路径完全一致或结尾匹配即视为目标进程
                    bool isMatch = processPath == targetPath ||
                                   processPath.EndsWith(targetFileName);
                    if (!isMatch)
                    {
                        continue;
                    }

                    _logManager.AddLog(_config.Name, $"尝试关闭进程（PID：{process.Id}）");

                    // 第一步：尝试优雅关闭（向进程窗口发送关闭消息，让服务端正常存档退出）
                    if (process.CloseMainWindow())
                    {
                        if (process.WaitForExit(2000))
                        {
                            _logManager.AddLog(_config.Name, $"进程正常关闭（PID：{process.Id}）");
                            isKilled = true;
                            break;
                        }
                    }

                    // 第二步：优雅关闭失败或超时，强制结束进程
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
}
