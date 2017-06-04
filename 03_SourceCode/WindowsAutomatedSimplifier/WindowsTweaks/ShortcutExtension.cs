using Microsoft.Win32;

namespace WindowsAutomatedSimplifier.WindowsTweaks
{
    public static class ShortcutExtension
    {
        public static void DisableShortcutExtension() => Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "link", new byte[] { 00, 00, 00, 00 }, RegistryValueKind.Binary);

        public static void EnableShortcutExtension() //by deleting link
        {
                Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer", "link", new byte[] { 19, 00, 00, 00 });
        }
    }
}
