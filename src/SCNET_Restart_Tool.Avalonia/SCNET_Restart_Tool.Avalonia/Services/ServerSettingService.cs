using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端 ServerSetting.json 的读取快照：ServerPort 与 RCON 全部 6 个配置键
/// （RconEnabled / RconBindAddress / RconPort / RconAllowPlaintext / RconCertificatePath / RconCommandTimeoutSeconds）。
/// </summary>
public sealed class ServerSettingSnapshot
{
    /// <summary>游戏服务端口（键：ServerPort，默认 28887）</summary>
    public int ServerPort { get; init; }

    /// <summary>是否启用 RCON 远程控制（键：RconEnabled，默认 false）</summary>
    public bool RconEnabled { get; init; }

    /// <summary>RCON 监听地址（键：RconBindAddress，空白即环回 127.0.0.1，默认 127.0.0.1）</summary>
    public string BindAddress { get; init; } = "127.0.0.1";

    /// <summary>RCON 监听端口（键：RconPort，默认 28900）</summary>
    public int RconPort { get; init; }

    /// <summary>
    /// 是否允许明文传输（键：RconAllowPlaintext，默认 false）。
    /// 服务端语义：true = 纯明文（不加载证书）；false = 纯 TLS（明文请求直接丢弃）——二选一，非共存。
    /// </summary>
    public bool AllowPlaintext { get; init; }

    /// <summary>TLS 证书（pfx）路径（键：RconCertificatePath，空 = 服务端首次启动自签并持久化；相对路径按服务端外部数据根解析）</summary>
    public string CertificatePath { get; init; } = "";

    /// <summary>单条 RCON 指令的最长等待秒数（键：RconCommandTimeoutSeconds，范围 1-120，默认 10）</summary>
    public int CommandTimeoutSeconds { get; init; } = 10;
}

/// <summary>
/// 服务端设置读写服务：直接读写服务端根目录下的 ServerSetting.json，
/// 支持查看/修改游戏端口（ServerPort）与 RCON 全部 6 个配置键，并与工具侧配置同步。
///
/// 与服务端解析规则对齐（ServerSettingJson.cs / ServerSetting.Normalize）：
/// - 键名为英文属性名，可出现在任意嵌套节内，根级扁平键优先级最高；
/// - 写入时优先「就地更新已存在的键」，键不存在时落到规范节（网络与中转 / 远程控制（RCON）），
///   其余所有配置原样保留（JsonNode 整树往返，不丢字段）；
/// - 端口范围 1-65535，且 RconPort 不得等于 ServerPort（两端口相同时服务端会直接关闭 RCON）；
/// - RconBindAddress 空白回退 127.0.0.1 并 Trim；RconCertificatePath 只 Trim；
/// - RconCommandTimeoutSeconds 服务端会 Clamp 到 1-120，此处提前按同范围校验。
///
/// 工具侧同步语义（由调用方应用）：
/// - 指令通讯端口对齐 RconPort；
/// - 开启 RCON 视为启用指令功能；
/// - UseTls 对齐 !RconAllowPlaintext（服务端传输层二选一，不一致就连不上）；
/// - RconBindAddress 为环回地址时连接 IP 对齐该地址，非环回（如 0.0.0.0）时保留用户配置。
///
/// 注意：修改落盘后需重启服务端进程（或重载存档触发 Apply）才会生效；
/// RCON 真正可用还需设置口令（由本工具启动服务端时自动注入 SC_RCON_PASSWORD，或服务端控制台 rcon password）。
/// </summary>
public static class ServerSettingService
{
    /// <summary>配置文件名（与服务端 ServerSettingStore.FileName 一致）</summary>
    private const string FileName = "ServerSetting.json";

    /// <summary>ServerPort 所属的规范节名（与服务端 ServerSettingJson 分组一致）</summary>
    private const string NetworkSection = "网络与中转";

    /// <summary>RCON 键所属的规范节名（与服务端 ServerSettingJson 分组一致）</summary>
    private const string RconSection = "远程控制（RCON）";

    /// <summary>游戏端口默认值（与服务端 SettingsManager.DefaultServerPort 一致）</summary>
    public const int DefaultServerPort = 28887;

