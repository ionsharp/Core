using System;

namespace Imagin.Common.Collections.Events
{
    public abstract class NewItemEventArgs<T> : EventArgs
    {
        public T NewItem
        {
            get; set;
        }

        public NewItemEventArgs(T t)
        {
            this.NewItem = t;
        }
    }
}
