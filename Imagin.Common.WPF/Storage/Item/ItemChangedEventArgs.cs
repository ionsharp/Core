using Imagin.Common.Input;
using System;

namespace Imagin.Common.Storage
{
    public class ItemChangedEventArgs<T> : EventArgs<T>
    {
        public new ItemProperty Parameter => (ItemProperty)base.Parameter;

        public ItemChangedEventArgs(T item, ItemProperty itemProperty) : base(item, itemProperty) { }
    }

    public class ItemCreatedEventArgs<T> : EventArgs<T>
    {
        public ItemCreatedEventArgs(T item) : base(item) { }
    }
    
    public class ItemDeletedEventArgs : EventArgs<string>
    {
        public string Path => Value;

        public ItemDeletedEventArgs(string path) : base(path) { }
    }

    public class ItemRenamedEventArgs : EventArgs
    {
        public readonly string OldPath;

        public readonly string NewPath;

        public ItemRenamedEventArgs(string oldPath, string newPath) : base()
        {
            OldPath = oldPath;
            NewPath = newPath;
        }
    }
}