using System;
using System.Windows;
using System.Drawing.Text;
using WindowsAutomatedSimplifier.Repository;
using Microsoft.Win32;
using FontFamily = System.Drawing.FontFamily;

namespace WindowsAutomatedSimplifier.ChangeFont
{
    public partial class FontPicker : Window
    {
        private const string SOURCE_PATH = @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics\";

        public FontPicker()
        {
            InitializeComponent();

            foreach (FontFamily item in new InstalledFontCollection().Families)
                LbFontFamily.Items.Add(item.Name);

            CbFont.SelectedIndex = 0;
        }

        private void BtnUseFont_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = CbFont.SelectedItem.ToString();
            byte[] value = Registry.GetValue(SOURCE_PATH, selectedItem, "") as byte[];
            if (value == null || LbFontFamily.SelectedItem == null)
            {
                AutoClosingMessageBox.Show("No font selected or other error!", "Error occured", 2000);
                return;
            }

            value = GetNewFontBytes(value, LbFontFamily.SelectedItem.ToString());
            Registry.SetValue(SOURCE_PATH, selectedItem, value);

            Close();
        }

        private static byte[] GetNewFontBytes(byte[] byteEntry, string fontname)
        {
            for (int i = 28, j = 0; i < 92; i += 2, j++) {
                if (j < fontname.Length) byteEntry[i] = Convert.ToByte(fontname[j]);
                else byteEntry[i] = 00;
            }
            return byteEntry;
        }
    }
}
