using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowsAutomatedSimplifier.FileSystem
{
    /// <summary>
    /// Interaktionslogik für SearchFilter.xaml
    /// </summary>
    public partial class SearchFilter : UserControl
    {
        public SearchFilter()
        {
            InitializeComponent();
            CSizeFrom.ItemsSource = Enum.GetValues(typeof(ByteSize)).Cast<ByteSize>();
            CSizeFrom.SelectedIndex = 1;
            CSizeTo.ItemsSource = Enum.GetValues(typeof(ByteSize)).Cast<ByteSize>();
            CSizeTo.SelectedIndex = 1;
        }


        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled =  Regex.IsMatch(e.Text, "[^0-9]+");
        }
    }
}
