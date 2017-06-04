using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Diagnostics;
using System.Linq;
using Button = System.Windows.Controls.Button;
using TextBox = System.Windows.Controls.TextBox;
using System.Windows.Controls;

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

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            hotkeyInstance.ExampleForm_FormClosing();
			string[] allPaths = { TxtPath01.Text, TxtPath02.Text, TxtPath03.Text, TxtPath04.Text, TxtPath05.Text };
            File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WAS\\ShortcutList.txt", allPaths);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WAS");
            string path = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WAS\\ShortcutList.txt");
            if (!File.Exists(path))
            {
                FileStream x = File.Create(path);
                x.Close();
            }

            string[] values = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WAS\\ShortcutList.txt");
            if (values.Length > 0)
            {
                TxtPath01.Text = values[0];
                TxtPath02.Text = values[1];
                TxtPath03.Text = values[2];
                TxtPath04.Text = values[3];
                TxtPath05.Text = values[4];
            }
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
