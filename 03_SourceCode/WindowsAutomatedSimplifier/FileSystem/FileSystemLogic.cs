using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsAutomatedSimplifier.Repository;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;

namespace WindowsAutomatedSimplifier.FileSystem
{
    internal class FileSystemLogic
    {
        private static List<string> _whiteList;

        /// <summary>
        /// Delete empty directories in a given root directory.
        /// </summary>
        /// <param name="rootDirectory">Root directory to start.</param>
        /// <param name="whiteList">List of ignored Directories.</param>
        /// <param name="onlytopdir">Only delete empty directories in the root directory.</param>
        public static void DeleteEmptyDirectories(string rootDirectory, List<string> whiteList = null, bool onlytopdir = false)
        {
            if (String.IsNullOrEmpty(rootDirectory))
            {
                Logger.Log(@"RootDirectory is a null reference or an empty string");
                return;
            }
            _whiteList = whiteList ?? new List<string>();

            if (onlytopdir) DelEmptyDirs(rootDirectory);
            else DelEmptyDirsRecursive(rootDirectory);
        }

        private static void DelEmptyDirsRecursive(string rootDirectory)
        {
            try
            {
                foreach (string d in Directory.EnumerateDirectories(rootDirectory))
                    if (!_whiteList.Contains(d)) DelEmptyDirsRecursive(d);

                if (!Directory.EnumerateFileSystemEntries(rootDirectory).Any())
                {
                    try { Directory.Delete(rootDirectory); }
                    catch (Exception e) { Logger.LogAdd(e.Message, e.StackTrace); }
                }
            }
            catch (UnauthorizedAccessException e) { Logger.LogAdd(e.Message, e.StackTrace); }
        }

        private static void DelEmptyDirs(string directory)
        {
            try
            {
                foreach (string d in Directory.EnumerateDirectories(directory))
                {
                    if (!Directory.EnumerateFileSystemEntries(d).Any())
                    {
                        try { Directory.Delete(d); }
                        catch (Exception e) { Logger.LogAdd(e.Message, e.StackTrace); }
                    }
                }
            }
            catch (UnauthorizedAccessException e) { Logger.LogAdd(e.Message, e.StackTrace); }
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
            }

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

            if (!String.IsNullOrWhiteSpace(regexStr))
            {
                Regex regex;
                try { regex = new Regex(regexStr); }
                catch (Exception) { return null; }
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

            if (!String.IsNullOrWhiteSpace(sizeFrom))
            {
                ByteSize bs;
                if (!ByteSize.TryParse(sizeFrom, out bs))
                    Logger.Log("Invalid size input");
                else files = files.Where(entry => bs.Bytes < new FileInfo(entry).Length);
            }

            if (!String.IsNullOrWhiteSpace(sizeTo))
            {
                ByteSize bs;
                if (!ByteSize.TryParse(sizeTo, out bs))
                    Logger.Log("Invalid size input");
                else files = files.Where(entry => bs.Bytes > new FileInfo(entry).Length);
            }

            return files.Select(file => new FileInfo(file)).ToList();
        }

        /// <summary>
        /// Iterate through fileSystem without getting Errors.
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

        public void NumberFilesPattern(List<FileInfo> files, string pattern, SortBy order)
        {
            files = SortFileList(files, order);
        }

        private static List<FileInfo> SortFileList(List<FileInfo> files, SortBy order)
        {
            switch (order)
            {
                case SortBy.OrigName:
                    return files.OrderBy(e => e.Name).ToList();
                case SortBy.CreationDate:
                    return files.OrderBy(e => e.CreationTime).ToList();
                case SortBy.EditDate:
                    return files.OrderBy(e => e.LastWriteTime).ToList();
                case SortBy.Size:
                    return files.OrderBy(e => e.Length).ToList();
                default:
                    return files;
            }
        }

        public static IEnumerable<string> IterateThroughFiles(string rootPath, string searchPattern = "*")
        {
            foreach (string file in GetFiles(rootPath, searchPattern))
                yield return file;

            foreach (string directory in GetDirectories(rootPath))
            {
                foreach (string file in IterateThroughFiles(directory, searchPattern))
                    yield return file;
            }
        }

        private static IEnumerable<string> GetDirectories(string directory)
        {
            IEnumerable<string> subDirectories = null;
            try { subDirectories = Directory.EnumerateDirectories(directory, "*.*", SearchOption.TopDirectoryOnly); }
            catch (UnauthorizedAccessException) { }

            if (subDirectories != null)
            {
                foreach (string subDirectory in subDirectories)
                    yield return subDirectory;
            }
        }

        private static IEnumerable<string> GetFiles(string directory, string searchPattern)
        {
            IEnumerable<string> files = null;
            try { files = Directory.EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly); }
            catch (UnauthorizedAccessException) { }

            if (files != null)
            {
                foreach (string file in files)
                    yield return file;
            }
        }

        public static void ConcatFiles(IEnumerable<string> sourcePaths, string outputFilePath)
        {
            using (FileStream outputStream = File.Create(outputFilePath))
            {
                foreach (string inputFilePath in sourcePaths)
                    using (FileStream inputStream = File.OpenRead(inputFilePath))
                        inputStream.CopyTo(outputStream);
            }
        }
    }
    public enum SortBy
    {
        OrigName,
        CreationDate,
        EditDate,
        Size,
        None
    }
}
