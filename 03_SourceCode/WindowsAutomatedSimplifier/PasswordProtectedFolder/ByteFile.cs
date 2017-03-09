using System;
using System.IO;

namespace WindowsAutomatedSimplifier.PasswordProtectedFolder
{
    internal class ByteFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string RelativePath { get; set; }
        public byte[] Content { get; set; }
        public int Length { get; set; }

        public ByteFile(string path, string root)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists) return;
            Content = File.ReadAllBytes(path);
            Length = Content.Length;
            if (file.Length != Length) throw new Exception("Error when reading the file! File sizes are not same!");
            Path = path;
            RelativePath = GetRelativePath(path, root);
            Name = path.Substring(path.LastIndexOf(@"\", StringComparison.Ordinal));
        }

        public ByteFile(string path, string root, string password)
        {
            FileInfo file = new FileInfo(path);
            if (!file.Exists) return;
            Content = Encryption.EncryptBytes(File.ReadAllBytes(path), password);
            Length = Content.Length;
            //if (file.Length != Length) throw new Exception("Error when reading the file! File sizes are not same!");
            Path = path;
            RelativePath = GetRelativePath(path, root);
            Name = path.Substring(path.LastIndexOf(@"\", StringComparison.Ordinal));
        }
        public static string GetRelativePath(string path, string root) => path.Substring(root.Length);

        public override string ToString() => RelativePath + "?" + Length;
    }
}
