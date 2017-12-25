using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imagin.NET.Demo
{
    public class StorageCollection : ConcurrentCollection<StorageObjectModel>
    {
        string lastPath = string.Empty;
        public string LastPath
        {
            get
            {
                return lastPath;
            }
            set
            {
                lastPath = value;
                OnPropertyChanged("LastPath");
            }
        }

        public async void Set(string Path)
        {
            Clear();

            var Items = default(IEnumerable<string>);

            await Task.Run(new Action(() =>
            {
                try
                {
                    Items = Path.IsNullOrEmpty() ? System.IO.Directory.GetLogicalDrives() : System.IO.Directory.EnumerateFileSystemEntries(Path);
                }
                catch
                {
                    Items = null;
                }
            }));

            if (Items != null)
            {
                await App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    foreach (var i in Items)
                        Add(new StorageObjectModel(i));
                }));
            }

            LastPath = Path;
        }

        public StorageCollection(string path) : base()
        {
            var paths = path.IsNullOrEmpty() ? System.IO.Directory.GetLogicalDrives() : System.IO.Directory.GetFileSystemEntries(path);
            foreach (var i in paths)
                Add(new StorageObjectModel(i));
        }
    }
}
