using System.Windows;

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
            using (DataBase db = new DataBase("Passwords.db"))
            {
                db.CreateTable();
                db.Read();
            }
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            if (Utilities.DBExists)
            {
                MessageBoxResult result = CustomMessageBox.MessageBox.Show("Open Recent?", $"Open {Utilities.settings.PWsFilePath}?",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    Utilities.OpenDB();
            }
            throw new System.NotImplementedException();
        }

        private void New_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Delete_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
