using System;
using System.IO;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端配置模型（保持与原项目字段一致，便于配置互导）
/// </summary>
public class ServerConfig
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "新服务端";
    public string ExePath { get; set; } = "";
    public string ExeName { get; set; } = "";
    public string Ip { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 5612;
    public string Password { get; set; } = "";
    public string ScheduleTime { get; set; } = "01:00";
    public double IntervalHours { get; set; } = 0;
    public DateTime LastIntervalClose { get; set; } = DateTime.MinValue;
    public bool IsMonitoring { get; set; } = false;

    /// <summary>运行时状态（不序列化，仅内存使用）</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public ServerStatus Status { get; set; } = ServerStatus.Stopped;

    public bool EnableCommands { get; set; } = false;

    /// <summary>
    /// 服务端根目录（由可执行文件路径推导，仅运行时计算）
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

    public string BugsLogPath => Path.Combine(ServerRootPath, "Bugs");
    public string CharacterSkinsPath => Path.Combine(ServerRootPath, "CharacterSkins");
    public string ConfigsPath => Path.Combine(ServerRootPath, "Configs");
    public string TexturePacksPath => Path.Combine(ServerRootPath, "TexturePacks");
    public string NetModsPath => Path.Combine(ServerRootPath, "NetMods");
    public string PluginsPath => Path.Combine(ServerRootPath, "Plugins");
    public string WorldsPath => Path.Combine(ServerRootPath, "Worlds");

    public bool IsFolderExists(string folderPath)
    {
        return !string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath);
    }

    public ServerConfig Clone()
    {
        return (ServerConfig)MemberwiseClone();
    }

    // ===== XAML 绑定辅助属性（不序列化）=====

    /// <summary>表格显示的 IP:端口</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string DisplayIpPort => $"{Ip}:{Port}";

    /// <summary>状态中文文本</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string StatusText => Status switch
    {
        ServerStatus.Running => "运行中",
        ServerStatus.Monitoring => "监控中",
        ServerStatus.Starting => "启动中",
        ServerStatus.Stopping => "停止中",
        _ => "已停止"
    };

    /// <summary>状态对应颜色画刷（供 XAML 绑定）</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public Microsoft.UI.Xaml.Media.SolidColorBrush StatusBrush => Status switch
    {
        ServerStatus.Running => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(46, 204, 113)),
        ServerStatus.Monitoring => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(155, 89, 182)),
        ServerStatus.Starting => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(241, 196, 15)),
        ServerStatus.Stopping => new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorArgb(231, 76, 60)),
        _ => new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray)
    };

    private static Windows.UI.Color ColorArgb(byte r, byte g, byte b)
        => Windows.UI.Color.FromArgb(255, r, g, b);

    public void RaisePropertyChanged()
    {
        // 简化实现：直接通知绑定刷新（供 UI 手动调用）
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(StatusText)));
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(StatusBrush)));
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Status)));
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(DisplayIpPort)));
    }

    public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
}