    /// <summary>RCON 端口默认值（与服务端 ServerSetting.RconPort 一致）</summary>
    public const int DefaultRconPort = 28900;

    /// <summary>RCON 监听地址默认值（与服务端一致：仅环回，公网暴露必须显式改写）</summary>
    public const string DefaultBindAddress = "127.0.0.1";

    /// <summary>指令超时秒数默认值（与服务端 RconCommandTimeoutSeconds 一致）</summary>
    public const int DefaultCommandTimeoutSeconds = 10;

    /// <summary>指令超时秒数下限（与服务端 Clamp 下限一致）</summary>
    public const int MinCommandTimeoutSeconds = 1;

    /// <summary>指令超时秒数上限（与服务端 Clamp 上限一致）</summary>
    public const int MaxCommandTimeoutSeconds = 120;

    /// <summary>序列化选项：缩进保持与服务端一致；中文键不转义，便于运维直接查看编辑</summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>全局日志管理器（单例）：读取 / 写入 / 校验失败均落日志，便于排查配置问题</summary>
    private static readonly LogManager _logManager = LogManager.GetInstance();

    /// <summary>解析出的当前值（文件缺失或键不存在时取服务端同款默认值）</summary>
    public static ServerSettingSnapshot Defaults => new()
    {
        ServerPort = DefaultServerPort,
        RconEnabled = false,
        BindAddress = DefaultBindAddress,
        RconPort = DefaultRconPort,
        AllowPlaintext = false,
        CertificatePath = "",
        CommandTimeoutSeconds = DefaultCommandTimeoutSeconds
    };

    /// <summary>配置文件完整路径（由服务器程序路径推导服务端根目录）</summary>
    public static string GetSettingPath(ServerConfig config)
    {
        string root = string.IsNullOrEmpty(config.ExePath)
            ? ""
            : (Path.GetDirectoryName(config.ExePath) ?? "");
        return Path.Combine(root, FileName);
    }

    /// <summary>读取服务端设置。文件不存在或解析失败时返回 false 并给出可读原因。</summary>
    public static bool TryRead(ServerConfig config, out ServerSettingSnapshot snapshot, out string error)
    {
        snapshot = Defaults;
        string path = GetSettingPath(config);
        if (string.IsNullOrEmpty(config.ExePath))
        {
            error = "未配置服务器程序路径，无法定位 ServerSetting.json";
            _logManager.AddLog(config.Name, $"读取 ServerSetting.json 失败：{error}", "调试");
            return false;
        }
        if (!File.Exists(path))
        {
            error = $"配置文件不存在：{path}";
            // 选中服务器预填时每回都会触发，属于常态情况，只记调试日志
            _logManager.AddLog(config.Name, $"ServerSetting.json 不存在（显示默认值）：{path}", "调试");
            return false;
        }

        try
        {
            JsonNode? root = JsonNode.Parse(File.ReadAllText(path));
            if (root is not JsonObject)
            {
                error = "ServerSetting.json 根节点不是 JSON 对象";
                _logManager.AddLog(config.Name, $"读取 ServerSetting.json 失败：{error}", "错误");
                return false;
            }

            // 与服务端语义一致：先递归收全部层级的嵌套标量键，再由根级扁平键覆盖（与出现顺序无关）
            var values = CollectValues((JsonObject)root);
            snapshot = Normalize(SnapshotFromValues(values));
            error = "";
            _logManager.AddLog(config.Name, DescribeSnapshot("已读取 ServerSetting.json", snapshot, path), "调试");
            return true;
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            error = $"读取 ServerSetting.json 失败：{ex.Message}";
            _logManager.AddLog(config.Name, error, "错误");
            return false;
        }
    }

