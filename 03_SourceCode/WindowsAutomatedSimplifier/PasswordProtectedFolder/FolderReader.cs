using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsAutomatedSimplifier.PasswordProtectedFolder
{
    internal class FolderReader
    {
        private readonly string _filePath;
        public static List<Header> Headers { get; } = new List<Header>();
        private readonly int _headerlength = 14 + Environment.NewLine.Length;

        public FolderReader(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            _filePath = filePath;
            foreach (string line in File.ReadLines(_filePath))
            {
                if (line.StartsWith("--header_end--")) break;
                _headerlength += line.Length + Environment.NewLine.Length;
                string[] buf = line.Split('?');
                Headers.Add(new Header(buf[0], buf[1], buf[2]));
            }

            new PasswordProtectedFolder().ShowDialog();
        }
        
        public byte[] ReadFileByIndex(int index)
        {
            byte[] file = ReadFromPosition(Headers[index].Position, Headers[index].Length);
            return Encryption.DecryptBytes(file, "password");
        }

        private byte[] ReadFromPosition(int position, int length)
        {
            byte[] data = new byte[length];
            using (FileStream fs = new FileStream(_filePath, FileMode.Open))
            {
                fs.Position = position + _headerlength;
                int actualRead = 0;
                do {
                    actualRead += fs.Read(data, actualRead, length - actualRead);
                } while (actualRead != length && fs.Position < fs.Length);
            }
            return data;
        }

        public void SaveAllFiles()
        {
            for (int i = 0; SaveFileByIndex(i); i++) ;
        }

        public bool SaveFileByIndex(int index)
        {
            try
            {
                //TODO überarbeiten - Geschwindigkeit optimieren indem nur einmal aufgerufen!
                string path = Path.GetDirectoryName(_filePath) + "\\" + Path.GetFileNameWithoutExtension(_filePath);
                Directory.CreateDirectory(path);
                //TODO relativen Pfad für Unterverzeichnisse hinzufügen
                File.WriteAllBytes(path + Headers[index].Filename, ReadFileByIndex(index));
            }
            catch (Exception) {
                return false;
            }
            return true;
        }
    }

    public struct Header
    {
        public Header(string path, string length, string position)
        {
            Path = path;
            Filename = path.Substring(path.LastIndexOf(@"\", StringComparison.Ordinal));
            Length = int.Parse(length);
            Position = int.Parse(position);
        }

        public string Path { get;}
        public string Filename { get; }
        public int Position { get; }
        public int Length { get; }
    }
}
