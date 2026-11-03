using System.Text;
using SCNET_Restart_Tool;

namespace SCNET_Restart_Tool.Avalonia;

/// <summary>
/// 帮助菜单内容（功能说明 / 项目信息 / 版本信息 / 检查更新 / 关于）。
///
/// 内容与当前功能保持一致；所有版本信息统一取自 <see cref="AppVersion"/>，
/// 其版本号按构建时间自动生成，避免帮助内容与程序实际构建不一致。
/// </summary>
public static class HelpContent
{
    /// <summary>功能说明：按主界面各功能区块描述实际行为与操作方法</summary>
    public static string FunctionDescription => @"【服务器管理】
- 支持同时配置与管理多个 SCNET 服务端实例，各自独立设置监控与重启策略
- 列表展示名称、程序名、IP:端口、程序路径与实时运行状态
- 「增加 / 修改 / 删除」维护配置，配置保存于程序目录 servers_config.json
- 监控中的服务器禁止修改与删除，避免运行中的配置被误改
- 「文件夹」进入该服务器的目录管理页

【运行控制】
- 「启动 / 停止 / 重启」作用于当前选中的服务器，「停止全部」可一次关闭所有运行中的服务端
- 重启时先关闭进程并等待其完全退出，再启动新实例，避免端口被占用
- 程序路径无效时直接给出提示，不会误启动其他程序

【监控与自动重启】
- 开启监控后每 3 秒检测一次进程状态，服务异常退出时自动拉起（最多重试 3 次）
- 定时重启：每天到设定时间（HH:mm）自动关闭服务端，随后由监控重新拉起
- 间隔重启：按设定的小时数周期关闭，填 0 表示不启用该策略
- 启用指令功能时，关闭前先经 RCON 发送 stop 指令（服务端延迟 9 秒存档退出），超时再兜底强杀
- 监控状态随配置持久化，下次启动程序自动恢复

【远程指令（RCON 标准接口）】
- 与 ScNetworkDEV 服务端的 RCON 通道对接：HTTP/1.1 短连接，默认端口 28900
- 默认 TLS 加密（服务端自签证书），仅内网受信环境可取消「加密连接」选项
- 勾选「启用指令功能」并配置 IP / 端口 / RCON 口令后可发送指令（口令必填，至少 12 字符）
- 由本工具启动服务端时会自动注入 SC_RCON_PASSWORD 环境变量应用口令（服务端手动设置过则不覆盖）
- 底部「常用指令」下拉内置服务端支持的常用命令（stop / list / kick / ban / op / give / time / gamerule 等），选中即填入输入框；也可手动输入，可省略前导斜杠
- 输入框内按回车等效于点击「发送」；需要带参数的指令（如 kick <玩家>）选中后请替换尖括号占位符
- 底层自带连接 5 秒 / 响应 15 秒超时，不会因网络问题卡住界面

【服务端设置（ServerSetting.json）】
- 选中服务器即自动读取其根目录 ServerSetting.json，预填服务端端口与 RCON 全部配置
- 支持直接修改：服务端端口（ServerPort）、RCON 开关（RconEnabled）、监听地址（RconBindAddress）、
  RCON 端口（RconPort）、明文开关（RconAllowPlaintext）、TLS 证书路径（RconCertificatePath，空=自签）、
  指令超时秒数（RconCommandTimeoutSeconds，1-120）
- 「应用到服务端」就地写回，其余服务端配置原样保留；写入后需重启服务端生效
- 应用时自动同步本工具连接参数：通讯端口对齐 RCON 端口；开启 RCON 顺带启用指令功能；
  加密方式对齐传输层（服务端明文 / TLS 二选一）；监听地址为环回时连接 IP 一并对齐
- 监听地址默认仅环回 127.0.0.1；改为 0.0.0.0 等对外地址等于把控制面暴露到所有网卡，请自行承担风险
- 允许明文时口令会在网络上以可读形式出现，仅建议本机 / 受信内网使用
- 口令（≥12 字符）在本工具编辑面板配置即可：启动服务端时自动经 SC_RCON_PASSWORD 注入，无需在服务端控制台手动执行 rcon password（服务端已手动设置过口令时不覆盖，以服务端为准）

【文件夹管理】
- 按标签页管理 Bugs、CharacterSkins、Configs、TexturePacks、NetMods、Plugins、Worlds 目录
- 通用目录支持刷新、返回上级、打开目录、添加文件、删除选中
- Bugs 页可直接分页预览 Game.log，并支持清理过期日志
- 「批量备份所有目录」可一次性备份服务端的各个资源目录

【运行日志】
- 主界面底部实时显示运行日志，按 500ms 批量刷新，避免高频日志导致界面卡顿
- 日志同时按日期写入程序目录 log_yyyyMMdd.txt，界面显示可随时清空（不影响日志文件）

【帮助菜单】
- 右上角「帮助」提供：功能说明、项目信息、版本信息、检查更新、关于

【使用步骤】
1. 点击「增加」填写服务器名称、程序路径（浏览选择服务端程序）、IP / 端口 / 密码
2. 按需设置定时重启时间与间隔小时数，决定是否启用指令功能，点击「保存配置」
3. 选中服务器后可启动 / 停止 / 重启，或「开启监控」交由程序自动守护
4. 需要远程操作时，在指令框输入内容并点击「发送指令」";

