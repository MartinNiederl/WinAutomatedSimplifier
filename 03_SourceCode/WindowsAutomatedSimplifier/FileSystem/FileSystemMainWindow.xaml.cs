using System.Windows;

namespace WindowsAutomatedSimplifier.FileSystem
{
    /// <summary>
    /// Interaktionslogik für FileSystemMainWindow.xaml
    /// </summary>
    public partial class FileSystemMainWindow : Window
    {
        public FileSystemMainWindow()
        {
            InitializeComponent();
            PTControl.ShowPage(new SearchFilter());
        }
    }
}
