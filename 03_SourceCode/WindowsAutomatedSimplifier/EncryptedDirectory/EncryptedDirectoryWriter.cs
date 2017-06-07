using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAutomatedSimplifier.FileSystem;

namespace WindowsAutomatedSimplifier.EncryptedDirectory
{
    internal class EncryptedDirectoryWriter
    {
        private readonly List<EncryptedFile> _files = new List<EncryptedFile>();
        public EncryptedDirectoryWriter(string directory, string password)
        {
            Parallel.ForEach(FileSystemLogic.IterateThroughFiles(directory),
                file => _files.Add(new EncryptedFile(file, directory, password)));

            CreateFile($"{directory}.pwf");

            Create(directory, password);
        }

        public async void CreateFile(string path)
        {
            using (Task<byte[]> headerTask = CreateHeaderAsync())
            using (Task<byte[]> filesTask = CombineFilesAsync())
            {
                var allResults = await Task.WhenAll(headerTask, filesTask);
                byte[] header = allResults[0];
                byte[] files = allResults[1];

                byte[] combined = new byte[header.Length + files.Length];
                Buffer.BlockCopy(header, 0, combined, 0, header.Length);
                Buffer.BlockCopy(files, 0, combined, header.Length, files.Length);

                File.WriteAllBytes(path, combined);
            }
        }

        private Task<byte[]> CreateHeaderAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                StringBuilder sb = new StringBuilder();
                int position = 0;
                foreach (EncryptedFile file in _files)
                {
                    sb.Append(file + "?" + position + Environment.NewLine);
                    position += file.Length;
                }
                return Encoding.UTF8.GetBytes(sb + "--header_end--" + Environment.NewLine);
            });
        }

        private Task<byte[]> CombineFilesAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                EncryptedFile[] arrays = _files.ToArray();

                byte[] rv = new byte[arrays.Sum(a => a.Length)];
                int offset = 0;
                foreach (EncryptedFile byteFile in arrays)
                {
                    byte[] array = byteFile.Content;
                    Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                    offset += array.Length;
                }
                return rv;
            });
        }

        private void Create(string directory, string password)
        {
            long position = 0;
            string bodyPath = directory + @"-body.tmp",
                headerPath = directory + @"-header.tmp";

            File.Create(bodyPath).Close();
            File.Create(headerPath).Close();

            using (var body = new FileStream(bodyPath, FileMode.Append))
            {
                foreach (string file in FileSystemLogic.IterateThroughFiles(directory))
                {
                    EncryptedFile encrypted = new EncryptedFile(file, directory, password);
                    body.Write(encrypted.Content, 0, encrypted.Length);

                    File.AppendAllText(headerPath, encrypted + "?" + position + Environment.NewLine, Encoding.ASCII);
                    position += encrypted.Length;
                }
            }
            File.AppendAllText(headerPath, "--header_end--" + Environment.NewLine, Encoding.ASCII);

            FileSystemLogic.ConcatFiles(new[] { headerPath, bodyPath }, directory + " allesklar.pwf");

            File.Delete(bodyPath);
            File.Delete(headerPath);
        }

        public void Print()
        {
            foreach (EncryptedFile file in _files)
                Console.WriteLine(@"RelPath: " + file.RelativePath + @" Length: " + file.Length);
        }
    }
}
