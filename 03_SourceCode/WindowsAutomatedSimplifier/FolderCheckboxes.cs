using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsAutomatedSimplifier
{
    public static class FolderCheckboxes
    {
        /*
          <CheckBox x:Name="checkBox" Content="Enable Checkboxes" HorizontalAlignment="Left" Margin="208,18,0,0" Grid.Row="1" VerticalAlignment="Top" Checked="checkBox_Checked" Unchecked="checkBox_Unchecked"/>
        */

        //Enables the Checkboxes in the windows explorer

        public static void checkBox_Checked()
        {
            Console.WriteLine("Checkboxes: enabled");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AutoCheckSelect", 1, RegistryValueKind.DWord);
        }

        public static void checkBox_Unchecked()
        {
            Console.WriteLine("Checkboxes: disabled");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "AutoCheckSelect", 0, RegistryValueKind.DWord);
        }

    }
}
