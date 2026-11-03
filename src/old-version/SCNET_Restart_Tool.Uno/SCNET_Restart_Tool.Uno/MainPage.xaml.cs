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
/// 主页面：服务器列表管理、运行控制、监控调度与运行日志展示。
///
/// 职责说明（高内聚、低耦合设计）：
/// - 本页仅负责「UI 协调」：响应用户操作、调用底层服务并刷新界面；
/// - 对话框、确认询问、文件选择等平台相关逻辑已委托给 DialogService；
/// - 监控调度与进程启停已委托给 ServerMonitor（内部组合 ServerProcessService / ServerCommandService）；
/// - 配置持久化已委托给 ServerConfigManager，日志写入委托给 LogManager。
/// 页面自身不再包含任何进程操作、TCP 通讯或对话框创建细节。
/// </summary>
public sealed partial class MainPage : Page
{
    // ==================== 私有状态 ====================

    /// <summary>服务器列表（XAML 绑定数据源，ObservableCollection 自动通知列表刷新）</summary>
    private readonly ObservableCollection<ServerConfig> _serverItems = new();

    /// <summary>当前选中的服务器</summary>
    private ServerConfig? _selectedServer;

    /// <summary>是否处于编辑模式（新增或修改中，编辑期间禁止其他操作）</summary>
    private bool _isEditing;

    /// <summary>是否为新增模式（true=新增，false=编辑已有服务器）</summary>
    private bool _isAdding;

    /// <summary>全局日志管理器（单例）</summary>
    private readonly LogManager _logManager = LogManager.GetInstance();

    /// <summary>服务器 ID → 监控器 映射表，保证每个服务器只有一个监控实例</summary>
    private readonly Dictionary<int, ServerMonitor> _serverMonitors = new();

    /// <summary>日志节流缓冲：高频率日志先写入缓冲，再由定时器批量刷新到界面</summary>
    private readonly List<string> _logBuffer = new();

    /// <summary>日志批量刷新定时器（500ms 一次）</summary>
    private DispatcherQueueTimer _logFlushTimer = null!;

    /// <summary>服务器列表（XAML 通过 x:Bind 绑定）</summary>
    public ObservableCollection<ServerConfig> ServerItems => _serverItems;

    public MainPage()
    {
        this.InitializeComponent();

        // 将静态工具类 FolderOperations 的 UI 交互委托指向 DialogService（统一对话框入口）。
        // 注意：XamlRoot 在页面加载后才有效，因此使用延迟求值的 lambda，真正弹出时才会访问。
        FolderOperations.ConfirmAsync = (message, title) => DialogService.ConfirmAsync(this.XamlRoot, message, title);
        FolderOperations.NotifyAsync = (message, title) => DialogService.ShowAsync(this.XamlRoot, title, message);
        FolderOperations.PickFileAsync = (title, filterName, extensions) => DialogService.PickFileAsync(this.XamlRoot, title, filterName, extensions);

        InitializeLogBuffer();
        LoadServerData();
        UpdateButtonStates();
    }

    #region 日志节流缓冲

    /// <summary>清空日志显示（仅清界面，不删除磁盘日志文件）</summary>
    private void ClearLogBtn_Click(object sender, RoutedEventArgs e)
    {
        logTextBox.Text = "";
        _logManager.AddLog("系统", "已清空日志显示");
    }

