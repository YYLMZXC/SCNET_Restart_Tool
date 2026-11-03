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
            var result = JsonSerializer.Deserialize<List<ServerConfig>>(json, JsonOptions);
            return result ?? new List<ServerConfig>();
        }
        catch (Exception ex)
        {
            LogManager.GetInstance().AddLog("系统", $"配置加载失败：{ex.Message}，将使用默认配置", "错误");
            return new List<ServerConfig> { new ServerConfig { Id = 1 } };
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
