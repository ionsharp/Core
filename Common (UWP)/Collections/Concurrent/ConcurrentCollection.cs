using System.Collections.Generic;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// Provides a collection that can be modified safely on other threads. The notify event is thrown using the dispatcher from the event listener(s).
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class ConcurrentCollection<T> : ConcurrentCollectionBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentCollection() : base(new ConcurrentCollectionViewModel<T>())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public ConcurrentCollection(T Item) : base(new ConcurrentCollectionViewModel<T>(), Item)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(params T[] Items) : base(new ConcurrentCollectionViewModel<T>(), Items)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(IEnumerable<T> Items) : base(new ConcurrentCollectionViewModel<T>(), Items)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(params IEnumerable<T>[] Items) : base(new ConcurrentCollectionViewModel<T>(), Items)
        {
        }
    }
}