    /// <summary>项目信息：项目简介、特点、技术栈与运行环境</summary>
    public static string ProjectInfo => $@"项目名称：SCNET_Restart_Tool
当前版本：{AppVersion.Full}
构建时间：{AppVersion.BuildTimeText}

项目简介：
SCNET 服务端管理工具，用于批量管理 SCNET 服务端程序，实时监控进程状态，
在服务异常退出时自动重启，并提供定时 / 间隔重启与 TCP 远程指令控制，
减少人工值守成本，提高服务可用性。

项目特点：
- 多服务器统一管理，配置集中保存
- 进程状态实时监控，异常退出自动恢复
- 定时重启与间隔重启两种策略可组合使用
- RCON 标准远程控制（HTTP/1.1 + TLS，口令认证）
- 目录快捷访问，支持批量备份与日志清理
- 运行日志实时展示并按日期落盘

技术栈：
- 语言：C#
- 运行时：.NET 10
- 界面：Avalonia UI 11（Fluent 主题，跨平台 XAML）
- 配置：System.Text.Json（servers_config.json）
- 通讯：RCON（HTTP/1.1 over TLS，ScNetworkDEV 服务端标准接口）

适用平台：
- Windows 10 / 11
- macOS 12 及以上
- Linux（支持 .NET 10 的发行版）

项目地址（三处仓库内容同步）：
{AppVersion.ProjectUrlText}

许可证：MIT

注意事项：
- Windows 下监控系统进程建议以管理员身份运行
- 优雅关闭仅在 Windows 有效，macOS / Linux 上为直接结束进程
- RCON 口令至少 12 个字符（服务端要求）；由本工具启动服务端时自动注入，服务端手动设置过则以服务端为准
- 服务端默认仅监听环回地址（127.0.0.1:28900），远程管理需显式修改服务端配置
- 取消「加密连接」仅在已受信的本机 / 内网环境使用，公网必须保留 TLS";

