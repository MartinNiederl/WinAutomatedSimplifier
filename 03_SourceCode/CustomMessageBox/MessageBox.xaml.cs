using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CustomMessageBox
{
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();
        }

        private static MessageBox _messageBox;
        private static MessageBoxResult _result = MessageBoxResult.No;

        public static MessageBoxResult Show(string caption, string msg, MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.ConfirmationWithYesNo:
                    return Show(caption, msg, MessageBoxButton.YesNo, MessageBoxImage.Question);
                case MessageBoxType.ConfirmationWithYesNoCancel:
                    return Show(caption, msg, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                case MessageBoxType.Information:
                    return Show(caption, msg, MessageBoxButton.OK, MessageBoxImage.Information);
                case MessageBoxType.Error:
                    return Show(caption, msg, MessageBoxButton.OK, MessageBoxImage.Error);
                case MessageBoxType.Warning:
                    return Show(caption, msg, MessageBoxButton.OK, MessageBoxImage.Warning);
                default:
                    return MessageBoxResult.No;
            }
        }

        public string ShowInput(string caption, string text)
        {
            _messageBox = new MessageBox
            {
                TxtMsg = {Text = text},
                Title = caption,
                Input = {Visibility = Visibility.Visible}
            };
            SetVisibilityOfButtons(MessageBoxButton.OK);
            _messageBox.ShowDialog();
            return _messageBox.Input.Text;
        }

        public static MessageBoxResult Show(string msg, MessageBoxType type)
        {
            return Show(string.Empty, msg, type);
        }

        public static MessageBoxResult Show(string msg)
        {
            return Show(string.Empty, msg, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string caption, string text)
        {
            return Show(caption, text, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string caption, string text, MessageBoxButton button)
        {
            return Show(caption, text, button, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string caption, string text, MessageBoxButton button, MessageBoxImage image)
        {
            _messageBox = new MessageBox
            {
                TxtMsg = { Text = text },
                Title = caption
            };
            SetVisibilityOfButtons(button);
            SetImageOfMessageBox(image);
            _messageBox.ShowDialog();
            return _result;
        }

        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    _messageBox.BtnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.BtnNo.Visibility = Visibility.Collapsed;
                    _messageBox.BtnYes.Visibility = Visibility.Collapsed;
                    _messageBox.BtnOk.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    _messageBox.BtnNo.Visibility = Visibility.Collapsed;
                    _messageBox.BtnYes.Visibility = Visibility.Collapsed;
                    _messageBox.BtnCancel.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    _messageBox.BtnOk.Visibility = Visibility.Collapsed;
                    _messageBox.BtnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.BtnNo.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    _messageBox.BtnOk.Visibility = Visibility.Collapsed;
                    _messageBox.BtnCancel.Focus();
                    break;
            }
        }
        private static void SetImageOfMessageBox(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Warning:
                    _messageBox.SetImage("Warning.png");
                    break;
                case MessageBoxImage.Question:
                    _messageBox.SetImage("Question.png");
                    break;
                case MessageBoxImage.Information:
                    _messageBox.SetImage("Information.png");
                    break;
                case MessageBoxImage.Error:
                    _messageBox.SetImage("Error.png");
                    break;
                default:
                    _messageBox.Img.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(sender, BtnOk))
                _result = MessageBoxResult.OK;
            else if (Equals(sender, BtnYes))
                _result = MessageBoxResult.Yes;
            else if (Equals(sender, BtnNo))
                _result = MessageBoxResult.No;
            else if (Equals(sender, BtnCancel))
                _result = MessageBoxResult.Cancel;
            else _result = MessageBoxResult.None;

            _messageBox.Close();
        }
        private void SetImage(string imageName)
        {
            BitmapImage image = new BitmapImage(new Uri($"Icons/{imageName}", UriKind.Relative));
            MBWnd.Icon = image;
            Img.Visibility = Visibility.Visible;
            Img.Source = image;
        }
    }
}
