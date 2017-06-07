using System.Windows;

namespace WindowsAutomatedSimplifier.FileSystem
{
    /// <summary>
    /// Interaktionslogik für FileSystemMainWindow.xaml
    /// </summary>
    public partial class FileSystemMainWindow
    {
        public static FileSystemMainWindow Instance { get; private set; }
        public FileSystemMainWindow()
        {
            InitializeComponent();
            Instance = this;
            PTControl.ShowPage(new SearchFilter());
        }
    }
}
