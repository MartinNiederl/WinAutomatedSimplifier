using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAutomatedSimplifier.FileSystem
{
    internal class FileSystem
    {
        private void DeleteEmptyDirectories(string path)
        {
            if (path == null) return;
            while (path.EndsWith("\\")) path = path.Substring(0, path.Length - 1);

            DirectoryInfo di = new DirectoryInfo(@"C:\MyProject\Project1\file1\file2\file3\file4\");
            const string root = @"C:\MyProject\Project1"; // no trailing slash!
            while (di != null && di.FullName != root && !di.EnumerateFiles().Any() && !di.EnumerateDirectories().Any())
            {
                di.Delete();
                di = di.Parent;
            }
        }

        public static void DeleteEmptyDirectories(string directoryPath, bool topDirectoryOnly)
        {
            // Get all directories; directory search option based upon parameter value.
            IEnumerable<string> allDirectoryPaths =
                 Directory.EnumerateDirectories(directoryPath, "*",
                 (topDirectoryOnly) ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);

            // Delete directories with 0 files.
            foreach (string path in allDirectoryPaths)
                if (!Directory.EnumerateFiles(path).Any()) Directory.Delete(path);
        }
    }
}
