using System;
using System.Threading;
using System.Windows;

namespace WindowsAutomatedSimplifier.Repository
{
    public class AutoClosingMessageBox
    {
        public Timer TimeoutTimer { get; }
        public string Caption { get; }

        private AutoClosingMessageBox(string text, string caption, int timeout)
        {
            Caption = caption;
            TimeoutTimer = new Timer(OnTimerElapsed, null, timeout, Timeout.Infinite);
            using (TimeoutTimer)
                MessageBox.Show(text, caption);
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }

        private void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", Caption);
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            TimeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
