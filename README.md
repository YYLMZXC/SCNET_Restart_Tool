

# SCNET_Restart_Tool

## 项目介绍

SCNET_Restart_Tool 是一款专业的服务端程序自动监控和重启工具，专为确保 SCNET 服务端程序能够持续稳定运行而设计。该工具通过实时监控服务运行状态，提供多种智能重启机制，并支持远程命令控制，有效提高服务的可靠性和运维效率。

## 功能特性

### 1. 自动故障检测与重启
- 实时监控指定服务进程的运行状态
- 当检测到服务意外终止时，自动执行重启操作
- 记录重启日志，便于问题追溯和分析

### 2. 定时计划重启
- 支持设置每日或每周固定时间点自动重启服务
- 可配置多个定时重启任务
- 重启前可设置延迟时间，确保服务正常关闭

### 3. 间隔周期重启
- 支持按照设定的时间间隔（如每6小时、每12小时）自动重启服务
- 适合需要定期清理内存和资源的服务场景
- 可自定义间隔时间单位（分钟、小时、天）

### 4. 远程命令控制
- 提供网络接口，允许通过 TCP 协议远程发送命令
- 支持远程查看服务状态、手动触发重启、调整配置参数
- 内置简单的身份验证机制，保障安全性

### 5. 配置管理
- 支持图形界面配置服务参数
- 自动保存配置到本地文件，下次启动时自动加载
- 支持导入导出配置，便于多环境部署

### 6. 多平台支持
- 提供 Windows 图形界面版本，操作简便直观
- 提供 Linux 命令行版本，适合服务器环境部署
- 支持不同版本 .NET 运行时环境

## 技术架构

### 核心技术
- **开发语言**：C#
- **框架**：.NET Framework 4.7.2 (Windows版) / .NET 8.0 (Linux版)
- **GUI框架**：Windows Forms (Windows版)
- **网络通信**：Socket TCP/IP
- **进程管理**：System.Diagnostics
- **配置管理**：XML配置文件

### 主要组件说明

1. **Form1.cs**
   - Windows 版的主窗体界面和核心逻辑
   - 包含进程监控、定时任务、远程通信等功能模块
   - 负责用户交互和配置管理

2. **LinuxRestartTool/Program.cs**
   - Linux 版的核心逻辑实现
   - 命令行界面，适合服务器环境后台运行
   - 支持配置文件启动和参数控制

3. **Properties/AssemblyInfo.cs**
   - 程序集元数据配置
   - 包含版本信息、产品描述、版权声明等

4. **App.config**
   - 应用程序配置文件
   - 包含框架版本信息和其他全局配置

## 安装与配置

### 系统要求
- **Windows 版**：Windows 7 或更高版本，安装 .NET Framework 4.7.2 或更高版本
- **Linux 版**：支持 .NET 8.0 的 Linux 发行版（如 Ubuntu 22.04+, CentOS 9+ 等）

### 安装步骤

1. **Windows 版安装**
   - 从发布页面下载最新版本的 `SCNET_Restart_Tool.zip`
   - 解压到任意目录
   - 运行 `SCNET_Restart_Tool.exe` 启动程序
   - 如需开机自启，可创建快捷方式并放入启动文件夹

2. **Linux 版安装**
   - 从发布页面下载最新版本的 `LinuxRestartTool.zip`
   - 解压到目标目录
   - 安装 .NET 8.0 Runtime：`sudo apt-get install -y dotnet-runtime-8.0`（Ubuntu/Debian）
   - 运行程序：`dotnet LinuxRestartTool.dll`
   - 如需后台运行，可使用 systemd 服务配置或 nohup 命令

### 配置说明

#### Windows 版配置
1. 启动程序后，在主界面填写以下信息：
   - **服务端路径**：SCNET 服务端程序的完整路径（如 `D:\SCNET\Server.exe`）
   - **进程名称**：服务端进程的名称（如 `Server`）
   - **监控间隔**：检查服务状态的时间间隔（秒）

2. 配置重启策略：
   - **定时重启**：勾选启用，设置具体时间点
   - **间隔重启**：勾选启用，设置间隔时间和单位

3. 远程控制配置：
   - 勾选启用远程控制
   - 设置监听端口（默认 12345）
   - 设置访问密码（可选，增强安全性）

4. 点击保存配置，配置将自动保存到程序目录下的配置文件中

#### Linux 版配置
Linux 版通过配置文件 `default_program.config` 进行设置：
```
# 服务端程序路径
TargetExecutablePath=/path/to/scnet/server
# 服务端进程名称
TargetExecutableName=scnet-server
# 定时重启时间（格式：HH:MM）
ScheduledCloseTime=02:00
# 间隔重启小时数（0表示禁用）
IntervalHours=6
# 监控间隔（秒）
MonitorIntervalSeconds=30
```

## 使用指南

### Windows 版使用
1. 启动程序后，配置服务端路径和进程名称
2. 选择需要的重启策略（自动重启、定时重启、间隔重启）
3. 点击「开始监控」按钮，程序开始监控服务状态
4. 监控状态将显示在界面上，包括当前状态、运行时长、重启次数等
5. 可通过界面按钮手动控制服务的启动、停止和重启

### Linux 版使用
1. 在终端中运行 `dotnet LinuxRestartTool.dll` 启动监控
2. 程序将在控制台显示运行状态信息
3. 可通过 `Ctrl+C` 停止监控程序
4. 后台运行方式：`nohup dotnet LinuxRestartTool.dll > restart.log 2>&1 &`

### 远程命令使用
通过 TCP 客户端连接到监控程序的监听端口，可发送以下命令：
- `status`：获取当前服务状态
- `restart`：立即重启服务
- `stop`：停止服务
- `start`：启动服务
- `config`：查看当前配置
- `exit`：退出远程连接

## 常见问题解答

**Q: 程序无法启动或提示缺少 .NET Framework？**
A: 请确保您的系统已安装 .NET Framework 4.7.2 或更高版本，可以从微软官网下载安装。

**Q: 监控程序能够检测到服务崩溃，但无法自动重启？**
A: 请检查服务端程序路径是否正确，以及程序是否有足够的权限启动服务。

**Q: 远程命令无法连接到监控程序？**
A: 请检查防火墙设置是否允许指定端口的入站连接，以及监控程序是否已启用远程控制功能。

**Q: 如何查看程序运行日志？**
A: Windows 版日志保存在程序目录下的 `log` 文件夹中，Linux 版日志默认输出到控制台或指定的日志文件中。

## 开发与贡献

### 开发环境搭建
1. 安装 Visual Studio 2022 或更高版本
2. 克隆项目代码：`git clone https://gitee.com/your-username/SCNET_Restart_Tool.git`
3. 打开 `SCNET_Restart_Tool.sln` 解决方案文件
4. 还原 NuGet 包：`dotnet restore`
5. 构建解决方案：`Build > Build Solution`

### 贡献指南
1. Fork 本项目仓库
2. 创建您的特性分支：`git checkout -b feature/AmazingFeature`
3. 提交您的更改：`git commit -m 'Add some AmazingFeature'`
4. 推送到分支：`git push origin feature/AmazingFeature`
5. 提交 Pull Request

## 许可证

本项目采用 MIT 许可证。详情请查看项目中的 LICENSE 文件。

## 免责声明

本工具仅供学习和参考使用，使用本工具时请确保遵守相关法律法规。对于因使用本工具而导致的任何直接或间接损失，作者不承担任何责任。

---

如有任何问题或建议，请提交 Issue 至本项目的 Gitee 页面。