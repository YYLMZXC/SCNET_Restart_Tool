using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SCNET_Restart_Tool;

/// <summary>
/// RCON 指令执行结果（对应服务端响应体 {"ok":..,"output":[..]}/{"ok":false,"error":..}）。
/// </summary>
public sealed class RconCommandResult
{
    /// <summary>指令是否被服务端接受执行</summary>
    public bool Ok { get; }

    /// <summary>指令输出的全部行（Ok=false 时为空）</summary>
    public IReadOnlyList<string> Lines { get; }

    /// <summary>失败原因（Ok=true 时为 null）</summary>
    public string? Error { get; }

    private RconCommandResult(bool ok, IReadOnlyList<string> lines, string? error)
    {
        Ok = ok;
        Lines = lines;
        Error = error;
    }

    public static RconCommandResult Success(IReadOnlyList<string> lines) => new(true, lines, null);

    public static RconCommandResult Failure(string error) => new(false, Array.Empty<string>(), error);

    /// <summary>合并为单行展示文本（弹窗 / 日志用）</summary>
    public string ToDisplayText()
    {
        if (!Ok) return Error ?? "未知错误";
        return Lines.Count == 0 ? "（无输出）" : string.Join(Environment.NewLine, Lines);
    }
}

/// <summary>
/// 服务端指令服务：按 ScNetworkDEV 服务端（Survivalcraft/Servers/Rcon）的 RCON 标准接口通讯。
///
/// 协议约定（与服务端 RconServer 实现对齐）：
/// - 传输层：HTTP/1.1 单次请求-响应（短连接），服务端回复后即关闭连接；
///   默认 TLS（服务端首次启动自签证书，客户端按自签信任），可按配置退回明文；
/// - 指令端点：POST /rcon，请求头 X-RCON-Password 携带口令原文，
///   请求体 {"command":"..."}（命令可省略前导 '/'，服务端会自动补上）；
/// - 响应体：{"ok":true,"output":["..."]} 或 {"ok":false,"error":"..."}；
/// - 状态码：200 成功 / 400 请求不合法 / 401 认证失败 / 409 指令未执行 / 429 触发限流退避；
/// - 健康探测：GET /health，无需口令，返回 {"ok":true}，可用于确认 RCON 端口存活。
///
/// 超时与取消：连接超时 5 秒、响应超时 15 秒（与服务端 Socket 收发超时一致），
/// 全链路支持 CancellationToken 取消，避免外部通讯卡住调用方线程。
/// </summary>
public sealed class ServerCommandService
{
    /// <summary>RCON 协议路径：指令端点（与服务端 RconServer.PathRcon 一致）</summary>
    private const string PathRcon = "/rcon";

    /// <summary>RCON 协议路径：健康探测端点（无需口令）</summary>
    private const string PathHealth = "/health";

    /// <summary>RCON 口令请求头（与服务端 RconServer.PasswordHeader 一致）</summary>
    private const string PasswordHeader = "X-RCON-Password";

    /// <summary>
    /// RCON 口令最短长度（与服务端 RconPasswordHash.MinimumPasswordLength 一致）：
    /// 过短的口令在服务端设置口令时即被拒绝。
    /// </summary>
    public const int MinimumPasswordLength = 12;

    /// <summary>RCON 口令环境变量（与服务端 RconManager 读取的变量名一致）：
    /// 供启动服务端进程时注入，服务端仅在凭据文件不存在时应用（不覆盖手动设置的口令）</summary>
    public const string PasswordEnvironmentVariable = "SC_RCON_PASSWORD";

    /// <summary>建立 TCP 连接的超时时间</summary>
    private static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(5);

    /// <summary>读取响应的超时时间（与服务端 Send/ReceiveTimeout=15s 对齐）</summary>
    private static readonly TimeSpan ResponseTimeout = TimeSpan.FromSeconds(15);

    /// <summary>响应体最大读取字节数（防御异常超大响应）</summary>
    private const int MaxResponseBytes = 512 * 1024;

