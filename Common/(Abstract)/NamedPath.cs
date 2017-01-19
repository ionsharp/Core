using Imagin.Common.Extensions;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NamedFile : NamedObject
    {
        protected string path = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public virtual string Path
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

        public NamedFile() : base()
        {
        }

        public NamedFile(string Path) : this(Path.GetFileName(), Path)
        {
        }

        public NamedFile(string name, string path) : base(name)
        {
            Path = path;
        }

        public override string ToString()
        {
            return path;
        }
    }
}
