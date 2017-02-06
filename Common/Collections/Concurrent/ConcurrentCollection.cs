using Imagin.Common.Collections.Generic;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
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
    public class ConcurrentCollection<T> : ConcurrentCollectionBase<T>, ICollection, ICollection<T>, IList, IList<T>, ITrackableCollection, ITrackableCollection<T>, INotifyPropertyChanged
    {
        #region Properties

        event EventHandler<EventArgs<object>> itemAdded;
        event EventHandler<EventArgs<object>> ITrackableCollection.ItemAdded
        {
            add
            {
                itemAdded += value;
            }
            remove
            {
                itemAdded -= value;
            }
        }
        /// <summary>
        /// Occurs when a single item is added.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemAdded;

        event EventHandler<EventArgs<IEnumerable<object>>> itemsAdded;
        event EventHandler<EventArgs<IEnumerable<object>>> ITrackableCollection.ItemsAdded
        {
            add
            {
                itemsAdded += value;
            }
            remove
            {
                itemsAdded -= value;
            }
        }
        /// <summary>
        /// Occurs when any number of items are added.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsAdded;

        event EventHandler<EventArgs> ITrackableCollection.ItemsChanged
        {
            add
            {
                ItemsChanged += value;
            }
            remove
            {
                ItemsChanged -= value;
            }
        }
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event EventHandler<EventArgs> ItemsChanged;

        event EventHandler<EventArgs> ITrackableCollection.ItemsCleared
        {
            add
            {
                ItemsCleared += value;
            }
            remove
            {
                ItemsCleared -= value;
            }
        }
        /// <summary>
        /// Occurs when the collection is cleared.
        /// </summary>
        public event EventHandler<EventArgs> ItemsCleared;

        event EventHandler<EventArgs<object>> itemInserted;
        event EventHandler<EventArgs<object>> ITrackableCollection.ItemInserted
        {
            add
            {
                itemInserted += value;
            }
            remove
            {
                itemInserted -= value;
            }
        }
        /// <summary>
        /// Occurs when a single item is inserted.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemInserted;

        event EventHandler<EventArgs<object>> itemRemoved;
        event EventHandler<EventArgs<object>> ITrackableCollection.ItemRemoved
        {
            add
            {
                itemRemoved += value;
            }
            remove
            {
                itemRemoved -= value;
            }
        }
        /// <summary>
        /// Occurs when a single item is removed.
        /// </summary>
        public event EventHandler<EventArgs<T>> ItemRemoved;

        event EventHandler<EventArgs<IEnumerable<object>>> itemsRemoved;
        event EventHandler<EventArgs<IEnumerable<object>>> ITrackableCollection.ItemsRemoved
        {
            add
            {
                itemsRemoved += value;
            }
            remove
            {
                itemsRemoved -= value;
            }
        }
        /// <summary>
        /// Occurs when any number of items are removed.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsRemoved;

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return DoBaseRead(() => ReadCollection.Count);
            }
        }

        bool isEmpty = true;
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
            set
            {
                isEmpty = value;
                OnPropertyChanged("IsEmpty");
            }
        }
        bool ITrackableCollection.IsEmpty
        {
            get
            {
                return isEmpty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return DoBaseRead(() => ((ICollection<T>)ReadCollection).IsReadOnly);
            }
        }

        #endregion

        #region ConcurrentCollection

        /// <summary>
        /// 
        /// </summary>
        public ConcurrentCollection() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public ConcurrentCollection(T Item) : base()
        {
            Add(Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(params T[] Items) : base(Items)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(IEnumerable<T> Items) : base(Items)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(params IEnumerable<T>[] Items) : base()
        {
            Items.ForEach<IEnumerable<T>>(i => i.ForEach(j => Add(j)));
        }

        #endregion

        #region Methods

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            DoBaseRead(() => ((ICollection)ReadCollection).CopyTo(array, index));
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return DoBaseRead(() => ((ICollection)ReadCollection).IsSynchronized);
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return DoBaseRead(() => ((ICollection)ReadCollection).SyncRoot);
            }
        }

        #endregion 

        #region IList 

        int IList.Add(object Item)
        {
            var Result = DoBaseWrite(() => ((IList)WriteCollection).Add(Item));
            OnItemAdded((T)Item);
            OnItemsChanged();
            return Result;
        }

        bool IList.Contains(object value)
        {
            return DoBaseRead(() => ((IList)ReadCollection).Contains(value));
        }

        bool IList.IsFixedSize
        {
            get
            {
                return DoBaseRead(() => ((IList)ReadCollection).IsFixedSize);
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return DoBaseRead(() => ((IList)ReadCollection).IsReadOnly);
            }
        }

        int IList.IndexOf(object Item)
        {
            return DoBaseRead(() => ((IList)ReadCollection).IndexOf(Item));
        }

        object IList.this[int index]
        {
            get
            {
                return DoBaseRead(() => {
                    return ((IList)ReadCollection)[index];
                });
            }
            set
            {
                DoBaseWrite(() => {
                    ((IList)WriteCollection)[index] = value;
                });
            }
        }

        void IList.Insert(int i, object Item)
        {
            DoBaseWrite(() => ((IList)WriteCollection).Insert(i, Item));
            OnItemInserted((T)Item, i);
            OnItemsChanged();
        }

        void IList.Remove(object Item)
        {
            DoBaseWrite(() => ((IList)WriteCollection).Remove(Item));
            OnItemRemoved((T)Item);
            OnItemsChanged();
        }

        void IList.RemoveAt(int i)
        {
            var Item = this[i];
            DoBaseWrite(() => ((IList)WriteCollection).RemoveAt(i));
            OnItemRemoved(Item);
            OnItemsChanged();
        }

        #endregion

        #region Public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return DoBaseRead(() => ReadCollection[index]);
            }
            set
            {
                DoBaseWrite(() => WriteCollection[index] = value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public void Add(T Item)
        {
            DoBaseWrite(() => WriteCollection.Add(Item));
            OnItemAdded(Item);
            OnItemsChanged();
        }
        void ITrackableCollection.Add(object Item)
        {
            Add((T)Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public void Add(IEnumerable<T> Items)
        {
            Items.ForEach(i => DoBaseWrite(() => WriteCollection.Add(i)));
            OnItemsAdded(Items);
            OnItemsChanged();
        }
        void ITrackableCollection.Add(IEnumerable<object> Items)
        {
            Add(Items.Cast<T>());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            DoBaseClear(null);
            OnItemsCleared();
            OnItemsChanged();
        }
        void ITrackableCollection.Clear()
        {
            Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return DoBaseRead(() =>
            {
                return ReadCollection.Contains(item);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            DoBaseRead(() =>
            {
                if (array.Count() >= ReadCollection.Count)
                    ReadCollection.CopyTo(array, arrayIndex);
                else Console.Out.WriteLine("ConcurrentObservableCollection attempting to copy into wrong sized array (array.Count < ReadCollection.Count)");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return DoBaseRead(() => ReadCollection.IndexOf(item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="Item"></param>
        public void Insert(int i, T Item)
        {
            DoBaseWrite(() => WriteCollection.Insert(i, Item));
            OnItemInserted(Item, i);
            OnItemsChanged();
        }
        void ITrackableCollection.Insert(int i, object Item)
        {
            Insert(i, (T)Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool Remove(T Item)
        {
            var Result = DoBaseWrite(() => WriteCollection.Remove(Item));
            OnItemRemoved(Item);
            OnItemsChanged();
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        void ITrackableCollection<T>.Remove(T Item)
        {
            DoBaseWrite(() => ((ITrackableCollection<T>)WriteCollection).Remove(Item));
            OnItemRemoved(Item);
            OnItemsChanged();
        }
        void ITrackableCollection.Remove(object Item)
        {
            Remove((T)Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public void Remove(IEnumerable<T> Items)
        {
            if (Items?.Count() > 0)
            {
                var Set = new HashSet<T>(Items);

                var i = 0;
                while (i < Count)
                {
                    if (Set.Contains(this[i]))
                        RemoveAt(i);
                    else i++;
                }

                OnItemsRemoved(Items);
                OnItemsChanged();
            }
        }
        void ITrackableCollection.Remove(IEnumerable<object> Items)
        {
            Remove(Items.Cast<T>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public void RemoveAt(int i)
        {
            var Item = this[i];
            DoBaseWrite(() => WriteCollection.RemoveAt(i));
            OnItemRemoved(Item);
            OnItemsChanged();
        }
        void ITrackableCollection.RemoveAt(int i)
        {
            RemoveAt(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Indices"></param>
        public void RemoveAt(params int[] Indices)
        {
            var Items = new List<T>();
            var j = 0;
            foreach (var i in Indices)
            {
                Items.Add(this[i]);
                Remove(Items[j]);
                j++;
            }
            OnItemsRemoved(Items);
            OnItemsChanged();
        }
        void ITrackableCollection.RemoveAt(params int[] Indices)
        {
            RemoveAt(Indices);
        }

        #endregion

        #region Public (Async)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task BeginClear()
        {
            await Task.Run(() => Clear());
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected virtual void OnItemAdded(T Item)
        {
            itemAdded?.Invoke(this, new EventArgs<object>(Item));
            ItemAdded?.Invoke(this, new EventArgs<T>(Item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        protected virtual void OnItemsAdded(IEnumerable<T> Items)
        {
            itemsAdded?.Invoke(this, new EventArgs<IEnumerable<object>>(Items.As<IEnumerable>()?.Cast<object>() ?? Enumerable.Empty<object>()));
            ItemsAdded?.Invoke(this, new EventArgs<IEnumerable<T>>(Items));
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnItemsChanged()
        {
            OnPropertyChanged("Count");
            IsEmpty = Count == 0;

            ItemsChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnItemsCleared()
        {
            ItemsCleared?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Index"></param>
        protected virtual void OnItemInserted(T Item, int Index)
        {
            itemInserted?.Invoke(this, new EventArgs<object>(Item, Index));
            ItemInserted?.Invoke(this, new EventArgs<T>(Item, Index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected virtual void OnItemRemoved(T Item)
        {
            itemRemoved?.Invoke(this, new EventArgs<object>(Item));
            ItemRemoved?.Invoke(this, new EventArgs<T>(Item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldItems"></param>
        protected virtual void OnItemsRemoved(IEnumerable<T> OldItems)
        {
            itemsRemoved?.Invoke(this, new EventArgs<IEnumerable<object>>(OldItems.As<IEnumerable>()?.Cast<object>() ?? Enumerable.Empty<object>()));
            ItemsRemoved?.Invoke(this, new EventArgs<IEnumerable<T>>(OldItems));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        public virtual void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        #endregion

        #endregion
    }
}
