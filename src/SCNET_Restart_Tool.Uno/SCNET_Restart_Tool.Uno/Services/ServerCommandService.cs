using System;
using System.IO;
using System.Net.Sockets;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端指令服务：通过 TCP 协议向服务端发送管理指令并读取响应。
/// 从 ServerMonitor 中独立出来，使「指令通讯」与「监控调度」职责分离：
/// - ServerCommandService：只负责指令的发送与响应读取；
/// - ServerMonitor：负责决定何时发送指令。
/// 该服务不依赖任何 UI，若配置了密码则自动完成认证，简单易复用。
/// </summary>
public sealed class ServerCommandService
{
    private readonly ServerConfig _config;
    private readonly LogManager _logManager;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="config">服务端配置（使用 Ip、Port、Password 等连接参数）</param>
    public ServerCommandService(ServerConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logManager = LogManager.GetInstance();
    }

    /// <summary>
    /// 发送指令到服务端并返回响应内容。
    /// 协议约定：
    /// - 未配置密码：直接发送 "command {指令}"；
    /// - 配置了密码：先发送 "password {密码} command {指令}" 完成认证并执行。
    /// 所有异常（连接失败、超时等）都会被捕获并记录日志，返回错误描述文本。
    /// </summary>
    /// <param name="command">要发送的指令文本（如 "close 9 例行维护"）</param>
    /// <returns>服务端返回的响应文本；失败时返回错误描述</returns>
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
