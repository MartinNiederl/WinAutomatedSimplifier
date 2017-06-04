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

        public AddEntry(string username, string password, string url, string notes) : this()
        {
            TxtUsername.Text = username;
            TxtPassword.Text = password;
            TxtURL.Text = url;
            TxtNotes.Text = notes;
        }

        public AddEntry(PasswordEntity entity) : this(entity.Username, entity.Password, entity.Url, entity.Notes) { }
    
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
            PeInput = new PasswordEntity(TxtUsername.Text, TxtPassword.Text, TxtURL.Text, TxtNotes.Text);
        }

        public PasswordEntity PeInput { get; private set; }
    }
}
