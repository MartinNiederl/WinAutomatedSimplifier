using System;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WindowsAutomatedSimplifier.Repository
{
    internal class FileDialogs
    {
        public static CommonFileDialog OpenFolderDialog(string title = "Choose root directory...", bool multiselect = false, string rootDirectory = "")
        {
            CommonOpenFileDialog dlg = new CommonOpenFileDialog
            {
                Title = title,
                IsFolderPicker = true,
                InitialDirectory = rootDirectory == "" ? Environment.SpecialFolder.MyDocuments.ToString() : rootDirectory,
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = rootDirectory == "" ? Environment.SpecialFolder.MyDocuments.ToString() : rootDirectory,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = multiselect,
                ShowPlacesList = true
            };

            return dlg.ShowDialog() != CommonFileDialogResult.Ok ? null : dlg;
        }
    }
}
