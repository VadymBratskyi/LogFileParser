using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogParserWithMongoDb
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogParser());
        }

        public static void Process()
        {
            System.Diagnostics.ProcessStartInfo st = new ProcessStartInfo(@"C:\Program Files\MongoDB\Server\3.4\bin\mongod.exe");
            st.WindowStyle = ProcessWindowStyle.Hidden;
            st.Arguments = @"--dbpath C:\data --port 27017";
            System.Diagnostics.Process.Start(st);
        }
    }
}
