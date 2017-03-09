using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WindowsAutomatedSimplifier.RegistryHelper
{
    public class RegistryAPI
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

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
            RegistryKey key = StringToKey(keyPath);
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
            RegistryKey key = StringToKey(keyPath.Substring(0, indexofLast));
            key.DeleteSubKey(keyPath.Substring(indexofLast + 1));
        }

        public void DeleteKeyTree(string keyPath)
        {
            int indexofLast = keyPath.LastIndexOf("\\", StringComparison.Ordinal);
            RegistryKey key = StringToKey(keyPath.Substring(0, indexofLast));
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

        private bool IsPathValid(string path)
        {
            //TODO implement check for registryPath
            return false;
        }

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
                case "HKEY_DYN_DATA": return Registry.DynData;
                default: return Registry.LocalMachine;
            }
        }

        public static void InitRegistry()
        {
            if (IsRegInit()) return;
            try
            {
                RegistryKey fileReg = Registry.CurrentUser.CreateSubKey(@"Software\Classes\.pwf");
                RegistryKey appReg = Registry.CurrentUser.CreateSubKey(@"Software\Classes\Applications\WindowsAutomatedSimplifier.exe");
                RegistryKey appAssoc = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.pwf");

                fileReg?.CreateSubKey("DefaultIcon")?.SetValue("", @"C:\Users\Mani\Documents\Schule\Projektentwicklung\Key.ico");
                fileReg?.CreateSubKey("PerceivedType")?.SetValue("", "Text");

                appReg?.CreateSubKey("DefaultIcon")?.SetValue("", @"C:\Users\Mani\Documents\Schule\Projektentwicklung\Key.ico");
                appReg?.CreateSubKey(@"shell\open\command")?.SetValue("", "\"" + System.Windows.Forms.Application.ExecutablePath + "\" \"%1\"");

                //appAssoc?.CreateSubKey("UserChoice")?.SetValue("Progid", "\"" + System.Windows.Forms.Application.ExecutablePath + "\" \"%1\"");
                appAssoc?.CreateSubKey("OpenWithList")?.SetValue("a", "WindowsAutomatedSimplifier.EXE");
                appAssoc?.OpenSubKey("OpenWithList", true)?.SetValue("MRUList", "a");
            }
            catch (UnauthorizedAccessException uae)
            {
                Console.WriteLine(uae.Message);
            }


            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);

            #region --OLDVERSION--

            //RegistryAPI api = new RegistryAPI();
            //RegistryKey key = api.AddKey(@"HKEY_CLASSES_ROOT\Software\Classes\directory\shell", "WAS", "Verschlüsseln");
            //api.AddKey(key, "command", @"C:\Users\Mani\Documents\GitHub\WindowsAutomatedSimplifier\03_SourceCode\WindowsAutomatedSimplifier\bin\Debug\WindowsAutomatedSimplifier.exe %1");

            // not working yet!

            //key = api.AddKey("HKEY_CLASSES_ROOT", "PasswordSecuredFolder", "");
            //key = api.AddKey(key, "shell", "");
            //key = api.AddKey(key, "Entschlüsseln", "");
            //api.AddKey(key, "command", @"C:\Users\Mani\Documents\GitHub\WindowsAutomatedSimplifier\03_SourceCode\WindowsAutomatedSimplifier\bin\Debug\WindowsAutomatedSimplifier.exe %1");

            #endregion --OLDVERSION--
        }

        private static bool IsRegInit() => Registry.ClassesRoot.OpenSubKey(@"Software\Classes\.pwf") != null;
    }
}