    /// <summary>
    /// 日志节流机制说明：
    /// 监控状态检测每 3 秒产生一条日志，若每条都直接刷新 TextBox 会造成频繁布局开销。
    /// 因此日志事件先追加到缓冲列表（可能来自后台线程，需加锁保护），
    /// 再由 DispatcherQueueTimer 每 500ms 批量刷入界面文本。
    /// </summary>
    private void InitializeLogBuffer()
    {
        // 订阅全局日志事件，追加到缓冲（带时间戳）
        _logManager.LogAdded += (log) =>
        {
            lock (_logBuffer)
            {
                _logBuffer.Add($"[{DateTime.Now:HH:mm:ss}] {log}");
            }
        };

        // 定时将缓冲内容一次性写入日志框，降低 UI 刷新频率
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

    /// <summary>从配置文件加载全部服务器，并为每个服务器创建对应的监控器实例</summary>
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

    /// <summary>
    /// 为指定服务器创建监控器，并挂接状态变更回调与消息请求回调。
    /// - 状态变更回调：刷新列表显示并更新按钮可用性（回调可能来自后台线程，需切回 UI 线程）；
    /// - 消息请求回调：将监控器内部提示（如“启动失败”）显示为对话框。
    /// 若服务器配置为「监控中」，则创建后立即启动监控。
    /// </summary>
    private void CreateServerMonitor(ServerConfig server)
    {
        // 防止同一服务器重复创建监控器
        if (_serverMonitors.ContainsKey(server.Id)) return;

        var monitor = new ServerMonitor(server, (s) =>
        {
            // 状态变更时刷新列表显示（跨线程安全：TryEnqueue 切回 UI 线程）
            _ = DispatcherQueue.TryEnqueue(() =>
            {
                var item = _serverItems.FirstOrDefault(x => x.Id == s.Id);
                item?.RaisePropertyChanged();
                UpdateButtonStates();
            });
        });

        // 监控器内部产生的消息（如“启动失败”“禁止多开”）显示为对话框
        monitor.MessageRequested += async (title, message) =>
        {
            await DispatcherQueue.EnqueueAsync(async () =>
            {
                await DialogService.ShowAsync(this.XamlRoot, title, message);
            });
        };

        _serverMonitors[server.Id] = monitor;

        // 配置为常驻监控的服务器，启动后立即进入监控状态
        if (server.IsMonitoring)
        {
            monitor.Start();
        }
    }

    #endregion

    #region 服务器列表选择

    /// <summary>列表选中项变化：将选中服务器加载到右侧编辑面板（编辑模式下不响应切换）</summary>
    private void ServersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isEditing) return;

