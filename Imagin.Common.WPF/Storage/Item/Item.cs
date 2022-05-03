using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Imagin.Common.Storage
{
    public abstract class Item : BaseNamable
    {
        #region Properties

        static readonly Dictionary<string, PropertyChangedEventArgs> EventArgCache;

        [Hidden]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        protected DateTime accessed;
        [Convert(typeof(RelativeTimeConverter))]
        [DisplayName("Accessed")]
        [ReadOnly]
        public virtual DateTime Accessed
        {
            get => accessed;
            set => this.Change(ref accessed, value);
        }

        protected DateTime created;
        [Convert(typeof(RelativeTimeConverter))]
        [DisplayName("Created")]
        [ReadOnly]
        public virtual DateTime Created
        {
            get => created;
            set => this.Change(ref created, value);
        }

        protected DateTime modified;
        [Convert(typeof(RelativeTimeConverter))]
        [DisplayName("Modified")]
        [ReadOnly]
        public virtual DateTime Modified
        {
            get => modified;
            set => this.Change(ref modified, value);
        }

        bool isChanged;
        [Hidden]
        public bool IsChanged
        {
            get => isChanged;
            private set => this.Change(ref isChanged, value);
        }

        bool isHidden = false;
        [Category("Attributes")]
        [DisplayName("Hidden")]
        public virtual bool IsHidden
        {
            get => isHidden;
            set
            {
                Try.Invoke(() =>
                {
                    if (value)
                        File.Long.AddAttribute(path, FileAttributes.Hidden);

                    else File.Long.RemoveAttribute(path, FileAttributes.Hidden);
                    isHidden = value;
                },
                e => Analytics.Log.Write<Item>(e));
                this.Changed(() => IsHidden);
            }
        }

        bool isReadOnly = false;
        [Category("Attributes")]
        [DisplayName("ReadOnly")]
        public virtual bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                Try.Invoke(() =>
                {
                    if (value)
                        File.Long.AddAttribute(path, FileAttributes.ReadOnly);

                    else File.Long.RemoveAttribute(path, FileAttributes.ReadOnly);
                    isReadOnly = value;
                },
                e => Analytics.Log.Write<Item>(e));
                this.Changed(() => IsReadOnly);
            }
        }

        bool isSelected = false;
        [Hidden]
        public virtual bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        protected string path;
        [DisplayName("Path")]
        [Featured]
        [ReadOnly]
        public virtual string Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        protected long size = 0L;
        [DisplayName("Size")]
        [Convert(typeof(FileSizeConverter), FileSizeFormat.BinaryUsingSI)]
        [ReadOnly]
        public long Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        protected ItemType type;
        [Hidden]
        public ItemType Type
        {
            get => type;
            protected set => this.Change(ref type, value);
        }

        protected int permissions = 0;
        [Hidden]
        public int Permissions
        {
            get => permissions;
            set => this.Change(ref permissions, value, () => Permissions);
        }

        #endregion

        #region Item

        Item() : base() { }

        protected Item(ItemType type, Origin origin, string path) : this()
        {
            Type = type;
            Path = path;
        }

        static Item()
        {
            EventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
        }

        #endregion

        #region Methods

        void From(FileSystemInfo i)
        {
            if (i == null)
                return;

            Accessed
                = i.LastAccessTime;
            Created
                = i.CreationTime;

            //...

            isHidden
                = this is Drive || path == StoragePath.Root ? false : i.Attributes.HasFlag(FileAttributes.Hidden);
            this.Changed(() => IsHidden);

            isReadOnly
                = i.Attributes.HasFlag(FileAttributes.ReadOnly);
            this.Changed(() => IsReadOnly);

            //...

            Modified
                = i.LastWriteTime;
            Name
                = System.IO.Path.GetFileName(i.FullName);
            
            if (i is FileInfo)
            {
                Size = i.To<FileInfo>().Length;
            }
            else if (i is DirectoryInfo) { }
        }

        //...

        public abstract FileSystemInfo Read();

        //...

        public async Task RefreshAsync() => await RefreshAsync(Path);

        public async Task RefreshAsync(string Path) => await Task.Run(() => Refresh(Path));

        //...

        public void Refresh() => From(Read());

        public void Refresh(string path)
        {
            Path = path;
            Refresh();
        }

        #endregion

        #region BaseLong

        public class BaseLong
        {
            protected const int FILE_ATTRIBUTE_ARCHIVE = 0x20;
            protected const int INVALID_FILE_ATTRIBUTES = -1;

            protected const int FILE_READ_DATA = 0x0001;
            protected const int FILE_WRITE_DATA = 0x0002;
            protected const int FILE_APPEND_DATA = 0x0004;
            protected const int FILE_READ_EA = 0x0008;
            protected const int FILE_WRITE_EA = 0x0010;

            protected const int FILE_READ_ATTRIBUTES = 0x0080;
            protected const int FILE_WRITE_ATTRIBUTES = 0x0100;

            protected const int FILE_SHARE_NONE = 0x00000000;
            protected const int FILE_SHARE_READ = 0x00000001;

            protected const int FILE_ATTRIBUTE_DIRECTORY = 0x10;

            protected const long FILE_GENERIC_WRITE = STANDARD_RIGHTS_WRITE | FILE_WRITE_DATA | FILE_WRITE_ATTRIBUTES | FILE_WRITE_EA | FILE_APPEND_DATA | SYNCHRONIZE;

            protected const long FILE_GENERIC_READ = STANDARD_RIGHTS_READ | FILE_READ_DATA | FILE_READ_ATTRIBUTES | FILE_READ_EA | SYNCHRONIZE;

            protected const long READ_CONTROL = 0x00020000L;
            protected const long STANDARD_RIGHTS_READ = READ_CONTROL;
            protected const long STANDARD_RIGHTS_WRITE = READ_CONTROL;

            protected const long SYNCHRONIZE = 0x00100000L;

            protected const int CREATE_NEW = 1;
            protected const int CREATE_ALWAYS = 2;
            protected const int OPEN_EXISTING = 3;

            protected const int MAX_PATH = 260;
            protected const int MAX_ALTERNATE = 14;

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            protected struct WIN32_FIND_DATA
            {
                public FileAttributes dwFileAttributes;
                public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
                public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
                public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
                public uint nFileSizeHigh; //changed all to uint, otherwise you run into unexpected overflow
                public uint nFileSizeLow;  //|
                public uint dwReserved0;   //|
                public uint dwReserved1;   //v
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
                public string cFileName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
                public string cAlternate;
            }

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool CopyFileW(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern int GetFileAttributesW(string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool DeleteFileW(string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool MoveFileW(string lpExistingFileName, string lpNewFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool SetFileTime(SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool GetFileTime(SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool FindClose(IntPtr hFindFile);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool RemoveDirectory(string path);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern bool CreateDirectory(string lpPathName, IntPtr lpSecurityAttributes);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            protected static extern int SetFileAttributesW(string lpFileName, int fileAttributes);
        }

        #endregion
    }
}