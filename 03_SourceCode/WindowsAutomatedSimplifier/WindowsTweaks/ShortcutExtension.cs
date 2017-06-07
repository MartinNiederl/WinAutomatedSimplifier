using System;
using Microsoft.Win32;

namespace WindowsAutomatedSimplifier.WindowsTweaks
{
    public static class ShortcutExtension
    {
        private static string explorerPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer";

        public static void EnableShortcutExtension() => Registry.SetValue(explorerPath, "link", 1);

        public static void DisableShortcutExtension() => Registry.SetValue(explorerPath, "link", 0);}
}
