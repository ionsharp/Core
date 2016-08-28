using System;
using System.Collections.Generic;

namespace Imagin.Common.Collections.Events
{
    public class ItemsAddedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> NewItems;
        public ItemsAddedEventArgs(IEnumerable<T> t)
        {
            this.NewItems = t;
        }
    }
}
