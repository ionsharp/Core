using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// Provides a collection that is immutable or cannot be changed.
    /// </summary>
    [Serializable]
    public class ImmutableCollection<T> : ImmutableCollectionBase<T>
    {
        /// <summary>
        /// The base collection that is wrapped by this class to restrict access
        /// </summary>
        IList<T> Source;

        /// <summary>
        /// 
        /// </summary>
        public override int Count
        {
            get
            {
                return Source.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public ImmutableCollection(IEnumerable<T> source)
        {
            Source = new List<T>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        public ImmutableCollection() : this(Enumerable.Empty<T>())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public override bool Contains(T Item)
        {
            return Source.Contains(Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Array"></param>
        /// <param name="ArrayIndex"></param>
        public override void CopyTo(T[] Array, int ArrayIndex)
        {
            Source.CopyTo(Array, ArrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<T> GetEnumerator()
        {
            return Source.GetEnumerator();
        }
    }
}
