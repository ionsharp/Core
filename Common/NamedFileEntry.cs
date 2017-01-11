using Imagin.Common.Extensions;
using System;

namespace Imagin.Common
{
    [Serializable]
    public class NamedFileEntry : NamedEntry
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

        public NamedFileEntry() : base()
        {
        }

        public NamedFileEntry(string Path) : this(Path.GetFileName(), Path)
        {
        }

        public NamedFileEntry(string name, string path) : base(name)
        {
            Path = path;
        }

        public override string ToString()
        {
            return path;
        }
    }
}
