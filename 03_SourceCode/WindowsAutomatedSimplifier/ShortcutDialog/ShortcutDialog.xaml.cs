﻿using GlobalHotkeyExampleForm;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WindowsAutomatedSimplifier.ShortcutDialog
{
    /// <summary>
    /// Interaction logic for ShortcutDialog.xaml
    /// </summary>
    public partial class ShortcutDialog : Window
    {
        private GlobalHotkeys gH;

        public ShortcutDialog()
        {
            InitializeComponent();
            gH = new GlobalHotkeys(this);
        }

        public void textBox01_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox01.Text);
        }

        public void textBox02_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox02.Text);
        }

        public void textBox03_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox03.Text);
        }

        public void textBox04_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox04.Text);
        }

        public void textBox05_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", textBox05.Text);
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            gH.ExampleForm_FormClosing();
            String[] allPaths = new string[5];
            allPaths[0] = textBox01.Text;
            allPaths[1] = textBox02.Text;
            allPaths[2] = textBox03.Text;
            allPaths[3] = textBox04.Text;
            allPaths[4] = textBox05.Text;

            //File.WriteAllLines((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WindowsAutomatedSimplifier\ShortcutList.txt"), allPaths);
            File.WriteAllLines(@"C:\Users\david\Documents\WinAutomatedSimplifier\WinAutomatedSimplifier\03_SourceCode\WindowsAutomatedSimplifier\ShortcutDialog\ShortcutList.txt", allPaths);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            //Read Paths from file.
            string[] values = new string[5];
            values = File.ReadAllLines(@"C:\Users\david\Documents\WinAutomatedSimplifier\WinAutomatedSimplifier\03_SourceCode\WindowsAutomatedSimplifier\ShortcutDialog\ShortcutList.txt");

            textBox01.Text = values[0];
            textBox02.Text = values[1];
            textBox03.Text = values[2];
            textBox04.Text = values[3];
            textBox05.Text = values[4];
        }

        private void GetFolderPath(TextBox tb)
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb.Text = fbd.SelectedPath;
            }
        }

        private void sH05_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(textBox05);
        }

        private void sH04_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(textBox04);
        }

        private void sH03_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(textBox03);
        }

        private void sH02_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(textBox02);
        }

        private void sH01_Click(object sender, RoutedEventArgs e)
        {
            GetFolderPath(textBox01);
        }
    }
}

/*
var fbd = new System.Windows.Forms.FolderBrowserDialog();
if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
{
    Button x = sender as Button;
    int row = Grid.GetRow(x);
    var temp = sHGrid.Children.Cast<UIElement>().Where(i => Grid.GetRow(i) == row).ElementAt(1) as TextBox;
    Console.WriteLine(fbd.SelectedPath);
    //temp.Text = fbd.SelectedPath;
}
*/
