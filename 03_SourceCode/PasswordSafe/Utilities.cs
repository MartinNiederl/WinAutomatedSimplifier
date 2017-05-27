// ReSharper disable InconsistentNaming
using System;
using System.IO;
using System.Windows;
using CustomMessageBox;
using Microsoft.Win32;
using PasswordSafe.Properties;
using MessageBox = CustomMessageBox.MessageBox;

namespace PasswordSafe
{
    internal class Utilities
    {
        private static string masterPassword;
        private const string validationString = "PasswordValidation-TestString";
        public static DataBase DB { get; private set; }
        public static readonly Settings settings = Settings.Default;

        public static bool DBExists => File.Exists(settings.PWsFilePath);

        public static bool DBisOpen => DB != null;


        public static bool CreateNewDB()
        {
            //Choose the new filename and location
            SaveFileDialog sfDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".db",
                Filter = "Database File (.db)|*.db"
            };

            if (sfDialog.ShowDialog() == false) return false;
            settings.PWsFilePath = sfDialog.FileName;   //set the settings path if chosen file

            //Request the master password / match requirements
            string password;
            do
            {
                password = new MessageBox().ShowInput("Enter master password...", "Enter new master password:");

                if (password.Length < 5 || password.Length > 30)
                {
                    MessageBoxResult result = MessageBox.Show("Password invalid!", "Password is too short or too long! Terminate?",
                        MessageBoxType.ConfirmationWithYesNo);

                    if (result != MessageBoxResult.No && result != MessageBoxResult.Cancel) return false;
                }
                else break;
            } while (true);

            //Initialize Database / add encrypted validation string for checking password when logging in next time
            DB = DataBase.GetInstance(settings.PWsFilePath);
            DB.Initialize(Crypting.EncryptString(validationString, password));

            return true;
        }

        public static void OpenDB(string password, string path = "")
        {
            //Check the correctness of the password
            if (!ValidatePassword(password))
            {
                MessageBox.Show("Invalid Password", "The password you entered is not correct!", MessageBoxType.Error);
                return;
            }

            //Use settings filepath if no path given
            if (path.Length == 0 || !File.Exists(path)) path = settings.PWsFilePath;
            if (!File.Exists(path)) return;

            masterPassword = password;
            DB = DataBase.GetInstance(path);
        }

        public static bool DeleteDB()
        {
            try
            {
                if (DBExists)
                {
                    Dispose();
                    File.Delete(settings.PWsFilePath);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Not Deleted...", "Could not delete passwords!", MessageBoxType.Information);
            }

            return !File.Exists(settings.PWsFilePath);
        }

        public static bool ValidatePassword(string password)
        {
            string cipher = DB.GetValidationString();
            if (!validationString.Equals(Crypting.DecryptString(cipher, password))) return false;

            masterPassword = password;
            return true;
        }

        public static void AddEntry(PasswordEntity entity)
        {
            string username = Crypting.EncryptString(entity.Username, masterPassword);
            string password = Crypting.EncryptString(entity.Password, masterPassword);
            string url = Crypting.EncryptString(entity.Url, masterPassword);
            string notes = Crypting.EncryptString(entity.Notes, masterPassword);

            int id = DB.InsertEntry(username, password, url, notes);


        }

        public static void Update(int id, string username = null, string password = null, string url = null, string notes = null)
        {
            DB.UpdateEntry(username, password, url, notes, id);
        }

        public static void Delete(int id)
        {
            DB.DeleteEntry(id);
        }

        public static void Dispose()
        {
            DB.Dispose();
            DB = null;
        }
    }
}
