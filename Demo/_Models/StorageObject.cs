using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Imagin.NET.Demo
{
    public class StorageObject : NamedObject
    {
        bool isExpanded = false;
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");

                if (value)
                {
                    Items.Clear();
                    try
                    {
                        foreach (var i in System.IO.Directory.EnumerateFileSystemEntries(Path))
                            Items.Add(new StorageObject(i));
                    }
                    catch
                    {
                        //Do nothing!
                    }
                }
            }
        }

        bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        string path = string.Empty;
        [Imagin.Common.Category("General")]
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
                OnPropertyChanged("Path");
            }
        }

        long size = 0L;
        public long Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
                OnPropertyChanged("Size");
            }
        }

        DateTime accessed = default(DateTime);
        public DateTime Accessed
        {
            get
            {
                return this.accessed;
            }
            set
            {
                this.accessed = value;
                OnPropertyChanged("Accessed");
            }
        }

        DateTime created = default(DateTime);
        public DateTime Created
        {
            get
            {
                return this.created;
            }
            set
            {
                this.created = value;
                OnPropertyChanged("Created");
            }
        }

        DateTime modified = default(DateTime);
        public DateTime Modified
        {
            get
            {
                return this.modified;
            }
            set
            {
                this.modified = value;
                OnPropertyChanged("Modified");
            }
        }

        string type = default(string);
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        ObservableCollection<StorageObject> items = new ObservableCollection<StorageObject>();
        public ObservableCollection<StorageObject> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
                OnPropertyChanged("Items");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        async void Set(string Path)
        {
            await Task.Run(() =>
            {
                bool IsFolder = System.IO.Directory.Exists(Path), IsFile = System.IO.File.Exists(Path);

                if (IsFolder)
                    Set(new System.IO.DirectoryInfo(Path));
                else if (IsFile)
                    Set(new System.IO.FileInfo(Path));

                Name = !IsFolder && !IsFile ? Path : Path.GetFileName();
            });
        }

        void Set(System.IO.FileSystemInfo Info)
        {
            Path = Info.FullName;
            Accessed = Info.LastAccessTime;
            Created = Info.CreationTime;
            Modified = Info.LastWriteTime;
        }

        void Set(System.IO.DirectoryInfo Info)
        {
            Type = "Folder";
            Set(Info as System.IO.FileSystemInfo);
        }

        void Set(System.IO.FileInfo Info)
        {
            Size = Info.Length;
            Type = "File";
            Set(Info as System.IO.FileSystemInfo);
        }

        public StorageObject() : base("New File System Entry")
        {
        }

        public StorageObject(string path) : base()
        {
            Path = path;
            Set(path);
        }
    }
}
