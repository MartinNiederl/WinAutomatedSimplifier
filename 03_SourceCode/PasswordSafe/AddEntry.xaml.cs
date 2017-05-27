using System;
using System.Windows;

namespace PasswordSafe
{
    /// <summary>
    /// Interaction logic for AddEntry.xaml
    /// </summary>
    public partial class AddEntry : Window
    {
        public AddEntry() => InitializeComponent();

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            TxtUsername.Text = "";
            TxtPassword.Text = "";
            TxtURL.Text = "";
            TxtNotes.Text = "";
        }

        private void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            Utilities.AddEntry(new PasswordEntity(TxtUsername.Text, TxtPassword.Text, TxtURL.Text, TxtNotes.Text));
        }
    }
}
