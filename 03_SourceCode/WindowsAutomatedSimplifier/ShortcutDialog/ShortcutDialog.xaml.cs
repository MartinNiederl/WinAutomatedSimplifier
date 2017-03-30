using GlobalHotkeyExampleForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WindowsAutomatedSimplifier.DeCompress;
using WindowsAutomatedSimplifier.Repository;

namespace WindowsAutomatedSimplifier.ShortcutDialog
{
    /// <summary>
    /// Interaction logic for ShortcutDialog.xaml
    /// </summary>
    public partial class ShortcutDialog : Window
    {
        private GlobalHotkeys ef;

        public ShortcutDialog()
        {
            InitializeComponent();
            ef = GlobalHotkeys.getInstance(this);
        }

        public void SH01_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @"C:\Users\david\AppData");
        }

        public void SH02_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @"C:\ProgramData");
        }

        public void SH03_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @"C:\Users\david\Desktop");
        }

        public void SH04_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @"C:\ProgramData");
        }

        public void SH05_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @"C:\ProgramData");
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ef.ExampleForm_FormClosing();
        }
    }
}
