using System;
using System.Windows.Forms;

namespace WindowsAutomatedSimplifier.ShortcutDialog
{
    public class GlobalHotkeys : Form
    {
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

            //Test Hotkeys
            RegisterHotKey(Handle, 0, (int)KeyModifier.Control, Keys.D1.GetHashCode());
            RegisterHotKey(Handle, 1, (int)KeyModifier.Control, Keys.D2.GetHashCode());
            RegisterHotKey(Handle, 2, (int)KeyModifier.Control, Keys.D3.GetHashCode());
            RegisterHotKey(Handle, 3, (int)KeyModifier.Control, Keys.D4.GetHashCode());
            RegisterHotKey(Handle, 4, (int)KeyModifier.Control, Keys.D5.GetHashCode());
        }

        //Handles all the incoming Hotkeys
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.

                if (key == Keys.D1 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 1");
                    SDialogInstance.RunCommand(SDialogInstance.TxtPath01, null);
                }
                else if (key == Keys.D2 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 2");
                    SDialogInstance.RunCommand(SDialogInstance.TxtPath02, null);
                }
                else if (key == Keys.D3 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 3");
                    SDialogInstance.RunCommand(SDialogInstance.TxtPath03, null);
                }
                else if (key == Keys.D4 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 4");
                    SDialogInstance.RunCommand(SDialogInstance.TxtPath04, null);
                }
                else if (key == Keys.D5 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 5");
                    SDialogInstance.RunCommand(SDialogInstance.TxtPath05, null);
                }
            }
        }

        //Closes the Hotkeys
        public void ExampleForm_FormClosing()
        {
            UnregisterHotKey(Handle, 0);
            UnregisterHotKey(Handle, 1);
        }

        private void ExampleForm_Load(object sender, EventArgs e)
        {

        }
    }
}