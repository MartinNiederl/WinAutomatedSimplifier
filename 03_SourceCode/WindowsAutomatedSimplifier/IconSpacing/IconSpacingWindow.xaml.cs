using System.Windows;
using System.Windows.Controls;
using WindowsAutomatedSimplifier.Repository;

namespace WindowsAutomatedSimplifier.IconSpacing
{
    public partial class IconSpacingWindow
    {
        private static double _hdef, _vdef;
        public IconSpacingWindow()
        {
            InitializeComponent();

            _hdef = HSlider.Value = IconSpacing.GetHorizontalSpacing() * -1;
            _vdef = VSlider.Value = IconSpacing.GetVerticalSpacing() * -1;

            WindowManager.AddWindow(this);
        }

        private void ApplyChangesBtn_OnClick(object sender, RoutedEventArgs e)
        {
            IconSpacing.EditSpacing((int) HSlider.Value * -1, (int) VSlider.Value * -1);
            Close();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem)?.Name)
            {
                case "Default":
                    HSlider.Value = 1710;
                    VSlider.Value = 1125;
                    break;
                case "Reset":
                    HSlider.Value = IconSpacing.GetHorizontalSpacing() * -1;
                    VSlider.Value = IconSpacing.GetVerticalSpacing() * -1;
                    break;
            }
        }

        private void ResetSliderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            HSlider.Value = _hdef;
            VSlider.Value = _vdef;
        }
    }
}