    /// <summary>
    /// 修改服务端设置（就地更新已存在的键，缺失键落规范节，其余字段原样保留）。
    /// 文件不存在时创建一份只含目标键的新配置。
    /// </summary>
    /// <param name="config">目标服务器（用 ExePath 定位服务端根目录）</param>
    /// <param name="settings">要写入的完整设置快照（写入前按服务端规则校验与规范化）</param>
    /// <param name="error">失败原因（成功时为空串）</param>
    public static bool TryWrite(ServerConfig config, ServerSettingSnapshot settings, out string error)
    {
        settings = Normalize(settings);

        if (settings.ServerPort < 1 || settings.ServerPort > 65535)
        {
            error = $"服务端端口无效：{settings.ServerPort}（应在 1-65535 之间）";
            _logManager.AddLog(config.Name, $"写入 ServerSetting.json 被拒绝：{error}", "警告");
            return false;
        }
        if (settings.RconPort < 1 || settings.RconPort > 65535)
        {
            error = $"RCON 端口无效：{settings.RconPort}（应在 1-65535 之间）";
            _logManager.AddLog(config.Name, $"写入 ServerSetting.json 被拒绝：{error}", "警告");
            return false;
        }
        // 服务端 Normalize 规则：两端口相同时会直接关闭 RCON —— 提前拦截，避免写出自相矛盾的配置
        if (settings.RconEnabled && settings.RconPort == settings.ServerPort)
        {
            error = "RCON 端口不能与服务端端口相同（服务端会因此直接关闭 RCON）";
            _logManager.AddLog(config.Name, $"写入 ServerSetting.json 被拒绝：{error}", "警告");
            return false;
        }
        if (settings.CommandTimeoutSeconds < MinCommandTimeoutSeconds || settings.CommandTimeoutSeconds > MaxCommandTimeoutSeconds)
        {
            error = $"指令超时秒数无效：{settings.CommandTimeoutSeconds}（应在 {MinCommandTimeoutSeconds}-{MaxCommandTimeoutSeconds} 之间，服务端会强制收敛到该范围）";
            _logManager.AddLog(config.Name, $"写入 ServerSetting.json 被拒绝：{error}", "警告");
            return false;
        }

        string path = GetSettingPath(config);
        if (string.IsNullOrEmpty(config.ExePath))
        {
            error = "未配置服务器程序路径，无法定位 ServerSetting.json";
            _logManager.AddLog(config.Name, $"写入 ServerSetting.json 失败：{error}", "错误");
            return false;
        }

        try
        {
            bool fileExisted = File.Exists(path);
            JsonObject root;
            if (fileExisted)
            {
                root = JsonNode.Parse(File.ReadAllText(path)) as JsonObject
                       ?? throw new JsonException("根节点不是 JSON 对象");
            }
            else
            {
                root = new JsonObject();
            }

            // 旧值快照：供日志描述「改了什么」；无变化时跳过写盘，避免无意义的文件时间戳变动
            var updated = settings;
            string changes = fileExisted ? DescribeChanges(Normalize(SnapshotFromValues(CollectValues(root))), updated) : "";

            if (fileExisted && changes.Length == 0)
            {
                error = "";
                _logManager.AddLog(config.Name, "ServerSetting.json 内容无变化，跳过写入", "调试");
                return true;
            }

            // 就地更新（仅第一处命中；服务端读取时根级键优先，此写法与已有文件的键位保持一致）。
            // 注意：必须用强类型 JsonValue.Create(int/bool/string) 生成 JsonValuePrimitive——
            // 经 object 装箱的 JsonValue.Create(object) 会生成 JsonValueCustomized，
            // 写出时需要 TypeInfoResolver（本项目为 source-gen 模式，无反射元数据，会直接抛异常）
            WriteKey(root, "ServerPort", JsonValue.Create(updated.ServerPort)!);
            WriteKey(root, "RconEnabled", JsonValue.Create(updated.RconEnabled)!);
            WriteKey(root, "RconBindAddress", JsonValue.Create(updated.BindAddress)!);
            WriteKey(root, "RconPort", JsonValue.Create(updated.RconPort)!);
            WriteKey(root, "RconAllowPlaintext", JsonValue.Create(updated.AllowPlaintext)!);
            WriteKey(root, "RconCertificatePath", JsonValue.Create(updated.CertificatePath)!);
            WriteKey(root, "RconCommandTimeoutSeconds", JsonValue.Create(updated.CommandTimeoutSeconds)!);

            File.WriteAllText(path, root.ToJsonString(JsonOptions));
            error = "";
            _logManager.AddLog(config.Name, fileExisted
                ? $"ServerSetting.json 已更新：{changes}（{path}）"
                : DescribeSnapshot("已创建 ServerSetting.json", updated, path));
            return true;
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            error = $"写入 ServerSetting.json 失败：{ex.Message}";
            _logManager.AddLog(config.Name, error, "错误");
            return false;
        }
    }

