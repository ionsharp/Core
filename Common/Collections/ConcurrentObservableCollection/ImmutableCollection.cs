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
        IList<T> _baseCollection;

        #endregion Private Fields

        #region ImmutableCollection

        public ImmutableCollection(IEnumerable<T> source) {
            _baseCollection = new List<T>(source);
        }

        public ImmutableCollection() {
            _baseCollection = new List<T>();
        }

        #endregion Constructors

        #region Methods

        #region Overrides

        public override int Count {
            get {
            return _baseCollection.Count;
            }
        }

        public override bool Contains(T item) {
            return _baseCollection.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex) {
            _baseCollection.CopyTo(array, arrayIndex);
        }

        public override IEnumerator<T> GetEnumerator() {
            return _baseCollection.GetEnumerator();
        }

        #endregion 

        #endregion 
    }
}
