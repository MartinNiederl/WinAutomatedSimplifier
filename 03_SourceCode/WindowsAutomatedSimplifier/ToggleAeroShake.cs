using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsAutomatedSimplifier
{
    public static class ToggleAeroShake
    {
        /*
          <CheckBox x:Name="checkBox" Content="Disable Aero Shake" HorizontalAlignment="Left" Margin="208,18,0,0" Grid.Row="1" VerticalAlignment="Top" Checked="checkBox_Checked" Unchecked="checkBox_Unchecked"/>
        */

        public static void checkBox_Unchecked()
        {
            Console.WriteLine("Aero Shake: disabled");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1, RegistryValueKind.DWord);
        }

        public static void checkBox_Checked()
        {
            Console.WriteLine("Aero Shake: enabled");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 0, RegistryValueKind.DWord);
        }
    }
}
