using System;
using System.Windows;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Environment;

namespace WindowsAutomatedSimplifier.ShortcutDialog
{
    /// <summary>
    /// Interaction logic for ShortcutDialog.xaml
    /// </summary>
    public partial class ShortcutDialog : Window
    {
        public GlobalHotkeys GHInstance { get; }

        public ShortcutDialog()
        {
            InitializeComponent();
            GHInstance = new GlobalHotkeys(this);
        }

        public void SH01_Click(object sender, RoutedEventArgs e)
        {
            RunCommand(GetFolderPath(SpecialFolder.ApplicationData));
        }

        public void SH02_Click(object sender, RoutedEventArgs e)
        {
            RunCommand(@"C:\ProgramData");
        }

        public void SH03_Click(object sender, RoutedEventArgs e)
        {
            RunCommand(GetFolderPath(SpecialFolder.Desktop));
        }

        public void SH04_Click(object sender, RoutedEventArgs e)
        {
            RunCommand(@"C:\ProgramData");
        }

        public void SH05_Click(object sender, RoutedEventArgs e)
        {
            RunCommand(@"C:\ProgramData");
        }

        private static void RunCommand(string arguments)
        {
            try
            {
                Task.Factory.StartNew(() => Process.Start("explorer.exe", arguments));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GHInstance.WindClose();
        }
    }
}
