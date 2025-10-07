using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Net.Sockets;

namespace SCNET_Restart_Tool
{
    public partial class ToolMain : Form
    {
        private System.Windows.Forms.Timer timerDaily;
        private System.Windows.Forms.Timer timerContinuous;
        private const string ConfigFileName = "default_program.config"; // 配置文件
        private string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        private const string DefaultExe = "TerminalEntry.exe";
        private string targetExecutableName; // 存储文件名
        private string targetExecutablePath; // 存储完整路径

        // 定时关闭相关变量
        private DateTime scheduledCloseTime = new DateTime(1, 1, 1, 1, 0, 0);

        // 间隔关闭相关变量（支持小数小时）
        private double intervalHours = 0;
        private DateTime lastIntervalCloseTime = DateTime.MinValue;

        // 服务IP、端口、密码和命令
        private string serviceIp = "127.0.0.1"; // 默认IP
        private int servicePort = 5612; // 默认端口
        private string servicePassword = ""; // 默认密码
        private string serviceCommand = ""; // 默认命令

        // 新增：监控状态变量
        private bool isMonitoring = false;

        public ToolMain()
        {
            InitializeComponent();
            LoadDefaultProgram();
            InitializeTimers();
            FindTargetExecutable();
            UpdateMonitorButtonState(); // 初始化按钮状态
        }

        // 新增：监控按钮状态更新
        private void UpdateMonitorButtonState()
        {
            if (isMonitoring)
            {
                monitorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
                monitorButton.Text = "关闭服务端监控";
            }
            else
            {
                monitorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
                monitorButton.Text = "开启服务端监控";
            }
        }

        // 新增：监控按钮点击事件
        private void MonitorButton_Click(object sender, EventArgs e)
        {
            isMonitoring = !isMonitoring;
            UpdateMonitorButtonState();

            if (isMonitoring)
            {
                UpdateStatusLabel("服务端监控已开启");
                timerContinuous.Start();
                timerDaily.Start();
            }
            else
            {
                UpdateStatusLabel("服务端监控已关闭");
                timerContinuous.Stop();
                timerDaily.Stop();
            }
        }

