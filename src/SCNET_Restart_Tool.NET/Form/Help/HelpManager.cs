using System.Windows.Forms;

namespace SCNET_Restart_Tool.Form.Help
{
    /// <summary>
    /// 帮助管理器 - 提供表单跳转功能
    /// </summary>
    public static class HelpManager
    {
       
        /// <summary>
        /// 显示功能说明表单
        /// </summary>
        public static void ShowFunctionDescription()
        {
            using (var form = new FunctionDescriptionForm())
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// 显示项目信息表单
        /// </summary>
        public static void ShowProjectInfo()
        {
            using (var form = new ProjectInfoForm())
            {
                form.ShowDialog();
            }
        }

        /// <summary>
        /// 显示版本信息表单
        /// </summary>
        public static void ShowVersionInfo()
        {
            using (var form = new VersionInfoForm())
            {
                form.ShowDialog();
            }
        }
    }
}