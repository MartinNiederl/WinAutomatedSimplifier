using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WindowsAutomatedSimplifier.ChangeFont;
//using System.Windows.Forms;
using WindowsAutomatedSimplifier.DeCompress;
using WindowsAutomatedSimplifier.IconSpacing;
using WindowsAutomatedSimplifier.PasswordProtectedFolder;
using WindowsAutomatedSimplifier.RegistryHelper;
using WindowsAutomatedSimplifier.Repository;
using Microsoft.Win32;
using Button = System.Windows.Controls.Button;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WindowsAutomatedSimplifier
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowManager.AddWindow(this);
            RegistryAPI.InitRegistry();
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
            OpenFileDialog ofDialog = new OpenFileDialog { Filter = "Archive Files (*.7z;*.rar;*.tar;*.zip;*.gz)|*.7z;*.rar;*.tar;*.zip;*.gz" };
            if (ofDialog.ShowDialog() == false) return;

            //Dialog for selecting the folder where the decompressed files should get stored
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            if (fbDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            Archive archive = new Archive(new List<FileInfo> { new FileInfo(ofDialog.FileName) });
            archive.Decompress(fbDialog.SelectedPath);

            //MessageBox to confirm the functionality (closes after 1 second)
            AutoClosingMessageBox.Show("Finished Successfully", "Closing...", 1000);
        }

        private void BtnIconSpacing_OnClick(object sender, RoutedEventArgs e) => new IconSpacingWindow().ShowDialog();

        private void BtnCreateProtectedFolder_Click(object sender, RoutedEventArgs e)
        {
            //FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            //if (fbDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            //new ProtectedFolder(fbDialog.SelectedPath);
            new ProtectedFolder(@"C:\Users\Mani\Documents\Schule\Projektentwicklung\PWF TestOrdner", "password");
        }

        private void BtnReadProtectedFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PasswordEncryptedFile (*.pwf)|*.pwf";
            ofd.ShowDialog();
            FolderReader fr = new FolderReader(ofd.FileName);
            fr.SaveAllFiles();
        }

        private void BtnDeleteEmptyFolders_Click(object sender, RoutedEventArgs e) => FileSystem.FileSystem.DeleteEmptyDirectories("C:\\Users\\Mani\\Documents\\Schule\\Projektentwicklung\\TESTORDNER", false);

        private void BtnSetAeroSpeed_Click(object sender, RoutedEventArgs e) => Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DesktopLivePreviewHoverTime", (int)MSecSlider.Value, RegistryValueKind.DWord);

        private void SCExtension(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Name.Contains("Activate")) ShortcutExtension.se_enable();
            else ShortcutExtension.se_disable();
        }

        private void UpdateRegistry_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    Verb = "runas",
                    Arguments = "/f /im explorer.exe",
                    FileName = @"C:\windows\system32\taskkill.exe"
                };
                Process process = new Process { StartInfo = startInfo };
                process.Start();
                process.WaitForExit();
                startInfo = new ProcessStartInfo()
                {
                    Verb = "runas",
                    FileName = @"C:\windows\explorer.exe"
                };
                process = new Process { StartInfo = startInfo };
                process.Start();
            });
        }

        private void FontChange_Click(object sender, RoutedEventArgs e)
        {
            new FontPicker().ShowDialog();
        }


        private void checkBox_Checked(object sender, RoutedEventArgs e) => FolderCheckboxes.checkBox_Checked();
        private void checkBox_Unchecked(object sender, RoutedEventArgs e) => FolderCheckboxes.checkBox_Unchecked();

        private void checkBox_BlackTheme_Checked(object sender, RoutedEventArgs e) => BlackTheme.checkBox_Checked();
        private void checkBox_BlackTheme_Unchecked(object sender, RoutedEventArgs e) => BlackTheme.checkBox_Unchecked();

        private void checkBox_AeroShake_Checked(object sender, RoutedEventArgs e) => ToggleAeroShake.checkBox_Checked();
        private void checkBox_AeroShake_Unchecked(object sender, RoutedEventArgs e) => ToggleAeroShake.checkBox_Unchecked();

        private void applyPreviewSizeChange_Click(object sender, RoutedEventArgs e) => TaskbarPreviewWindow.WindowSize((int)slider_TaskbarPreview.Value);
        private void restorePreviewSize_Click(object sender, RoutedEventArgs e)
        {
            TaskbarPreviewWindow.RestoreWindowSize();
            slider_TaskbarPreview.Value = 250;
        }
    }
}