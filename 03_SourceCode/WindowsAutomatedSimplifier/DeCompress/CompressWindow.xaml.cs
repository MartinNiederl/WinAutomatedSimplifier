using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WindowsAutomatedSimplifier.Repository;
using SharpCompress.Common;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WindowsAutomatedSimplifier.DeCompress
{
    public partial class CompressWindow
    {
        private readonly List<NamedList> _compTypeList = ListManagement.Compress;
        private Archive _archive;
        public CompressWindow()
        {
            InitializeComponent();
            WindowManager.AddWindow(this);

            CobArchive.ItemsSource = _compTypeList;
        }

        private bool OpenFileDialog()
        {
            OpenFileDialog ofDialog = new OpenFileDialog { Multiselect = true };
            if (ofDialog.ShowDialog() == false) return false;

            _archive = new Archive(new List<FileInfo>(ofDialog.FileNames.Select(filename => new FileInfo(filename))));

            return true;
        }

        private void cob_archive_SelectionChanged(object sender, SelectionChangedEventArgs e) => CobCompress.ItemsSource = _compTypeList[CobArchive.SelectedIndex].List;

        private void BtnCompress_Click(object sender, RoutedEventArgs e)
        {
            ArchiveType archiveType;
            if (!Enum.TryParse(CobArchive.SelectedItem.ToString(), out archiveType)) return;

            CompressionType compressionType;
            if (!Enum.TryParse(CobCompress.SelectedItem.ToString(), out compressionType)) return;

            string path = Path.Text, fileName = Filename.Text;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            fileName = fileName.Replace(".", "") + ArchiveTypeToEnding(archiveType);

            if (OpenFileDialog()) Task.Run(() => _archive.Compress(archiveType, compressionType, path + "\\" + fileName));
        }

        private void BtnChoosePath_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fbd.SelectedPath = rootPath;
                fbd.ShowDialog();

                if (fbd.SelectedPath != "" && fbd.SelectedPath != rootPath && Directory.Exists(fbd.SelectedPath)) Path.Text = fbd.SelectedPath;
            }
        }

        public static string ArchiveTypeToEnding(ArchiveType at)
        {
            switch (at)
            {
                case ArchiveType.Rar: return ".rar";
                case ArchiveType.Zip: return ".zip";
                case ArchiveType.Tar: return ".tar";
                case ArchiveType.SevenZip: return ".7z"; 
                case ArchiveType.GZip: return ".gz";
                default: return "";
            }
        }
    }
}
