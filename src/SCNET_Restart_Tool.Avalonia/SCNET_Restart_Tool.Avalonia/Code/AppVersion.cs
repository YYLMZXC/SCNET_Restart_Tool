using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SCNET_Restart_Tool;

/// <summary>
/// 应用版本信息（版本号按构建时间自动生成，无需手工维护）。
///
/// 版本规则：
/// - 完整版本号：yy.MM.dd-HHmm（如 26.11.03-2010，取自编译时刻）；
/// - 简短版本号：yy.MM.dd（如 26.11.03，用于窗口标题）。
///
/// 构建时间由项目文件在编译时写入程序集元数据（BuildTimeTicks），
/// 因此每次重新编译版本号都会自动更新，不存在版本号与实际构建不一致的问题。
/// </summary>
public static class AppVersion
{
    /// <summary>产品名称（窗口标题与帮助内容统一使用）</summary>
    public const string ProductName = "SCNET服务端管理工具";

    /// <summary>项目地址（Gitee / CNB / GitHub 三处仓库，帮助内容与检查更新提示共用）</summary>
    public static readonly string[] ProjectUrls =
    {
        "https://gitee.com/yylmzxc/SCNET_Restart_Tool",
        "https://cnb.cool/SurvivalcraftTool/SCNET_Restart_Tool",
        "https://github.com/YYLMZXC/SCNET_Restart_Tool"
    };

    /// <summary>项目地址文本：每行一个，便于在对话框中直接展示</summary>
    public static string ProjectUrlText => string.Join(Environment.NewLine, ProjectUrls);

    /// <summary>构建时间（本地时间，编译时写入程序集元数据）</summary>
    private static readonly DateTime BuildTime = ResolveBuildTime();

    /// <summary>完整版本号，形如 26.11.03-2010</summary>
    public static string Full => BuildTime.ToString("yy.MM.dd-HHmm", CultureInfo.InvariantCulture);

    /// <summary>简短版本号，形如 26.11.03（窗口标题只显示到日）</summary>
    public static string Short => BuildTime.ToString("yy.MM.dd", CultureInfo.InvariantCulture);

    /// <summary>构建时间文本，形如 2026-11-03 20:10:33</summary>
    public static string BuildTimeText => BuildTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

    /// <summary>窗口标题：产品名 + 简短版本号（如 SCNET服务端管理工具 26.11.03）</summary>
    public static string WindowTitle => $"{ProductName} {Short}";

    /// <summary>
    /// 解析构建时间：优先读取程序集元数据；元数据缺失（如旧版本程序集）时
    /// 退回程序集文件写入时间，仍失败则使用当前时间，保证界面始终有版本号可显示。
    /// </summary>
    private static DateTime ResolveBuildTime()
    {
        var fromMetadata = ReadBuildTimeFromMetadata();
        if (fromMetadata.HasValue) return fromMetadata.Value;

        try
        {
            return File.GetLastWriteTime(typeof(AppVersion).Assembly.Location);
        }
        catch (Exception)
        {
            // 单文件发布等场景下可能无法获取路径，版本显示不应影响程序运行
            return DateTime.Now;
        }
    }

    /// <summary>从程序集元数据读取构建时间（Ticks，避免格式化时的区域性差异）</summary>
    private static DateTime? ReadBuildTimeFromMetadata()
    {
        try
        {
            var value = typeof(AppVersion).Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(a => a.Key == "BuildTimeTicks")?.Value;

            return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ticks)
                ? new DateTime(ticks)
                : null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
