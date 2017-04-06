using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WindowsAutomatedSimplifier.Repository.TreeViewWithCheckBoxes
{
    public static class VirtualToggleButton
    {
        #region attached properties

        #region IsChecked

        /// <summary>
        /// IsChecked Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.RegisterAttached("IsChecked", typeof(bool?), typeof(VirtualToggleButton),
                new FrameworkPropertyMetadata((bool?)false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnIsCheckedChanged));

        /// <summary>
        /// Gets the IsChecked property.  This dependency property 
        /// indicates whether the toggle button is checked.
        /// </summary>
        public static bool? GetIsChecked(DependencyObject d)
        {
            return (bool?)d.GetValue(IsCheckedProperty);
        }

        /// <summary>
        /// Sets the IsChecked property.  This dependency property 
        /// indicates whether the toggle button is checked.
        /// </summary>
        public static void SetIsChecked(DependencyObject d, bool? value)
        {
            d.SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsChecked property.
        /// </summary>
        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement pseudobutton = d as UIElement;
            if (pseudobutton == null) return;

            bool? newValue = (bool?)e.NewValue;

            switch (newValue)
            {
                case true:
                    RaiseEvent(pseudobutton, ToggleButton.CheckedEvent);
                    break;
                case false:
                    RaiseEvent(pseudobutton, ToggleButton.UncheckedEvent);
                    break;
                case null:
                    RaiseEvent(pseudobutton, ToggleButton.IndeterminateEvent);
                    break;
            }
        }

        #endregion

        #region IsThreeState

        /// <summary>
        /// IsThreeState Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsThreeStateProperty =
            DependencyProperty.RegisterAttached("IsThreeState", typeof(bool), typeof(VirtualToggleButton),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Gets the IsThreeState property.  This dependency property 
        /// indicates whether the control supports two or three states.  
        /// IsChecked can be set to null as a third state when IsThreeState is true.
        /// </summary>
        public static bool GetIsThreeState(DependencyObject d) => (bool)d.GetValue(IsThreeStateProperty);

        /// <summary>
        /// Sets the IsThreeState property.  This dependency property 
        /// indicates whether the control supports two or three states. 
        /// IsChecked can be set to null as a third state when IsThreeState is true.
        /// </summary>
        public static void SetIsThreeState(DependencyObject d, bool value) => d.SetValue(IsThreeStateProperty, value);

        #endregion

        #region IsVirtualToggleButton

        /// <summary>
        /// IsVirtualToggleButton Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsVirtualToggleButtonProperty =
            DependencyProperty.RegisterAttached("IsVirtualToggleButton", typeof(bool), typeof(VirtualToggleButton),
                new FrameworkPropertyMetadata(false, OnIsVirtualToggleButtonChanged));

        /// <summary>
        /// Gets the IsVirtualToggleButton property.  This dependency property 
        /// indicates whether the object to which the property is attached is treated as a VirtualToggleButton.  
        /// If true, the object will respond to keyboard and mouse input the same way a ToggleButton would.
        /// </summary>
        public static bool GetIsVirtualToggleButton(DependencyObject d) => (bool)d.GetValue(IsVirtualToggleButtonProperty);

        /// <summary>
        /// Sets the IsVirtualToggleButton property.  This dependency property 
        /// indicates whether the object to which the property is attached is treated as a VirtualToggleButton.  
        /// If true, the object will respond to keyboard and mouse input the same way a ToggleButton would.
        /// </summary>
        public static void SetIsVirtualToggleButton(DependencyObject d, bool value) => d.SetValue(IsVirtualToggleButtonProperty, value);

        /// <summary>
        /// Handles changes to the IsVirtualToggleButton property.
        /// </summary>
        private static void OnIsVirtualToggleButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IInputElement element = d as IInputElement;
            if (element == null) return;

            if ((bool)e.NewValue)
            {
                element.MouseLeftButtonDown += OnMouseLeftButtonDown;
                element.KeyDown += OnKeyDown;
            }
            else
            {
                element.MouseLeftButtonDown -= OnMouseLeftButtonDown;
                element.KeyDown -= OnKeyDown;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// A static helper method to raise the selected event on a target element.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        /// <param name="rEvent">Event which gets raisen</param>
        internal static RoutedEventArgs RaiseEvent(UIElement target, RoutedEvent rEvent)
        {
            if (target == null) return null;

            RoutedEventArgs args = new RoutedEventArgs { RoutedEvent = rEvent };
            RaiseEvent(target, args);
            return args;
        }

        #region private methods

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            UpdateIsChecked(sender as DependencyObject);
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.OriginalSource != sender) return;

            if (e.Key == Key.Space)
            {
                // ignore alt+space which invokes the system menu
                if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) return;

                UpdateIsChecked(sender as DependencyObject);
                e.Handled = true;
            }
            else if (e.Key == Key.Enter && ((bool?)(sender as DependencyObject)?.GetValue(KeyboardNavigation.AcceptsReturnProperty) ?? false))
            {
                UpdateIsChecked((DependencyObject)sender);
                e.Handled = true;
            }
        }

        private static void UpdateIsChecked(DependencyObject d)
        {
            bool? isChecked = GetIsChecked(d);

            if (isChecked == true) SetIsChecked(d, GetIsThreeState(d) ? null : (bool?)false);
            else SetIsChecked(d, isChecked.HasValue);
        }

        private static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
        {
            UIElement element = target as UIElement;

            if (element != null) element.RaiseEvent(args);
            else (target as ContentElement)?.RaiseEvent(args);
        }

        #endregion
    }
}