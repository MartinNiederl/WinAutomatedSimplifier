using System;
using System.Windows.Forms;
using TextBox = System.Windows.Controls.TextBox;

namespace WindowsAutomatedSimplifier.ShortcutDialog
{
    public class GlobalHotkeys : Form
    {
        private const int SCCOUNT = 5;
        private const int NUMKEYSTART = 48;  //48 is the begin of the number keys

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private static ShortcutDialog SDialogInstance;

        private enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }
 
        public GlobalHotkeys(ShortcutDialog sDialogInstanceInput)
        {
            SDialogInstance = sDialogInstanceInput;

            for (int i = 0; i < SCCOUNT; i++)
                RegisterHotKey(Handle, i, (int) KeyModifier.Control, i + NUMKEYSTART);
        }

        //Handles all the incoming Hotkeys
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
            if (message.Msg != 0x0312) return;

            Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
            KeyModifier modifier = (KeyModifier)((int)message.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.

            if (modifier == KeyModifier.Control)
            {
                string name = "TxtPath" + ((int) key - NUMKEYSTART).ToString().PadLeft(2, '0');
                SDialogInstance.RunCommand(SDialogInstance.FindName(name), null);
            }
        }

        //Closes the Hotkeys
        public void CloseForm()
        {
            for (int i = 0; i < SCCOUNT; i++)
                UnregisterHotKey(Handle, i);
        }
    }
}