    /// <summary>写单个键：先尝试就地更新，未命中时落到规范节</summary>
    private static void WriteKey(JsonObject root, string key, JsonNode value, string section = RconSection)
    {
        if (!SetKeyInPlace(root, key, value))
        {
            AddToSection(root, section, key, value);
        }
    }

    /// <summary>
    /// 按服务端规则规范化快照：监听地址空白回退环回并 Trim（大小写不敏感的 Localhost 规整为 IP），
    /// 证书路径 Trim。数值键的范围校验由 TryWrite 的显式拒绝承担（与服务端「静默收敛」不同，
    /// 工具侧要让用户看到确切原因），此处不做静默改写。
    /// </summary>
    private static ServerSettingSnapshot Normalize(ServerSettingSnapshot snapshot) => new()
    {
        ServerPort = snapshot.ServerPort,
        RconEnabled = snapshot.RconEnabled,
        BindAddress = string.IsNullOrWhiteSpace(snapshot.BindAddress)
            ? DefaultBindAddress
            : (string.Equals(snapshot.BindAddress.Trim(), "localhost", StringComparison.OrdinalIgnoreCase)
                ? DefaultBindAddress
                : snapshot.BindAddress.Trim()),
        RconPort = snapshot.RconPort,
        AllowPlaintext = snapshot.AllowPlaintext,
        CertificatePath = snapshot.CertificatePath?.Trim() ?? "",
        CommandTimeoutSeconds = snapshot.CommandTimeoutSeconds
    };

    // ==================== 读取辅助 ====================

    /// <summary>
    /// 按服务端语义收集键值：先递归收全部层级的嵌套标量键，再由根级扁平键覆盖（与出现顺序无关）。
    /// </summary>
    private static System.Collections.Generic.Dictionary<string, JsonNode> CollectValues(JsonObject root)
    {
        var values = new System.Collections.Generic.Dictionary<string, JsonNode>();
        CollectNestedScalarKeys(root, values);
        foreach (var pair in root)
        {
            if (pair.Value is not JsonObject && pair.Key is not null)
            {
                values[pair.Key] = pair.Value!;
            }
        }
        return values;
    }

    /// <summary>从收集好的键值表解析出全部管理键的快照（键缺失时取服务端默认值）</summary>
    private static ServerSettingSnapshot SnapshotFromValues(System.Collections.Generic.Dictionary<string, JsonNode> values) => new()
    {
        ServerPort = ReadInt(values, "ServerPort", DefaultServerPort),
        RconEnabled = ReadBool(values, "RconEnabled", false),
        BindAddress = ReadString(values, "RconBindAddress", DefaultBindAddress),
        RconPort = ReadInt(values, "RconPort", DefaultRconPort),
        AllowPlaintext = ReadBool(values, "RconAllowPlaintext", false),
        CertificatePath = ReadString(values, "RconCertificatePath", ""),
        CommandTimeoutSeconds = ReadInt(values, "RconCommandTimeoutSeconds", DefaultCommandTimeoutSeconds)
    };

    /// <summary>生成一行快照描述（日志用）</summary>
    private static string DescribeSnapshot(string prefix, ServerSettingSnapshot snapshot, string path) =>
        $"{prefix}：服务端端口 {snapshot.ServerPort}；RCON {(snapshot.RconEnabled ? "开启" : "关闭")}" +
        $"（地址 {snapshot.BindAddress}，端口 {snapshot.RconPort}，" +
        $"{(snapshot.AllowPlaintext ? "明文传输" : "TLS 加密")}，" +
        $"证书 {(snapshot.CertificatePath.Length == 0 ? "自签" : snapshot.CertificatePath)}，" +
        $"指令超时 {snapshot.CommandTimeoutSeconds}s）（{path}）";

