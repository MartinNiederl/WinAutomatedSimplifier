using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsAutomatedSimplifier.ShortcutDialog
{
    public class GlobalHotkeys : Form
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static ShortcutDialog _eventRef;

        private enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }
 
        public GlobalHotkeys(ShortcutDialog eventRefInput)
        {
            RegisterHotKey(Handle, 0, (int)KeyModifier.Control, Keys.D1.GetHashCode());
            RegisterHotKey(Handle, 1, (int)KeyModifier.Control, Keys.D2.GetHashCode());
            _eventRef = eventRefInput;
        }

        //Handles all the incoming Hotkeys
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)message.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.

                if (key == Keys.D1 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine(@"Pressed CTRL + 1");
                    _eventRef.SH01_Click(new object(), new System.Windows.RoutedEventArgs());
                }
                else if (key == Keys.D2 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine(@"Pressed CTRL + 2");
                    _eventRef.SH02_Click(new object(), new System.Windows.RoutedEventArgs());
                }
            }
        }

        //Closes the Hotkeys
        public void WindClose()
        {
            UnregisterHotKey(Handle, 0);
            UnregisterHotKey(Handle, 1);
        }
    }
}