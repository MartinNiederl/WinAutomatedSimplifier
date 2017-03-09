using Microsoft.Win32;

namespace WindowsAutomatedSimplifier.IconSpacing
{
    internal class IconSpacingLogic
    {
        private readonly RegistryKey _windowMetrics;
        public IconSpacingLogic()
        {
            _windowMetrics = Registry.CurrentUser.OpenSubKey("Control Panel")?.OpenSubKey("Desktop")?.OpenSubKey("WindowMetrics", true);
        }

        /// <summary>
        /// Edit desktop icon spacing.
        /// </summary>
        /// <param name="horizontal">Value for horizontal spacing</param>
        /// <param name="vertical">Value for vertical spacing</param>
        public void EditSpacing(int horizontal, int vertical)
        {
            _windowMetrics?.SetValue("IconSpacing", horizontal, RegistryValueKind.String);
            _windowMetrics?.SetValue("IconVerticalSpacing", vertical, RegistryValueKind.String);
        }
        public int GetHorizontalSpacing() => int.Parse(_windowMetrics?.GetValue("IconSpacing").ToString());
        public int GetVerticalSpacing() => int.Parse(_windowMetrics?.GetValue("IconVerticalSpacing").ToString());
    }
}
