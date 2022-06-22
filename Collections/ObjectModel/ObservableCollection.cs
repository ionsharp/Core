using Imagin.Core.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Imagin.Core.Collections.Generic
{
    /// <summary>
    /// Implementation of a dynamic data collection based on generic <see cref="ObservableCollection{T}"/>, implementing <see cref="INotifyCollectionChanged"/> to notify listeners when items get added, removed or the whole list is refreshed.
    /// </summary>
    /// <remarks>
    /// Extended version of <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.
    /// </remarks>
    [Ignore(nameof(Count)), Serializable]
    public class ObservableCollection<T> : Collection<T>, ICollectionChanged, IPropertyChanged
    {
        #region (class) SimpleMonitor

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

        #region (event) ICollectionChanged.CollectionChanged

        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item.
        /// </summary>
        /// <remarks>
        /// see <seealso cref="INotifyCollectionChanged"/>
        /// </remarks>
        [field: NonSerializedAttribute()]
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region (event) IPropertyChanged.PropertyChanged

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        [field: NonSerialized]
        protected virtual event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }

        #endregion

        #region (event) Removing

        [field: NonSerialized]
        public event CancelEventHandler<object> Removing;

        #endregion

        #region Fields

        //This must agree with Binding.IndexerName.  It is declared separately here so as to avoid a dependency on PresentationFramework.dll.
        private const string IndexerName = "Item[]";

        private SimpleMonitor _monitor = new SimpleMonitor();

        #endregion

        #region Properties

        [Hidden]
        public bool IsEmpty => Count == 0;

        [Hidden]
        public bool IsNotEmpty => !IsEmpty;

        #endregion

        #region BindableCollection

        /// <summary>
        /// Initializes a new instance of <see cref="ObservableCollection{T}"/> that is empty and has default initial capacity.
        /// </summary>
        public ObservableCollection() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class
        /// that contains elements copied from the specified list
        /// </summary>
        /// <param name="list">The list whose elements are copied to the new list.</param>
        /// <remarks>
        /// The elements are copied onto the <see cref="ObservableCollection{T}"/> in the same order they are read by the enumerator of the list.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> list is a null reference </exception>
        public ObservableCollection(List<T> list)
            : base((list != null) ? new List<T>(list.Count) : list)
        {
            // Workaround for VSWhidbey bug 562681 (tracked by Windows bug 1369339).
            // We should be able to simply call the base(list) ctor.  But Collection<T>
            // doesn't copy the list (contrary to the documentation) - it uses the
            // list directly as its storage.  So we do the copying here.
            // 
            CopyFrom(list);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that contains
        /// elements copied from the specified collection and has sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <remarks>
        /// The elements are copied onto the <see cref="ObservableCollection{T}"/> in the same order they are read by the enumerator of the collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> collection is a null reference </exception>
        public ObservableCollection(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

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
                    {
                        items.Add(enumerator.Current);
                    }
                }
            }
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event to any listeners
        /// </summary>
        void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        /// <summary>
        /// Helper to raise CollectionChanged event with action == Reset to any listeners
        /// </summary>
        void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #region Protected

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when the list is being cleared;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void ClearItems()
        {
            CheckReentrancy();
            for (int i = 0, j = Count; i < j; i++)
                RemoveAt(0);

            /*
            base.ClearItems();
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(IsNotEmpty));

            OnPropertyChanged(IndexerName);
            OnCollectionReset();
            */
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when an item is removed from list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected override void RemoveItem(int index)
        {
            CheckReentrancy();
            T removedItem = this[index];

            var e = new CancelEventArgs<object>(removedItem);
            OnRemoving(e);

            if (e.Cancel)
                return;

            base.RemoveItem(index);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(IsNotEmpty));

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

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(IsNotEmpty));

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

        //...

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

        #endregion

        #region Virtual

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
        /// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        protected virtual void OnRemoving(CancelEventArgs<object> e) => Removing?.Invoke(this, e);

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #endregion
    }
}