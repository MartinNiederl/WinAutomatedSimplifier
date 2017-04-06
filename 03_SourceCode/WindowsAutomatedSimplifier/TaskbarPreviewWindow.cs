using Microsoft.Win32;

namespace WindowsAutomatedSimplifier
{
    public static class TaskbarPreviewWindow
    {
        private const string KEYPATH = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband";
        private const string MINTHUMBSIZE = "MinThumbSizePx";
        private const string NUMTHUMBS = "NumThumbnails";

        public static void SetWindowSize(int inputSlider) => Registry.SetValue(KEYPATH, MINTHUMBSIZE, inputSlider, RegistryValueKind.DWord);

        public static int GetWindowSize() => (int)Registry.GetValue(KEYPATH, MINTHUMBSIZE, -1);

        public static void RestoreWindowSize()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(KEYPATH, true))
                if (key?.GetValue(MINTHUMBSIZE) != null) key.DeleteValue(MINTHUMBSIZE);
        }

        //TODO add value input
        //TODO correct calls with values
        public static void SetThumbCount(int inputSlider) => Registry.SetValue(KEYPATH, NUMTHUMBS, inputSlider, RegistryValueKind.DWord);

        public static int GetThumbCount() => (int)Registry.GetValue(KEYPATH, MINTHUMBSIZE, -1);

        public static void RestoreThumbCount()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(KEYPATH, true))
                if (key?.GetValue(MINTHUMBSIZE) != null) key.DeleteValue(MINTHUMBSIZE);
        }
    }
}