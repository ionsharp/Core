using System;

namespace Imagin.Common.Storage
{
    [Serializable]
    public class Favorite : Base
    {
        string path;
        public string Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        public Favorite() { }

        public Favorite(string path)
        {
            Path = path;
        }
    }
}