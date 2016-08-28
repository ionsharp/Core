using System.Collections.Generic;

namespace Imagin.Common.Collections.Events
{
    public class ItemsClearedEventArgs<T> : RemovedEventArgs<T>
    {
        public ItemsClearedEventArgs(IEnumerable<T> Old) : base(Old) { }
    }
}
