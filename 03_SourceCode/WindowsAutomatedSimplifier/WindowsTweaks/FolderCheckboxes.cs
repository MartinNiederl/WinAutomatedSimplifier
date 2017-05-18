using Microsoft.Win32;

namespace WindowsAutomatedSimplifier.WindowsTweaks
{
    public static class FolderCheckboxes
    {
        private const string KEYPATH = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string VALUE = "AutoCheckSelect";

        public static void EnableCheckboxes() => Registry.SetValue(KEYPATH, VALUE, 1, RegistryValueKind.DWord);

        public static void DisableCheckboxes() => Registry.SetValue(KEYPATH, VALUE, 0, RegistryValueKind.DWord);
    }
}
