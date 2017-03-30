using System;
using System.Windows.Forms;
using WindowsAutomatedSimplifier.ShortcutDialog;

namespace GlobalHotkeyExampleForm
{
    public partial class GlobalHotkeys : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static GlobalHotkeys instance;
        private static ShortcutDialog sh;

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }
 
        private GlobalHotkeys()
        {
            //Test Hotkeys
            RegisterHotKey(this.Handle, 0, (int)KeyModifier.Control, Keys.D1.GetHashCode());       // Register Shift + A as global hotkey.
            RegisterHotKey(this.Handle, 1, (int)KeyModifier.Control, Keys.D2.GetHashCode());       // Register CTRL + B as global hotkey.
        }

        public static GlobalHotkeys getInstance(ShortcutDialog input)
        {
            if (instance == null)
            {
                instance = new GlobalHotkeys();
                sh = input;
            }
            return instance;
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
                    sh.SH01_Click(new object(), new System.Windows.RoutedEventArgs());
                }
                else if (key == Keys.D2 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 2");
                    sh.SH02_Click(new object(), new System.Windows.RoutedEventArgs());
                }
            }
        }

        //Closes the Hotkeys
        public void ExampleForm_FormClosing()
        {
            UnregisterHotKey(this.Handle, 0);
            UnregisterHotKey(this.Handle, 1);
        }

        private void ExampleForm_Load(object sender, EventArgs e)
        {

        }
    }
}