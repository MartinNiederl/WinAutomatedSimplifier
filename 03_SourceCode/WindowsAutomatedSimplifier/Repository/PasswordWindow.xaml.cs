using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WindowsAutomatedSimplifier.Repository
{
    /// <summary>
    /// Interaktionslogik für PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
            PasswordBox.Focus();
            ShowDialog();
        }

        public string Password { get; private set; } = "";
        
        private void BtnUse_Click(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;

            if (Password == null || Password.Trim(' ') == "") AutoClosingMessageBox.Show("The password you entered is not allowed", "Invalid Password!", 3000);
            else Close();
        } 

        private void PasswordWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (Password != null && Password.Trim(' ') != "") return;

            AutoClosingMessageBox.Show("The password you entered is not allowed", "Invalid Password!", 3000);
            e.Cancel = true;
        }

        private void PasswordWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) BtnUse_Click(sender, new RoutedEventArgs());
        }
    }
}
