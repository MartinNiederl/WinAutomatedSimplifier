using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WindowsAutomatedSimplifier.Repository;
using Microsoft.Win32;
using SharpCompress.Common;

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

            var fileInfos = new List<FileInfo>();
            fileInfos.AddRange(ofDialog.FileNames.Select(filename => new FileInfo(filename)));

            _archive = new Archive(fileInfos);

            return true;
        }

        private void cob_archive_SelectionChanged(object sender, SelectionChangedEventArgs e) => CobCompress.ItemsSource = _compTypeList[CobArchive.SelectedIndex].List;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ArchiveType archiveType;
            if (!Enum.TryParse(CobArchive.SelectedItem.ToString(), out archiveType)) return;

            CompressionType compressionType;
            if (!Enum.TryParse(CobCompress.SelectedItem.ToString(), out compressionType)) return;

            string path = Path.Text, fileName = Filename.Text;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            fileName = fileName.IndexOf(".", StringComparison.Ordinal) < 0 ? fileName : fileName.Remove(fileName.IndexOf(".", StringComparison.Ordinal));

            if (OpenFileDialog()) _archive.Compress(archiveType, compressionType, path + "//" + fileName);
        }
    }
}