    private readonly ServerConfig _config;
    private readonly LogManager _logManager;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="config">服务端配置（使用 Ip、Port、Password、UseTls 等连接参数）</param>
    public ServerCommandService(ServerConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logManager = LogManager.GetInstance();
    }

    /// <summary>
    /// 发送指令到服务端并返回执行结果。
    /// 所有异常（连接失败、超时、取消等）都会被捕获并记录日志，返回失败结果。
    /// </summary>
    /// <param name="command">指令文本（如 "stop 9 例行维护"、"say hello"；可带或不带前导 '/'）</param>
    /// <param name="cancellationToken">取消令牌（关闭监控器 / 退出程序时可中断等待）</param>
    public async Task<RconCommandResult> SendCommandAsync(string command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            return RconCommandResult.Failure("指令内容为空");
        }

        // 标准协议要求口令必填且长度达标：无口令的 RCON 在服务端侧本就不会监听
        if (_config.Password.Length < MinimumPasswordLength)
        {
            var error = $"RCON 口令未配置或长度不足 {MinimumPasswordLength} 字符（服务端要求），无法发送指令";
            _logManager.AddLog(_config.Name, error, "错误");
            return RconCommandResult.Failure(error);
        }

        _logManager.AddLog(_config.Name, $"发送指令：{command}");
        try
        {
            string body = JsonSerializer.Serialize(new { command });
            RconHttpResponse response = await SendHttpRequestAsync("POST", PathRcon, body,
                (builder, payload) =>
                {
                    builder.Append(PasswordHeader).Append(": ").Append(_config.Password).Append("\r\n");
                }, cancellationToken).ConfigureAwait(false);

            RconCommandResult result = ParseCommandResponse(response);
            _logManager.AddLog(_config.Name, result.Ok
                ? $"指令响应：{result.ToDisplayText()}"
                : $"指令执行失败：{result.ToDisplayText()}", result.Ok ? "信息" : "警告");
            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            const string error = "指令发送已取消";
            _logManager.AddLog(_config.Name, error, "调试");
            return RconCommandResult.Failure(error);
        }
        catch (OperationCanceledException)
        {
            // 走到这里的是连接/读写超时（非调用方主动取消）：给出明确原因而非含糊的「操作已取消」
            var error = $"RCON 通讯超时（连接 {ConnectTimeout.TotalSeconds:0} 秒 / 响应 {ResponseTimeout.TotalSeconds:0} 秒上限，" +
                        $"目标 {_config.Ip}:{_config.Port}）";
            _logManager.AddLog(_config.Name, error, "错误");
            return RconCommandResult.Failure(error);
        }
        catch (Exception ex)
        {
            var error = $"指令发送失败：{ex.Message}";
            _logManager.AddLog(_config.Name, error, "错误");
            return RconCommandResult.Failure(error);
        }
    }

    /// <summary>
    /// 健康探测：GET /health（无需口令）。
    /// 用于确认服务端 RCON 端口存活，不执行任何指令。
    /// </summary>
    public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            RconHttpResponse response = await SendHttpRequestAsync("GET", PathHealth, "", null, cancellationToken)
                .ConfigureAwait(false);
            bool healthy = response.StatusCode == 200;
            _logManager.AddLog(_config.Name,
                $"RCON 健康探测{(healthy ? "成功" : "异常")}（{_config.Ip}:{_config.Port} → HTTP {response.StatusCode}）", "调试");
            return healthy;
        }
        catch (Exception ex)
        {
            _logManager.AddLog(_config.Name, $"RCON 健康探测失败：{ex.Message}", "调试");
            return false;
        }
    }

    // ==================== HTTP 传输实现 ====================

    /// <summary>
    /// 执行一次单请求-响应的 HTTP/1.1 通讯（服务端回复后即关闭连接，读取以 EOF / Content-Length 为准）。
    /// 连接与响应分别受 ConnectTimeout / ResponseTimeout 约束，可通过取消令牌中断。
    /// </summary>
    private async Task<RconHttpResponse> SendHttpRequestAsync(
        string method, string path, string body,
        Action<StringBuilder, byte[]>? appendHeaders,
        CancellationToken cancellationToken)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        _logManager.AddLog(_config.Name,
            $"RCON 连接 {_config.Ip}:{_config.Port}（传输：{(_config.UseTls ? "TLS" : "明文")}，{method} {path}）", "调试");

        using var client = new TcpClient();
        timeout.CancelAfter(ConnectTimeout);
        await client.ConnectAsync(_config.Ip, _config.Port, timeout.Token).ConfigureAwait(false);

        // 建立传输流：默认 TLS（服务端使用自签证书，按自签信任校验），配置明文时直连
        Stream stream;
        if (_config.UseTls)
        {
            var ssl = new SslStream(client.GetStream(), leaveInnerStreamOpen: false,
                new RemoteCertificateValidationCallback(ValidateSelfSignedCertificate));
            timeout.CancelAfter(ResponseTimeout);
            await ssl.AuthenticateAsClientAsync(new SslClientAuthenticationOptions
            {
                TargetHost = _config.Ip,
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
                CertificateRevocationCheckMode = X509RevocationMode.NoCheck
            }, timeout.Token).ConfigureAwait(false);
            _logManager.AddLog(_config.Name,
                $"RCON TLS 握手完成：{ssl.SslProtocol}（套件 {ssl.NegotiatedCipherSuite}）", "调试");
            stream = ssl;
        }
        else
        {
            stream = client.GetStream();
        }

        using (stream)
        {
            byte[] payload = Encoding.UTF8.GetBytes(body);
            var builder = new StringBuilder(256);
            builder.Append(method).Append(' ').Append(path).Append(" HTTP/1.1\r\n");
            builder.Append("Host: ").Append(_config.Ip).Append(':').Append(_config.Port).Append("\r\n");
            builder.Append("Content-Type: application/json; charset=utf-8\r\n");
            builder.Append("Content-Length: ").Append(payload.Length).Append("\r\n");
            builder.Append("Connection: close\r\n");
            appendHeaders?.Invoke(builder, payload);
            builder.Append("\r\n");

            byte[] head = Encoding.ASCII.GetBytes(builder.ToString());
            timeout.CancelAfter(ResponseTimeout);
            await stream.WriteAsync(head, timeout.Token).ConfigureAwait(false);
            if (payload.Length > 0)
            {
                await stream.WriteAsync(payload, timeout.Token).ConfigureAwait(false);
            }
            await stream.FlushAsync(timeout.Token).ConfigureAwait(false);

            byte[] raw = await ReadUntilEndAsync(stream, timeout.Token).ConfigureAwait(false);
            RconHttpResponse response = ParseHttpResponse(raw);
            _logManager.AddLog(_config.Name,
                $"RCON 响应到达：{method} {path} → HTTP {response.StatusCode}（{raw.Length} 字节）", "调试");
            return response;
        }
    }

    /// <summary>
    /// TLS 证书校验回调：服务端 RCON 证书默认为首次启动自签生成，
    /// 客户端按「信任该端点自签证书」策略放行（管理通道通常部署于本机 / 内网受信环境）。
    /// </summary>
    private static bool ValidateSelfSignedCertificate(object sender, X509Certificate? certificate,
        X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        // 自签证书只可能出现 PolicyErrors，其余错误同样放行：校验目标是「可用性」而非「身份链」
        return true;
    }

    /// <summary>循环读取响应直到对端关闭（Connection: close）或达到上限</summary>
    private static async Task<byte[]> ReadUntilEndAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var buffer = new MemoryStream();
        byte[] chunk = new byte[4096];
        while (buffer.Length < MaxResponseBytes)
        {
            int read = await stream.ReadAsync(chunk.AsMemory(), cancellationToken).ConfigureAwait(false);
            if (read <= 0) break;
            await buffer.WriteAsync(chunk.AsMemory(0, read), cancellationToken).ConfigureAwait(false);
        }
        return buffer.ToArray();
    }

    // ==================== 响应解析 ====================

    /// <summary>把 RCON 响应体解析为指令结果，并按状态码给出可读的失败描述</summary>
    private static RconCommandResult ParseCommandResponse(RconHttpResponse response)
    {
        if (response.StatusCode != 200)
        {
            return RconCommandResult.Failure(DescribeStatus(response));
        }

        try
        {
            using JsonDocument document = JsonDocument.Parse(response.Body);
            JsonElement root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return RconCommandResult.Failure("响应体不是合法 JSON 对象");
            }

            bool ok = root.TryGetProperty("ok", out JsonElement okElement)
                      && okElement.ValueKind == JsonValueKind.True;

            if (!ok)
            {
                string error = root.TryGetProperty("error", out JsonElement errorElement)
                                && errorElement.ValueKind == JsonValueKind.String
                    ? errorElement.GetString() ?? "未知错误"
                    : "未知错误";
                return RconCommandResult.Failure(error);
            }

            var lines = new List<string>();
            if (root.TryGetProperty("output", out JsonElement outputElement)
                && outputElement.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in outputElement.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.String)
                    {
                        lines.Add(item.GetString() ?? string.Empty);
                    }
                }
            }
            return RconCommandResult.Success(lines);
        }
        catch (JsonException)
        {
            return RconCommandResult.Failure("响应体不是合法 JSON");
        }
    }

    /// <summary>按 HTTP 状态码生成失败描述（401/429 的响应体不含区别信息，与服务端约定一致）</summary>
    private static string DescribeStatus(RconHttpResponse response)
    {
        return response.StatusCode switch
        {
            400 => $"请求不合法：{ExtractError(response, "请求体不合法")}",
            401 => "认证失败（请检查 RCON 口令）",
            404 => "未知接口（目标端口可能不是 RCON 通道）",
            405 => "接口不支持该请求方法",
            409 => $"指令未执行：{ExtractError(response, "命令未执行")}",
            429 => "认证尝试过于频繁，已触发服务端限流，请稍后再试",
            _ => $"服务端返回异常状态码 {response.StatusCode}"
        };
    }

    /// <summary>从错误响应体提取 error 字段，失败时回退默认描述</summary>
    private static string ExtractError(RconHttpResponse response, string fallback)
    {
        try
        {
            using JsonDocument document = JsonDocument.Parse(response.Body);
            if (document.RootElement.TryGetProperty("error", out JsonElement element)
                && element.ValueKind == JsonValueKind.String)
            {
                return element.GetString() ?? fallback;
            }
        }
        catch (JsonException)
        {
            // 响应体不是 JSON 时使用回退描述
        }
        return fallback;
    }

    /// <summary>解析原始 HTTP 响应：状态行 + 头 + 体</summary>
    private static RconHttpResponse ParseHttpResponse(byte[] raw)
    {
        string text = Encoding.UTF8.GetString(raw);
        int separator = text.IndexOf("\r\n\r\n", StringComparison.Ordinal);
        if (separator < 0)
        {
            throw new InvalidDataException("响应缺少 HTTP 头结束标记");
        }

        string head = text[..separator];
        string body = text[(separator + 4)..];

        string[] lines = head.Split("\r\n");
        string[] statusParts = lines[0].Split(' ');
        if (statusParts.Length < 2 || !int.TryParse(statusParts[1], out int statusCode))
        {
            throw new InvalidDataException("响应状态行无法解析");
        }

        // 按 Content-Length 截取响应体，防御对端关闭前残留的无关字节
        foreach (string line in lines[1..])
        {
            int colon = line.IndexOf(':');
            if (colon <= 0) continue;
            string name = line[..colon].Trim();
            if (string.Equals(name, "Content-Length", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(line[(colon + 1)..].Trim(), out int length)
                && length >= 0 && length <= body.Length)
            {
                body = body[..length];
                break;
            }
        }

        return new RconHttpResponse(statusCode, body);
    }

    /// <summary>最小 HTTP 响应（状态码 + 响应体）</summary>
    private readonly record struct RconHttpResponse(int StatusCode, string Body);
}
