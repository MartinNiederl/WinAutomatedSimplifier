using System;
using System.Collections.Generic;
using System.IO;
using WindowsAutomatedSimplifier.Repository;

namespace WindowsAutomatedSimplifier.EncryptedDirectory
{
    internal class EncryptedDirectoryReader
    {
        public static EncryptedDirectoryReader Instance { get; set; }
        public static string FilePath { get; set; }
        public static List<Header> Headers { get; } = new List<Header>();
        private readonly int _headerlength = 14 + Environment.NewLine.Length;

        private readonly PasswordWindow _pw;

        public EncryptedDirectoryReader(string filePath)
        {
            //TODO Add invalid password support
            _pw = new PasswordWindow();

            if (string.IsNullOrEmpty(filePath)) return;

            FilePath = filePath;
            foreach (string line in File.ReadLines(FilePath))
            {
                if (line.StartsWith("--header_end--")) break;
                _headerlength += line.Length + Environment.NewLine.Length;
                string[] buf = line.Split('?');
                Headers.Add(new Header(buf[0], buf[1], buf[2]));
            }
            Instance = this;
            new EncryptedDirectoryUI().ShowDialog();
        }

        public byte[] ReadFileByIndex(int index)
        {
            byte[] file = ReadFromPosition(Headers[index].Position, Headers[index].Length);
            return Encryption.DecryptBytes(file, _pw.Password);
        }

        private byte[] ReadFromPosition(int position, int length)
        {
            byte[] data = new byte[length];
            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                fs.Position = position + _headerlength;
                int actualRead = 0;
                do
                {
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
                string path = Path.GetDirectoryName(FilePath) + "\\" + Path.GetFileNameWithoutExtension(FilePath);
                Directory.CreateDirectory(path);
                //TODO relativen Pfad für Unterverzeichnisse hinzufügen
                File.WriteAllBytes(path + Headers[index].Filename, ReadFileByIndex(index));
            }
            catch (Exception) { return false; }
            return true;
        }

        public bool SaveFileByPosition(int position, int lenght, string filename)
        {
            try
            {
                string path = Path.GetDirectoryName(FilePath) + "\\" + Path.GetFileNameWithoutExtension(FilePath);
                Directory.CreateDirectory(path);

                byte[] file = ReadFromPosition(position, lenght);
                File.WriteAllBytes(path + filename, Encryption.DecryptBytes(file, _pw.Password));
            }
            catch (Exception) { return false; }
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

        public string Path { get; }
        public string Filename { get; }
        public int Position { get; }
        public int Length { get; }
    }
}
