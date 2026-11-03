namespace SCNET_Restart_Tool.Uno;

/// <summary>
/// 帮助内容常量（移植自 WinForms 版帮助窗体）
/// </summary>
public static class HelpContent
{
    public static string FunctionDescription => @"功能说明

【重启功能】
- 提供SCNET客户端的快速重启功能
- 自动检测正在运行的SCNET进程
- 优雅关闭当前进程后启动新实例

【自动配置】
- 自动识别安装路径
- 智能检测客户端版本
- 支持多版本SCNET客户端

【错误处理】
- 提供详细的错误日志记录
- 异常情况的友好提示
- 自动恢复机制

【界面说明】
- 简洁的操作界面
- 一键式操作流程
- 实时状态反馈

【使用方法】
1. 确保SCNET客户端已正确安装
2. 点击主界面的「重启」按钮
3. 等待重启完成

如有任何问题，请联系技术支持。";

    public static string ProjectInfo => @"项目信息

项目名称：SCNET_Restart_Tool

项目描述：
SCNET客户端重启工具，用于快速重启SCNET客户端，提供自动化的重启流程和错误处理机制。

项目特点：
- 简单易用的操作界面
- 快速的重启响应
- 稳定的错误处理
- 智能的路径识别

开发环境：
- .NET 10
- C#
- Uno Platform

适用环境：
- Windows 10/11
- 支持32位和64位系统
- 需要管理员权限运行

注意事项：
- 请确保正确安装了SCNET客户端
- 程序需要访问系统进程和文件系统权限
- 如有任何问题，请联系开发团队

本工具仅用于辅助SCNET客户端的管理和维护。";

    public static string VersionInfo => @"=== SCNET_Restart_Tool 版本信息 ===

【当前版本】 v1.0.0
【发布日期】 2023年10月15日
【开发团队】 系统维护组

=== 版本更新日志 ===

【v1.0.0】
- 初始版本发布
- 支持SCNET服务一键重启功能
- 自动配置服务参数
- 实时监控服务状态
- 详细日志记录功能
- 支持常见问题解决方案查询

=== 系统要求 ===

- 操作系统: Windows 10/11
- .NET 10 或更高版本
- 内存: 2GB 或更高
- 磁盘空间: 50MB 可用空间
- 管理员权限";
}
