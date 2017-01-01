using System.Collections.Generic;

namespace Imagin.Common.Collections.Concurrent
{
    public class AbstractObjectCollection : ConcurrentObservableCollection<AbstractObject>
    {
        public AbstractObjectCollection() : base()
        {
        }

        public AbstractObjectCollection(IEnumerable<AbstractObject> Items) : base(Items)
        {
        }
    }
}
