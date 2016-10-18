using System;
using Imagin.Common.Extensions;

namespace Imagin.Common.IO
{
    [Serializable]
    public class FileEntry : NamedEntry
    {
        string path = string.Empty;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        public override string ToString()
        {
            return path;
        }

        public FileEntry() : base()
        {
        }

        public FileEntry(string Path) : this(Path.GetFileName(), Path)
        {
        }

        public FileEntry(string Name, string Path) : base(Name)
        {
            this.Name = Name;
            this.Path = Path;
        }
    }
}
