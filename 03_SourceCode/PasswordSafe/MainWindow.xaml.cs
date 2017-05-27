using System;
using System.Collections.Generic;
using System.Windows;
using CustomMessageBox;
using Microsoft.Win32;
using PasswordSafe.Properties;
using MessageBox = CustomMessageBox.MessageBox;
using MessageBoxImage = CustomMessageBox.MessageBoxImage;

namespace PasswordSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TestDB();
        }

        private void TestDB()
        {
            string password = "highlysafe123pass";
            string encrypted = Crypting.EncryptString("Alles klar im Busch?", password);
            Console.WriteLine("Encrypted: " + encrypted);
            string decrypted = Crypting.DecryptString(encrypted, password);
            Console.WriteLine("Decrypted: " + decrypted);


            //using (DataBase db = new DataBase("Passwords.db"))
            //{
            //    db.CreateTable();
            //    db.Read();
            //}
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SavedOpenDB()) return;

            if (Utilities.DBExists)
            {
                MessageBoxResult result = CustomMessageBox.MessageBox.Show("Open Recent?", $"Open {Utilities.settings.PWsFilePath}?",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    Utilities.OpenDB(RequestMasterPassword());
                    PopulateGrid(Utilities.DB.ReadEntity());
                    return;
                }
            }

            if (FindDBFileDialog())
            {
                Utilities.OpenDB(RequestMasterPassword());
                PopulateGrid(Utilities.DB.ReadEntity());
            }
        }

        private static string RequestMasterPassword() => new MessageBox().ShowInput("Enter password...", "Enter the password for your safe.");

        private void PopulateGrid(IEnumerable<PasswordEntity> list)
        {
            PasswordGrid.ItemsSource = list;

            Delete.Visibility = Visibility.Visible;
            AddEntry.Visibility = Visibility.Visible;
        }

        private void New_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SavedOpenDB()) return;

            if (Utilities.CreateNewDB()) PopulateGrid(Utilities.DB.ReadEntity());
            else
                CustomMessageBox.MessageBox.Show("Not created!", "No Database created!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (CustomMessageBox.MessageBox.Show("Delete Database?", "Are you sure deleting all Passwords?", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            if (Utilities.DeleteDB()) CustomMessageBox.MessageBox.Show("Success...", "Successfully deleted!", MessageBoxType.Information);
        }

        private bool SavedOpenDB()
        {
            if (Utilities.DBisOpen)
            {
                MessageBoxResult result =
                    CustomMessageBox.MessageBox.Show("Closing...", "Close currently open?", MessageBoxButton.YesNo);

                if (result != MessageBoxResult.Yes) return false;

                Delete.Visibility = Visibility.Collapsed;
                Utilities.Dispose();
            }
            return true;
        }

        private void AddEntry_OnClick(object sender, RoutedEventArgs e) => new AddEntry().ShowDialog();

        public static bool FindDBFileDialog()
        {
            OpenFileDialog ofDialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = ".db",
                Filter = "Database File (.db)|*.db"
            };

            if (ofDialog.ShowDialog() != true) return false;
            Settings.Default.PWsFilePath = ofDialog.FileName;
            return true;
        }
    }
}
