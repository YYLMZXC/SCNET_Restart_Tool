using System;
using System.Windows.Forms;

namespace SCNET_Restart_Tool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ToolMain());
        }
    }
}