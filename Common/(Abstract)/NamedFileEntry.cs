using Imagin.Common.Extensions;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NamedFileEntry : NamedEntry
    {
        string path = string.Empty;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public NamedFileEntry() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        public NamedFileEntry(string Path) : this(Path.GetFileName(), Path)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public NamedFileEntry(string name, string path) : base(name)
        {
            Path = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return path;
        }
    }
}
