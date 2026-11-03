using System;
using System.IO;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端配置模型（保持与原项目字段一致，便于配置互导）。
///
/// 职责说明（本类承担两类职责，均与 XAML 绑定绑定密切相关，刻意不做拆分）：
/// 1. 持久化模型：Id、Name、ExePath 等字段直接序列化为 servers_config.json；
/// 2. 派生辅助属性：ServerRootPath 及各类子目录路径由 ExePath 运行时推导；
/// 3. XAML 显示属性：DisplayIpPort / StatusText / StatusBrush 供列表绑定显示，
///    它们均为只读且标注 JsonIgnore，不会污染配置文件。
/// 其中 Status 为运行时状态，同样不序列化，仅存在于内存中。
/// </summary>
public class ServerConfig
{
    // ==================== 持久化字段（直接对应配置文件字段） ====================

    /// <summary>服务器唯一标识（用于区分多开实例与监控器映射）</summary>
    public int Id { get; set; } = 0;

    /// <summary>服务器显示名称</summary>
    public string Name { get; set; } = "新服务端";

    /// <summary>服务器程序可执行文件完整路径</summary>
    public string ExePath { get; set; } = "";

    /// <summary>服务器程序文件名（由路径推导，便于界面展示）</summary>
    public string ExeName { get; set; } = "";

    /// <summary>指令通讯 IP 地址</summary>
    public string Ip { get; set; } = "127.0.0.1";

    /// <summary>指令通讯端口</summary>
    public int Port { get; set; } = 5612;

    /// <summary>指令通讯密码（为空则免密认证）</summary>
    public string Password { get; set; } = "";

    /// <summary>每日定时关闭时间（HH:mm 格式，如 "01:00"）</summary>
    public string ScheduleTime { get; set; } = "01:00";

    /// <summary>间隔关闭小时数（0 表示不启用间隔关闭）</summary>
    public double IntervalHours { get; set; } = 0;

    /// <summary>上次间隔关闭时间（作为下次间隔关闭的计时起点）</summary>
    public DateTime LastIntervalClose { get; set; } = DateTime.MinValue;

    /// <summary>是否处于监控中（持久化标记，用于程序重启后自动恢复监控）</summary>
    public bool IsMonitoring { get; set; } = false;

    /// <summary>运行时状态（不序列化，仅内存使用，由 ServerMonitor 维护）</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public ServerStatus Status { get; set; } = ServerStatus.Stopped;

    /// <summary>是否启用指令功能（发送 close 等管理指令）</summary>
    public bool EnableCommands { get; set; } = false;

    // ==================== 派生路径属性（由 ExePath 运行时推导，不序列化） ====================

    /// <summary>
    /// 服务端根目录（由可执行文件路径推导，仅运行时计算）。
    /// 路径无效或文件不存在时返回空字符串，避免组合出错误子目录。
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string ServerRootPath
    {
        get
        {
            if (string.IsNullOrEmpty(ExePath) || !File.Exists(ExePath))
                return "";
            return Path.GetDirectoryName(ExePath) ?? "";
        }
    }

    /// <summary>Bug 日志目录</summary>
    public string BugsLogPath => Path.Combine(ServerRootPath, "Bugs");

    /// <summary>角色皮肤目录</summary>
    public string CharacterSkinsPath => Path.Combine(ServerRootPath, "CharacterSkins");

    /// <summary>配置文件目录</summary>
    public string ConfigsPath => Path.Combine(ServerRootPath, "Configs");

    /// <summary>材质包目录</summary>
    public string TexturePacksPath => Path.Combine(ServerRootPath, "TexturePacks");

    /// <summary>网络模组目录</summary>
    public string NetModsPath => Path.Combine(ServerRootPath, "NetMods");

    /// <summary>插件目录</summary>
    public string PluginsPath => Path.Combine(ServerRootPath, "Plugins");

    /// <summary>世界存档目录</summary>
    public string WorldsPath => Path.Combine(ServerRootPath, "Worlds");

    /// <summary>判断指定文件夹是否存在</summary>
    /// <param name="folderPath">要检查的文件夹路径</param>
    public bool IsFolderExists(string folderPath)
    {
        return !string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath);
    }

    /// <summary>浅拷贝当前配置（新增/编辑时避免引用共享导致互相影响）</summary>
    public ServerConfig Clone()
    {
        return (ServerConfig)MemberwiseClone();
    }

    // ==================== XAML 绑定辅助属性（均不序列化） ====================

    /// <summary>列表显示的 IP:端口 文本</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string DisplayIpPort => $"{Ip}:{Port}";

    /// <summary>状态中文文本（供列表显示）</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string StatusText => Status switch
    {
        ServerStatus.Running => "运行中",
        ServerStatus.Monitoring => "监控中",
        ServerStatus.Starting => "启动中",
        ServerStatus.Stopping => "停止中",
        _ => "已停止"
    };

    /// <summary>状态对应颜色画刷（供 XAML 列表状态着色）</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public Microsoft.UI.Xaml.Media.SolidColorBrush StatusBrush => Status switch
    {
        ServerStatus.Running => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(46, 204, 113)),
        ServerStatus.Monitoring => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(155, 89, 182)),
        ServerStatus.Starting => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(241, 196, 15)),
        ServerStatus.Stopping => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(231, 76, 60)),
        _ => new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray)
    };

    /// <summary>构造 ARGB 颜色（固定不透明度 255）</summary>
    private static Windows.UI.Color ColorArgb(byte r, byte g, byte b)
        => Windows.UI.Color.FromArgb(255, r, g, b);

    /// <summary>
    /// 手动触发绑定属性刷新通知。
    /// 本模型未实现完整 INotifyPropertyChanged（各字段无自动通知），
    /// 因此状态变化后需由调用方手动调用本方法，通知 XAML 重新读取显示属性。
    /// </summary>
    public void RaisePropertyChanged()
    {
        // 简化实现：仅通知 XAML 已绑定的几个显示属性
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(StatusText)));
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(StatusBrush)));
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Status)));
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(DisplayIpPort)));
    }

    /// <summary>属性变更通知事件（XAML 绑定引擎订阅）</summary>
    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
}
