using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsAutomatedSimplifier
{
    public static class TaskbarPreviewWindow
    {

        /*
        <Button x:Name="button" Content="Apply" HorizontalAlignment="Left" Margin="159,24,0,0" VerticalAlignment="Top" Width="91" Click="WindowSize"/>
        <Button x:Name="button_Copy" Content="Restore" HorizontalAlignment="Left" Margin="270,24,0,0" VerticalAlignment="Top" Width="75" Click="RestoreWindowSize"/>
        <Slider x:Name="slider02" HorizontalAlignment="Left" Margin="159,0,0,0" VerticalAlignment="Top" Width="101" Maximum="500" Value="250" Background="Transparent" SmallChange="1" IsSnapToTickEnabled="True" Grid.Row="1"/>
        */

        public static void WindowSize(int inputSlider)
        {
            Console.WriteLine("TaskbarWindowPreview: Applied");
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband", "MinThumbSizePx", (int)inputSlider, RegistryValueKind.DWord);
        }

        public static void RestoreWindowSize()
        {
            Console.WriteLine("TaskbarWindowPreview: reset");
            RegistryKey baseKey = Registry.CurrentUser;
            RegistryKey subKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Taskband", true);
            if (subKey.GetValue("MinThumbSizePx") != null)
            {
                subKey.DeleteValue("MinThumbSizePx");
            }
            baseKey.Close();
        }
    }
}
