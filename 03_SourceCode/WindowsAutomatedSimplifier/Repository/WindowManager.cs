using System.Collections.Generic;
using System.Windows;

namespace WindowsAutomatedSimplifier.Repository
{
    internal static class WindowManager
    {
        public static List<Window> Windows { get; } = new List<Window>();

        public static void AddWindow(Window window) => Windows.Add(window);

        public static bool CloseWindowAtIndex(int index)
        {
            if (index < 0 || index > Windows.Count - 1) return false;
            Windows[index].Close();
            return true;
        }

        public static bool CloseFirstOpenedWindow() => CloseWindowAtIndex(0);

        public static bool CloseLastOpenedWindow() => CloseWindowAtIndex(Windows.Count);
    }
}
