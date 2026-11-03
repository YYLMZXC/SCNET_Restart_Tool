using System.Text.Json.Serialization;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务器管理方式：区分本地与远程两种管理域。
/// - Local（本地管理）：服务端与本工具同机，可控制进程启停、读写 ServerSetting.json、管理服务器目录文件夹；
/// - Remote（远程管理）：服务端位于其他机器，本工具仅通过 RCON 网络通道与其通讯
///   （状态探测 GET /health、停止/指令 POST /rcon），不依赖任何本地路径。
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ManageMode
{
    /// <summary>本地管理：依赖 ExePath 的进程级控制（默认值，兼容旧配置）</summary>
    Local = 0,

    /// <summary>远程管理：仅 RCON 网络通道，无本地进程/文件访问能力</summary>
    Remote = 1
}
