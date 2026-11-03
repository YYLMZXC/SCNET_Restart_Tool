using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SCNET_Restart_Tool;

/// <summary>
/// 服务端配置读写管理器（JSON 格式，存放于程序目录）
/// </summary>
public static class ServerConfigManager
{
    private static readonly string ConfigFileName = "servers_config.json";
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>完整配置文件路径</summary>
    public static string ConfigPath =>
        Path.Combine(AppContext.BaseDirectory, ConfigFileName);

    /// <summary>加载全部服务端配置</summary>
    public static List<ServerConfig> LoadAll()
    {
        if (!File.Exists(ConfigPath))
        {
            return new List<ServerConfig> { new ServerConfig { Id = 1 } };
        }
        try
        {
            string json = File.ReadAllText(ConfigPath);
            var result = JsonSerializer.Deserialize<List<ServerConfig>>(json, JsonOptions) ?? new List<ServerConfig>();
            MigrateLegacyPort(result);
            return result;
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog("系统", $"配置加载失败：{ex.Message}，将使用默认配置", "错误");
            return new List<ServerConfig> { new ServerConfig { Id = 1 } };
        }
    }

    /// <summary>
    /// 旧协议端口迁移：指令通道已切换为 ScNetworkDEV RCON 标准接口，
    /// 旧配置里仍写旧默认端口（5612）的条目视为「从未手动改过端口」，直接迁移到实际默认端口（28900）。
    /// 用户自行设置过的其他端口值一律保留不动。
    /// </summary>
    private static void MigrateLegacyPort(List<ServerConfig> servers)
    {
        int migrated = 0;
        foreach (var server in servers)
        {
            if (server.Port == ServerConfig.LegacyPort)
            {
                server.Port = ServerConfig.DefaultRconPort;
                migrated++;
            }
        }
        if (migrated > 0)
        {
            LogManager.GetInstance().AddLog("系统",
                $"已将 {migrated} 个服务器的旧默认端口 {ServerConfig.LegacyPort} 迁移为 RCON 实际默认端口 {ServerConfig.DefaultRconPort}");
            // 立即落盘，避免迁移结果只存在于内存（下次保存任意配置前退出程序会丢失）
            SaveAll(servers);
        }
    }

    /// <summary>保存全部服务端配置</summary>
    public static void SaveAll(List<ServerConfig> servers)
    {
        try
        {
            string json = JsonSerializer.Serialize(servers, JsonOptions);
            File.WriteAllText(ConfigPath, json);
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog("系统", $"配置保存失败：{ex.Message}", "错误");
        }
    }

    /// <summary>生成新的服务端 ID</summary>
    public static int GenerateNewId(List<ServerConfig> servers)
    {
        int maxId = 0;
        foreach (var server in servers)
        {
            if (server.Id > maxId)
                maxId = server.Id;
        }
        return maxId + 1;
    }
}
