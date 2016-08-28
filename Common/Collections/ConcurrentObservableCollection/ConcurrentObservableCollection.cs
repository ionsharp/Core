using Imagin.Common.Extensions;
using Imagin.Common.Collections.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// This class provides a collection that can be bound to
    /// a WPF control, where the collection can be modified from a thread
    /// that is not the GUI thread. The notify event is thrown using the
    /// dispatcher from the event listener(s).
    /// </summary>
    [Serializable]
    public class ConcurrentObservableCollection<T> : ConcurrentObservableBase<T>, IList<T>, ICollection<T>, IList, ICollection, INotifyPropertyChanged
    {
        #region Properties

        [field: NonSerializedAttribute()]
        /// <summary>
        /// Occurs when a single item is added.
        /// </summary>
        public event EventHandler<ItemAddedEventArgs<T>> ItemAdded;

        /// <summary>
        /// Occurs when a single item is inserted.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<ItemInsertedEventArgs<T>> ItemInserted;

        /// <summary>
        /// Occurs when a single item is removed.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<ItemRemovedEventArgs<T>> ItemRemoved;

        [field: NonSerializedAttribute()]
        /// <summary>
        /// Occurs when any number of items are added.
        /// </summary>
        public event EventHandler<ItemsAddedEventArgs<T>> ItemsAdded;

        [field: NonSerializedAttribute()]
        /// <summary>
        /// Occurs when entire collection is cleared.
        /// </summary>
        public event EventHandler<ItemsClearedEventArgs<T>> ItemsCleared;

        [field: NonSerializedAttribute()]
        /// <summary>
        /// Occurs when collection has changed.
        /// </summary>
        public event EventHandler<ItemsChangedEventArgs<T>> ItemsChanged;

        [field: NonSerializedAttribute()]
        /// <summary>
        /// Occurs when items are removed from this.
        /// </summary>
        public event EventHandler<ItemsRemovedEventArgs<T>> ItemsRemoved;

        bool isEmpty = true;
        public bool IsEmpty
        {
            get
            {
                return this.isEmpty;
            }
            set
            {
                this.isEmpty = value;
                this.OnPropertyChanged("IsEmpty");
            }
        }

        int total = 0;
        public int Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
                this.OnPropertyChanged("Total");
            }
        }

        #endregion

        #region ConcurrentObservableCollection

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ConcurrentObservableCollection() : base()
        {
            this.ItemsChanged += ConcurrentObservableCollection_ItemsChanged;
        }

        public ConcurrentObservableCollection(T Item) : base()
        {
            this.ItemsChanged += ConcurrentObservableCollection_ItemsChanged;
            this.Add(Item);
        }

        public ConcurrentObservableCollection(params T[] Items) : base(Items)
        {
            this.ItemsChanged += ConcurrentObservableCollection_ItemsChanged;
        }

        /// <summary>
        /// Constructor that takes an enumerable from which the collection is populated
        /// </summary>
        /// <param name="enumerable"></param>
        public ConcurrentObservableCollection(IEnumerable<T> Items) : base(Items)
        {
            this.ItemsChanged += ConcurrentObservableCollection_ItemsChanged;
        }

        public ConcurrentObservableCollection(params IEnumerable<T>[] Collections) : base()
        {
            this.ItemsChanged += ConcurrentObservableCollection_ItemsChanged;
            foreach (IEnumerable<T> Items in Collections)
            {
                foreach (T t in Items)
                    this.Add(t);
            }
        }

        #endregion

        #region Methods

        #region Events

        void ConcurrentObservableCollection_ItemsChanged(object sender, ItemsChangedEventArgs<T> e)
        {
            int Count = this.Count;
            this.IsEmpty = Count == 0;
            this.Total = Count;
        }

        #endregion

        #region Helpers

        void OnItemAdded(T Item)
        {
            if (this.ItemAdded != null)
                this.ItemAdded(this, new ItemAddedEventArgs<T>(Item));
        }

        void OnItemInserted(T Item, int Index)
        {
            if (this.ItemInserted != null)
                this.ItemInserted(this, new ItemInsertedEventArgs<T>(Item, Index));
        }

        void OnItemRemoved(T Item)
        {
            if (this.ItemRemoved != null)
                this.ItemRemoved(this, new ItemRemovedEventArgs<T>(Item));
        }

        void OnItemsAdded(IEnumerable<T> Items)
        {
            if (this.ItemsAdded != null)
                this.ItemsAdded(this, new ItemsAddedEventArgs<T>(Items));
        }

        void OnItemsCleared(IEnumerable<T> Items)
        {
            if (this.ItemsCleared != null)
                this.ItemsCleared(this, new ItemsClearedEventArgs<T>(Items));
        }

        void OnItemsChanged()
        {
            if (this.ItemsChanged != null)
                this.ItemsChanged(this, new ItemsChangedEventArgs<T>());
        }

        void OnItemsRemoved(IEnumerable<T> OldItems)
        {
            if (this.ItemsRemoved != null)
                this.ItemsRemoved(this, new ItemsRemovedEventArgs<T>(OldItems));
        }

        #endregion

        #region Private

        object GetParent(T Item, IEnumerable<T> Items, uint Level, object Parent = null)
        {
            if (Item == null)
                return null;
            if (Level == 0)
                Parent = this;
            foreach (T t in Items)
            {
                if (t.Equals(Item))
                    return Parent;
                else
                {
                    dynamic T = (dynamic)t;
                    if (DynamicExtensions.PropertyExists(T, "Items") && T.Items is IEnumerable<T>)
                    {
                        object p = this.GetParent(Item, T.Items, 1, t);
                        if (p != null)
                            return p;
                    }
                }
            }
            return null;
        }

        bool IsNull(params T[] Items)
        {
            foreach (T Item in Items)
            {
                if (Item == null)
                    return true;
            }
            return false;
        }

        bool IsNull(IEnumerable<T> Items)
        {
            return Items == null ? true : false;
        }

        async void Move(T Item, Direction Direction)
        {
            object Parent = await this.GetParent(Item);

            if (Parent == null)
                return;

            dynamic p = (dynamic)Parent;

            int Index = -1;
            T Previous = default(T);
            T Next = default(T);
            if (Parent.Equals(this))
            {
                Index = this.IndexOf(Item);
                if (Index == -1)
                    return;
                if (Direction == Direction.Up)
                {
                    if ((Index - 1) >= 0)
                        Previous = this[Index - 1];
                    if (Previous != null)
                    {
                        this.Remove(Previous);
                        this.Insert(Index, Previous);
                    }
                }
                else if (Direction == Direction.Down)
                {
                    if ((Index + 1) < this.Count)
                        Next = this[Index + 1];
                    if (Next != null)
                    {
                        this.Remove(Next);
                        this.Insert(Index, Next);
                    }
                }
            }
            else if (DynamicExtensions.PropertyExists(p, "Items") && p.Items is IEnumerable<T>)
            {
                Index = p.Items.IndexOf(Item);
                if (Index == -1)
                    return;
                if (Direction == Direction.Up)
                {
                    if ((Index - 1) >= 0)
                        Previous = p.Items[Index - 1];
                    if (Previous != null)
                    {
                        p.Items.Remove(Previous);
                        if (Index >= p.Items.Count)
                            p.Items.Add(Previous);
                        else
                            p.Items.Insert(Index, Previous);
                    }
                }
                else if (Direction == Direction.Down)
                {
                    if ((Index + 1) < p.Items.Count)
                        Next = p.Items[Index + 1];
                    if (Next != null)
                    {
                        p.Items.Remove(Next);
                        p.Items.Insert(Index, Next);
                    }
                }
            }
        }

        #endregion

        #region Public

        public void Add(IEnumerable<T> Items)
        {
            foreach (T t in Items)
                this.Add(t);
        }

        public void Add(params T[] Items)
        {
            this.Add(Items as IEnumerable<T>);
        }

        /// <summary>
        /// Adds first item to second.
        /// </summary>
        public void AddTo(T First, T Second)
        {
        }

        public async Task<object> GetParent(T Item)
        {
            object Parent = null;
            await Task.Run(new Action(() => Parent = this.GetParent(Item, this, 0)));
            return Parent;
        }

        /// <summary>
        /// Moves item up one level.
        /// </summary>
        public async void LevelUp(T Item)
        {
            if (this.IsNull(Item))
                return;
            //Get parent of item.
            object p1 = await this.GetParent(Item);
            if (p1 == null || p1.Equals(this))
                return;
            else
            {
                //Get parent of parent
                object p2 = await this.GetParent((T)p1);
                if (p2 == null)
                    return;
                else
                {
                    //Start by removing item from p1.
                    dynamic d1 = (dynamic)p1;
                    if (DynamicExtensions.PropertyExists(d1, "Items") && d1.Items is IEnumerable<T>)
                        d1.Items.Remove(Item);
                    //End with adding item to p2.
                    if (p2.Equals(this))
                        this.Add(Item);
                    else
                    {
                        dynamic d2 = (dynamic)p2;
                        if (DynamicExtensions.PropertyExists(d2, "Items") && d2.Items is IEnumerable<T>)
                            d2.Items.Add(Item);
                    }
                }
            }
        }

        public void MoveUp(T Item)
        {
            this.Move(Item, Direction.Up);
        }

        public void MoveDown(T Item)
        {
            this.Move(Item, Direction.Down);
        }

        /// <summary>
        /// Removes given item recursively.
        /// </summary>
        public async void Remove(T Item, Action Callback = null)
        {
            if (this.IsNull(Item))
                return;
            object Parent = await this.GetParent(Item);
            if (Parent == null)
                return;
            else if (Parent.Equals(this))
                this.Remove(Item);
            else
            {
                dynamic i = (dynamic)Parent;
                if (i.PropertyExists("Items") && i.Items is IEnumerable<T>)
                    i.Items.Remove(Item);
            }
            if (Callback != null)
                Callback.Invoke();
            this.OnItemRemoved(Item);
            this.OnItemsChanged();
        }

        /// <summary>
        /// Removes all occurences of each item in the specified collection.
        /// </summary>
        public void RemoveAll(IEnumerable<T> OldItems)
        {
            if (this.IsNull(OldItems))
                return;
            var set = new HashSet<T>(OldItems);
            var list = this as List<T>;
            int i = 0;
            while (i < this.Count)
            {
                if (set.Contains(this[i]))
                    this.RemoveAt(i);
                else i++;
            }
            this.OnItemsRemoved(OldItems);
            this.OnItemsChanged();
        }

        /// <summary>
        /// Wraps item in a virtual folder via recursive search (assumes Parent.Items exists).
        /// </summary>
        /// <param name="Folder">The container item.</param>
        /// <param name="Item">The item to wrap.</param>
        /// <returns>Result of wrap attempt; false if failure.</returns>
        public async Task<bool> Wrap(T Folder, T Item)
        {
            if (Item == null)
                return false;

            object Parent = await this.GetParent(Item);

            if (Parent == null)
                return false;

            dynamic f = (dynamic)Folder;
            if (!f.PropertyExists("Items") || !(f.Items is IEnumerable<T>))
                return false;

            dynamic p = (dynamic)Parent;
            if (Parent.Equals(this))
            {
                try
                {
                    this.Insert(this.IndexOf(Item), Folder);
                    this.Remove(Item);
                    f.Items.Add(Item);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else if (p.PropertyExists("Items") && p.Items is IEnumerable<T>)
            {
                try
                {
                    p.Items.Insert(p.Items.IndexOf(Item), Folder);
                    p.Items.Remove(Item);
                    f.Items.Add(Item);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region IList<T> 

        public int IndexOf(T item)
        {
            return DoBaseRead(() => {
                return ReadCollection.IndexOf(item);
            });
        }

        public void Insert(int index, T item)
        {
            DoBaseWrite(() =>
            {
                WriteCollection.Insert(index, item);
            });
            this.OnItemsChanged();
        }

        public void RemoveAt(int index)
        {
            DoBaseWrite(() =>
            {
                WriteCollection.RemoveAt(index);
            });
            this.OnItemsChanged();
        }

        public T this[int index]
        {
            get
            {
                return DoBaseRead(() => {
                    return ReadCollection[index];
                });
            }
            set
            {
                DoBaseWrite(() => {
                    WriteCollection[index] = value;
                });
            }
        }

        #endregion

        #region ICollection<T>

        public void Add(T Item)
        {
            DoBaseWrite(() =>
            {
                WriteCollection.Add(Item);
            });
            this.OnItemAdded(Item);
            this.OnItemsChanged();
        }

        public void Clear()
        {
            DoBaseClear(() =>
            {
            });
            this.OnItemsChanged();
        }

        public bool Contains(T item)
        {
            return DoBaseRead(() =>
            {
                return ReadCollection.Contains(item);
            });
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            DoBaseRead(() =>
            {
                if (array.Count() >= ReadCollection.Count)
                    ReadCollection.CopyTo(array, arrayIndex);
                else
                    Console.Out.WriteLine("ConcurrentObservableCollection attempting to copy into wrong sized array (array.Count < ReadCollection.Count)");
            });
        }

        public int Count
        {
            get
            {
                return DoBaseRead(() =>
                {
                    return ReadCollection.Count;
                });
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return DoBaseRead(() => {
                    return ((ICollection<T>)ReadCollection).IsReadOnly;
                });
            }
        }

        public bool Remove(T Item)
        {
            bool Result = DoBaseWrite(() =>
            {
                return WriteCollection.Remove(Item);
            });
            this.OnItemRemoved(Item);
            this.OnItemsChanged();
            return Result;
        }

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            DoBaseRead(() => {
                ((ICollection)ReadCollection).CopyTo(array, index);
            });
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return DoBaseRead(() => {
                    return ((ICollection)ReadCollection).IsSynchronized;
                });
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return DoBaseRead(() => {
                    return ((ICollection)ReadCollection).SyncRoot;
                });
            }
        }

        #endregion 

        #region IList 

        int IList.Add(object value)
        {
            return DoBaseWrite(() => {
                return ((IList)WriteCollection).Add(value);
            });
        }

        bool IList.Contains(object value)
        {
            return DoBaseRead(() => {
                return ((IList)ReadCollection).Contains(value);
            });
        }

        int IList.IndexOf(object value)
        {
            return DoBaseRead(() => {
                return ((IList)ReadCollection).IndexOf(value);
            });
        }

        void IList.Insert(int index, object value)
        {
            DoBaseWrite(() => {
                ((IList)WriteCollection).Insert(index, value);
            });
        }

        bool IList.IsFixedSize
        {
            get
            {
                return DoBaseRead(() => {
                    return ((IList)ReadCollection).IsFixedSize;
                });
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return DoBaseRead(() => {
                    return ((IList)ReadCollection).IsReadOnly;
                });
            }
        }

        void IList.Remove(object value)
        {
            DoBaseWrite(() => {
                ((IList)WriteCollection).Remove(value);
            });
        }

        void IList.RemoveAt(int index)
        {
            DoBaseWrite(() => {
                ((IList)WriteCollection).RemoveAt(index);
            });
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

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #endregion
    }
}
