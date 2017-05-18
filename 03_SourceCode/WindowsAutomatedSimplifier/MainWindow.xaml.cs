using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WindowsAutomatedSimplifier.ChangeFont;
using WindowsAutomatedSimplifier.DeCompress;
using WindowsAutomatedSimplifier.FileSystem;
using WindowsAutomatedSimplifier.IconSpacing;
using WindowsAutomatedSimplifier.NetworkSettings;
using WindowsAutomatedSimplifier.PasswordProtectedFolder;
using WindowsAutomatedSimplifier.RegistryHelper;
using WindowsAutomatedSimplifier.Repository;
using WindowsAutomatedSimplifier.WindowsTweaks;
using Microsoft.Win32;
using static System.Windows.Application;
using Button = System.Windows.Controls.Button;
using MessageBox = CustomMessageBox.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using PasswordWindow = WindowsAutomatedSimplifier.Repository.PasswordWindow;

namespace WindowsAutomatedSimplifier
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowManager.AddWindow(this);
            Task.Run(() => RegistryAPI.InitRegistry());
        }


        private void OpenShortcutDialog(object sender, RoutedEventArgs e)
        {
            //new Short
            new ShortcutDialog.ShortcutDialog().Show();
        }

        /// <summary>
        /// Event for decommpressing files
        /// </summary>
        private void Btn_compress_OnClick(object sender, RoutedEventArgs e)
        {
            //Open new window for compress interaction
            new CompressWindow().ShowDialog();
        }

        /// <summary>
        /// Event for decompressing files
        /// </summary>
        private void Btn_decompress_OnClick(object sender, RoutedEventArgs e)
        {
            //Dialog for selecting the file for decompressing
            OpenFileDialog ofDialog = new OpenFileDialog
            {
                Filter = "Archive Files (*.7z;*.rar;*.tar;*.zip;*.gz)|*.7z;*.rar;*.tar;*.zip;*.gz"
            };
            if (ofDialog.ShowDialog() == false) return;

            //Dialog for selecting the folder where the decompressed files should get stored
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            if (fbDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            Archive archive = new Archive(new List<FileInfo> {new FileInfo(ofDialog.FileName)});
            Task.Run(() =>
            {
                archive.Decompress(fbDialog.SelectedPath);
                AutoClosingMessageBox.Show("Decompressing Finished Successfully", "Closing...", 1000);
            });
        }

        private void BtnIconSpacing_OnClick(object sender, RoutedEventArgs e) => new IconSpacingWindow().ShowDialog();

        private void BtnCreateProtectedFolder_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fbd.SelectedPath = rootPath;
                fbd.ShowDialog();

                if (fbd.SelectedPath != "" && fbd.SelectedPath != rootPath && Directory.Exists(fbd.SelectedPath))
                {
                    PasswordWindow pw = new PasswordWindow();
                    pw.ShowDialog();
                    new ProtectedFolder(fbd.SelectedPath, pw.Password);
                }
            }
        }

        private void BtnReadProtectedFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog {Filter = "PasswordEncryptedFile (*.pwf)|*.pwf"};
            ofd.ShowDialog();
            FolderReader fr = new FolderReader(ofd.FileName);
            fr.SaveAllFiles();
        }

        private void BtnDeleteEmptyFolders_Click(object sender, RoutedEventArgs e) => Task.Factory.StartNew(() => FileSystem.FileSystemLogic
            .DeleteEmptyDirectories(@"C:\Users\Mani\Documents\Schule\Projektentwicklung\TESTORDNER"));

        private void BtnSetAeroSpeed_Click(object sender, RoutedEventArgs e) => Registry.SetValue(
            @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            "DesktopLivePreviewHoverTime", (int) MSecSlider.Value, RegistryValueKind.DWord);

        private void SCExtension(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Name.Contains("Activate")) ShortcutExtension.EnableShortcutExtension();
            else ShortcutExtension.DisableShortcutExtension();
        }

        private void UpdateRegistry_Click(object sender, RoutedEventArgs e) => RegistryAPI.UpdateRegistry();

        private void FontChange_Click(object sender, RoutedEventArgs e) => new FontPicker().ShowDialog();


        private void checkBox_Checked(object sender, RoutedEventArgs e) => FolderCheckboxes.EnableCheckboxes();
        private void checkBox_Unchecked(object sender, RoutedEventArgs e) => FolderCheckboxes.DisableCheckboxes();

        private void checkBox_BlackTheme_Checked(object sender, RoutedEventArgs e) => BlackTheme.EnableBlackTheme();
        private void checkBox_BlackTheme_Unchecked(object sender, RoutedEventArgs e) => BlackTheme.DisableBlackTheme();

        private void checkBox_AeroShake_Checked(object sender, RoutedEventArgs e) => ToggleAeroShake.EnableAeroShake();

        private void checkBox_AeroShake_Unchecked(object sender, RoutedEventArgs e) => ToggleAeroShake
            .DisableAeroShake();

        private void applyPreviewSizeChange_Click(object sender, RoutedEventArgs e) => TaskbarPreviewWindow
            .SetWindowSize((int) slider_TaskbarPreview.Value);

        private void restorePreviewSize_Click(object sender, RoutedEventArgs e)
        {
            TaskbarPreviewWindow.RestoreWindowSize();
            slider_TaskbarPreview.Value = 300;
        }

        private void EncryptDecryptTest_Click(object sender, RoutedEventArgs e)
        {
            const string path = @"C:\Users\Mani\Documents\Schule\Projektentwicklung\";
            const string sourcefile = "emptyFolders.txt", targetfile = "emptyFolders-2.txt";
            const string password = "geheimespasswort";

            byte[] prev = File.ReadAllBytes(path + sourcefile);
            byte[] enc = Encryption.EncryptBytes(prev, password);
            byte[] dec = Encryption.DecryptBytes(enc, password);

            File.WriteAllBytes(path + targetfile, dec);
        }

        private void BtnNetwork_OnClick(object sender, RoutedEventArgs e) => new Network().ShowDialog();

        private void BtnFileSystem_OnClick(object sender, RoutedEventArgs e) => new FileSystemMainWindow().ShowDialog();

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Update Changes?",
                "Update changes now? Else on next Windows start...", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) RegistryAPI.UpdateRegistry();

            Window wind = sender as Window;
            foreach (Window k in Current.Windows)
                if (wind != null && !wind.Equals(k)) k.Close();
        }
    }
}