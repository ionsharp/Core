using Imagin.Common.Extensions;

namespace Imagin.Common.IO
{
    public class FileSystemObject : NamedObject
    {
        private string path = string.Empty;
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

        public FileSystemObject() : base()
        {
        }

        public FileSystemObject(string Path) : this(Path.GetFileName(), Path)
        {
        }

        public FileSystemObject(string Name, string Path) : base(Name)
        {
            this.Path = Path;
        }
    }
}
