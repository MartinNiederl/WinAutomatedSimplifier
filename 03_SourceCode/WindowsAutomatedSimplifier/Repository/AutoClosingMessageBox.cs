using System;
using System.Threading;
using System.Windows;

namespace WindowsAutomatedSimplifier.Repository
{
    public class AutoClosingMessageBox
    {
        public Timer Timer { get; set; }
        public string Caption { get; }
        private readonly int _timeout;

        private AutoClosingMessageBox(string text, string caption, int timeout)
        {
            Caption = caption;
            _timeout = timeout;
            Timer = new Timer(OnTimerElapsed, null, _timeout, Timeout.Infinite);
            using (Timer) MessageBox.Show(text, caption);
        }

        public void PauseTimer() => Timer.Dispose();
        public void ContinueTimer() => Timer = new Timer(OnTimerElapsed, null, _timeout, Timeout.Infinite);

        /// <summary>
        /// Creates and shows a new Messagebox which closes automatically.
        /// </summary>
        /// <param name="text">Message of the Box.</param>
        /// <param name="caption">Title showed in the Box.</param>
        /// <param name="timeout">Time in ms after the messagebox should close.</param>
        public static AutoClosingMessageBox Show(string text, string caption, int timeout) => new AutoClosingMessageBox(text, caption, timeout);

        private void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", Caption);

            if (mbWnd != IntPtr.Zero) SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            Timer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
