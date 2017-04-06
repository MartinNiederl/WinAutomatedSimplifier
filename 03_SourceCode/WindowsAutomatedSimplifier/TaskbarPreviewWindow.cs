using Microsoft.Win32;

namespace WindowsAutomatedSimplifier
{
    public static class TaskbarPreviewWindow
    {
        private const string KEYPATH = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband";
        private const string VALUE = "MinThumbSizePx";

        public static void SetWindowSize(int inputSlider) => Registry.SetValue(KEYPATH, VALUE, inputSlider, RegistryValueKind.DWord);

        public static int GetWindowSize() => (int)Registry.GetValue(KEYPATH, VALUE, -1);

        public static void RestoreWindowSize()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(KEYPATH, true))
                if (key?.GetValue(VALUE) != null) key.DeleteValue(VALUE);
        }
    }
}