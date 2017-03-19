using System.Windows;
using System.Windows.Input;
using WindowsAutomatedSimplifier.Repository.TreeViewWithCheckBoxes;

namespace WindowsAutomatedSimplifier.PasswordProtectedFolder
{
    /// <summary>
    /// Interaktionslogik für PasswordProtectedFolder.xaml
    /// </summary>
    public partial class PasswordProtectedFolder : Window
    {
        public PasswordProtectedFolder()
        {
            InitializeComponent();

            TreeViewModel root = Tree.Items[0] as TreeViewModel;

            CommandBindings.Add(
                new CommandBinding(ApplicationCommands.Undo,
                    (sender, e) =>
                    {
                        e.Handled = true;
                        if (root != null) root.IsChecked = false;
                        Tree.Focus();
                    },
                    (sender, e) =>
                    {
                        e.Handled = true;
                        e.CanExecute = root?.IsChecked != false;
                    }));

            Tree.Focus();
        }
    }
}