    /// <summary>对比新旧快照，只列出发生变化的键（日志「改了什么」用）；无变化返回空串</summary>
    private static string DescribeChanges(ServerSettingSnapshot oldSnapshot, ServerSettingSnapshot updated)
    {
        var items = new System.Collections.Generic.List<string>();
        if (oldSnapshot.ServerPort != updated.ServerPort)
        {
            items.Add($"ServerPort {oldSnapshot.ServerPort} → {updated.ServerPort}");
        }
        if (oldSnapshot.RconEnabled != updated.RconEnabled)
        {
            items.Add($"RconEnabled {(oldSnapshot.RconEnabled ? "开启" : "关闭")} → {(updated.RconEnabled ? "开启" : "关闭")}");
        }
        if (oldSnapshot.BindAddress != updated.BindAddress)
        {
            items.Add($"RconBindAddress {oldSnapshot.BindAddress} → {updated.BindAddress}");
        }
        if (oldSnapshot.RconPort != updated.RconPort)
        {
            items.Add($"RconPort {oldSnapshot.RconPort} → {updated.RconPort}");
        }
        if (oldSnapshot.AllowPlaintext != updated.AllowPlaintext)
        {
            items.Add($"RconAllowPlaintext {(oldSnapshot.AllowPlaintext ? "明文" : "TLS")} → {(updated.AllowPlaintext ? "明文" : "TLS")}");
        }
        if (oldSnapshot.CertificatePath != updated.CertificatePath)
        {
            items.Add($"RconCertificatePath {(oldSnapshot.CertificatePath.Length == 0 ? "（自签）" : oldSnapshot.CertificatePath)} → {(updated.CertificatePath.Length == 0 ? "（自签）" : updated.CertificatePath)}");
        }
        if (oldSnapshot.CommandTimeoutSeconds != updated.CommandTimeoutSeconds)
        {
            items.Add($"RconCommandTimeoutSeconds {oldSnapshot.CommandTimeoutSeconds} → {updated.CommandTimeoutSeconds}");
        }
        return string.Join("；", items);
    }

    /// <summary>递归收集嵌套节内的标量键（不收根级扁平键，根级由调用方按「优先覆盖」语义另行合入）</summary>
    private static void CollectNestedScalarKeys(JsonObject node, System.Collections.Generic.Dictionary<string, JsonNode> values)
    {
        foreach (var pair in node)
        {
            if (pair.Value is JsonObject nested)
            {
                CollectNestedScalarKeys(nested, values);
            }
            else if (pair.Key is not null)
            {
                values[pair.Key] = pair.Value!;
            }
        }
    }

    private static int ReadInt(System.Collections.Generic.Dictionary<string, JsonNode> values, string key, int fallback)
    {
        return values.TryGetValue(key, out JsonNode? node) && node is JsonValue value && value.TryGetValue<int>(out int result)
            ? result
            : fallback;
    }

    private static bool ReadBool(System.Collections.Generic.Dictionary<string, JsonNode> values, string key, bool fallback)
    {
        return values.TryGetValue(key, out JsonNode? node) && node is JsonValue value && value.TryGetValue<bool>(out bool result)
            ? result
            : fallback;
    }

    private static string ReadString(System.Collections.Generic.Dictionary<string, JsonNode> values, string key, string fallback)
    {
        return values.TryGetValue(key, out JsonNode? node) && node is JsonValue value && value.TryGetValue<string>(out string? result)
            ? result
            : fallback;
    }

    // ==================== 写入辅助 ====================

    /// <summary>
    /// 就地更新已存在的键：深度优先查找首个同名标量键并改值。
    /// 返回是否命中；未命中（返回 false）时由调用方把键落到规范节。
    /// value 由调用方以强类型 JsonValue.Create(int/bool/string) 生成（避免装箱走 JsonValueCustomized 路径）。
    /// </summary>
    private static bool SetKeyInPlace(JsonObject node, string key, JsonNode value)
    {
        foreach (var pair in node)
        {
            if (pair.Value is JsonObject nested)
            {
                if (SetKeyInPlace(nested, key, value))
                {
                    return true;
                }
            }
            else if (pair.Key == key)
            {
                node[key] = value;
                return true;
            }
        }
        return false;
    }

    /// <summary>把键写入指定节（节不存在时创建），返回是否已写入</summary>
    private static bool AddToSection(JsonObject root, string section, string key, JsonNode value)
    {
        JsonObject target;
        if (root.TryGetPropertyValue(section, out JsonNode? sectionNode) && sectionNode is JsonObject existing)
        {
            target = existing;
        }
        else
        {
            target = new JsonObject();
            root[section] = target;
        }
        target[key] = value;
        return true;
    }
}
