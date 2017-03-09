using System.Windows;
using System.Windows.Controls;
using WindowsAutomatedSimplifier.Repository;

namespace WindowsAutomatedSimplifier.IconSpacing
{
    public partial class IconSpacingWindow
    {
        private readonly IconSpacingLogic _isLogic;
        private static double _hdef, _vdef;
        public IconSpacingWindow()
        {
            InitializeComponent();
            _isLogic = new IconSpacingLogic();
            _hdef = HSlider.Value = _isLogic.GetHorizontalSpacing() * -1;
            _vdef = VSlider.Value = _isLogic.GetVerticalSpacing() * -1;

            WindowManager.AddWindow(this);
        }

        private void ApplyChangesBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _isLogic.EditSpacing((int) HSlider.Value * -1, (int) VSlider.Value * -1);
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
                    HSlider.Value = _isLogic.GetHorizontalSpacing() * -1;
                    VSlider.Value = _isLogic.GetVerticalSpacing() * -1;
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
