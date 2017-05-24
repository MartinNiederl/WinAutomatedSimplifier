// ReSharper disable InconsistentNaming
using System;
using System.IO;
using Microsoft.Win32;
using PasswordSafe.Properties;

namespace PasswordSafe
{
    internal class Utilities : IDisposable
    {
        private static DataBase db;
        public static readonly Settings settings = Settings.Default;

        public static bool DBExists => File.Exists(settings.PWsFilePath);

        public static void OpenDB(string path = "")
        {
            if (path.Length == 0 || !File.Exists(path)) path = settings.PWsFilePath;
            
            db = new DataBase(path);
            //db.ReadEntity();
        }
       
        public static bool CreateNewDB()
        {
            SaveFileDialog sfDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".db",
                Filter = "Database File (.db)|*.db"
            };

            settings.PWsFilePath = sfDialog.FileName;
            return sfDialog.ShowDialog() == true;
        }

        public static bool FindExistingDB()
        {
            OpenFileDialog ofDialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = ".db",
                Filter = "Database File (.db)|*.db"
            };

            settings.PWsFilePath = ofDialog.FileName;
            return ofDialog.ShowDialog() == true;
        }

        public static bool DeleteDB()
        {
            try
            {
                if (DBExists) File.Delete(settings.PWsFilePath);
            }
            catch (Exception) { }

            return !File.Exists(settings.PWsFilePath);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
