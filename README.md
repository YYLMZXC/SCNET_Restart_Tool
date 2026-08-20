# SCNET_Restart_Tool

## 项目简介

SCNET_Restart_Tool 是一款专为 SCNET 服务端程序（如生存战争服务器）设计的高级监控与自动重启工具。该工具通过实时监控服务运行状态，提供多种智能重启机制和远程控制功能，确保服务端程序持续稳定运行，最大限度减少人工干预，提高服务可用性和运维效率。

![主界面预览](src/SCNET_Restart_Tool.NET/Res/index.png)

## 核心功能特性

### 多服务器管理
- 支持同时配置和管理多个服务端实例
- 每个服务器可独立设置监控参数和重启策略
- 直观的列表界面展示所有服务器运行状态和基本信息

### 智能故障检测与自动重启
- 实时监控指定服务进程状态，支持进程名+路径双重验证机制
- 服务意外终止时自动执行重启操作，确保服务连续性
- 内置智能重试机制，应对启动失败的情况
- 详细的操作日志记录，便于问题追溯和系统分析

### 灵活的重启计划策略
- **定时计划重启**：支持设置每日固定时间点自动重启服务
- **间隔周期重启**：可按设定的时间间隔（小时）自动重启服务
- 重启前可发送优雅关闭指令，确保服务正常终止
- 重启倒计时提示，便于用户了解系统状态变更

### 远程命令控制系统
- 提供 TCP 协议接口，可远程向服务端发送自定义命令
- 支持命令发送前的密码验证，保障系统安全性
- 实时显示命令执行结果，便于远程操作监控

### 便捷的文件夹管理
- 集成服务器相关文件夹快速访问功能
- 支持直接打开配置、日志、插件、模组、纹理、世界等常用文件夹
- 提供图标化的文件夹操作界面，提高管理效率

### 完善的日志系统
- 实时记录所有操作和状态变更
- 支持日志文件保存，便于后期查阅和分析
- 可自定义日志保存路径和记录级别

### 程序自检与权限管理
- 自动检测管理员权限，确保程序正常运行
- 提供权限提升选项，避免因权限不足导致功能受限

### 跨平台支持
- 新版基于 Uno Platform，UI 层使用 XAML（WinUI），理论上可扩展到 Windows / macOS / Linux / WebAssembly 等多平台
- 旧版 Windows 版提供 WinForms 图形界面，Linux 版提供命令行界面

## 技术架构

### 开发技术栈
- **开发语言**：C#
- **运行时**：.NET 10.0
- **UI 框架（新版）**：Uno Platform 6.6（WinUI / XAML），单项目模式（`UnoSingleProject`）
- **UI 框架（旧版）**：Windows Forms
- **网络通信**：Socket TCP/IP
- **进程管理**：System.Diagnostics、WMI
- **配置管理（新版）**：JSON 序列化（`ServerConfigManager`）
- **配置管理（旧版）**：XML 序列化
- **日志系统**：自定义 `LogManager` 组件

## 项目结构

```
SCNET_Restart_Tool/
├── README.md                      # 项目说明文档
├── src/
│   ├── SCNET_Restart_Tool.NET/    # 旧版 WinForms 项目（Windows / Linux CLI）
│   │   ├── 界面/                  # WinForms 界面代码（主窗体、设置、文件夹管理等）
│   │   ├── Res/                   # 资源文件（图标、预览图 index.png 等）
│   │   └── ...                    # 模型、服务、配置等核心逻辑
│   └── SCNET_Restart_Tool.Uno/    # 新版 Uno Platform 项目（推荐）
│       ├── Directory.Build.props / Directory.Packages.props / global.json
│       ├── SCNET_Restart_Tool.Uno.sln
│       └── SCNET_Restart_Tool.Uno/          # 应用主工程
│           ├── App.xaml / App.xaml.cs       # 应用入口与资源
│           ├── MainPage.xaml / MainPage.xaml.cs  # 主界面
│           ├── FolderManagerPage.xaml/.cs   # 文件夹管理器页
│           ├── Code/                        # 核心逻辑（模型与服务）
│           │   ├── ServerStatus.cs          # 服务器状态枚举
│           │   ├── ServerConfig.cs          # 服务器配置模型
│           │   ├── ServerConfigManager.cs   # JSON 配置读写
│           │   ├── ServerMonitor.cs         # 进程监控 / 启停 / TCP 指令
│           │   ├── LogManager.cs            # 日志管理器（单例）
│           │   ├── LogItem.cs               # 日志项数据结构
│           │   └── FolderOperations.cs      # 文件夹操作（确认/通知/选文件委托）
│           ├── Controls/                    # 自定义控件
│           │   ├── FolderBrowserControl.xaml/.cs  # 文件夹浏览控件
│           │   └── BugsLogControl.xaml/.cs       # Bugs 日志控件
│           ├── DispatcherQueueExtensions.cs # DispatcherQueue 异步扩展
│           └── Properties/                  # 发布配置等
└── ...
```

