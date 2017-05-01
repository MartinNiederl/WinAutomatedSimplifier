using System;
using System.IO;
using System.Linq;

namespace WindowsAutomatedSimplifier.FileSystem
{
    internal class FileSystemLogic
    {
        public static void DeleteEmptyDirectories(string dir)
        {
            if (string.IsNullOrEmpty(dir)) throw new ArgumentException(@"Starting directory is a null reference or an empty string", nameof(dir));

            try
            {
                foreach (string d in Directory.EnumerateDirectories(dir))
                    DeleteEmptyDirectories(d);
                
                if (!Directory.EnumerateFileSystemEntries(dir).Any())
                {
                    try { Directory.Delete(dir); }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }
                    catch (Exception) { }
                }
            }
            catch (UnauthorizedAccessException) { }
        }
    }
}
