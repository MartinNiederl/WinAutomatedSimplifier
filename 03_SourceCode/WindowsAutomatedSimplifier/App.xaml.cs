using System;
using System.IO;
using System.Linq;
using System.Windows;
using WindowsAutomatedSimplifier.EncryptedDirectory;
using PasswordWindow = WindowsAutomatedSimplifier.Repository.PasswordWindow;

namespace WindowsAutomatedSimplifier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length < 1) return;

            string dir = e.Args.Aggregate("", (current, eArg) => current + " " + eArg);

            if (Directory.Exists(dir))
            {
                PasswordWindow pw = new PasswordWindow();
                new EncryptedDirectoryWriter(dir, pw.Password);
            }
            else if (File.Exists(dir) && dir.EndsWith(".pwf"))
            {
                EncryptedDirectoryReader fr = new EncryptedDirectoryReader(dir);
                fr.SaveAllFiles();
            }
            else MessageBox.Show(dir, "Ungültiges Verzeichnis!");
            Environment.Exit(0);
        }
    }
}
