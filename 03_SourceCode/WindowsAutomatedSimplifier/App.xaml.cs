using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WindowsAutomatedSimplifier.PasswordProtectedFolder;

namespace WindowsAutomatedSimplifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0) return;
            string dir = e.Args.Aggregate("", (current, eArg) => current + " " + eArg);

            if (Directory.Exists(dir)) new ProtectedFolder(dir, "password");
            else if (File.Exists(dir) && dir.EndsWith(".pwf"))
            {
                FolderReader fr = new FolderReader(dir);
                fr.SaveAllFiles();
            }
            else MessageBox.Show(dir, "Ungültiges Verzeichnis!");
            Environment.Exit(0);
        }
    }
}
