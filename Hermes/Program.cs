using Hermes.locatable_resources;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hermes
{
    static class Program
    {
        #region constants
        static string titleApp = StringResources.app_name;
        #endregion

        #region dll

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Globalization.CultureInfo cultureInfo = System
                .Globalization.CultureInfo.CreateSpecificCulture("en-EN");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IntPtr nWnd = FindWindow(null, titleApp);
            if (nWnd.Equals(new System.IntPtr(0)))
            {
                Application.Run(new mdiMain());
            }
            else
            {
                SetForegroundWindow(nWnd);
                Application.Exit();
            }
        }
    }
}
