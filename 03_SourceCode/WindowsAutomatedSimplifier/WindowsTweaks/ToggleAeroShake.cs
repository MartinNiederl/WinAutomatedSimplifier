using Microsoft.Win32;

namespace WindowsAutomatedSimplifier.WindowsTweaks
{
    public static class ToggleAeroShake
    {
        private const string KEYPATH = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced";
        private const string VALUE = "DisallowShaking";

        public static void DisableAeroShake() => Registry.SetValue(KEYPATH, VALUE, 1, RegistryValueKind.DWord); 

        public static void EnableAeroShake() => Registry.SetValue(KEYPATH, VALUE, 0, RegistryValueKind.DWord);
    }
}
