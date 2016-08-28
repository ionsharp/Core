using System;

namespace Imagin.Common.Collections.Events
{
    public class ItemRemovedEventArgs<T> : EventArgs
    {
        public T OldItem;
        public ItemRemovedEventArgs(T t)
        {
            this.OldItem = t;
        }
    }
}