    /// <summary>版本信息：当前版本（构建时间生成）、版本规则与更新日志</summary>
    public static string VersionInfo
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine($"=== {AppVersion.ProductName} 版本信息 ===");
            sb.AppendLine();
            sb.AppendLine($"【当前版本】 {AppVersion.Full}");
            sb.AppendLine($"【构建时间】 {AppVersion.BuildTimeText}");
            sb.AppendLine("【版本规则】 版本号取自构建时间，格式 yy.MM.dd-HHmm，每次编译自动更新");
            sb.AppendLine();
            sb.AppendLine("=== 版本更新日志 ===");
            sb.AppendLine();
            sb.AppendLine("【本版本（版本号按构建时间自动编号）】");
            sb.AppendLine($"- 当前构建：{AppVersion.Full}（{AppVersion.BuildTimeText}）");
            sb.AppendLine("- 指令通道更新为 ScNetworkDEV 服务端 RCON 标准接口（HTTP/1.1 + TLS，默认端口 28900）");
            sb.AppendLine("- 加载配置时自动把旧默认端口 5612 迁移为实际默认端口 28900（自定义端口不受影响）");
            sb.AppendLine("- 新增服务端设置分组：直读直写 ServerSetting.json，可改服务端端口、开启 RCON 与 RCON 端口");
            sb.AppendLine("- 由本工具启动服务端时自动注入 SC_RCON_PASSWORD 口令（服务端手动设置过则不覆盖），口令校验对齐服务端 12 字符下限");
            sb.AppendLine("- 指令区新增「常用指令」下拉快捷选择与「发送」按钮（输入框回车等效），内置服务端实际支持的命令集");
            sb.AppendLine("- 服务端设置扩展为 RCON 全部 6 键（监听地址 / 明文开关 / 证书路径 / 指令超时），应用时自动同步本工具的端口 / 加密 / 连接地址");
            sb.AppendLine("- 优雅关闭指令由 close 改为标准 stop 指令（延迟 9 秒存档退出）");
            sb.AppendLine("- 指令通讯加入连接 / 响应超时，网络异常不再无限等待");
            sb.AppendLine("- 帮助菜单内容更新：功能说明、项目信息、版本信息、检查更新、关于");
            sb.AppendLine("- 版本号改为按构建时间自动生成，窗口标题仅显示到日（yy.MM.dd）");
            sb.AppendLine("- 界面标题栏同步显示当前版本号");
            sb.AppendLine();
            sb.AppendLine("【v2.1.0】Avalonia 跨平台版");
            sb.AppendLine("- 新增基于 Avalonia UI 11 的跨平台桌面版本，一套代码支持 Windows / macOS / Linux");
            sb.AppendLine("- 对话框与文件选择改用 DialogWindow / StorageProvider，不再依赖 WinRT");
            sb.AppendLine();
            sb.AppendLine("【v2.0.0】Uno 迁移版");
            sb.AppendLine("- UI 层迁移至 Uno Platform 重写，核心逻辑层保持复用");
            sb.AppendLine("- 配置格式由 XML 切换为 JSON");
            sb.AppendLine();
            sb.AppendLine("【v1.0.0】");
            sb.AppendLine("- 初始版本：多服务器监控与自动重启");
            sb.AppendLine();
            sb.AppendLine("=== 系统要求 ===");
            sb.AppendLine();
            sb.AppendLine("- 操作系统：Windows 10/11、macOS 12+、Linux（.NET 10 支持）");
            sb.AppendLine("- 运行时：.NET 10");
            sb.AppendLine("- 内存：512MB 或更高");
            sb.AppendLine("- 磁盘空间：50MB 可用空间");
            sb.AppendLine("- 权限：Windows 下建议以管理员身份运行");
            return sb.ToString().TrimEnd();
        }
    }

    /// <summary>检查更新：单机工具，无在线更新服务，提示手动核对项目地址</summary>
    public static string CheckUpdate => $@"当前版本：{AppVersion.Full}
构建时间：{AppVersion.BuildTimeText}

本工具未接入在线更新服务，新版本请前往以下项目地址查看（三处仓库同步发布）：
{AppVersion.ProjectUrlText}

版本号即构建日期（yy.MM.dd-HHmm），若项目页面的最新构建日期比当前版本新，
下载最新程序覆盖旧文件即可，配置文件 servers_config.json 会保留原有服务器配置。";

    /// <summary>关于：版本、用途、项目地址与版权</summary>
    public static string About => $@"{AppVersion.ProductName} {AppVersion.Full}

构建时间：{AppVersion.BuildTimeText}

用于 SCNET 服务端的批量管理、状态监控与自动重启，
支持定时 / 间隔重启、RCON 远程指令、目录管理与日志记录。

技术栈：.NET 10 + Avalonia UI 11（Windows / macOS / Linux）

项目地址：
{AppVersion.ProjectUrlText}
许可证：MIT
Copyright © 2025-2026 yylmzxc";
}
