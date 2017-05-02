using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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

            CommonOpenFileDialog dlg = new CommonOpenFileDialog
            {
                Title = "My Title",
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


            if (dlg.ShowDialog() != CommonFileDialogResult.Ok) return;

            Task.Factory.StartNew(() =>
                {
                    string regexStr = "";
                    DateTime? creationDateFrom = new DateTime?();
                    DateTime? creationDateTo = new DateTime?();
                    DateTime? lastChangeFrom = new DateTime?();
                    DateTime? lastChangeTo = new DateTime?();
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

                    return FileSystemLogic.GetFilteredFiles(dlg.FileName, regexStr,
                            creationDateFrom, creationDateTo, lastChangeFrom, lastChangeTo,
                            sizeFrom, sizeTo)
                        .ToList();
                }, TaskCreationOptions.LongRunning)
                .ContinueWith(prev =>
                {
                    //TODO open new Window with selected Files
                    ProgBar.Dispatcher.Invoke(() => ProgBar.Visibility = Visibility.Hidden);
                    Console.WriteLine(prev.Result.Count);
                    _working = false;
                });
        }
    }
}
