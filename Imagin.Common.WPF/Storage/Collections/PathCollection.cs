using System.IO;
using System.Linq;

namespace Imagin.Common.Storage
{
    public class PathCollection : StorageCollection<string>
    {
        public PathCollection() : base() { }

        public PathCollection(Filter filter) : base(filter) { }

        public PathCollection(string path, Filter filter) : base(path, filter) { }

        //...

        protected override string this[string path] => this.FirstOrDefault(i => i == path);

        protected override ItemProperty GetChangedProperty(string input) => ItemProperty.None;

        //...

        protected override string ToDrive(DriveInfo input) => input.Name;

        protected override string ToFile(string input) => input;

        protected override string ToFolder(string input) => input;
    }
}