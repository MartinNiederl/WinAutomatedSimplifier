using System;
using System.Collections.Generic;
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
    public partial class ShortcutDialog
    {
        // ReSharper disable InconsistentNaming
        private readonly GlobalHotkeys hotkeyInstance;
        private static readonly string APPDATA = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinAS");
        private static readonly string scListPath = Path.Combine(APPDATA, "Shortcutlist.txt");

        public ShortcutDialog()
        {
            InitializeComponent();
            hotkeyInstance = new GlobalHotkeys(this);
        }

        public void Close()
        {
            var paths = HotkeyInterface.Children.OfType<TextBox>().Select(el => el.Text);

            if (!File.Exists(scListPath)) File.Create(scListPath);
            File.WriteAllLines(scListPath, paths);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Directory.CreateDirectory(APPDATA);
            if (!File.Exists(scListPath))
                File.Create(scListPath).Close();

            var values = File.ReadAllLines(scListPath);

            for (int i = 1; i < values.Length; i++)
            {
                string name = "TxtPath" + i.ToString().PadLeft(2, '0');
                TextBox box = (TextBox)FindName(name);
                if (box != null) box.Text = values[i - 1];
            }
        }

        public void RunCommand(object sender, MouseButtonEventArgs e)
        {
            Close();

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
