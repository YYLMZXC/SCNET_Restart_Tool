using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
string source = File.ReadAllText(@"d:\sc\cnb\sc\server\ServerSetting.json");
var root = JsonNode.Parse(source) as JsonObject ?? throw new InvalidOperationException();

// 模拟 TryWrite 全部 7 键：3 个 int、2 个 bool、2 个 string（强类型创建）
Write(root, "ServerPort", JsonValue.Create(28888)!);
Write(root, "RconEnabled", JsonValue.Create(true)!);
Write(root, "RconBindAddress", JsonValue.Create("0.0.0.0")!);
Write(root, "RconPort", JsonValue.Create(28901)!);
Write(root, "RconAllowPlaintext", JsonValue.Create(false)!);
Write(root, "RconCertificatePath", JsonValue.Create(@"C:\certs\rcon.pfx")!);
Write(root, "RconCommandTimeoutSeconds", JsonValue.Create(30)!);

string output = root.ToJsonString(options);
Console.WriteLine($"序列化成功，长度 {output.Length}");

// 回读验证全键
var v = new Dictionary<string, JsonNode>();
Collect(root, v);
foreach (var p in root) if (p.Value is not JsonObject) v[p.Key!] = p.Value!;
Console.WriteLine($"ServerPort={Ri("ServerPort")} RconEnabled={Rb("RconEnabled")} Bind={Rs("RconBindAddress")} " +
                  $"Port={Ri("RconPort")} Plain={Rb("RconAllowPlaintext")} Cert={Rs("RconCertificatePath")} Timeout={Ri("RconCommandTimeoutSeconds")}");
return 0;

static void Write(JsonObject root, string key, JsonNode value)
{
    if (!Set(root, key, value)) { JsonObject t; if (root.TryGetPropertyValue("远程控制（RCON）", out var n) && n is JsonObject o) t = o; else { t = new JsonObject(); root["远程控制（RCON）"] = t; } t[key] = value; }
}
static bool Set(JsonObject node, string key, JsonNode value)
{
    foreach (var p in node) { if (p.Value is JsonObject n) { if (Set(n, key, value)) return true; } else if (p.Key == key) { node[key] = value; return true; } }
    return false;
}
static void Collect(JsonObject node, Dictionary<string, JsonNode> v)
{
    foreach (var p in node) { if (p.Value is JsonObject n) Collect(n, v); else if (p.Key is not null) v[p.Key] = p.Value!; }
}
int Ri(string k) => v.TryGetValue(k, out var n) && n is JsonValue j && j.TryGetValue<int>(out var r) ? r : -1;
bool Rb(string k) => v.TryGetValue(k, out var n) && n is JsonValue j && j.TryGetValue<bool>(out var r) && r;
string Rs(string k) => v.TryGetValue(k, out var n) && n is JsonValue j && j.TryGetValue<string>(out var r) ? r : "";
