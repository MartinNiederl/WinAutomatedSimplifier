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
        private static ShortcutDialog sh;

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }
 
        public GlobalHotkeys(ShortcutDialog shInput)
        {
            sh = shInput;

            //Test Hotkeys
            RegisterHotKey(this.Handle, 0, (int)KeyModifier.Control, Keys.D1.GetHashCode());
            RegisterHotKey(this.Handle, 1, (int)KeyModifier.Control, Keys.D2.GetHashCode());
            RegisterHotKey(this.Handle, 2, (int)KeyModifier.Control, Keys.D3.GetHashCode());
            RegisterHotKey(this.Handle, 3, (int)KeyModifier.Control, Keys.D4.GetHashCode());
            RegisterHotKey(this.Handle, 4, (int)KeyModifier.Control, Keys.D5.GetHashCode());
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
                    sh.textBox01_MouseDoubleClick(new object(), new System.Windows.RoutedEventArgs());
                }
                else if (key == Keys.D2 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 2");
                    sh.textBox02_MouseDoubleClick(new object(), new System.Windows.RoutedEventArgs());
                }
                else if (key == Keys.D3 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 3");
                    sh.textBox03_MouseDoubleClick(new object(), new System.Windows.RoutedEventArgs());
                }
                else if (key == Keys.D4 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 4");
                    sh.textBox04_MouseDoubleClick(new object(), new System.Windows.RoutedEventArgs());
                }
                else if (key == Keys.D5 && modifier == KeyModifier.Control)
                {
                    Console.WriteLine("Pressed CTRL + 5");
                    sh.textBox05_MouseDoubleClick(new object(), new System.Windows.RoutedEventArgs());
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