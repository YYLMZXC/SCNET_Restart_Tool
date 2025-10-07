using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public partial class SettingsForm : Form
    {
        // 存储设置（可扩展更多配置）
        public SettingsModel AppSettings { get; private set; }

        public SettingsForm(SettingsModel currentSettings)
        {
            InitializeComponent();
            AppSettings = currentSettings ?? new SettingsModel();
            // 加载当前设置到界面
            LoadSettingsToUI();
        }

        // 加载现有设置
        private void LoadSettingsToUI()
        {
            txtLogPath.Text = AppSettings.LogSavePath;
            chkAutoClearLogs.Checked = AppSettings.AutoClearLogs;
            numAutoClearDays.Value = AppSettings.AutoClearDays;
            
        }

       

        // 选择日志保存路径
        private void btnSelectLogPath_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog
            {
                Description = "选择日志保存目录"
            })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtLogPath.Text = fbd.SelectedPath;
                }
            }
        }

        // 保存设置
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            AppSettings.LogSavePath = txtLogPath.Text.Trim();
            AppSettings.AutoClearLogs = chkAutoClearLogs.Checked;
            AppSettings.AutoClearDays = (int)numAutoClearDays.Value;
            

            // 保存设置到本地文件（后续实现）
            SettingsManager.SaveSettings(AppSettings);

            

            MessageBox.Show("设置已保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        // 取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    // 配置模型（存储所有隐藏设置）
    [Serializable]
    public class SettingsModel : ICloneable
    {
        // 日志保存路径（默认程序目录）
        public string LogSavePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        // 是否自动清理日志
        public bool AutoClearLogs { get; set; } = false;
        // 自动清理日志的天数（超过此天数的日志将被删除）
        public int AutoClearDays { get; set; } = 7;
        // 自定义程序图标路径
        public string CustomIconPath { get; set; } = "";
        // 可扩展其他设置：如监控定时器间隔、日志级别等

        // 添加Clone方法实现深拷贝
        public object Clone()
        {
            // 使用JSON序列化实现深拷贝，确保所有属性都被复制
            return JsonConvert.DeserializeObject<SettingsModel>(
                JsonConvert.SerializeObject(this)
            );
        }
    }

    // 配置管理类（负责加载/保存设置）
    public static class SettingsManager
    {
        private static readonly string _settingsPath = "app_settings.json";
        private static readonly JsonSerializerSettings _jsonOptions = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects
        };

        // 加载设置
        public static SettingsModel LoadSettings()
        {
            if (!File.Exists(_settingsPath))
            {
                return new SettingsModel(); // 返回默认设置
            }

            try
            {
                string json = File.ReadAllText(_settingsPath);
                return JsonConvert.DeserializeObject<SettingsModel>(json, _jsonOptions) ?? new SettingsModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置加载失败：{ex.Message}\n将使用默认设置", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new SettingsModel();
            }
        }

        // 保存设置
        public static void SaveSettings(SettingsModel settings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(settings, _jsonOptions);
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}