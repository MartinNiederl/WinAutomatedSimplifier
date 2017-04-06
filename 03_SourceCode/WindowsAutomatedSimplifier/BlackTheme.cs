using Microsoft.Win32;

namespace WindowsAutomatedSimplifier
{
    public static class BlackTheme
    {
        private const string KEYPATH = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string VALUE = "AppsUseLightTheme";

        public static void EnableBlackTheme() => Registry.SetValue(KEYPATH, VALUE, 0, RegistryValueKind.DWord);

        public static void DisableBlackTheme() => Registry.SetValue(KEYPATH, VALUE, 1, RegistryValueKind.DWord);
    }
}
