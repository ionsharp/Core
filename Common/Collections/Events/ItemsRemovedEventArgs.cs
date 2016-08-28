using System.Collections.Generic;

namespace Imagin.Common.Collections.Events
{
    public class ItemsRemovedEventArgs<T> : RemovedEventArgs<T>
    {
        public ItemsRemovedEventArgs(IEnumerable<T> Old) : base(Old) { }
    }
}
