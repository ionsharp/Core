using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// Provides a collection that can be modified safely on other threads. The notify event is thrown using the dispatcher from the event listener(s).
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    [Serializable]
    public class ConcurrentCollection<T> : BaseConcurrentCollection<T>, ICollectionChanged, ICollection, ICollection<T>, IList, IList<T>, IPropertyChanged
    {
        #region Events

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        [field: NonSerialized()]
        event CancelEventHandler<object> RemovingInternal;
        event CancelEventHandler<object> ICollectionChanged.Removing
        {
            add => RemovingInternal += value;
            remove => RemovingInternal -= value;
        }
        [field: NonSerialized()]
        public event CancelEventHandler<T> Removing;

        #endregion

        #region Properties

        public int Count 
            => DoBaseRead(() => ReadCollection.Count);

        public bool IsEmpty
            => Count == 0;

        public bool IsNotEmpty
            => !IsEmpty;
        
        public bool IsReadOnly 
            => DoBaseRead(() => ((ICollection<T>)ReadCollection).IsReadOnly);

        #endregion

        #region ConcurrentCollection

        public ConcurrentCollection() : base() { }

        public ConcurrentCollection(params T[] input) : base(input) { }

        public ConcurrentCollection(IEnumerable<T> input) : base(input) { }

        public ConcurrentCollection(params IEnumerable<T>[] Items) : base() => Items.ForEach(i => i.ForEach(j => Add(j)));

        #endregion

        #region Methods

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
            => DoBaseRead(() => ((ICollection)ReadCollection).CopyTo(array, index));

        bool ICollection.IsSynchronized
            => DoBaseRead(() => ((ICollection)ReadCollection).IsSynchronized);

        object ICollection.SyncRoot
            => DoBaseRead(() => ((ICollection)ReadCollection).SyncRoot);

        #endregion

        #region IList 

        object IList.this[int i]
        {
            get => DoBaseRead(() => ((IList)ReadCollection)[i]);
            set
            {
                if (OnRemoving(this[i]).Cancel)
                    return;

                DoBaseWrite(() => ((IList)WriteCollection)[i] = value);
            }
        }

        int IList.Add(object input)
        {
            var result = DoBaseWrite(() => ((IList)WriteCollection).Add(input));
            OnAdded((T)input);
            OnChanged();
            return result;
        }

        void IList.Clear()
            => DoBaseClear(null);

        bool IList.Contains(object input)
            => DoBaseRead(() => ((IList)ReadCollection).Contains(input));

        bool IList.IsFixedSize 
            => DoBaseRead(() => ((IList)ReadCollection).IsFixedSize);

        bool IList.IsReadOnly
            => DoBaseRead(() => ((IList)ReadCollection).IsReadOnly);

        int IList.IndexOf(object input)
            => DoBaseRead(() => ((IList)ReadCollection).IndexOf(input));

        void IList.Insert(int i, object input)
        {
            DoBaseWrite(() => ((IList)WriteCollection).Insert(i, input));
            OnAdded((T)input);
            OnChanged();
        }

        void IList.Remove(object input)
        {
            if (OnRemoving((T)input).Cancel)
                return;

            DoBaseWrite(() => ((IList)WriteCollection).Remove(input));
            OnRemoved((T)input);
            OnChanged();
        }

        void IList.RemoveAt(int i)
        {
            if (OnRemoving(this[i]).Cancel)
                return;

            var item = this[i];
            DoBaseWrite(() => ((IList)WriteCollection).RemoveAt(i));
            OnRemoved(item);
            OnChanged();
        }

        #endregion

        #region Protected (virtual)

        protected virtual void OnAdded(T input) { }

        protected virtual void OnChanged()
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(IsNotEmpty));
        }

        protected virtual void OnRemoved(T input) { }

        protected virtual CancelEventArgs<T> OnRemoving(T input)
        {
            var e = new CancelEventArgs<T>(input);
            OnRemoving(e);
            return e;
        }

        protected virtual void OnRemoving(CancelEventArgs<T> e)
        {
            var f = new CancelEventArgs<object>(e.Value);
            RemovingInternal?.Invoke(this, f);

            e.Cancel = f.Cancel;
            Removing?.Invoke(this, e);
        }

        public virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Public

        public T this[int i]
        {
            get => DoBaseRead(() => ReadCollection[i]);
            set
            {
                if (OnRemoving(this[i]).Cancel)
                    return;

                DoBaseWrite(() => WriteCollection[i] = value);
            }
        }

        public void Add(T input)
        {
            DoBaseWrite(() => WriteCollection.Add(input));
            OnAdded(input);
            OnChanged();
        }

        public virtual void Clear()
            => DoBaseClear(null);

        public bool Contains(T input) 
            => DoBaseRead(() => ReadCollection.Contains(input));

        public void CopyTo(T[] array, int arrayIndex)
        {
            DoBaseRead(() =>
            {
                if (array.Count() >= ReadCollection.Count)
                {
                    ReadCollection.CopyTo(array, arrayIndex);
                }
                else Log.Write<ConcurrentCollection<T>>("Attempted to copy into wrong sized array (array.Count < ReadCollection.Count).");
            });
        }

        public int IndexOf(T input)
            => DoBaseRead(() => ReadCollection.IndexOf(input));

        public void Insert(int i, T input)
        {
            DoBaseWrite(() => WriteCollection.Insert(i, input));
            OnAdded(input);
            OnChanged();
        }

        public bool Remove(T input)
        {
            if (OnRemoving(input).Cancel)
                return false;

            var result = DoBaseWrite(() => WriteCollection.Remove(input));
            OnRemoved(input);
            OnChanged();
            return result;
        }

        public void RemoveAt(int i)
        {
            if (OnRemoving(this[i]).Cancel)
                return;

            var item = this[i];
            DoBaseWrite(() => WriteCollection.RemoveAt(i));
            OnRemoved(item);
            OnChanged();
        }

        #endregion

        #region Public (async)

        public async Task ClearAsync() => await Task.Run(() => Clear());

        #endregion

        #endregion
    }
}