        private void LoadDefaultProgram()
        {
            if (!File.Exists(ConfigFileName))
            {
                // 配置文件不存在，创建默认配置
                SetDefaultExecutable();
                SaveDefaultProgram();
                UpdateStatusLabel($"创建默认配置: {targetExecutablePath}");
            }
            else
            {
                try
                {
                    string[] configLines = File.ReadAllLines(ConfigFileName);

                    // 处理空白配置文件
                    if (configLines.Length == 0)
                    {
                        SetDefaultExecutable();
                        SaveDefaultProgram();
                        UpdateStatusLabel($"配置文件空白，使用默认程序: {targetExecutablePath}");
                        return;
                    }

                    // [0] 完整路径
                    // [1] 文件名
                    // [2] 定时关闭时间 (HH:mm)
                    // [3] 间隔小时数
                    // [4] 上次间隔关闭时间 (DateTime格式)
                    // [5] 服务IP
                    // [6] 服务端口
                    // [7] 服务密码
                    // [8] 服务命令
                    if (configLines.Length >= 9)
                    {
                        string savedPath = configLines[0];
                        string savedName = configLines[1];

                        // 验证配置路径是否有效
                        if (File.Exists(savedPath) && Path.GetFileName(savedPath) == savedName)
                        {
                            targetExecutablePath = savedPath;
                            targetExecutableName = savedName;
                        }
                        else
                        {
                            SetDefaultExecutable();
                        }

                        if (TimeSpan.TryParse(configLines[2], out TimeSpan time))
                        {
                            scheduledCloseTime = DateTime.Today.Add(time);
                        }
                        else
                        {
                            scheduledCloseTime = new DateTime(1, 1, 1, 1, 0, 0);
                        }

                        // 使用InvariantCulture解析小数小时
                        if (double.TryParse(configLines[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double hours) && hours >= 0)
                        {
                            intervalHours = hours;
                        }
                        else
                        {
                            intervalHours = 0;
                        }

                        if (DateTime.TryParse(configLines[4], out DateTime lastClose))
                        {
                            lastIntervalCloseTime = lastClose;
                        }
                        else
                        {
                            lastIntervalCloseTime = DateTime.MinValue;
                        }

                        serviceIp = configLines[5];
                        if (int.TryParse(configLines[6], out int port))
                        {
                            servicePort = port;
                        }
                        servicePassword = configLines[7];
                        serviceCommand = configLines[8];

                        // 显示友好的时间描述
                        string intervalDesc = intervalHours < 1 ?
                            $"{(intervalHours * 60):0}分钟" :
                            $"{intervalHours:0.###}小时";

                        UpdateStatusLabel($"配置加载成功\n定时关闭: {scheduledCloseTime:HH:mm}\n间隔关闭: {intervalDesc}\n服务IP: {serviceIp}\n服务端口: {servicePort}");
                    }
                    else
                    {
                        SetDefaultExecutable();

                        if (configLines.Length >= 1)
                        {
                            string savedPath = configLines[0];
                            if (File.Exists(savedPath))
                            {
                                targetExecutablePath = savedPath;
                                targetExecutableName = Path.GetFileName(savedPath);
                            }
                        }

                        SaveDefaultProgram();
                        UpdateStatusLabel($"升级旧配置成功: {targetExecutablePath}");
                    }
                }
                catch (Exception ex)
                {
                    SetDefaultExecutable();
                    SaveDefaultProgram();
                    UpdateStatusLabel($"配置读取错误: {ex.Message}\n使用默认程序: {targetExecutablePath}");
                }
            }

            // 更新UI控件显示的值（使用不变文化格式）
            timeInput.Text = scheduledCloseTime.ToString("HH:mm");
            intervalInput.Text = intervalHours.ToString("0.###", CultureInfo.InvariantCulture);
            ipInput.Text = serviceIp;
            portInput.Text = servicePort.ToString();
            passwordInput.Text = servicePassword;
            commandInput.Text = serviceCommand;
        }

        private void SetDefaultExecutable()
        {
            targetExecutablePath = Path.Combine(defaultDirectory, DefaultExe);
            targetExecutableName = DefaultExe;
        }

        private void SaveDefaultProgram()
        {
            try
            {
                File.WriteAllLines(ConfigFileName, new[] {
                    targetExecutablePath,
                    targetExecutableName,
                    scheduledCloseTime.ToString("HH:mm"),
                    intervalHours.ToString(CultureInfo.InvariantCulture), // 使用不变格式保存小数
                    lastIntervalCloseTime.ToString("O"),
                    serviceIp,
                    servicePort.ToString(),
                    servicePassword,
                    serviceCommand
                });
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"保存配置失败: {ex.Message}");
            }
        }

        private void VerifyTargetExecutable()
        {
            if (File.Exists(targetExecutablePath))
            {
                UpdateStatusLabel($"找到目标程序：{targetExecutablePath}");
            }
            else
            {
                UpdateStatusLabel($"未找到目标程序：{targetExecutableName}\n路径：{targetExecutablePath}");
            }
        }

        private void InitializeTimers()
        {
            timerDaily = new System.Windows.Forms.Timer();
            timerDaily.Interval = 1000;
            timerDaily.Tick += TimerDaily_Tick;
            timerDaily.Stop(); // 初始停止

            timerContinuous = new System.Windows.Forms.Timer();
            timerContinuous.Interval = 1000;
            timerContinuous.Tick += TimerContinuous_Tick;
            timerContinuous.Stop(); // 初始停止
        }

        private void TimerDaily_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime scheduledTimeToday = DateTime.Today.Add(scheduledCloseTime.TimeOfDay);

            if (now > scheduledTimeToday)
            {
                scheduledTimeToday = scheduledTimeToday.AddDays(1);
            }

            if (now.Hour == scheduledCloseTime.Hour &&
                now.Minute == scheduledCloseTime.Minute &&
                now.Second == 0)
            {
                UpdateStatusLabel($"定时关闭时间到 ({scheduledCloseTime:HH:mm})，关闭程序...");
                KillTargetProcess();
            }

