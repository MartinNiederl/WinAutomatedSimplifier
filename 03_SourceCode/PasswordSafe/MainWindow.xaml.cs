using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        private ObservableCollection<PasswordEntity> ObsColl;
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
                MessageBoxResult result = MessageBox.Show("Open Recent?", $"Open {Utilities.settings.PWsFilePath}?",
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
            ObsColl = new ObservableCollection<PasswordEntity>(list);
            PasswordGrid.ItemsSource = ObsColl;

            Delete.Visibility = Visibility.Visible;
            AddEntry.Visibility = Visibility.Visible;
        }

        private void New_OnClick(object sender, RoutedEventArgs e)
        {
            if (!SavedOpenDB()) return;

            if (Utilities.CreateNewDB()) PopulateGrid(Utilities.DB.ReadEntity());
            else
                MessageBox.Show("Not created!", "No Database created!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteSafe_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Delete Database?", "Are you sure deleting all Passwords?", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            if (Utilities.DeleteDB()) MessageBox.Show("Success...", "Successfully deleted!", MessageBoxType.Information);
        }

        private bool SavedOpenDB()
        {
            if (Utilities.DBisOpen)
            {
                MessageBoxResult result =
                    MessageBox.Show("Closing...", "Close currently open?", MessageBoxButton.YesNo);

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

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordEntity item = GetItem(sender);
            if (item == null) return;
            
            ObsColl.Remove(item);
            Utilities.Delete(item.ID);
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordEntity item = GetItem(sender);
            if (item == null) return;

            AddEntry window = new AddEntry(item);
            PasswordEntity input = window.PeInput;
            if (input == null)
            {
                MessageBox.Show("Nothing supplied...", "No new Data supplied!", MessageBoxType.Warning);
                return;
            }

            PasswordEntity entity = ObsColl.FirstOrDefault(el => el.Equals(item));
            entity = entity?.Update(input);
            Utilities.Update(entity);
        }

        private void CopyPassword_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordEntity item = GetItem(sender);
            if (item == null) return;

            Clipboard.SetText(item.Password);
        }

        private void CopyUsername_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordEntity item = GetItem(sender);
            if (item == null) return;

            Clipboard.SetText(item.Username);
        }

        private static PasswordEntity GetItem(object sender)
        {
            ContextMenu contextMenu = (ContextMenu)((MenuItem)sender).Parent; //Get the ContextMenu to which the menuItem belongs

            DataGrid item = (DataGrid)contextMenu.PlacementTarget;   //Find the placementTarget
            if (item.SelectedCells.Count < 1) return null;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            return (PasswordEntity)item.SelectedCells[0].Item;
        }
    }
}
