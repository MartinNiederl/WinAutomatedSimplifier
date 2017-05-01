using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace WindowsAutomatedSimplifier.FileSystem
{
    internal class FileSystemLogic
    {
        //TODO add ByteSize... to convert sizes

        /// <summary>
        /// Delete empty directories in a given root directory.
        /// </summary>
        /// <param name="rootDirectory">Root directory to start.</param>
        public static void DeleteEmptyDirectories(string rootDirectory)
        {
            if (string.IsNullOrEmpty(rootDirectory)) throw new ArgumentException(@"Starting rootDirectory is a null reference or an empty string", nameof(rootDirectory));

            try
            {
                foreach (string d in Directory.EnumerateDirectories(rootDirectory))
                    DeleteEmptyDirectories(d);

                if (!Directory.EnumerateFileSystemEntries(rootDirectory).Any())
                {
                    try { Directory.Delete(rootDirectory); }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }
                    catch (Exception) { }
                }
            }
            catch (UnauthorizedAccessException) { }
        }

        /// <summary>
        /// Moves a list of files to a given rootDirectory.
        /// </summary>
        /// <param name="fileInfos">List of the files.</param>
        /// <param name="directory">Path of the new rootDirectory.</param>
        public bool MoveFiles(IEnumerable<FileInfo> fileInfos, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Console.Error.WriteLine("Given directory does not exist!");
                return false;
            }

            try { Parallel.ForEach(fileInfos, fileInfo => fileInfo.MoveTo(directory + fileInfo.Name + fileInfo)); }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes a list of files.
        /// </summary>
        /// <param name="fileInfos">List of the files.</param>
        /// <param name="toRecycleBin">Option to move the files to the recyle bin instead of deleting them instantly.</param>
        public void DeleteFiles(IEnumerable<FileInfo> fileInfos, bool toRecycleBin)
        {
            Parallel.ForEach(fileInfos, fileInfo =>
            {
                if (toRecycleBin)
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fileInfo.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin,
                        UICancelOption.ThrowException);
                else
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fileInfo.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin,
                        UICancelOption.ThrowException);
            });
        }
    }
}
