using Microsoft.Win32;

namespace WindowsAutomatedSimplifier
{
    public static class ShortcutExtension
    {
        public static void DisableShortcutExtension() => Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "link", new byte[] { 00, 00, 00, 00 }, RegistryValueKind.Binary);

        public static void EnableShortcutExtension()
        {
            using (RegistryKey baseKey = Registry.CurrentUser)
                baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\", true)?.DeleteValue("link");
        }
    }
}
