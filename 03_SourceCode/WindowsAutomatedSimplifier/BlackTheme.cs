using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsAutomatedSimplifier
{
    public static class BlackTheme
    {
        //Enabled
        public static void checkBox_Checked()
        {
            Console.WriteLine("Dark Theme: enabled");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0, RegistryValueKind.DWord);
        }
        //Disabled
        public static void checkBox_Unchecked()
        {
            Console.WriteLine("Dark Theme: disabled");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1, RegistryValueKind.DWord);
        }
    }
}
