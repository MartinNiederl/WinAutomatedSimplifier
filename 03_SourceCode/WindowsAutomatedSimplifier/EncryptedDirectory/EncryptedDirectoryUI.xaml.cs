using System.Windows;
using System.Windows.Input;
using WindowsAutomatedSimplifier.Repository.TreeViewWithCheckBoxes;

namespace WindowsAutomatedSimplifier.EncryptedDirectory
{
    /// <summary>
    /// Interaktionslogik für EncryptedDirectoryUI.xaml
    /// </summary>
    public partial class EncryptedDirectoryUI : Window
    {
        private readonly TreeViewModel _root;
        public EncryptedDirectoryUI()
        {
            InitializeComponent();

            _root = Tree.Items[0] as TreeViewModel;

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo,
                    (sender, e) =>
                    {
                        e.Handled = true;
                        if (_root != null) _root.IsChecked = false;
                        Tree.Focus();
                    }));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo,
                    (sender, e) =>
                    {
                        e.Handled = true;
                        if (_root != null) _root.IsChecked = true;
                        Tree.Focus();
                    }));

            Tree.Focus();
        }

        private void BtnDecrypt_OnClick(object sender, RoutedEventArgs e)
        {
            var checkedItems = TreeViewModel.GetCheckedItems(_root);

            foreach (TreeViewModel tvm in checkedItems)
            {
                Header h = tvm.FileInfo;
                EncryptedDirectoryReader.Instance.SaveFileByPosition(h.Position, h.Length, h.Filename);
            }
        }

        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var checkedItems = TreeViewModel.GetCheckedItems(_root);


            throw new System.NotImplementedException();
        }
    }
}
