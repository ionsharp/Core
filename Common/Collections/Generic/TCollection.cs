using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// Implementation of a dynamic data collection based on generic Collection&lt;T&gt;,
    /// implementing INotifyCollectionChanged to notify listeners
    /// when items get added, removed or the whole list is refreshed.
    /// </summary>
    [Serializable]
    public class TCollection<T> : TCollectionBase<T>, INotifyCollectionChanged, INotifyPropertyChanged, IObservableCollection, IObservableCollection<T>
    {
        #region Classes

        /// <summary>
        /// This helps prevent reentrant calls.
        /// </summary>
        class SimpleMonitor : IDisposable
        {
            int _busyCount;

            public bool Busy => _busyCount > 0;

            public void Enter() => ++_busyCount;

            public void Dispose() => --_busyCount;
        }

        #endregion

        #region Events
        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item.
        /// </summary>
        /// <remarks>
        /// see <seealso cref="INotifyCollectionChanged"/>
        /// </remarks>
        [field: NonSerialized()]
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }

        event EventHandler<EventArgs<object>> itemAdded;
        event EventHandler<EventArgs<object>> IObservableCollection.ItemAdded
        {
            add => itemAdded += value;
            remove => itemAdded -= value;
        }
        /// <summary>
        /// Occurs when a single item is added.
        /// </summary>
        [field: NonSerialized()]
        public event EventHandler<EventArgs<T>> ItemAdded;

        event EventHandler<EventArgs> IObservableCollection.ItemsChanged
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
        [field: NonSerialized()]
        public event EventHandler<EventArgs> ItemsChanged;

        event EventHandler<EventArgs> IObservableCollection.ItemsCleared
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
        [field: NonSerialized()]
        public event EventHandler<EventArgs> ItemsCleared;

        event EventHandler<EventArgs<object>> itemInserted;
        event EventHandler<EventArgs<object>> IObservableCollection.ItemInserted
        {
            add => itemInserted += value;
            remove => itemInserted -= value;
        }
        /// <summary>
        /// Occurs when a single item is inserted.
        /// </summary>
        [field: NonSerialized()]
        public event EventHandler<EventArgs<T>> ItemInserted;

        event EventHandler<EventArgs<object>> itemRemoved;
        event EventHandler<EventArgs<object>> IObservableCollection.ItemRemoved
        {
            add => itemRemoved += value;
            remove => itemRemoved -= value;
        }
        /// <summary>
        /// Occurs when a single item is removed.
        /// </summary>
        [field: NonSerialized()]
        public event EventHandler<EventArgs<T>> ItemRemoved;

        event EventHandler<EventArgs> IObservableCollection.PreviewItemsCleared
        {
            add
            {
                PreviewItemsCleared += value;
            }
            remove
            {
                PreviewItemsCleared -= value;
            }
        }
        /// <summary>
        /// Occurs just before the collection is cleared.
        /// </summary>
        [field: NonSerialized()]
        public event EventHandler<EventArgs> PreviewItemsCleared;

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        [field: NonSerialized()]
        protected virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Fields

        const string CountString = "Count";

        /// <summary>
        /// This must agree with Binding.IndexerName. It is declared separately here so as to avoid a dependency on PresentationFramework.dll.
        /// </summary>
        const string IndexerName = "Item[]";

        SimpleMonitor _monitor = new SimpleMonitor();

        #endregion 

        #region Properties

        bool _isEmpty = true;
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get => _isEmpty;
            set => OnPropertyChanged(nameof(IsEmpty));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of ObservableCollection that is empty and has default initial capacity.
        /// </summary>
        public TCollection() : base() { }

        /// <summary>
        /// Initializes a new instance of the ObservableCollection class
        /// that contains elements copied from the specified list
        /// </summary>
        /// <param name="list">The list whose elements are copied to the new list.</param>
        /// <remarks>
        /// The elements are copied onto the ObservableCollection in the
        /// same order they are read by the enumerator of the list.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> list is a null reference </exception>
        public TCollection(List<T> list) : base((list != null) ? new List<T>(list.Count) : list) => CopyFrom(list);

        /// <summary>
        /// Initializes a new instance of the ObservableCollection class that contains
        /// elements copied from the specified collection and has sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <remarks>
        /// The elements are copied onto the ObservableCollection in the
        /// same order they are read by the enumerator of the collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> collection is a null reference </exception>
        public TCollection(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            CopyFrom(collection);
        }

        #endregion

        #region Methods

        #region Private

        void CopyFrom(IEnumerable<T> collection)
        {
            IList<T> items = Items;
            if (collection != null && items != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                        items.Add(enumerator.Current);
                }
            }
        }

        void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void OnPropertyChanged(string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Protected

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when the list is being cleared;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void ClearItems()
        {
            CheckReentrancy();
            base.ClearItems();
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionReset();
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when an item is removed from list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void RemoveItem(int index)
        {
            CheckReentrancy();
            T removedItem = this[index];

            base.RemoveItem(index);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when an item is added to list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void InsertItem(int index, T item)
        {
            CheckReentrancy();
            base.InsertItem(index, item);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when an item is set in list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void SetItem(int index, T item)
        {
            CheckReentrancy();
            T originalItem = this[index];
            base.SetItem(index, item);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
        }

        /// <summary>
        /// Called by base class ObservableCollection&lt;T&gt; when an item is to be moved within the list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T removedItem = this[oldIndex];

            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, removedItem);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        }

        /// <summary>
        /// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        /// <summary>
        /// Raise CollectionChanged event to any listeners.
        /// Properties/methods modifying this ObservableCollection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
        /// </remarks>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                using (BlockReentrancy())
                {
                    CollectionChanged(this, e);
                }
            }
        }

        /// <summary>
        /// Disallow reentrant attempts to change this collection. E.g. a event handler
        /// of the CollectionChanged event is not allowed to make changes to this collection.
        /// </summary>
        /// <remarks>
        /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
        /// <code>
        ///         using (BlockReentrancy())
        ///         {
        ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
        ///         }
        /// </code>
        /// </remarks>
        protected IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        /// <summary> Check and assert for reentrant attempts to change this collection. </summary>
        /// <exception cref="InvalidOperationException"> raised when changing the collection
        /// while another collection change is still being notified to other listeners </exception>
        protected void CheckReentrancy()
        {
            if (_monitor.Busy)
            {
                // we can allow changes if there's only one listener - the problem
                // only arises if reentrant changes make the original event args
                // invalid for later listeners.  This keeps existing code working
                // (e.g. Selector.SelectedItems).
                if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
                    throw new InvalidOperationException();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Move item at oldIndex to newIndex.
        /// </summary>
        public void Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public override void Add(T Item)
        {
            base.Add(Item);
            OnItemAdded(Item);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            OnPreviewItemsCleared();
            base.Clear();
            OnItemsCleared();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="Item"></param>
        public override void Insert(int i, T Item)
        {
            base.Insert(i, Item);
            OnItemInserted(Item, i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public override bool Remove(T Item)
        {
            var result = base.Remove(Item);
            OnItemRemoved(Item);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public override void RemoveAt(int i)
        {
            var Item = this[i];
            base.RemoveAt(i);
            OnItemRemoved(Item);
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnItemAdded(T item)
        {
            itemAdded?.Invoke(this, new EventArgs<object>(item));
            ItemAdded?.Invoke(this, new EventArgs<T>(item));
            OnItemsChanged();
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
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        protected virtual void OnItemInserted(T item, int index)
        {
            itemInserted?.Invoke(this, new EventArgs<object>(item, index));
            ItemInserted?.Invoke(this, new EventArgs<T>(item, index));
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnItemRemoved(T item)
        {
            itemRemoved?.Invoke(this, new EventArgs<object>(item));
            ItemRemoved?.Invoke(this, new EventArgs<T>(item));
            OnItemsChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnPreviewItemsCleared() => PreviewItemsCleared?.Invoke(this, new EventArgs());

        #endregion

        #endregion

        #region IObservableCollection

        void IObservableCollection.Add(object Item) => Add((T)Item);

        void IObservableCollection.Clear() => Clear();

        void IObservableCollection.Insert(int i, object Item) => Insert(i, (T)Item);

        bool IObservableCollection.Remove(object Item) => Remove((T)Item);

        void IObservableCollection.RemoveAt(int i) => RemoveAt(i);

        #endregion
    }
}
