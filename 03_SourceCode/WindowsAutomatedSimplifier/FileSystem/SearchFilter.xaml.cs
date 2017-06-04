using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Input;
using System.Windows.Threading;
using WindowsAutomatedSimplifier.Repository;
using Application = System.Windows.Application;
using UserControl = System.Windows.Controls.UserControl;

namespace WindowsAutomatedSimplifier.FileSystem
{
    /// <summary>
    /// Interaktionslogik für SearchFilter.xaml
    /// </summary>
    public partial class SearchFilter : UserControl
    {
        private static bool _working;
        public SearchFilter()
        {
            InitializeComponent();
            CSizeFrom.ItemsSource = Enum.GetValues(typeof(ByteSizeEnum)).Cast<ByteSizeEnum>();
            CSizeFrom.SelectedIndex = 1;
            CSizeTo.ItemsSource = Enum.GetValues(typeof(ByteSizeEnum)).Cast<ByteSizeEnum>();
            CSizeTo.SelectedIndex = 1;
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");

        private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
        {
            if (_working) return;
            _working = true;

            string rootPath = "";
            if (!Directory.Exists(TxtRootPath.Text))
            {
                CommonFileDialog dialog = OpenFileDialog("Please choose root path again, previous was invalid...");
                if (dialog == null) return;
                rootPath = dialog.FileName;
            }

            Task.Factory.StartNew(() =>
                {
                    string regexStr = "";
                    var creationDateFrom = new DateTime?();
                    var creationDateTo = new DateTime?();
                    var lastChangeFrom = new DateTime?();
                    var lastChangeTo = new DateTime?();
                    string sizeFrom = "";
                    string sizeTo = "";

                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        ProgBar.Visibility = Visibility.Visible;
                        regexStr = TxtRegEx.Text;
                        creationDateFrom = CreationDateFrom.SelectedDate;
                        creationDateTo = CreationDateTo.SelectedDate;
                        lastChangeFrom = LastChangeFrom.SelectedDate;
                        lastChangeTo = LastChangeTo.SelectedDate;
                        sizeFrom = SizeFrom.Text + CSizeFrom.Text;
                        sizeTo = SizeTo.Text + CSizeTo.Text;
                    }));

                    return FileSystemLogic.GetFilteredFiles(rootPath, regexStr, creationDateFrom, creationDateTo, lastChangeFrom, lastChangeTo, sizeFrom, sizeTo).ToList();
                }, TaskCreationOptions.LongRunning)
                .ContinueWith(prev =>
                {
                    ProgBar.Dispatcher.Invoke(() => ProgBar.Visibility = Visibility.Hidden);
                    FileSystemMainWindow.Instance.PTControl.Dispatcher.Invoke(() => FileSystemMainWindow.Instance.PTControl.ShowPage(new SelectedFileOverview()));
                    _working = false;
                });
        }


        private void TxtRootPath_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CommonFileDialog dialog = OpenFileDialog();
            if (dialog == null) return;
            TxtRootPath.Text = dialog.FileName;
        }

        private static CommonFileDialog OpenFileDialog(string title = "Choose root directory...")
        {
            CommonOpenFileDialog dlg = new CommonOpenFileDialog
            {
                Title = title,
                IsFolderPicker = true,
                InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString(),
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = Environment.SpecialFolder.MyDocuments.ToString(),
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            return dlg.ShowDialog() != CommonFileDialogResult.Ok ? null : dlg;
        }
    }
}
