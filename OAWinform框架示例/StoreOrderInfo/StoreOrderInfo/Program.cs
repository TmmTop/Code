using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetDimension.NanUI;

namespace StoreOrderInfo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            HtmlUILauncher.EnableFlashSupport = true;
            if (HtmlUILauncher.InitializeChromium((args => {
                args.Settings.LogSeverity = Chromium.CfxLogSeverity.Default;
            })))
            {
                HtmlUILauncher.RegisterEmbeddedScheme(System.Reflection.Assembly.GetExecutingAssembly());
                Application.Run(new UI());
            }
        }
    }
}
