using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace WindowsAutomatedSimplifier.Repository
{
    internal class Logger
    {
        private static readonly TextWriter Writer = TextWriter.Synchronized(File.AppendText(Application.StartupPath + "/WAS.log"));
        private static readonly object SyncObject = new object();

        public static void Log(string logMessage, [CallerMemberName] string memberName = "")
        {
            lock (SyncObject)
            {
                Writer.WriteLine("{0} {1} in {2}", DateTime.Now.TimeOfDay, DateTime.Now.ToShortDateString(), memberName);
                Writer.WriteLine("  :{0}", logMessage);
                Writer.WriteLine("-------------------------------");
                Writer.Flush();
            }
        }

        public static void LogAdd(string logMessage, string additional)
        {
            lock (SyncObject)
            {
                Writer.WriteLine("{0} {1}", DateTime.Now.TimeOfDay, DateTime.Now.ToShortDateString());
                Writer.WriteLine("  :{0}", logMessage);
                Writer.WriteLine(additional);
                Writer.WriteLine("-------------------------------");
                Writer.Flush();
            }
        }
    }
}
