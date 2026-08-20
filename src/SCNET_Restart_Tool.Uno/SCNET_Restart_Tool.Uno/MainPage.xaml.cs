using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SCNET_Restart_Tool;
using Windows.ApplicationModel.DataTransfer;

namespace SCNET_Restart_Tool.Uno;

/// <summary>
/// 主页面：服务器管理、运行控制、监控与日志
/// </summary>
public sealed partial class MainPage : Page
{
    private readonly ObservableCollection<ServerConfig> _serverItems = new();
    private ServerConfig? _selectedServer;
    private bool _isEditing;
    private bool _isAdding;
    private readonly LogManager _logManager = LogManager.GetInstance();
    private readonly Dictionary<int, ServerMonitor> _serverMonitors = new();
    private readonly List<string> _logBuffer = new();
    private DispatcherQueueTimer _logFlushTimer = null!;

    public ObservableCollection<ServerConfig> ServerItems => _serverItems;

    public MainPage()
    {
        this.InitializeComponent();

        // 设置 UI 委托回调（确认/提示/文件选择）
        FolderOperations.ConfirmAsync = ConfirmAsync;
        FolderOperations.NotifyAsync = NotifyAsync;
        FolderOperations.PickFileAsync = PickFileAsync;

        InitializeLogBuffer();
        LoadServerData();
        UpdateButtonStates();
    }

    #region 日志节流缓冲

    private void ClearLogBtn_Click(object sender, RoutedEventArgs e)
    {
        logTextBox.Text = "";
        _logManager.AddLog("系统", "已清空日志显示");
    }

    private void InitializeLogBuffer()
    {
        _logManager.LogAdded += (log) =>
        {
            lock (_logBuffer)
            {
                _logBuffer.Add($"[{DateTime.Now:HH:mm:ss}] {log}");
            }
        };

        _logFlushTimer = DispatcherQueue.CreateTimer();
        _logFlushTimer.Interval = TimeSpan.FromMilliseconds(500);
        _logFlushTimer.IsRepeating = true;
        _logFlushTimer.Tick += (s, e) =>
        {
            if (_logBuffer.Count == 0) return;

            lock (_logBuffer)
            {
                var batch = string.Join(Environment.NewLine, _logBuffer) + Environment.NewLine;
                logTextBox.Text += batch;
                _logBuffer.Clear();
            }
        };
        _logFlushTimer.Start();
    }

    #endregion

    #region 数据加载

    private void LoadServerData()
    {
        try
        {
            var configs = ServerConfigManager.LoadAll();
            _serverItems.Clear();
            foreach (var server in configs)
            {
                _serverItems.Add(server);
                CreateServerMonitor(server);
            }
            _logManager.AddLog("系统", "服务器配置加载完成");
        }
        catch (Exception ex)
        {
            _logManager.AddLog("系统", $"加载配置失败: {ex.Message}", "错误");
        }
    }

    private void CreateServerMonitor(ServerConfig server)
    {
        if (_serverMonitors.ContainsKey(server.Id)) return;

        var monitor = new ServerMonitor(server, (s) =>
        {
            // 状态变更时刷新列表显示（跨线程安全）
            _ = DispatcherQueue.TryEnqueue(() =>
            {
                var item = _serverItems.FirstOrDefault(x => x.Id == s.Id);
                item?.RaisePropertyChanged();
                UpdateButtonStates();
            });
        });
        monitor.MessageRequested += async (title, message) =>
        {
            await DispatcherQueue.EnqueueAsync(async () =>
            {
                await ShowDialogAsync(title, message);
            });
        };
        _serverMonitors[server.Id] = monitor;

        if (server.IsMonitoring)
        {
            monitor.Start();
        }
    }

    #endregion

    #region 服务器列表选择