            // 间隔关闭检查（支持小数小时）
            if (intervalHours > 0)
            {
                if (lastIntervalCloseTime == DateTime.MinValue)
                {
                    lastIntervalCloseTime = DateTime.Now;
                    SaveDefaultProgram();
                }

                DateTime nextIntervalClose = lastIntervalCloseTime.AddHours(intervalHours);

                if (DateTime.Now >= nextIntervalClose)
                {
                    // 显示友好的时间描述
                    string intervalDesc = intervalHours < 1 ?
                        $"{(intervalHours * 60):0}分钟" :
                        $"{intervalHours:0.###}小时";

                    UpdateStatusLabel($"间隔关闭时间到 ({intervalDesc})，关闭程序...");
                    KillTargetProcess();
                    lastIntervalCloseTime = DateTime.Now;
                    SaveDefaultProgram();
                }
            }
        }

        private void TimerContinuous_Tick(object sender, EventArgs e)
        {
            if (!IsProcessRunning(targetExecutableName))
            {
                UpdateStatusLabel($"未检测到 {targetExecutableName} 正在运行，将在5秒后启动...");
                System.Threading.Thread.Sleep(5000);
                StartTargetProcess();
            }
            else
            {
                UpdateStatusLabel($"{targetExecutableName} 正在运行...");
            }
        }

        private void FindTargetExecutable()
        {
            string currentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            targetExecutablePath = Path.Combine(currentDirectory, targetExecutableName);

            if (File.Exists(targetExecutablePath))
            {
                UpdateStatusLabel($"找到目标程序：{targetExecutablePath}");
            }
            else
            {
                UpdateStatusLabel($"未找到目标程序：{targetExecutableName}\n请确保该程序与本程序在同一目录下！");
            }
        }

