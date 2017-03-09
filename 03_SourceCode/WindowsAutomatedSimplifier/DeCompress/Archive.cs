using System.Collections.Generic;
using System.IO;
using WindowsAutomatedSimplifier.Repository;
using SharpCompress.Archive;
using SharpCompress.Common;
using SharpCompress.Writer;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WindowsAutomatedSimplifier.DeCompress
{
    public class Archive
    {
        private List<FileInfo> FileList { get; } = new List<FileInfo>();

        /// <summary>
        /// Create new Archive with existing list of files.
        /// </summary>
        /// <param name="fileList">Filelist for compressing or decompressing (only one file).</param>
        public Archive(List<FileInfo> fileList)
        {
            FileList = fileList;
        }

        /// <summary>
        /// Create new Archive and let the user choose the files.
        /// </summary>
        public Archive()
        {
            //Select files for compressing
            OpenFileDialog ofDialog = new OpenFileDialog { Multiselect = true };
            if (ofDialog.ShowDialog() == false) return;

            foreach (string filename in ofDialog.FileNames)
                FileList.Add(new FileInfo(filename));

            CompressWindow dc = new CompressWindow();
            dc.Show();
            WindowManager.CloseFirstOpenedWindow();
        }

        /// <summary>
        /// Compress previous selected files and save to specific path.
        /// </summary>
        /// <param name="archive">Type of the archive which gets created</param>
        /// <param name="compression">Type of the compression algorithm used for archiving</param>
        /// <param name="filepath">Path where the compressed file gets stored</param>
        public void Compress(ArchiveType archive, CompressionType compression, string filepath)
        {
            using (FileStream zip = File.OpenWrite(filepath + archive)) //change path
            using (IWriter zipWriter = WriterFactory.Open(zip, archive, compression))
                foreach (FileInfo file in FileList)
                    zipWriter.Write(file.Name, file);
        }

        /// <summary>
        /// Decompress previous selected archive file and save to specific folder.
        /// </summary>
        /// <param name="path">Path where the decompressed files get stored</param>
        public void Decompress(string path)
        {
            IArchive archive = ArchiveFactory.Open(FileList[0].FullName);
            foreach (IArchiveEntry entry in archive.Entries)
                if (!entry.IsDirectory) entry.WriteToDirectory(path, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
        }
    }
}