        _selectedServer = serversListView.SelectedItem as ServerConfig;
        LoadServerToEditPanel(_selectedServer);
        UpdateButtonStates();
    }

    /// <summary>点击列表项即视为选中</summary>
    private void ServersListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        serversListView.SelectedItem = e.ClickedItem;
    }

    /// <summary>
    /// 将服务器配置加载到右侧编辑面板；server 为 null 时填充默认值（用于新增模式）。
    /// </summary>
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

    /// <summary>进入新增模式：清空编辑面板并启用输入控件</summary>
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

    /// <summary>进入编辑模式：将选中服务器加载到编辑面板并启用输入控件</summary>
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

    /// <summary>
    /// 保存编辑：新增时创建新服务器并加入列表，编辑时更新选中项；
    /// 保存前进行名称非空、程序路径存在性校验，最后持久化配置并退出编辑模式。
    /// </summary>
    private async void SaveEditBtn_Click(object sender, RoutedEventArgs e)
    {
        // 校验一：服务器名称必填
        if (string.IsNullOrWhiteSpace(serverNameInput.Text))
        {
            await DialogService.ShowAsync(this.XamlRoot, "验证错误", "请输入服务器名称");
            return;
        }

        // 校验二：程序路径非空但不存在时，询问用户是否继续
        if (!string.IsNullOrWhiteSpace(exePathInput.Text) && !File.Exists(exePathInput.Text))
        {
            var result = await DialogService.ConfirmAsync(this.XamlRoot, "程序路径不存在，是否继续？", "路径验证");
            if (!result) return;
        }

        try
        {
            if (_isAdding)
            {
                // ---- 新增服务器：生成新 ID，构造配置对象 ----
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
                CreateServerMonitor(newServer); // 为新服务器创建监控器，支持后续启停与监控
                _logManager.AddLog("系统", $"已添加服务器: {newServer.Name}");
            }
            else if (_selectedServer != null)
            {
                // ---- 更新现有服务器：从编辑面板收集输入值写入配置对象 ----
                ApplyEditToServer(_selectedServer);
                _selectedServer.RaisePropertyChanged();
                _logManager.AddLog("系统", $"已更新服务器: {_selectedServer.Name}");
            }

            SaveServerConfigs();
            ExitEditMode();
        }
        catch (Exception ex)
        {
            _logManager.AddLog("系统", $"保存失败: {ex.Message}", "错误");
            await DialogService.ShowAsync(this.XamlRoot, "错误", $"保存失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 将编辑面板中的输入值写入服务器配置对象（编辑模式专用）。
    /// 抽取为独立方法，使 SaveEditBtn_Click 职责更清晰（仅做新增/编辑分支与持久化）。
    /// </summary>
    private void ApplyEditToServer(ServerConfig server)
    {
        server.Name = serverNameInput.Text.Trim();
        server.ExePath = exePathInput.Text.Trim();
        server.ExeName = Path.GetFileName(exePathInput.Text.Trim());
        server.Ip = ipInput.Text.Trim();
        server.Port = ParseInt(portInput.Text, server.Port);
        server.Password = passwordInput.Password;
        server.ScheduleTime = scheduleTimeInput.Text.Trim();
        server.IntervalHours = ParseDouble(intervalHoursInput.Text, server.IntervalHours);
        server.EnableCommands = enableCommandsChk.IsChecked == true;
    }

    /// <summary>取消编辑：退出编辑模式并恢复面板显示</summary>
    private void CancelEditBtn_Click(object sender, RoutedEventArgs e)
    {
        ExitEditMode();
        _logManager.AddLog("系统", "已取消编辑");
    }

    /// <summary>退出编辑模式：禁用输入控件，恢复选中服务器显示</summary>
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

    /// <summary>
    /// 删除选中服务器：先确认，再停止并移除其监控器，最后从列表与配置文件中移除。
    /// </summary>
    private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        var confirmed = await DialogService.ConfirmAsync(this.XamlRoot, $"确定要删除服务器「{_selectedServer.Name}」吗？", "确认删除");
        if (!confirmed) return;

        // 停止并移除该服务器的监控器（防止删除后定时器继续触发）
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

    /// <summary>浏览选择服务器程序（.exe），选择后自动填充路径，名称为空时自动取程序名</summary>
    private async void BrowseExeBtn_Click(object sender, RoutedEventArgs e)
    {
        var path = await DialogService.PickFileAsync(this.XamlRoot, "选择服务器程序", "可执行文件", new[] { ".exe" });
        if (string.IsNullOrEmpty(path)) return;

        exePathInput.Text = path;
        if (string.IsNullOrWhiteSpace(serverNameInput.Text))
        {
            serverNameInput.Text = Path.GetFileNameWithoutExtension(path);
        }
    }

    /// <summary>启用/禁用编辑面板中的全部输入控件（编辑模式开关）</summary>
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

    /// <summary>将当前服务器列表持久化到配置文件</summary>
    private void SaveServerConfigs()
    {
        ServerConfigManager.SaveAll(_serverItems.ToList());
    }

    #endregion

    #region 运行控制

    /// <summary>
    /// 启动选中服务器进程。
    /// 前置校验程序路径有效；进程启动为耗时操作，放入后台线程执行避免阻塞 UI。
    /// </summary>
    private async void StartBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        try
        {
            // 前置校验：程序路径必须有效
            if (string.IsNullOrWhiteSpace(_selectedServer.ExePath) || !File.Exists(_selectedServer.ExePath))
            {
                _logManager.AddLog(_selectedServer.Name, "程序路径无效，无法启动", "错误");
                await DialogService.ShowAsync(this.XamlRoot, "启动失败", "请配置有效的服务器程序路径");
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

    /// <summary>停止选中服务器进程（后台线程执行，避免阻塞 UI）</summary>
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

    /// <summary>重启选中服务器：先关闭进程，等待 1.5 秒后再启动</summary>
    private async void RestartBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        _logManager.AddLog(_selectedServer.Name, "开始重启服务器");
        if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
        {
            await Task.Run(() => monitor.KillProcess());
            await Task.Delay(1500); // 等待进程完全退出，避免端口占用
            await Task.Run(() => monitor.StartProcess());
            _selectedServer.RaisePropertyChanged();
            UpdateButtonStates();
        }
    }

    /// <summary>
    /// 停止所有运行中的服务器：
    /// 先确认，再统一标记为停止中，然后逐个关闭进程（间隔 500ms 避免资源竞争）。
    /// </summary>
    private async void StopAllBtn_Click(object sender, RoutedEventArgs e)
    {
        var runningServers = _serverItems.Where(s =>
            s.Status == ServerStatus.Running || s.Status == ServerStatus.Monitoring).ToList();

        if (runningServers.Count == 0)
        {
            await DialogService.ShowAsync(this.XamlRoot, "提示", "没有运行中的服务器");
            return;
        }

        var confirmed = await DialogService.ConfirmAsync(this.XamlRoot, $"确定要停止所有 {runningServers.Count} 个运行中的服务器吗？", "确认操作");
        if (!confirmed) return;

        // 先统一标记为停止中，立即刷新界面反馈
        foreach (var server in runningServers)
        {
            server.Status = ServerStatus.Stopping;
            server.RaisePropertyChanged();
        }

        // 逐个关闭进程，间隔等待，避免短时间内并发关闭造成资源竞争
        foreach (var server in runningServers)
        {
            if (_serverMonitors.TryGetValue(server.Id, out var monitor))
            {
                await Task.Run(() => monitor.KillProcess());
                await Task.Delay(500);
            }
        }

        // 全部关闭后刷新界面状态
        foreach (var server in runningServers)
        {
            server.RaisePropertyChanged();
        }
        UpdateButtonStates();
    }

    #endregion

    #region 监控与指令

    /// <summary>开启监控：启动定时器，并持久化监控标记</summary>
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

    /// <summary>关闭监控：停止定时器，并持久化监控标记</summary>
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

    /// <summary>发送指令到选中服务器（需启用指令功能且服务器处于运行状态），并显示响应结果</summary>
    private async void SendCommandBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null) return;

        // 校验一：指令内容不能为空
        if (string.IsNullOrWhiteSpace(commandInput.Text))
        {
            await DialogService.ShowAsync(this.XamlRoot, "提示", "请输入指令内容");
            return;
        }

        // 校验二：必须先启用指令功能
        if (!_selectedServer.EnableCommands)
        {
            await DialogService.ShowAsync(this.XamlRoot, "功能未启用", "请在编辑模式中启用指令功能");
            return;
        }

        if (_serverMonitors.TryGetValue(_selectedServer.Id, out var monitor))
        {
            // TCP 通讯为阻塞操作，放入后台线程执行
            string result = await Task.Run(() => monitor.SendServiceCommand(commandInput.Text.Trim()));
            await DialogService.ShowAsync(this.XamlRoot, "发送结果", $"指令响应:\n{result}");
        }
    }

    #endregion

    #region 文件夹管理器

    /// <summary>跳转到文件夹管理器页面（携带当前服务器参数）</summary>
    private void FolderManagerBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedServer == null)
        {
            _ = DialogService.ShowAsync(this.XamlRoot, "提示", "请先选择服务器");
            return;
        }

        Frame.Navigate(typeof(FolderManagerPage), _selectedServer);
    }

    #endregion

    #region 帮助

    /// <summary>
    /// 弹出帮助菜单：功能说明 / 项目信息 / 版本信息 / 检查更新 / 关于。
    /// 每个菜单项点击后均通过 DialogService 显示对应内容。
    /// </summary>
    private async void HelpBtn_Click(object sender, RoutedEventArgs e)
    {
        var menu = new MenuFlyout();

        var functionItem = new MenuFlyoutItem { Text = "功能说明" };
        functionItem.Click += async (s, args) => await DialogService.ShowAsync(this.XamlRoot, "功能说明", HelpContent.FunctionDescription);

        var projectItem = new MenuFlyoutItem { Text = "项目信息" };
        projectItem.Click += async (s, args) => await DialogService.ShowAsync(this.XamlRoot, "项目信息", HelpContent.ProjectInfo);

        var versionItem = new MenuFlyoutItem { Text = "版本信息" };
        versionItem.Click += async (s, args) => await DialogService.ShowAsync(this.XamlRoot, "版本信息", HelpContent.VersionInfo);

        var aboutItem = new MenuFlyoutItem { Text = "关于" };
        aboutItem.Click += async (s, args) => await DialogService.ShowAsync(this.XamlRoot, "关于软件", $"项目地址：https://gitee.com/sc-net/SCNET_Restart_Tool\n\n本工具为 SCNET 服务端管理工具 Uno 版。");

        var updateItem = new MenuFlyoutItem { Text = "检查更新" };
        updateItem.Click += async (s, args) => await DialogService.ShowAsync(this.XamlRoot, "检查更新", $"当前已是最新版本！\n版本号: v1.0.0");

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

    /// <summary>
    /// 根据当前选中项、编辑状态与服务器运行状态统一刷新各按钮可用性。
    /// 集中管理按钮状态，避免各事件处理中零散修改导致状态不一致；
    /// 同时刷新底部状态提示文本。
    /// </summary>
    private void UpdateButtonStates()
    {
        bool hasSelection = _selectedServer != null;
        bool hasRunningServers = _serverItems.Any(s =>
            s.Status == ServerStatus.Running || s.Status == ServerStatus.Monitoring);

        // 编辑类按钮：有选中且非编辑中，且监控中的服务器不允许编辑/删除
        editBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring != true;
        deleteBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring != true;
        folderManagerBtn.IsEnabled = hasSelection && !_isEditing;

        // 运行控制按钮：根据服务器状态决定
        startBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.Status == ServerStatus.Stopped;
        stopBtn.IsEnabled = hasSelection && !_isEditing &&
                            (_selectedServer?.Status == ServerStatus.Running ||
                             _selectedServer?.Status == ServerStatus.Monitoring);
        restartBtn.IsEnabled = hasSelection && !_isEditing &&
                               (_selectedServer?.Status == ServerStatus.Running ||
                                _selectedServer?.Status == ServerStatus.Monitoring);
        stopAllBtn.IsEnabled = hasRunningServers && !_isEditing;

        // 监控按钮：按当前是否监控中互斥启用
        startMonitorBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring != true;
        stopMonitorBtn.IsEnabled = hasSelection && !_isEditing && _selectedServer?.IsMonitoring == true;

        // 编辑模式按钮
        saveEditBtn.IsEnabled = _isEditing;
        cancelEditBtn.IsEnabled = _isEditing;

        // 指令按钮：需启用指令功能且服务器正在运行
        sendCommandBtn.IsEnabled = hasSelection && !_isEditing &&
                                   _selectedServer?.EnableCommands == true &&
                                   (_selectedServer?.Status == ServerStatus.Running ||
                                    _selectedServer?.Status == ServerStatus.Monitoring);

        // 底部状态提示文本
        statusHintText.Text = _isEditing
            ? "正在编辑配置，请保存或取消"
            : hasSelection
                ? $"已选择：{_selectedServer?.Name}（{_selectedServer?.StatusText}）"
                : "提示：选择服务器后可进行编辑与操作";
    }

    #endregion

    #region 数值解析辅助

    /// <summary>安全解析整数，解析失败时返回默认值（避免无效输入导致程序崩溃）</summary>
    private int ParseInt(string text, int defaultValue)
    {
        return int.TryParse(text, out var value) ? value : defaultValue;
    }

    /// <summary>安全解析浮点数，解析失败时返回默认值</summary>
    private double ParseDouble(string text, double defaultValue)
    {
        return double.TryParse(text, out var value) ? value : defaultValue;
    }

    #endregion
}
