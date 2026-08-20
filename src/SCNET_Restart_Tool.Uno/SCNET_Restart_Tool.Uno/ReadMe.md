# SCNET_Restart_Tool.Uno

基于 **Uno Platform 6.6**（WinUI / XAML）重写的 SCNET 服务端监控与自动重启工具。

## 技术栈

- **运行时**：.NET 10.0（目标框架 `net10.0-windows10.0.26100`）
- **UI 框架**：Uno Platform（`Uno.Sdk 6.6.42`，单项目模式 `UnoSingleProject=true`）
- **渲染器**：SkiaRenderer

## 项目结构

```
SCNET_Restart_Tool.Uno/
├── App.xaml / App.xaml.cs                  # 应用入口与全局资源
├── MainPage.xaml / MainPage.xaml.cs        # 主界面（服务器列表、编辑、监控、日志）
├── FolderManagerPage.xaml / .cs            # 文件夹管理器（TabView 多标签）
├── Code/                                   # 核心逻辑（与平台无关）
│   ├── ServerStatus.cs                     # 服务器状态枚举
│   ├── ServerConfig.cs                     # 服务器配置模型（含 XAML 绑定辅助属性）
│   ├── ServerConfigManager.cs              # JSON 配置读写（LoadAll / SaveAll / GenerateNewId）
│   ├── ServerMonitor.cs                    # 进程监控、自动重启、定时任务、TCP 指令
│   ├── LogManager.cs                       # 日志管理器（单例）
│   ├── LogItem.cs                          # 日志项数据结构
│   └── FolderOperations.cs                 # 文件夹操作（静态委托，由 UI 层注入）
├── Controls/
│   ├── FolderBrowserControl.xaml / .cs     # 文件夹浏览控件
│   └── BugsLogControl.xaml / .cs           # Bugs 日志控件
├── DispatcherQueueExtensions.cs            # DispatcherQueue.EnqueueAsync 扩展
└── Properties/PublishProfiles/             # 发布配置
```

## 构建

> 请使用 **Visual Studio 的 MSBuild** 构建。`dotnet build` 环境下 Uno 的 `XamlCompiler.exe`
> 可能静默退出（返回码 1 但无错误输出），导致无法定位 XAML 编译错误。

```powershell
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" `
  "SCNET_Restart_Tool.Uno.sln" -t:Rebuild -p:Configuration=Debug
```

输出：`bin\Debug\net10.0-windows10.0.26100\win-x64\SCNET_Restart_Tool.Uno.dll`

## 迁移要点（WinForms → Uno）

- 模型与服务层（`Code/`）近乎原样迁移
- UI 层使用 XAML 重写：
  - `MessageBox` → `ContentDialog`
  - 文件对话框 → `FileOpenPicker` + `WinRT.Interop.InitializeWithWindow`
  - WinForms `Timer` → `System.Threading.Timer` / `DispatcherQueueTimer`
  - `Invoke` → `DispatcherQueue.EnqueueAsync`
- `FolderOperations` 中 `ConfirmAsync` / `NotifyAsync` / `PickFileAsync` 为静态委托，由 UI 层注入实现
