using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            InitializeHelpContent();
        }

        private void InitializeHelpContent()
        {
            // 功能说明内容
            StringBuilder helpContent = new StringBuilder();
            helpContent.AppendLine("SCNET_Restart_Tool 功能说明：\n");
            helpContent.AppendLine("1. 多服务器管理");
            helpContent.AppendLine("   - 支持同时配置和管理多个服务端实例");
            helpContent.AppendLine("   - 每个服务器可独立设置监控参数和重启策略");
            helpContent.AppendLine("   - 直观的列表展示所有服务器的运行状态\n");
            helpContent.AppendLine("2. 自动故障检测与重启");
            helpContent.AppendLine("   - 实时监控指定服务进程的运行状态，支持进程名+路径双重验证");
            helpContent.AppendLine("   - 当检测到服务意外终止时，自动执行重启操作");
            helpContent.AppendLine("   - 内置重试机制，确保服务可靠启动");
            helpContent.AppendLine("   - 详细的日志记录，便于问题追溯和分析\n");
            helpContent.AppendLine("3. 定时计划重启");
            helpContent.AppendLine("   - 支持设置每日固定时间点自动重启服务");
            helpContent.AppendLine("   - 重启前可发送关闭指令，确保服务正常关闭");
            helpContent.AppendLine("   - 可自定义定时重启的时间点\n");
            helpContent.AppendLine("4. 间隔周期重启");
            helpContent.AppendLine("   - 支持按照设定的时间间隔（小时）自动重启服务");
            helpContent.AppendLine("   - 适合需要定期清理内存和资源的服务场景");
            helpContent.AppendLine("   - 自动记录上次重启时间，确保间隔准确\n");
            helpContent.AppendLine("5. 远程命令控制");
            helpContent.AppendLine("   - 提供 TCP 协议接口，可向服务端发送自定义命令");
            helpContent.AppendLine("   - 支持命令发送前的密码验证");
            helpContent.AppendLine("   - 实时显示命令执行结果\n");
            helpContent.AppendLine("6. 文件夹管理");
            helpContent.AppendLine("   - 集成服务器相关文件夹快速访问功能");
            helpContent.AppendLine("   - 支持直接打开配置、日志、插件等常用文件夹\n");
            helpContent.AppendLine("7. 跨平台支持");
            helpContent.AppendLine("   - Windows 版本：提供图形用户界面，操作简便直观");
            helpContent.AppendLine("   - Linux 版本：命令行界面，适合服务器环境后台运行\n");
            helpContent.AppendLine("8. 日志系统");
            helpContent.AppendLine("   - 实时记录所有操作和状态变更");
            helpContent.AppendLine("   - 支持日志文件保存，便于后期查阅");
            helpContent.AppendLine("   - 可自定义日志保存路径\n");
            helpContent.AppendLine("9. 程序自检与权限管理");
            helpContent.AppendLine("   - 自动检测管理员权限，确保程序正常运行");
            helpContent.AppendLine("   - 提供权限提升选项，避免因权限不足导致功能受限");

            txtHelpContent.Text = helpContent.ToString();

            // 项目信息内容
            StringBuilder projectInfo = new StringBuilder();
            projectInfo.AppendLine("SCNET_Restart_Tool 项目信息\n");
            projectInfo.AppendLine("项目名称：SCNET_Restart_Tool");
            projectInfo.AppendLine("项目类型：服务端程序自动监控和重启工具");
            projectInfo.AppendLine("适用对象：SCNET 服务端程序（如生存战争服务器）");
            projectInfo.AppendLine("项目地址：https://gitee.com/yylmzxc/SCNET_Restart_Tool");
            projectInfo.AppendLine("许可证：MIT 许可证\n");
            projectInfo.AppendLine("主要功能：");
            projectInfo.AppendLine("- 确保服务端程序持续稳定运行");
            projectInfo.AppendLine("- 实时监控服务运行状态");
            projectInfo.AppendLine("- 提供多种智能重启机制");
            projectInfo.AppendLine("- 支持远程命令控制");
            projectInfo.AppendLine("- 有效提高服务的可靠性和运维效率");

            txtProjectInfo.Text = projectInfo.ToString();

            // 版本信息内容
            StringBuilder versionInfo = new StringBuilder();
            versionInfo.AppendLine("SCNET_Restart_Tool 版本信息\n");

            // 获取程序集版本信息
            var assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
            var ver = assembly.GetName().Version;
            versionInfo.AppendLine($"当前版本：v{ver.Major}.{ver.Minor}.{ver.Build}");

            // 读取程序集的组织（Company）信息
            var companyAttr = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
            string company = companyAttr?.Company ?? "未指定"; // 若未设置，使用默认值

            // 拼接组织信息
            versionInfo.AppendLine($"组织：{company}");

            versionInfo.AppendLine("更新日期：2025年10月\n");
            versionInfo.AppendLine("主要更新内容：");
            versionInfo.AppendLine("- 优化了进程检测逻辑，使用进程名+路径双重验证提高准确性");
            versionInfo.AppendLine("- 增加了自动重试机制，确保服务可靠启动");
            versionInfo.AppendLine("- 改进了日志系统，支持不同日志级别和文件保存");
            versionInfo.AppendLine("- 优化了用户界面，提供更直观的操作体验");
            versionInfo.AppendLine("- 修复了权限相关问题，确保程序在各种环境下正常运行");
            versionInfo.AppendLine("- 添加了帮助窗口，提供完整的功能说明和项目信息");

            txtVersionInfo.Text = versionInfo.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}