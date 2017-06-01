using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;

namespace WindowsAutomatedSimplifier.ShortcutDialog
{
    /// <summary>
    /// Interaction logic for ShortcutDialog.xaml
    /// </summary>
    public partial class ShortcutDialog : Window
    {
        // ReSharper disable once InconsistentNaming
        private readonly GlobalHotkeys hotkeyInstance;

        public ShortcutDialog()
        {
            InitializeComponent();
            hotkeyInstance = new GlobalHotkeys(this);
        }

        /*public void textBox01_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", TxtPath01.Text);
        }

        public void textBox02_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", TxtPath02.Text);
        }

        public void textBox03_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", TxtPath03.Text);
        }

        public void textBox04_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", TxtPath04.Text);
        }

        public void textBox05_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", TxtPath05.Text);
        }*/

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            hotkeyInstance.ExampleForm_FormClosing();
            string[] allPaths = { TxtPath01.Text, TxtPath02.Text, TxtPath03.Text, TxtPath04.Text, TxtPath05.Text };

            //TODO Fix path
            //File.WriteAllLines(@"C:\Users\david\Documents\WinAutomatedSimplifier\WinAutomatedSimplifier\03_SourceCode\WindowsAutomatedSimplifier\ShortcutDialog\ShortcutList.txt", allPaths);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //Read Paths from file.TODO Fix path
            //var values = File.ReadAllLines(@"C:\Users\david\Documents\WinAutomatedSimplifier\WinAutomatedSimplifier\03_SourceCode\WindowsAutomatedSimplifier\ShortcutDialog\ShortcutList.txt");

            //TxtPath01.Text = values[0];
            //TxtPath02.Text = values[1];
            //TxtPath03.Text = values[2];
            //TxtPath04.Text = values[3];
            //TxtPath05.Text = values[4];
        }

        private void GetFolderPath(TextBox tb)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                tb.Text = fbd.SelectedPath;
        }

        private void sH05_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(TxtPath05);
        }

        private void sH04_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(TxtPath04);
        }

        private void sH03_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(TxtPath03);
        }

        private void sH02_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(TxtPath02);
        }

        private void sH01_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(TxtPath01);
        }


        public void RunCommand(object sender, MouseButtonEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box != null && box.Text.Length > 0)
                Process.Start("explorer.exe", box.Text);
        }

        private void ChoosePath(object sender, RoutedEventArgs e)
        {
            Button trigger = sender as Button;
            if (trigger == null) return;

            TextBox box = HotkeyInterface.Children
                .Cast<UIElement>()
                .First(el => Grid.GetRow(el) == Grid.GetRow(trigger) && Grid.GetColumn(el) == 1) as TextBox;

            if (box == null) return;

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                box.Text = fbd.SelectedPath;

        }
    }
}