        private bool IsProcessRunning(string processName)
        {
            return Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName)).Length > 0;
        }

        private void KillTargetProcess()
        {
            try
            {
                // 在关闭目标程序之前，发送命令到指定的服务
                SendServiceCommand("close 9 例行维护");
                UpdateStatusLabel($"延迟关闭程序：{targetExecutableName}");
                System.Threading.Thread.Sleep(1000);

                // 关闭目标程序
                Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(targetExecutableName));
                foreach (Process process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                }

                UpdateStatusLabel($"已关闭程序：{targetExecutableName}");
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"关闭程序时出错：{ex.Message}，尝试强制关闭...");

                // 尝试强制关闭目标程序
                try
                {
                    Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(targetExecutableName));
                    foreach (Process process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                    UpdateStatusLabel($"已强制关闭程序：{targetExecutableName}");
                }
                catch (Exception forceEx)
                {
                    UpdateStatusLabel($"强制关闭程序时出错：{forceEx.Message}");
                }
            }
        }

        private void SendServiceCommand(string command)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.SendTimeout = 5000;
                    client.ReceiveTimeout = 5000;
                    client.Connect(serviceIp, servicePort);

                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        writer.AutoFlush = true;

                        // 创建组合消息：password <密码> command <命令>
                        string combinedMessage;
                        if (!string.IsNullOrEmpty(servicePassword))
                        {
                            combinedMessage = $"password {servicePassword} command {command}";
                        }
                        else
                        {
                            combinedMessage = $"command {command}";
                        }

                        // 发送组合消息
                        writer.WriteLine(combinedMessage);

                        // 读取响应
                        string response = reader.ReadLine();

                        // 显示响应
                        UpdateStatusLabel($"发送命令到服务: {command}\n服务响应: {response}");

                        // 如果响应为空（可能是连接断开），显示错误信息
                        if (string.IsNullOrEmpty(response))
                        {
                            UpdateStatusLabel("未收到服务器响应，可能连接已断开");
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                UpdateStatusLabel($"网络连接失败: {ex.Message}");
            }
            catch (IOException ex)
            {
                UpdateStatusLabel($"通信超时或失败: {ex.Message}");
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"发送指令失败: {ex.Message}");
            }
        }

        private void StartTargetProcess()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = targetExecutablePath,
                    Arguments = "world=1",
                    UseShellExecute = true
                };
                Process.Start(startInfo);
                UpdateStatusLabel($"已启动程序：{targetExecutablePath}");
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"启动程序时出错：{ex.Message}");
            }
        }

        private void UpdateStatusLabel(string message)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action<string>(UpdateStatusLabel), message);
            }
            else
            {
                statusLabel.Text = message;
            }
        }

        private void killButton_Click(object sender, EventArgs e)
        {
            KillTargetProcess();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "可执行文件|*.exe";
                openFileDialog.Title = "选择要监控的程序";
                openFileDialog.InitialDirectory = defaultDirectory;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    targetExecutablePath = openFileDialog.FileName;
                    targetExecutableName = Path.GetFileName(openFileDialog.FileName);
                    SaveDefaultProgram();
                    VerifyTargetExecutable();
                }
            }
        }

        private void defaultButton_Click(object sender, EventArgs e)
        {
            serviceIp = ipInput.Text.Trim();
            if (int.TryParse(portInput.Text, out int port))
            {
                servicePort = port;
            }
            else
            {
                UpdateStatusLabel("端口号无效，请输入有效的数字端口");
                return;
            }
            servicePassword = passwordInput.Text.Trim();
            serviceCommand = commandInput.Text.Trim();

            SaveDefaultProgram();
            UpdateStatusLabel($"服务设置已保存\nIP: {serviceIp}\n端口: {servicePort}\n密码: {servicePassword}\n命令: {serviceCommand}");
        }

        private void saveTimeButton_Click(object sender, EventArgs e)
        {
            if (TimeSpan.TryParse(timeInput.Text, out TimeSpan time))
            {
                scheduledCloseTime = DateTime.Today.Add(time);
                SaveDefaultProgram();
                UpdateStatusLabel($"定时关闭时间已设置为: {timeInput.Text}");
            }
            else
            {
                UpdateStatusLabel("时间格式无效，请输入HH:mm格式");
            }
        }

        private void saveIntervalButton_Click(object sender, EventArgs e)
        {
            // 使用不变文化解析小数
            if (double.TryParse(intervalInput.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double hours) && hours >= 0)
            {
                intervalHours = hours;
                lastIntervalCloseTime = DateTime.MinValue;
                SaveDefaultProgram();

                // 显示友好的时间描述
                string intervalDesc = hours < 1 ?
                    $"{(hours * 60):0}分钟" :
                    $"{hours:0.###}小时";

                UpdateStatusLabel($"间隔关闭已设置为: {intervalDesc}");
            }
            else
            {
                UpdateStatusLabel("间隔时间无效，请输入0或正数（如0.6表示36分钟）");
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            // 获取当前输入框中的IP、端口、密码和命令
            string ip = ipInput.Text.Trim();
            int port;
            if (!int.TryParse(portInput.Text, out port))
            {
                commandStatusLabel.Text = "端口号无效";
                return;
            }

            string command = commandInput.Text.Trim();
            if (string.IsNullOrEmpty(command))
            {
                commandStatusLabel.Text = "指令不能为空";
                return;
            }

            string password = passwordInput.Text.Trim();

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.SendTimeout = 5000;
                    client.ReceiveTimeout = 5000;
                    client.Connect(ip, port);

                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        writer.AutoFlush = true;

                        // 创建组合消息：password <密码> command <命令>
                        string combinedMessage;
                        if (!string.IsNullOrEmpty(password))
                        {
                            combinedMessage = $"password {password} command {command}";
                        }
                        else
                        {
                            combinedMessage = $"command {command}";
                        }

                        // 发送组合消息
                        writer.WriteLine(combinedMessage);

                        // 读取响应
                        string response = reader.ReadLine();

                        // 显示响应
                        commandStatusLabel.Text = response;

                        // 如果响应为空（可能是连接断开），显示错误信息
                        if (string.IsNullOrEmpty(response))
                        {
                            commandStatusLabel.Text = "未收到服务器响应，可能连接已断开";
                        }
                    }
                }

                // 发送成功后，更新配置文件
                serviceIp = ip;
                servicePort = port;
                servicePassword = password;
                serviceCommand = command;
                SaveDefaultProgram();
                UpdateStatusLabel($"发送成功，配置已更新\nIP: {serviceIp}\n端口: {servicePort}");
            }
            catch (SocketException ex)
            {
                commandStatusLabel.Text = $"网络连接失败: {ex.Message}";
            }
            catch (IOException ex)
            {
                commandStatusLabel.Text = $"通信超时或失败: {ex.Message}";
            }
            catch (Exception ex)
            {
                commandStatusLabel.Text = $"发送指令失败: {ex.Message}";
            }
        }

        private void commandStatusLabel_Click(object sender, EventArgs e)
        {
        }

        private void portLabel_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}