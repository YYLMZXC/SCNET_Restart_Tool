using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端 ServerSetting.json 的读取快照（只提取管理工具关心的三个键）。
/// </summary>
public sealed class ServerSettingSnapshot
{
    /// <summary>游戏服务端口（键：ServerPort，默认 28887）</summary>
    public int ServerPort { get; init; }

    /// <summary>是否启用 RCON 远程控制（键：RconEnabled，默认 false）</summary>
    public bool RconEnabled { get; init; }

    /// <summary>RCON 监听端口（键：RconPort，默认 28900）</summary>
    public int RconPort { get; init; }
}

/// <summary>
/// 服务端设置读写服务：直接读写服务端根目录下的 ServerSetting.json，
/// 支持查看/修改游戏端口（ServerPort）、开启 RCON（RconEnabled）与 RCON 端口（RconPort）。
///
/// 与服务端解析规则对齐（ServerSettingJson.cs）：
/// - 键名为英文属性名，可出现在任意嵌套节内，根级扁平键优先级最高；
/// - 写入时优先「就地更新已存在的键」，键不存在时落到规范节（网络与中转 / 远程控制（RCON）），
///   其余所有配置原样保留（JsonNode 整树往返，不丢字段）；
/// - 端口写入前做范围校验，且 RconPort 不得等于 ServerPort
///   （服务端 Normalize 会在两端口相同时直接关闭 RCON，属于必须提前拦下的冲突）。
///
/// 注意：修改落盘后需重启服务端进程（或重载存档触发 Apply）才会生效；
/// RCON 真正可用还需在服务端控制台执行 rcon password 或设置 SC_RCON_PASSWORD 环境变量。
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

    /// <summary>序列化选项：缩进保持与服务端一致；中文键不转义，便于运维直接查看编辑</summary>
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>解析出的当前值（文件缺失或键不存在时取服务端同款默认值）</summary>
    public static ServerSettingSnapshot Defaults => new()
    {
        ServerPort = DefaultServerPort,
        RconEnabled = false,
        RconPort = DefaultRconPort
    };

    /// <summary>配置文件完整路径（由服务器程序路径推导服务端根目录）</summary>
    public static string GetSettingPath(ServerConfig config)
    {
        string root = string.IsNullOrEmpty(config.ExePath)
            ? ""
            : (Path.GetDirectoryName(config.ExePath) ?? "");
        return Path.Combine(root, FileName);
    }

    /// <summary>
    /// 读取服务端设置。文件不存在或解析失败时返回 false 并给出可读原因。
    /// </summary>
    public static bool TryRead(ServerConfig config, out ServerSettingSnapshot snapshot, out string error)
    {
        snapshot = Defaults;
        string path = GetSettingPath(config);
        if (string.IsNullOrEmpty(config.ExePath))
        {
            error = "未配置服务器程序路径，无法定位 ServerSetting.json";
            return false;
        }
        if (!File.Exists(path))
        {
            error = $"配置文件不存在：{path}";
            return false;
        }

        try
        {
            JsonNode? root = JsonNode.Parse(File.ReadAllText(path));
            if (root is not JsonObject)
            {
                error = "ServerSetting.json 根节点不是 JSON 对象";
                return false;
            }

            // 与服务端语义一致：先递归收全部层级的嵌套标量键，再由根级扁平键覆盖（与出现顺序无关）
            var rootObject = (JsonObject)root;
            var values = new System.Collections.Generic.Dictionary<string, JsonNode>();
            CollectNestedScalarKeys(rootObject, values);
            foreach (var pair in rootObject)
            {
                if (pair.Value is not JsonObject && pair.Key is not null)
                {
                    values[pair.Key] = pair.Value!;
                }
            }

            snapshot = new ServerSettingSnapshot
            {
                ServerPort = ReadInt(values, "ServerPort", DefaultServerPort),
                RconEnabled = ReadBool(values, "RconEnabled", false),
                RconPort = ReadInt(values, "RconPort", DefaultRconPort)
            };
            error = "";
            return true;
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            error = $"读取 ServerSetting.json 失败：{ex.Message}";
            return false;
        }
    }

    /// <summary>
    /// 修改服务端设置（就地更新已存在的键，缺失键落规范节，其余字段原样保留）。
    /// 文件不存在时创建一份只含目标键的新配置。
    /// </summary>
    /// <param name="config">目标服务器（用 ExePath 定位服务端根目录）</param>
    /// <param name="serverPort">新的游戏服务端口</param>
    /// <param name="rconEnabled">是否开启 RCON</param>
    /// <param name="rconPort">新的 RCON 端口</param>
    /// <param name="error">失败原因（成功时为空串）</param>
    public static bool TryWrite(ServerConfig config, int serverPort, bool rconEnabled, int rconPort, out string error)
    {
        if (serverPort < 1 || serverPort > 65535)
        {
            error = $"服务端端口无效：{serverPort}（应在 1-65535 之间）";
            return false;
        }
        if (rconPort < 1 || rconPort > 65535)
        {
            error = $"RCON 端口无效：{rconPort}（应在 1-65535 之间）";
            return false;
        }
        // 服务端 Normalize 规则：两端口相同时会直接关闭 RCON —— 提前拦截，避免写出自相矛盾的配置
        if (rconEnabled && rconPort == serverPort)
        {
            error = "RCON 端口不能与服务端端口相同（服务端会因此直接关闭 RCON）";
            return false;
        }

        string path = GetSettingPath(config);
        if (string.IsNullOrEmpty(config.ExePath))
        {
            error = "未配置服务器程序路径，无法定位 ServerSetting.json";
            return false;
        }

        try
        {
            JsonObject root;
            if (File.Exists(path))
            {
                root = JsonNode.Parse(File.ReadAllText(path)) as JsonObject
                       ?? throw new JsonException("根节点不是 JSON 对象");
            }
            else
            {
                root = new JsonObject();
            }

            // 就地更新（仅第一处命中；服务端读取时根级键优先，此写法与已有文件的键位保持一致）
            if (!SetKeyInPlace(root, "ServerPort", serverPort))
            {
                AddToSection(root, NetworkSection, "ServerPort", serverPort);
            }
            if (!SetKeyInPlace(root, "RconEnabled", rconEnabled))
            {
                AddToSection(root, RconSection, "RconEnabled", rconEnabled);
            }
            if (!SetKeyInPlace(root, "RconPort", rconPort))
            {
                AddToSection(root, RconSection, "RconPort", rconPort);
            }

            File.WriteAllText(path, root.ToJsonString(JsonOptions));
            error = "";
            return true;
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            error = $"写入 ServerSetting.json 失败：{ex.Message}";
            return false;
        }
    }

    // ==================== 读取辅助 ====================

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

    // ==================== 写入辅助 ====================

    /// <summary>
    /// 就地更新已存在的键：深度优先查找首个同名标量键并改值。
    /// 返回是否命中；未命中（返回 false）时由调用方把键落到规范节。
    /// </summary>
    private static bool SetKeyInPlace(JsonObject node, string key, object value)
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
                node[key] = JsonValue.Create(value);
                return true;
            }
        }
        return false;
    }

    /// <summary>把键写入指定节（节不存在时创建），返回是否已写入</summary>
    private static bool AddToSection(JsonObject root, string section, string key, object value)
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
        target[key] = JsonValue.Create(value);
        return true;
    }
}
