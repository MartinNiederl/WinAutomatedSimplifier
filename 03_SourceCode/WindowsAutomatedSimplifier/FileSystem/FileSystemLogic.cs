using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsAutomatedSimplifier.Repository;
using Microsoft.VisualBasic.FileIO;
using Microsoft.WindowsAPICodePack.Controls;
using SearchOption = System.IO.SearchOption;

namespace WindowsAutomatedSimplifier.FileSystem
{
    internal class FileSystemLogic
    {
        /// <summary>
        /// Delete empty directories in a given root directory.
        /// </summary>
        /// <param name="rootDirectory">Root directory to start.</param>
        public static void DeleteEmptyDirectories(string rootDirectory)
        {
            if (string.IsNullOrEmpty(rootDirectory))
            {
                Logger.Log(@"Starting rootDirectory is a null reference or an empty string");
                return;
            }

            try
            {
                foreach (string d in Directory.EnumerateDirectories(rootDirectory))
                    DeleteEmptyDirectories(d);

                if (!Directory.EnumerateFileSystemEntries(rootDirectory).Any())
                {
                    try { Directory.Delete(rootDirectory); }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException dnfe) { Logger.LogAdd(dnfe.Message, dnfe.StackTrace); }
                    catch (Exception e) { Logger.LogAdd(e.Message, e.StackTrace); }
                }
            }
            catch (UnauthorizedAccessException) { }
        }

        /// <summary>
        /// Filter files with given Filters.
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="regexStr"></param>
        /// <param name="creationDateFrom"></param>
        /// <param name="creationDateTo"></param>
        /// <param name="lastChangeFrom"></param>
        /// <param name="lastChangeTo"></param>
        /// <param name="sizeFrom"></param>
        /// <param name="sizeTo"></param>
        /// <param name="onlyTop"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetFilteredFiles(string rootDirectory, string regexStr, DateTime? creationDateFrom, DateTime? creationDateTo, DateTime? lastChangeFrom, DateTime? lastChangeTo, string sizeFrom, string sizeTo, bool onlyTop = false)
        {
            if (!Directory.Exists(rootDirectory))
            {
                Logger.Log("RootDirectory does not exist");
                return new List<FileInfo>();
            };

            IEnumerable<string> files;
            try
            {
                if (onlyTop) files = Directory.EnumerateFiles(rootDirectory);
                else
                {
                    var list = new List<string>();
                    AddFiles(rootDirectory, list);
                    files = list.AsEnumerable();
                }
            }
            catch (Exception e)
            {
                Logger.LogAdd(e.Message, e.StackTrace);
                return new List<FileInfo>();
            }

            if (!string.IsNullOrWhiteSpace(regexStr))
            {
                Regex regex = new Regex(regexStr);
                files = files.Where(entry => regex.IsMatch(entry));
            }

            if (creationDateFrom.HasValue)
            {
                DateTime dateFrom = creationDateFrom.Value;
                files = files.Where(entry => dateFrom < File.GetCreationTime(entry));
            }

            if (creationDateTo.HasValue)
            {
                DateTime dateTo = creationDateTo.Value;
                files = files.Where(entry => dateTo > File.GetCreationTime(entry));
            }

            if (lastChangeFrom.HasValue)
            {
                DateTime dateFrom = lastChangeFrom.Value;
                files = files.Where(entry => dateFrom < File.GetLastWriteTime(entry));
            }

            if (lastChangeTo.HasValue)
            {
                DateTime dateTo = lastChangeTo.Value;
                files = files.Where(entry => dateTo > File.GetLastWriteTime(entry));
            }

            if (!string.IsNullOrWhiteSpace(sizeFrom))
            {
                ByteSize bs;
                if (!ByteSize.TryParse(sizeFrom, out bs))
                    Logger.Log("Invalid size input");
                else files = files.Where(entry => bs.Bytes < new FileInfo(entry).Length);
            }

            if (!string.IsNullOrWhiteSpace(sizeTo))
            {
                ByteSize bs;
                if (!ByteSize.TryParse(sizeTo, out bs))
                    Logger.Log("Invalid size input");
                else files = files.Where(entry => bs.Bytes > new FileInfo(entry).Length);
            }

            return files.Select(file => new FileInfo(file)).ToList();
        }

        /// <summary>
        /// Custom method to iterate through fileSystem without getting Errors.
        /// </summary>
        /// <param name="path">Root path where to start.</param>
        /// <param name="files"></param>
        private static void AddFiles(string path, ICollection<string> files)
        {
            try
            {
                Directory.GetFiles(path)
                    .ToList()
                    .ForEach(files.Add);

                Directory.GetDirectories(path)
                    .ToList()
                    .ForEach(s => AddFiles(s, files));
            }
            catch (UnauthorizedAccessException ex) { Logger.Log(ex.Message); }
        }

        //TODO add AppDomain.CurrentDomain.UnhandledException support for auto logging

        /// <summary>
        /// Moves a list of files to a given rootDirectory.
        /// </summary>
        /// <param name="fileInfos">List of the files.</param>
        /// <param name="directory">Path of the new rootDirectory.</param>
        public bool MoveFiles(IEnumerable<FileInfo> fileInfos, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Logger.Log("Given directory does not exist!");
                return false;
            }

            try { Parallel.ForEach(fileInfos, fileInfo => fileInfo.MoveTo(directory + fileInfo.Name + fileInfo)); }
            catch (Exception e)
            {
                Logger.LogAdd(e.Message, e.StackTrace);
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
                try
                {
                    if (toRecycleBin)
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fileInfo.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin,
                            UICancelOption.ThrowException);
                    else
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fileInfo.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin,
                            UICancelOption.ThrowException);
                }
                catch (Exception e) { Logger.LogAdd(e.Message, e.StackTrace); }
            });
        }
    }
}