    private void ServersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isEditing) return;

        _selectedServer = serversListView.SelectedItem as ServerConfig;
        if (_selectedServer != null)
        {
            LoadServerToEditPanel(_selectedServer);
        }
        else
        {
            LoadServerToEditPanel(null);
        }
        UpdateButtonStates();
    }

    private void ServersListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        serversListView.SelectedItem = e.ClickedItem;
    }

    private void LoadServerToEditPanel(ServerConfig? server)
    {
        if (server == null)
        {
            serverNameInput.Text = "";
            exePathInput.Text = "";
            ipInput.Text = "127.0.0.1";
            portInput.Text = "5612";
            passwordInput.Password = "";
            scheduleTimeInput.Text = "01:00";
            intervalHoursInput.Text = "0";
            enableCommandsChk.IsChecked = false;
        }
        else
        {
            serverNameInput.Text = server.Name;
            exePathInput.Text = server.ExePath;
            ipInput.Text = server.Ip;
            portInput.Text = server.Port.ToString();
            passwordInput.Password = server.Password;
            scheduleTimeInput.Text = server.ScheduleTime;
            intervalHoursInput.Text = server.IntervalHours.ToString("0.##");
            enableCommandsChk.IsChecked = server.EnableCommands;
        }
    }

    #endregion

    #region 新增 / 编辑 / 删除

    private void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        _isEditing = true;
        _isAdding = true;
        _selectedServer = null;
        serversListView.SelectedItem = null!;
        LoadServerToEditPanel(null);
        SetEditControlsEnabled(true);
        UpdateButtonStates();
        _logManager.AddLog("系统", "进入新增服务器模式");
    }

    private void EditBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        _isEditing = true;
        _isAdding = false;
        LoadServerToEditPanel(_selectedServer);
        SetEditControlsEnabled(true);
        UpdateButtonStates();
        _logManager.AddLog("系统", $"进入编辑服务器模式: {_selectedServer.Name}");
    }

    private async void SaveEditBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(serverNameInput.Text))
        {
            await ShowDialogAsync("验证错误", "请输入服务器名称");
            return;
        }

        if (!string.IsNullOrWhiteSpace(exePathInput.Text) && !File.Exists(exePathInput.Text))
        {
            var result = await ConfirmAsync("程序路径不存在，是否继续？", "路径验证");
            if (!result) return;
        }

        try
        {
            if (_isAdding)
            {
                var newId = ServerConfigManager.GenerateNewId(_serverItems.ToList());
                var newServer = new ServerConfig
                {
                    Id = newId,
                    Name = serverNameInput.Text.Trim(),
                    ExePath = exePathInput.Text.Trim(),
                    ExeName = Path.GetFileName(exePathInput.Text.Trim()),
                    Ip = ipInput.Text.Trim(),
                    Port = ParseInt(portInput.Text, 5612),
                    Password = passwordInput.Password,
                    ScheduleTime = scheduleTimeInput.Text.Trim(),
                    IntervalHours = ParseDouble(intervalHoursInput.Text, 0),
                    EnableCommands = enableCommandsChk.IsChecked == true,
                    Status = ServerStatus.Stopped
                };

                _serverItems.Add(newServer);
                CreateServerMonitor(newServer);
                _logManager.AddLog("系统", $"已添加服务器: {newServer.Name}");
            }
            else if (_selectedServer != null)
            {
                _selectedServer.Name = serverNameInput.Text.Trim();
                _selectedServer.ExePath = exePathInput.Text.Trim();
                _selectedServer.ExeName = Path.GetFileName(exePathInput.Text.Trim());
                _selectedServer.Ip = ipInput.Text.Trim();
                _selectedServer.Port = ParseInt(portInput.Text, _selectedServer.Port);
                _selectedServer.Password = passwordInput.Password;
                _selectedServer.ScheduleTime = scheduleTimeInput.Text.Trim();
                _selectedServer.IntervalHours = ParseDouble(intervalHoursInput.Text, _selectedServer.IntervalHours);
                _selectedServer.EnableCommands = enableCommandsChk.IsChecked == true;
                _selectedServer.RaisePropertyChanged();
                _logManager.AddLog("系统", $"已更新服务器: {_selectedServer.Name}");
            }

            SaveServerConfigs();
            ExitEditMode();
        }
        catch (Exception ex)
        {
            _logManager.AddLog("系统", $"保存失败: {ex.Message}", "错误");
            await ShowDialogAsync("错误", $"保存失败: {ex.Message}");
        }
    }

    private void CancelEditBtn_Click(object sender, RoutedEventArgs e)
    {
        ExitEditMode();
        _logManager.AddLog("系统", "已取消编辑");
    }

    private void ExitEditMode()
    {
        _isEditing = false;
        _isAdding = false;
        SetEditControlsEnabled(false);
        if (_selectedServer != null)
        {
            LoadServerToEditPanel(_selectedServer);
        }
        UpdateButtonStates();
    }

    private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        var confirmed = await ConfirmAsync($"确定要删除服务器「{_selectedServer.Name}」吗？", "确认删除");
        if (!confirmed) return;

        if (_serverMonitors.ContainsKey(_selectedServer.Id))
        {
            _serverMonitors[_selectedServer.Id].Stop();
            _serverMonitors.Remove(_selectedServer.Id);
        }

        _serverItems.Remove(_selectedServer);
        SaveServerConfigs();
        _logManager.AddLog("系统", $"已删除服务器: {_selectedServer.Name}");
        _selectedServer = null;
        UpdateButtonStates();
        LoadServerToEditPanel(null);
    }

    private async void BrowseExeBtn_Click(object sender, RoutedEventArgs e)
    {
        var path = await PickFileAsync("选择服务器程序", "可执行文件", new[] { ".exe" });
        if (string.IsNullOrEmpty(path)) return;

        exePathInput.Text = path;
        if (string.IsNullOrWhiteSpace(serverNameInput.Text))
        {
            serverNameInput.Text = Path.GetFileNameWithoutExtension(path);
        }
    }

    private void SetEditControlsEnabled(bool enabled)
    {
        serverNameInput.IsEnabled = enabled;
        exePathInput.IsEnabled = enabled;
        browseExeBtn.IsEnabled = enabled;
        ipInput.IsEnabled = enabled;
        portInput.IsEnabled = enabled;
        passwordInput.IsEnabled = enabled;
        scheduleTimeInput.IsEnabled = enabled;
        intervalHoursInput.IsEnabled = enabled;
        enableCommandsChk.IsEnabled = enabled;
    }

    private void SaveServerConfigs()
    {
        ServerConfigManager.SaveAll(_serverItems.ToList());
    }

    #endregion

    #region 运行控制

    private async void StartBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        try
        {
            if (string.IsNullOrWhiteSpace(_selectedServer.ExePath) || !File.Exists(_selectedServer.ExePath))
            {
                _logManager.AddLog(_selectedServer.Name, "程序路径无效，无法启动", "错误");
                await ShowDialogAsync("启动失败", "请配置有效的服务器程序路径");
                return;
            }

            if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
            {
                _selectedServer.Status = ServerStatus.Starting;
                _selectedServer.RaisePropertyChanged();
                await Task.Run(() => monitor.StartProcess());
                _selectedServer.RaisePropertyChanged();
                UpdateButtonStates();
            }
        }
        catch (Exception ex)
        {
            _selectedServer.Status = ServerStatus.Stopped;
            _logManager.AddLog(_selectedServer.Name, $"启动失败: {ex.Message}", "错误");
            _selectedServer.RaisePropertyChanged();
        }
    }

    private async void StopBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
        {
            _selectedServer.Status = ServerStatus.Stopping;
            _selectedServer.RaisePropertyChanged();
            await Task.Run(() => monitor.KillProcess());
            _selectedServer.RaisePropertyChanged();
            UpdateButtonStates();
        }
    }

    private async void RestartBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        _logManager.AddLog(_selectedServer.Name, "开始重启服务器");
        if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
        {
            await Task.Run(() => monitor.KillProcess());
            await Task.Delay(1500);
            await Task.Run(() => monitor.StartProcess());
            _selectedServer.RaisePropertyChanged();
            UpdateButtonStates();
        }
    }

    private async void StopAllBtn_Click(object sender, RoutedEventArgs e)
    {
        var runningServers = _serverItems.Where(s =>
            s.Status == ServerStatus.Running || s.Status == ServerStatus.Monitoring).ToList();

        if (runningServers.Count == 0)
        {
            await ShowDialogAsync("提示", "没有运行中的服务器");
            return;
        }

        var confirmed = await ConfirmAsync($"确定要停止所有 {runningServers.Count} 个运行中的服务器吗？", "确认操作");
        if (!confirmed) return;

        foreach (var server in runningServers)
        {
            server.Status = ServerStatus.Stopping;
            server.RaisePropertyChanged();
        }

        await Task.Run(() =>
        {
            foreach (var server in runningServers)
            {
                if (_serverMonitors.TryGetValue(server.Id, out var monitor))
                {
                    monitor.KillProcess();
                    System.Threading.Thread.Sleep(500);
                }
            }
            System.Threading.Thread.Sleep(1000);
        });

        foreach (var server in runningServers)
        {
            server.RaisePropertyChanged();
        }
        UpdateButtonStates();
    }

    #endregion

    #region 监控与指令

    private void StartMonitorBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
        {
            monitor.Start();
            _selectedServer.IsMonitoring = true;
            _selectedServer.RaisePropertyChanged();
            UpdateButtonStates();
            _logManager.AddLog(_selectedServer.Name, "已开启监控");
        }
    }

    private void StopMonitorBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
        {
            monitor.Stop();
            _selectedServer.IsMonitoring = false;
            _selectedServer.RaisePropertyChanged();
            UpdateButtonStates();
            _logManager.AddLog(_selectedServer.Name, "已关闭监控");
        }
    }

    private async void SendCommandBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;
        if (string.IsNullOrWhiteSpace(commandInput.Text))
        {
            await ShowDialogAsync("提示", "请输入指令内容");
            return;
        }

        if (!_selectedServer.EnableCommands)
        {
            await ShowDialogAsync("功能未启用", "请在编辑模式中启用指令功能");
            return;
        }

        if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
        {
            string result = await Task.Run(() => monitor.SendServiceCommand(commandInput.Text.Trim()));
            await ShowDialogAsync("发送结果", $"指令响应:\n{result}");
        }
    }

    #endregion

    #region 文件夹管理器

    private void FolderManagerBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null)
        {
            _ = ShowDialogAsync("提示", "请先选择服务器");
            return;
        }

        Frame.Navigate(typeof(FolderManagerPage), _selectedServer);
    }

    #endregion

    #region 帮助

    private async void HelpBtn_Click(object sender, RoutedEventArgs e)
    {
        var menu = new MenuFlyout();
        var functionItem = new MenuFlyoutItem { Text = "功能说明" };
        functionItem.Click += async (s, args) => await ShowDialogAsync("功能说明", HelpContent.FunctionDescription);
        var projectItem = new MenuFlyoutItem { Text = "项目信息" };
        projectItem.Click += async (s, args) => await ShowDialogAsync("项目信息", HelpContent.ProjectInfo);
        var versionItem = new MenuFlyoutItem { Text = "版本信息" };
        versionItem.Click += async (s, args) => await ShowDialogAsync("版本信息", HelpContent.VersionInfo);
        var aboutItem = new MenuFlyoutItem { Text = "关于" };
        aboutItem.Click += async (s, args) => await ShowDialogAsync("关于软件", $"项目地址：https://gitee.com/sc-net/SCNET_Restart_Tool\n\n本工具为 SCNET 服务端管理工具 Uno 版。");
        var updateItem = new MenuFlyoutItem { Text = "检查更新" };
        updateItem.Click += async (s, args) => await ShowDialogAsync("检查更新", $"当前已是最新版本！\n版本号: v1.0.0");

        menu.Items.Add(functionItem);
        menu.Items.Add(projectItem);
        menu.Items.Add(versionItem);
        menu.Items.Add(new MenuFlyoutSeparator());
        menu.Items.Add(updateItem);
        menu.Items.Add(new MenuFlyoutSeparator());
        menu.Items.Add(aboutItem);

        menu.ShowAt(helpBtn);
        _logManager.AddLog("系统", "打开帮助菜单");
    }

    #endregion

    #region 按钮状态

    private void UpdateButtonStates()
    {
        bool hasSelection = _selectedServer != null;
        bool hasRunningServers = _serverItems.Any(s =>
            s.Status == ServerStatus.Running || s.Status == ServerStatus.Monitoring);

        editBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring != true;
        deleteBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring != true;
        folderManagerBtn.IsEnabled = hasSelection && !_isEditing;

        startBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.Status == ServerStatus.Stopped;
        stopBtn.IsEnabled = hasSelection && !_isEditing &&
                            (_selectedServer?.Status == ServerStatus.Running ||
                             _selectedServer?.Status == ServerStatus.Monitoring);
        restartBtn.IsEnabled = hasSelection && !_isEditing &&
                               (_selectedServer?.Status == ServerStatus.Running ||
                                _selectedServer?.Status == ServerStatus.Monitoring);
        stopAllBtn.IsEnabled = hasRunningServers && !_isEditing;

        startMonitorBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring != true;
        stopMonitorBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring == true;

        saveEditBtn.IsEnabled = _isEditing;
        cancelEditBtn.IsEnabled = _isEditing;

        sendCommandBtn.IsEnabled = hasSelection && !_isEditing &&
                                   _selectedServer?.EnableCommands == true &&
                                   (_selectedServer?.Status == ServerStatus.Running ||
                                    _selectedServer?.Status == ServerStatus.Monitoring);

        statusHintText.Text = _isEditing
            ? "正在编辑配置，请保存或取消"
            : hasSelection
                ? $"已选择：{_selectedServer?.Name}（{_selectedServer?.StatusText}）"
                : "提示：选择服务器后可进行编辑与操作";
    }

    #endregion

    #region 对话框辅助

    private ContentDialog CreateDialog(string title, string content, bool isPrimary = false)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = content,
            CloseButtonText = "确定",
            XamlRoot = this.XamlRoot
        };
        if (isPrimary)
        {
            dialog.PrimaryButtonText = "是";
            dialog.CloseButtonText = "否";
        }
        return dialog;
    }

    private async Task NotifyAsync(string message, string title)
    {
        await ShowDialogAsync(title, message);
    }

    private async Task ShowDialogAsync(string title, string content)
    {
        var dialog = CreateDialog(title, content);
        await dialog.ShowAsync();
    }

    private async Task<bool> ConfirmAsync(string message, string title)
    {
        var dialog = CreateDialog(title, message, isPrimary: true);
        var result = await dialog.ShowAsync();
        return result == ContentDialogResult.Primary;
    }

    private async Task<string> PickFileAsync(string title, string filterName, string[] extensions)
    {
#if WINDOWS
        var picker = new Windows.Storage.Pickers.FileOpenPicker();
        // Uno 桌面端需要初始化窗口句柄
        var window = (App.Current as App)?.MainWindow;
        if (window != null)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        }
        picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
        if (extensions != null && extensions.Length > 0)
        {
            foreach (var ext in extensions)
            {
                if (ext == "*") picker.FileTypeFilter.Add("*");
                else picker.FileTypeFilter.Add(ext);
            }
        }
        var file = await picker.PickSingleFileAsync();
        return file?.Path ?? "";
#else
        return "";
#endif
    }

    private int ParseInt(string text, int defaultValue)
    {
        return int.TryParse(text, out var value) ? value : defaultValue;
    }

    private double ParseDouble(string text, double defaultValue)
    {
        return double.TryParse(text, out var value) ? value : defaultValue;
    }

    #endregion
}
