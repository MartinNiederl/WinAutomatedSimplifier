using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using static System.Windows.Forms.Application;

namespace WindowsAutomatedSimplifier.RegistryHelper
{
    public class RegistryAPI
    {
        private static RegistryAPI _instance;

        public static RegistryAPI Instance => _instance ?? (_instance = new RegistryAPI());

        public void AddValue(string keyPath, string valueName, object value) => Registry.SetValue(keyPath, valueName, value);
        public void DeleteValue(string keyPath)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath, true))
                key?.DeleteValue("MyApp");
        }
        public void ChangeValue(string keyPath, string valueName, object value) => Registry.SetValue(keyPath, valueName, value);
        public string GetValue(string keyPath, string valueName) => (string)Registry.GetValue(keyPath, valueName, null);

        public RegistryKey AddKey(string keyPath, string newKey, string value)
        {
            using (RegistryKey key = StringToKey(keyPath))
                return AddKey(key, newKey, value);
        }

        public RegistryKey AddKey(RegistryKey key, string newKey, string value)
        {
            key = key.CreateSubKey(newKey);
            key?.SetValue("", value);
            return key;
        }

        public void DeleteKey(string keyPath)
        {
            int indexofLast = keyPath.LastIndexOf("\\", StringComparison.Ordinal);
            using (RegistryKey key = StringToKey(keyPath.Substring(0, indexofLast)))
                key.DeleteSubKey(keyPath.Substring(indexofLast + 1));
        }

        public void DeleteKeyTree(string keyPath)
        {
            int indexofLast = keyPath.LastIndexOf("\\", StringComparison.Ordinal);
            using (RegistryKey key = StringToKey(keyPath.Substring(0, indexofLast)))
                key.DeleteSubKeyTree(keyPath.Substring(indexofLast + 1));
        }

        public static RegistryKey StringToKey(string path)
        {
            RegistryKey key = GetRegistryHive(path);
            string substring = path.Substring(path.IndexOf("\\", StringComparison.Ordinal) + 1);
            do
            {
                int length = substring.IndexOf("\\", StringComparison.Ordinal);
                if (key?.OpenSubKey(length < 1 ? substring.Substring(0) : substring.Substring(0, length), true) == null) break;
                key = key.OpenSubKey(length < 1 ? substring.Substring(0) : substring.Substring(0, length), true);
                if (length < 1) break;
                substring = substring.Substring(length + 1);
            } while (true);
            return key;
        }
        public void CreateBackup(string exportPath, string registryPath)
        {
            string path = "\"" + exportPath + "\"";
            string key = "\"" + registryPath + "\"";

            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "regedit.exe";
                proc.StartInfo.UseShellExecute = false;

                proc = Process.Start("regedit.exe", "/e " + path + " " + key);
                proc?.WaitForExit();
            }
            catch (Exception) { proc?.Dispose(); }
        }

        private bool PathExists(string path) => GetRegistryHive(path).OpenSubKey(path) != null;

        private bool PathValueExists(string path, string value) => GetRegistryHive(path).OpenSubKey(path)?.GetValue(value) != null;

        private static RegistryKey GetRegistryHive(string path)
        {
            switch (path.Split('\\')[0].ToUpper())
            {
                case "HKEY_LOCAL_MACHINE": return Registry.LocalMachine;
                case "HKEY_CLASSES_ROOT": return Registry.ClassesRoot;
                case "HKEY_CURRENT_CONFIG": return Registry.CurrentConfig;
                case "HKEY_CURRENT_USER": return Registry.CurrentUser;
                case "HKEY_PERFORMANCE_DATA": return Registry.PerformanceData;
                case "HKEY_USERS": return Registry.Users;
                default: return Registry.LocalMachine;
            }
        }


        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public static void InitRegistry()
        {
            if (IsRegInit()) return;

            const string iconPath = @"C:\Users\Mani\Documents\Schule\Projektentwicklung\WinAutomatedSimplifier\03_SourceCode\Key.ico";
            string programPath = "\"" + ExecutablePath + "\" \"%1\"";

            using (RegistryKey fileReg = Registry.ClassesRoot.CreateSubKey(@".pwf"))
            using (RegistryKey appReg = Registry.ClassesRoot.CreateSubKey(@"Applications\WindowsAutomatedSimplifier.exe"))
            using (RegistryKey appAssoc = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.pwf"))
            using (RegistryKey assoc = Registry.ClassesRoot.CreateSubKey(@"pwf_auto_file\shell"))
            using (RegistryKey folder = Registry.ClassesRoot.OpenSubKey(@"Directory\shell", true)?.CreateSubKey("WAS"))
            {

                fileReg?.CreateSubKey("DefaultIcon")?.SetValue("", iconPath);
                fileReg?.CreateSubKey("PerceivedType")?.SetValue("", "Text");

                appReg?.CreateSubKey("DefaultIcon")?.SetValue("", iconPath);
                appReg?.CreateSubKey(@"shell\Decrypt\command")?.SetValue("", programPath);
                appReg?.CreateSubKey(@"shell\open\command")?.SetValue("", programPath);

                appAssoc?.DeleteSubKey("UserChoice", false);

                assoc?.CreateSubKey("Decrypt/command")?.SetValue("", programPath);
                assoc?.CreateSubKey("open/command")?.SetValue("", programPath);

                folder?.SetValue("", "Encrypt");
                folder?.CreateSubKey("command")?.SetValue("", programPath);
            }

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        private static bool IsRegInit() => Registry.ClassesRoot.OpenSubKey(".pwf") != null;

        public static void UpdateRegistry()
        {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        Verb = "runas",
                        Arguments = "/f /im explorer.exe",
                        FileName = @"C:\windows\system32\taskkill.exe"
                    }
                };
                process.Start();
                process.WaitForExit();

                new Process
                {
                    StartInfo =
                    {
                        FileName = $"{Environment.GetEnvironmentVariable("WINDIR")}\\explorer.exe",
                        UseShellExecute = true
                    }
                }.Start();
        }
    }
}
