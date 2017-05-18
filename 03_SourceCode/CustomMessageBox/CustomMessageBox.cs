namespace CustomMessageBox
{
    public enum MessageBoxType
    {
        ConfirmationWithYesNo = 0,
        ConfirmationWithYesNoCancel,
        Information,
        Error,
        Warning
    }

    internal enum MessageBoxImage
    {
        Warning = 0,
        Question,
        Information,
        Error,
        None
    }
}
