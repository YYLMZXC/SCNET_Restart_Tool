using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public static class ServerConfigManager
    {
        private static readonly string ConfigPath = "servers_config.json";
        private static readonly JsonSerializerSettings JsonOptions = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects
        };

        public static List<ServerConfig> LoadAll()
        {
            if (!File.Exists(ConfigPath))
            {
                return new List<ServerConfig> { new ServerConfig { Id = 1 } };
            }

            try
            {
                string json = File.ReadAllText(ConfigPath);
                var result = JsonConvert.DeserializeObject<List<ServerConfig>>(json, JsonOptions);
                return result ?? new List<ServerConfig>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"配置加载失败：{ex.Message}\n将使用默认配置");
                return new List<ServerConfig> { new ServerConfig { Id = 1 } };
            }
        }

        public static void SaveAll(List<ServerConfig> servers)
        {
            try
            {
                string json = JsonConvert.SerializeObject(servers, JsonOptions);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"配置保存失败：{ex.Message}");
            }
        }

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
}