using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ISPFSkeletonParser
{
    static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        public static DataSet ds2 = new DataSet();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            using (System.Threading.Mutex mutex =
                new System.Threading.Mutex(true, "SkelAnalyzer", out createdNew))
            {
                if (createdNew)
                {
                    #region Run a new app
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Query());
                    #endregion
                }
                else
                {
                    #region jump to already existing window
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in
                        Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                    #endregion
                }
            }
        }
    }
}
