using System;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// This class provides the base restrictions for an immutable collection
    /// </summary>
    [Serializable]
    public abstract class ImmutableCollectionBase<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the collection<T>.
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating that the collection is read-only.
        /// </summary>
        public bool IsReadOnly {
            get {
            return true;
            }
        }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Throws the exception System.NotSupportedException:
        /// </param>
        public void Add(T item) {
            throw (new System.NotSupportedException("The Swordfish.NET.Collections.KeyCollection<TKey,TValue> is read-only."));
        }

        /// <summary>
        /// Throws the exception System.NotSupportedException:
        /// </param>
        public void Clear() {
            throw (new System.NotSupportedException("The Swordfish.NET.Collections.KeyCollection<TKey,TValue> is read-only."));
        }

        /// <summary>
        /// Throws the exception System.NotSupportedException:
        /// </param>
        public bool Remove(T item)
        {
            throw (new System.NotSupportedException("The Swordfish.NET.Collections.KeyCollection<TKey,TValue> is read-only."));
        }

        #endregion

        #region Abstract

        /// </summary>
        /// <param name="item">The object to locate</param>
        /// <returns>true if item is found otherwise false</returns>
        public abstract bool Contains(T item);

        /// <summary>
        //  Copies the elements of the collection to an array, starting
        /// at a particular index.
        /// </summary>
        public abstract void CopyTo(T[] array, int arrayIndex);

        /// <summary>
        /// Gets the enumerator for the collection
        /// </summary>
        public abstract IEnumerator<T> GetEnumerator();

        #endregion

        #region Private

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((T[])array, index);
        }

        /// <summary>
        /// Gets the enumerator for the collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ICollection.IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #endregion
    }
}
