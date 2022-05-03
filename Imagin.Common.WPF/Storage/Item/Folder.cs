using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Imagin.Common.Storage
{
    public sealed class Folder : Container
    {
        public Folder(string path) : base(ItemType.Folder, Origin.Local, path) { }

        public class Long : BaseLong
        {
            #region Private

            const uint SHGFI_ICON = 0x100;

            const uint SHGFI_LARGEICON = 0x0;

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

            static string Combine(string path1, string path2)
            {
                return path1.TrimEnd('\\') + "\\" + path2.TrimStart('\\').TrimEnd('.');
            }

            static List<string> GetAllPathsFromPath(string path)
            {
                bool unc = false;
                var prefix = @"\\?\";
                if (path.StartsWith(prefix + @"UNC\"))
                {
                    prefix += @"UNC\";
                    unc = true;
                }
                var split = path.Split('\\');
                int i = unc ? 6 : 4;
                var list = new List<string>();
                var txt = "";

                for (int a = 0; a < i; a++)
                {
                    if (a > 0) txt += "\\";
                    txt += split[a];
                }
                for (; i < split.Length; i++)
                {
                    txt = Combine(txt, split[i]);
                    list.Add(txt);
                }

                return list;
            }

            static string GetCleanPath(string path)
            {
                if (path.StartsWith(@"\\?\UNC\")) return @"\\" + path.Substring(8);
                if (path.StartsWith(@"\\?\")) return path.Substring(4);
                return path;
            }

            static string GetWin32LongPath(string path)
            {

                if (path.StartsWith(@"\\?\")) return path;

                var newpath = path;
                if (newpath.StartsWith("\\"))
                {
                    newpath = @"\\?\UNC\" + newpath.Substring(2);
                }
                else if (newpath.Contains(":"))
                {
                    newpath = @"\\?\" + newpath;
                }
                else
                {
                    var currdir = Environment.CurrentDirectory;
                    newpath = Combine(currdir, newpath);
                    while (newpath.Contains("\\.\\")) newpath = newpath.Replace("\\.\\", "\\");
                    newpath = @"\\?\" + newpath;
                }
                return newpath.TrimEnd('.');
            }

            static void InternalGetDirectories(string path, string searchPattern, System.IO.SearchOption searchOption, ref List<string> dirs)
            {
                IntPtr findHandle = default;

                try
                {
                    findHandle = FindFirstFile(System.IO.Path.Combine(GetWin32LongPath(path), searchPattern), out WIN32_FIND_DATA findData);
                    if (findHandle != new IntPtr(-1))
                    {
                        do
                        {
                            if ((findData.dwFileAttributes & System.IO.FileAttributes.Directory) != 0)
                            {
                                if (findData.cFileName != "." && findData.cFileName != "..")
                                {
                                    string subdirectory = System.IO.Path.Combine(path, findData.cFileName);
                                    dirs.Add(GetCleanPath(subdirectory));
                                    if (searchOption == SearchOption.AllDirectories)
                                    {
                                        InternalGetDirectories(subdirectory, searchPattern, searchOption, ref dirs);
                                    }
                                }
                            }
                        } while (FindNextFile(findHandle, out findData));
                        FindClose(findHandle);
                    }
                    else
                    {
                        ThrowWin32Exception();
                    }
                }
                catch (Exception)
                {
                    if (findHandle != null)
                        FindClose(findHandle);

                    throw;
                }
            }

            static bool LongExists(string path)
            {
                var attr = GetFileAttributesW(path);
                return (attr != INVALID_FILE_ATTRIBUTES && ((attr & FILE_ATTRIBUTE_DIRECTORY) == FILE_ATTRIBUTE_DIRECTORY));
            }

            [DebuggerStepThrough]
            static void ThrowWin32Exception()
            {
                int code = Marshal.GetLastWin32Error();
                if (code != 0)
                {
                    throw new System.ComponentModel.Win32Exception(code);
                }
            }

            #endregion

            #region Public

            /// <summary>
            /// Gets the path with actual casing by querying each parent folder in the path. Performance is poor due to multiple queries!
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public static string ActualPath(string path)
            {
                IEnumerable<string> folders;

                var last = path;
                var current = System.IO.Path.GetDirectoryName(path);

                var result = string.Empty;
                while (true)
                {
                    folders = null;
                    try
                    {
                        folders = Directory.EnumerateDirectories(current);
                    }
                    catch { }

                    //This will always happen once we reach top-most folder (the associated drive)!
                    if (folders == null || folders.Empty())
                        break;

                    foreach (var i in folders)
                    {
                        if (i.ToLower().Equals(last.ToLower()))
                        {
                            var name = System.IO.Path.GetFileName(i);
                            result = result.Empty() ? name : $@"{name}\{result}";
                            last = current;
                            break;
                        }
                    }
                    current = System.IO.Path.GetDirectoryName(current);
                }

                var driveName = string.Empty;
                foreach (var i in Computer.Drives)
                {
                    if (path.ToLower().StartsWith(i.Name.ToLower()))
                        driveName = i.Name;
                }
                return $@"{driveName}{result}";
            }

            //...

            /// <summary>
            /// Gets a new path based on the given path.
            /// </summary>
            /// <param name="path">The path to evaluate.</param>
            /// <param name="nameFormat">How to format the name (not including extension).</param>
            /// <returns>A new path based on the old path.</returns>
            public static string ClonePath(string folderPath, string nameFormat = StoragePath.DefaultCloneFormat) => StoragePath.Clone(folderPath, nameFormat, i => Exists(i));

            //...

            public static void Create(string directoryPath)
            {
                if (string.IsNullOrWhiteSpace(directoryPath))
                    return;

                if (directoryPath.Length < MAX_PATH)
                {
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        System.IO.Directory.CreateDirectory(directoryPath);
                    }
                }
                else
                {
                    var paths = GetAllPathsFromPath(GetWin32LongPath(directoryPath));
                    foreach (var item in paths)
                    {
                        if (!LongExists(item))
                        {
                            var ok = CreateDirectory(item, IntPtr.Zero);
                            if (!ok)
                            {
                                ThrowWin32Exception();
                            }
                        }
                    }
                }
            }

            public static async Task<Result> TryCreate(string directoryPath)
            {
                Result result = null;
                await Task.Run(() =>
                {
                    try
                    {
                        Create(directoryPath);
                        result = new Success();
                    }
                    catch (Exception e)
                    {
                        result = new Error(e);
                    }
                });
                return result;
            }

            //...

            static void Delete(IEnumerable<string> directories)
            {
                foreach (var directory in directories)
                {
                    var files = GetFiles(directory, SearchOption.TopDirectoryOnly);
                    foreach (string i in files)
                        File.Long.Delete(i);

                    directories = GetFolders(directory, SearchOption.TopDirectoryOnly);
                    Delete(directories);

                    if (!RemoveDirectory(GetWin32LongPath(directory)))
                        ThrowWin32Exception();
                }
            }

            public static void Delete(string path, bool recursive = false)
            {
                if (path.Length < MAX_PATH)
                {
                    Directory.Delete(path, recursive);
                }
                else
                {
                    if (!recursive)
                    {
                        bool ok = RemoveDirectory(GetWin32LongPath(path));
                        if (!ok) ThrowWin32Exception();
                    }
                    else
                    {
                        var longPath = GetWin32LongPath(path);

                        var files = GetFiles(longPath, SearchOption.TopDirectoryOnly);
                        foreach (string i in files)
                            File.Long.Delete(i);

                        Delete(new string[] { longPath });
                    }
                }
            }

            //...

            /// <summary>
            /// Multiple slashes and periods return <see langword="false"/>!
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public static bool Exists(string path)
            {
                if (path == null)
                    return false;

                if (path.OnlyContains('/') || path.OnlyContains('\\') || path.OnlyContains('.'))
                    return false;

                if (path.Length < MAX_PATH)
                    return Directory.Exists(path);

                return LongExists(GetWin32LongPath(path));
            }

            //...

            async public static Task<long> TryGetSize(string folderPath, System.Threading.CancellationToken token)
            {
                long result = 0;
                await Task.Run(async () =>
                {
                    var files = Enumerable.Empty<string>();
                    Try.Invoke(() => files = GetFiles(folderPath));
                    foreach (var i in files)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        Try.Invoke(() =>
                        {
                            var fileInfo = new FileInfo(i);
                            result += fileInfo.Length;
                        });
                    }

                    var folders = Enumerable.Empty<string>();
                    Try.Invoke(() => folders = GetFolders(folderPath));
                    foreach (var i in folders)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        result += await TryGetSize(i, token);
                    }
                });
                return result;
            }

            //...

            public static IEnumerable<string> GetFiles(string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
            {
                if (path == StoragePath.Root)
                    yield break;

                var searchPattern = "*";

                var files = new List<string>();
                var directories = new List<string> { path };

                if (searchOption == SearchOption.AllDirectories)
                {
                    //Add all the subpaths
                    directories.AddRange(GetFolders(path, SearchOption.AllDirectories));
                }

                foreach (var i in directories)
                {
                    IntPtr findHandle = default;
                    try
                    {
                        findHandle = FindFirstFile(System.IO.Path.Combine(GetWin32LongPath(i), searchPattern), out WIN32_FIND_DATA findData);
                        if (findHandle != new IntPtr(-1))
                        {
                            do
                            {
                                if ((findData.dwFileAttributes & FileAttributes.Directory) == 0)
                                {
                                    string filename = System.IO.Path.Combine(i, findData.cFileName);
                                    files.Add(GetCleanPath(filename));
                                }
                            } while (FindNextFile(findHandle, out findData));
                            FindClose(findHandle);
                        }
                    }
                    catch (Exception)
                    {
                        if (findHandle != null)
                            FindClose(findHandle);

                        throw;
                    }
                }
                foreach (var i in files)
                {
                    if (System.IO.Path.GetFileName(i).ToLower() == "desktop.ini")
                    {
                        //This file is used by Windows to determine how a folder is displayed in Windows Explorer. Ignore it!
                        continue;
                    }
                    yield return i;
                }
            }

            public static IEnumerable<string> GetFolders(string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
            {
                if (path == StoragePath.Root)
                    return Enumerable.Empty<string>();

                var result = new List<string>();
                InternalGetDirectories(path, "*", searchOption, ref result);
                return result.ToArray();
            }

            public static IEnumerable<string> GetItems(string path)
            {
                foreach (var i in GetFolders(path))
                    yield return i;

                foreach (var i in GetFiles(path))
                    yield return i;
            }

            //...

            public static void Move(string source, string destination)
            {
                if (source.Length < MAX_PATH || destination.Length < MAX_PATH)
                {
                    Directory.Move(source, destination);
                }
                else if (!MoveFileW(GetWin32LongPath(source), GetWin32LongPath(destination)))
                    ThrowWin32Exception();
            }

            //...

            public static string Parent(string folderPath)
            {
                var result = new DirectoryInfo(folderPath);
                return result.Parent == null ? StoragePath.Root : result.Parent.FullName;
            }

            #endregion
        }
    }
}