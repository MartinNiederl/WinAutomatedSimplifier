using System;
using Microsoft.Win32;

namespace WindowsAutomatedSimplifier.IconSpacing
{
    internal class IconSpacing
    {
        private const string KEYPATH = @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics";
        private const string VALUEH = "IconSpacing";
        private const string VALUEV = "IconVerticalSpacing";

        /// <summary>
        /// Edit desktop icon spacing.
        /// </summary>
        /// <param name="horizontal">Value for horizontal spacing</param>
        /// <param name="vertical">Value for vertical spacing</param>
        public static void EditSpacing(int horizontal, int vertical)
        {
            Console.WriteLine("Horizontal: " + horizontal);
            Registry.SetValue(KEYPATH, VALUEH, horizontal, RegistryValueKind.String);
            Registry.SetValue(KEYPATH, VALUEV, vertical, RegistryValueKind.String);
        }
        public static int GetHorizontalSpacing()
        {
            int.TryParse(Registry.GetValue(KEYPATH, VALUEH, -1).ToString(), out int retVal);
            return retVal;
        }

        public static int GetVerticalSpacing()
        {
            int.TryParse(Registry.GetValue(KEYPATH, VALUEV, -1).ToString(), out int retVal);
            return retVal;
        }
    }
}