## 构建说明

> **注意**：新版 Uno 项目使用 `Uno.Sdk`（含 `XamlCompiler` 等 MSBuild 自定义任务）。
> 在 Windows 上请使用 **Visual Studio 的 MSBuild** 构建（`dotnet build` 环境下 `XamlCompiler.exe` 可能静默退出导致 XAML 编译异常）。

### 构建 Uno 新版（推荐）

```powershell
# 使用 VS MSBuild（路径以实际 VS 版本为准，此处为 VS 2026 Community）
& "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" `
  "src\SCNET_Restart_Tool.Uno\SCNET_Restart_Tool.Uno.sln" -t:Rebuild -p:Configuration=Debug
```

或在 Visual Studio 中直接打开 `src\SCNET_Restart_Tool.Uno\SCNET_Restart_Tool.Uno.sln` 进行构建与运行。

### 构建旧版 WinForms（Windows 版）

```powershell
dotnet build src\SCNET_Restart_Tool.NET\SCNET_Restart_Tool.NET.csproj
```

## 安装与使用

### 系统要求
- **Windows**：Windows 10/11，安装 .NET 10.0 Runtime 或更高版本
- **Linux（旧版 CLI）**：支持 .NET 10.0 的 Linux 发行版（如 Ubuntu 22.04+、CentOS 9+ 等）
- **硬件要求**：最小系统资源（CPU: 1GHz+，内存: 512MB+）

### 基本操作流程
1. **添加服务器**：在主界面点击「添加服务器」，填写服务器名称、程序路径、进程名等
2. **配置重启策略**：设置定时重启（HH:MM）或间隔重启（小时数，0 表示禁用）
3. **启动监控**：选择已配置的服务器，点击「开始监控」
4. **查看状态**：在主界面实时查看服务器运行状态和日志
5. **远程控制**：使用命令输入框向服务端发送指令（如 `close 9 例行维护`）
6. **管理文件夹**：点击「文件夹管理」，快速访问服务器相关目录

### 配置保存位置
- **新版（Uno）**：JSON 配置文件，由 `ServerConfigManager` 负责读写
- **旧版（WinForms）**：程序目录下 `ServerConfigs.xml`

## 注意事项

1. **管理员权限**：程序需要管理员权限才能正确监控和管理系统进程，请确保以管理员身份运行
2. **配置文件备份**：建议定期备份程序配置文件，以防止配置丢失
3. **网络安全**：使用远程命令功能时，请确保设置强密码并限制访问 IP
4. **日志管理**：定期清理日志文件，避免占用过多磁盘空间

## 迁移说明（WinForms → Uno）

- 纯逻辑层（`Code/` 下的模型与服务）近乎原样迁移，不依赖 UI 框架
- UI 层使用 XAML（WinUI）重写，替代原有 WinForms 窗体
- `MessageBox` / 文件对话框分别由 `ContentDialog` / `FileOpenPicker`（配合 `WinRT.Interop.InitializeWithWindow`）替代
- WinForms `Timer` / `Invoke` 分别由 `System.Threading.Timer` / `DispatcherQueue` 替代
- 文件对话框等 UI 能力通过 `FolderOperations` 中的静态委托（`ConfirmAsync` / `NotifyAsync` / `PickFileAsync`）由 UI 层注入

## 更新日志

### v2.0.0（迁移版）
- 项目迁移至 Uno Platform 6.6（WinUI / XAML），UI 层全面重写
- 配置管理由 XML 切换为 JSON
- 核心逻辑层（模型 / 服务）保持复用

### v1.1.0
- 项目框架升级至 .NET 10.0，Windows 版与 Linux 版统一使用 .NET 10.0 Runtime
- System.Management 依赖包同步升级至 10.0.0

### v1.0.0
- 初始版本发布
- 实现基本的服务器监控和自动重启功能
- 支持多服务器管理
- 提供 Windows 图形界面版本

## 许可证

本项目采用 MIT 许可证。详情请查看项目中的 LICENSE 文件。

## 免责声明

本工具仅供学习和参考使用，使用本工具时请确保遵守相关法律法规。对于因使用本工具而导致的任何直接或间接损失，作者不承担任何责任。

---

如有任何问题或建议，请提交 Issue 至本项目的仓库页面。
