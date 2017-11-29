using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
namespace SkeletonParserQuery
{
    static class Program
    {
        public static DataSet ds2 = new DataSet();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Query());
        }
    }
}
