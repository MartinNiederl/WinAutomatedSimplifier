using System.Windows.Controls;
using System.Windows.Input;
using WindowsAutomatedSimplifier.Repository.TreeViewWithCheckBoxes;

namespace WindowsAutomatedSimplifier.FileSystem
{
    /// <summary>
    /// Interaktionslogik für SelectedFileOverview.xaml
    /// </summary>
    public partial class SelectedFileOverview : UserControl
    {
        //private readonly TreeViewModel _root;
        public SelectedFileOverview()
        {
            InitializeComponent();

            //_root = Tree.Items[0] as TreeViewModel;

            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo,
            //    (sender, e) =>
            //    {
            //        e.Handled = true;
            //        if (_root != null) _root.IsChecked = false;
            //        Tree.Focus();
            //    }));

            //CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo,
            //    (sender, e) =>
            //    {
            //        e.Handled = true;
            //        if (_root != null) _root.IsChecked = true;
            //        Tree.Focus();
            //    }));

            //Tree.Focus();
        }
    }
}
