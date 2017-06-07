using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WindowsAutomatedSimplifier.ChangeFont;
using WindowsAutomatedSimplifier.DeCompress;
using WindowsAutomatedSimplifier.EncryptedDirectory;
using WindowsAutomatedSimplifier.FileSystem;
using WindowsAutomatedSimplifier.IconSpacing;
using WindowsAutomatedSimplifier.NetworkSettings;
using WindowsAutomatedSimplifier.RegistryHelper;
using WindowsAutomatedSimplifier.Repository;
using WindowsAutomatedSimplifier.WindowsTweaks;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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

            Archive archive = new Archive(new List<FileInfo> { new FileInfo(ofDialog.FileName) });
            Task.Run(() =>
            {
                archive.Decompress(fbDialog.SelectedPath);
                AutoClosingMessageBox.Show("Decompressing Finished Successfully", "Closing...", 1000);
            });
        }

        private void BtnIconSpacing_OnClick(object sender, RoutedEventArgs e) => new IconSpacingWindow();

        



        private void BtnSetAeroSpeed_Click(object sender, RoutedEventArgs e) => Registry.SetValue(
            @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
            "DesktopLivePreviewHoverTime", (int)MSecSlider.Value, RegistryValueKind.DWord);

        private void UpdateRegistry_Click(object sender, RoutedEventArgs e) => RegistryAPI.UpdateRegistry();

        private void FontChange_Click(object sender, RoutedEventArgs e) => new FontPicker();


        private void ToggleCheckBoxes_Checked(object sender, RoutedEventArgs e) => FolderCheckboxes.EnableCheckboxes();
        private void ToggleCheckBoxes_Unchecked(object sender, RoutedEventArgs e) => FolderCheckboxes.DisableCheckboxes();

        private void ToggleBlackTheme_Checked(object sender, RoutedEventArgs e) => BlackTheme.EnableBlackTheme();
        private void ToggleBlackTheme_Unchecked(object sender, RoutedEventArgs e) => BlackTheme.DisableBlackTheme();

        private void ToggleAeroShake_Checked(object sender, RoutedEventArgs e) => WindowsTweaks.ToggleAeroShake.EnableAeroShake();

        private void ToggleAeroShake_Unchecked(object sender, RoutedEventArgs e) => WindowsTweaks.ToggleAeroShake
            .DisableAeroShake();

        private void applyPreviewSizeChange_Click(object sender, RoutedEventArgs e) => TaskbarPreviewWindow
            .SetWindowSize((int)slider_TaskbarPreview.Value);

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

        private void BtnNetwork_OnClick(object sender, RoutedEventArgs e) => new Network();

        private void BtnFileSystem_OnClick(object sender, RoutedEventArgs e) => new FileSystemMainWindow();

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Update Changes?", "Update changes now?", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) RegistryAPI.UpdateRegistry();

            Window wind = sender as Window;
            foreach (Window k in Current.Windows)
                if (wind != null && !wind.Equals(k)) k.Close();
        }

        private void ToggleShortcutExtension_Checked(object sender, RoutedEventArgs e) => ShortcutExtension.EnableShortcutExtension();
        private void ToggleShortcutExtension_Unchecked(object sender, RoutedEventArgs e) => ShortcutExtension.DisableShortcutExtension();

        private void Design_Initialized(object sender, EventArgs e)
        {
            const string advancedKeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
            const string personalizeKeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            const string taskbandKeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband";
            const string explorerKeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

            ToggleAeroShake.IsChecked = (int)Registry.GetValue(advancedKeyPath, "DisallowShaking", false) == 0;
            ToggleBlackTheme.IsChecked = (int)Registry.GetValue(personalizeKeyPath, "AppsUseLightTheme", false) == 0;
            ToggleCheckBoxes.IsChecked = (int)Registry.GetValue(advancedKeyPath, "AutoCheckSelect", false) == 1;
            slider_TaskbarPreview.Value = (int)Registry.GetValue(taskbandKeyPath, "MinThumbSizePx", -1);
            MSecSlider.Value = (int)Registry.GetValue(advancedKeyPath, "DesktopLivePreviewHoverTime", -1);

            object val = Registry.GetValue(explorerKeyPath, "link", null);
            ToggleShortcutExtension.IsChecked = val == null || val.ToString() != "0";
        }


        #region DeleteEmptyDirectories
        private void DelEmptyDirs_Click(object sender, RoutedEventArgs e)
        {
            string rootPath = TxtDelDirsPath.Text;
            if (!Directory.Exists(rootPath))
            {
                CommonFileDialog folderDialog = FileDialogs.OpenFolderDialog();
                if (folderDialog == null) return;
                rootPath = folderDialog.FileName;
            }
            Task.Factory.StartNew(() => FileSystemLogic.DeleteEmptyDirectories(rootPath, EmptyFolderBlacklist.Items.OfType<string>().ToList()));
        }

        private void DelDirsChoosePath_Click(object sender, RoutedEventArgs e)
        {
            CommonFileDialog folderDialog = FileDialogs.OpenFolderDialog();
            if (folderDialog != null) TxtDelDirsPath.Text = folderDialog.FileName;
        }

        private void DelDirsClear_Click(object sender, RoutedEventArgs e) => TxtDelDirsPath.Text = "";
        
        private void AddToBlackList_Click(object sender, RoutedEventArgs e)
        {
            CommonFileDialog folderDialog = FileDialogs.OpenFolderDialog();
            if (folderDialog == null) return;
            EmptyFolderBlacklist.Items.Add(folderDialog.FileName);
        }

        private void RemoveFromBlackList_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = EmptyFolderBlacklist.SelectedItem;
            if (selectedItem != null) EmptyFolderBlacklist.Items.Remove(selectedItem);
            EmptyFolderBlacklist.SelectedIndex = 0;
        }
        #endregion //DeleteEmptyDirectories

        #region Encrypted Directory
        private void CreateEncryptedDirectory_Click(object sender, RoutedEventArgs e)
        {
            CommonFileDialog dialog = FileDialogs.OpenFolderDialog("Choose Directory...");
            if (dialog != null)
            {
                new EncryptedDirectoryWriter(dialog.FileName, new PasswordWindow().Password);
            }

            //using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            //{
            //    string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //    fbd.SelectedPath = rootPath;
            //    fbd.ShowDialog();

            //    if (fbd.SelectedPath != "" && Directory.Exists(fbd.SelectedPath))
            //    {
            //        PasswordWindow pw = new PasswordWindow();
            //        pw.ShowDialog();
            //        new EncryptedDirectoryWriter(fbd.SelectedPath, pw.Password);
            //    }
            //}
        }

        private void OpenEncryptedDirectory_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "PasswordEncryptedFile (*.pwf)|*.pwf" };
            ofd.ShowDialog();
            EncryptedDirectoryReader fr = new EncryptedDirectoryReader(ofd.FileName);
            fr.SaveAllFiles();
        }
        #endregion //Encrypted Directory
    }
}