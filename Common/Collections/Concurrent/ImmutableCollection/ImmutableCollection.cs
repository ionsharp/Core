using System;
using System.Collections.Generic;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// This class provides a functional immutable collection
    /// </summary>
    [Serializable]
    public class ImmutableCollection<T> : ImmutableCollectionBase<T>
    {
        #region Properties

        /// <summary>
        /// The base collection that is wrapped by this class to restrict access
        /// </summary>
        IList<T> baseCollection;

        #endregion Private Fields

        #region ImmutableCollection

        public ImmutableCollection(IEnumerable<T> source)
        {
            baseCollection = new List<T>(source);
        }

        public ImmutableCollection()
        {
            baseCollection = new List<T>();
        }

        #endregion Constructors

        #region Methods

        public override int Count
        {
            get
            {
                return baseCollection.Count;
            }
        }

        public override bool Contains(T Item)
        {
            return baseCollection.Contains(Item);
        }

        public override void CopyTo(T[] Array, int ArrayIndex)
        {
            baseCollection.CopyTo(Array, ArrayIndex);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return baseCollection.GetEnumerator();
        }

        #endregion 
    }
}
