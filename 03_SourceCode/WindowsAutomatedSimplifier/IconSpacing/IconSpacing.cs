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
            Registry.SetValue(KEYPATH, VALUEH, horizontal);
            Registry.SetValue(KEYPATH, VALUEV, vertical);
        }
        public static int GetHorizontalSpacing() => (int)Registry.GetValue(KEYPATH, VALUEH, -1);

        public static int GetVerticalSpacing() => (int)Registry.GetValue(KEYPATH, VALUEV, -1);
    }
}
