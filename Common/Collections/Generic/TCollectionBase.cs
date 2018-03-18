using System;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class TCollectionBase<T> : IList<T>, IList, IReadOnlyList<T>
    {
        #region Fields

        IList<T> _items;

        [NonSerialized]
        Object _syncRoot;

        #endregion

        #region Indexors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => _items[index];
            set
            {
                if (_items.IsReadOnly)
                    throw new NotSupportedException();

                if (index < 0 || index >= _items.Count)
                    throw new ArgumentOutOfRangeException();

                SetItem(index, value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// 
        /// </summary>
        protected IList<T> Items => _items;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public TCollectionBase() => _items = new List<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public TCollectionBase(IList<T> list) => _items = list ?? throw new ArgumentNullException(nameof(list));

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// Non-null values are fine. Only accept nulls if <see langword="T"/> is a class or <see cref="Nullable{T}"/>. Note that default(T) is not equal to <see langword="null"/> for value types except when <see langword="T"/> is <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static bool IsCompatibleObject(object value) => ((value is T) || (value == null && default(T) == null));

        #endregion

        #region Protected

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ClearItems() => _items.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void InsertItem(int index, T item) => _items.Insert(index, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        protected virtual void RemoveItem(int index) => _items.RemoveAt(index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void SetItem(int index, T item) => _items[index] = item;

        #endregion

        #region Public

        #region Sealed

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(T[] array, int index) => _items.CopyTo(array, index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item) => _items.Contains(item);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item) => _items.IndexOf(item);

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(T item)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            int index = _items.Count;
            InsertItem(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Clear()
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            ClearItems();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public virtual void Insert(int index, T item)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (index < 0 || index > _items.Count)
                throw new ArgumentOutOfRangeException();

            InsertItem(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Remove(T item)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            int index = _items.IndexOf(item);
            if (index < 0) return false;
            RemoveItem(index);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveAt(int index)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException();

            RemoveItem(index);
        }

        #endregion

        #endregion

        #endregion

        #region ICollection

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    ICollection c = _items as ICollection;
                    if (c != null)
                    {
                        _syncRoot = c.SyncRoot;
                    }
                    else
                    {
                        System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                    }
                }
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Rank != 1)
                throw new ArgumentException();

            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException();

            if (index < 0)
                throw new ArgumentOutOfRangeException();

            if (array.Length - index < Count)
                throw new ArgumentException();

            T[] tArray = array as T[];
            if (tArray != null)
            {
                _items.CopyTo(tArray, index);
            }
            else
            {
                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                    throw new ArgumentException();

                //
                // We can't cast array of value type to object[], so we don't support 
                // widening of primitive types here.
                //
                object[] objects = array as object[];
                if (objects == null)
                    throw new ArgumentException();

                int count = _items.Count;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        objects[index++] = _items[i];
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException();
                }
            }
        }

        #endregion

        #region ICollection<T>

        bool ICollection<T>.IsReadOnly => _items.IsReadOnly;

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_items).GetEnumerator();

        #endregion

        #region IList
        object IList.this[int index]
        {
            get { return _items[index]; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException();
                }

            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return _items.IsReadOnly;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                // There is no IList<T>.IsFixedSize, so we must assume that only
                // readonly collections are fixed size, if our internal item 
                // collection does not implement IList.  Note that Array implements
                // IList, and therefore T[] and U[] will be fixed-size.
                IList list = _items as IList;
                if (list != null)
                {
                    return list.IsFixedSize;
                }
                return _items.IsReadOnly;
            }
        }

        int IList.Add(object value)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                Add((T)value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException();
            }

            return this.Count - 1;
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
            {
                return Contains((T)value);
            }
            return false;
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                return IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException();
            }

        }

        void IList.Remove(object value)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (IsCompatibleObject(value))
                Remove((T)value);
        }

        #endregion
    }
}
