using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAutomatedSimplifier.PasswordProtectedFolder
{
    class EncryptedByteFile : ByteFile
    {
        public EncryptedByteFile(string path, string root)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists) return;
            Content = Encryption.EncryptBytes(File.ReadAllBytes(path), "password");
            Length = Content.Length;
            if (file.Length != Length) throw new Exception("Error when reading the file! File sizes are not same!");
            Path = path;
            RelativePath = GetRelativePath(path, root);
            Name = path.Substring(path.LastIndexOf(@"\", StringComparison.Ordinal));
        }
    }
}
