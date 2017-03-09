using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAutomatedSimplifier.PasswordProtectedFolder
{
    internal class ProtectedFolder
    {
        private readonly List<ByteFile> _files = new List<ByteFile>();
        public ProtectedFolder(string directory, string password)
        {
            DirectoryInfo di = new DirectoryInfo(directory);

            foreach (FileInfo fileInfo in di.GetFiles("*", SearchOption.AllDirectories))
                try { _files.Add(new ByteFile(fileInfo.FullName, directory, password)); }
                catch { }

            CreateFile(directory + ".pwf");
        }
        public async void CreateFile(string path)
        {
            Task<byte[]> headerTask = CreateHeaderAsync();
            Task<byte[]> filesTask = CombineFilesAsync();

            var allResults = await Task.WhenAll(headerTask, filesTask);
            byte[] header = allResults[0];
            byte[] files = allResults[1];

            byte[] combined = new byte[header.Length + files.Length];
            Buffer.BlockCopy(header, 0, combined, 0, header.Length);
            Buffer.BlockCopy(files, 0, combined, header.Length, files.Length);

            await Task.Run(delegate { File.WriteAllBytes(path, combined); });
        }

        private Task<byte[]> CreateHeaderAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                StringBuilder sb = new StringBuilder();
                int position = 0;
                foreach (ByteFile file in _files)
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
                ByteFile[] arrays = _files.ToArray();

                byte[] rv = new byte[arrays.Sum(a => a.Length)];
                int offset = 0;
                foreach (ByteFile byteFile in arrays)
                {
                    byte[] array = byteFile.Content;
                    Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                    offset += array.Length;
                }
                return rv;
            });
        }

        public void Print()
        {
            foreach (ByteFile file in _files)
                Console.WriteLine(@"RelPath: " + file.RelativePath + @" Length: " + file.Length);
        }
    }
}
