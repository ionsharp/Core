using Imagin.Common.Events;
using Imagin.Common.Extensions;
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
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    [Serializable]
    public class ConcurrentObservableCollection<T> : ConcurrentObservableBase<T>, ICollection<T>, IList<T>, IHierarchialCollection<T>, ITrackableCollection<T>, IList, ICollection, INotifyPropertyChanged
    {
        #region Properties

        #region ITrackableList<T>

        /// <summary>
        /// Occurs when a single item is added.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<T>> ItemAdded;

        /// <summary>
        /// Occurs when any number of items are added.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsAdded;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs> ItemsChanged;

        /// <summary>
        /// Occurs when the collection is cleared.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs> ItemsCleared;

        /// <summary>
        /// Occurs when a single item is inserted.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<T>> ItemInserted;

        /// <summary>
        /// Occurs when a single item is removed.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<T>> ItemRemoved;

        /// <summary>
        /// Occurs when any number of items are removed.
        /// </summary>
        [field: NonSerializedAttribute()]
        public event EventHandler<EventArgs<IEnumerable<T>>> ItemsRemoved;

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

        #endregion

        #region ConcurrentObservableCollection

        public ConcurrentObservableCollection() : base()
        {
        }

        public ConcurrentObservableCollection(T Item) : base()
        {
            this.Add(Item);
        }

        public ConcurrentObservableCollection(params T[] Items) : base(Items)
        {
        }

        public ConcurrentObservableCollection(IEnumerable<T> Items) : base(Items)
        {
        }

        public ConcurrentObservableCollection(params IEnumerable<T>[] Collections) : base()
        {
            foreach (IEnumerable<T> Items in Collections)
            {
                foreach (T t in Items)
                    this.Add(t);
            }
        }

        #endregion

        #region Methods

        #region Private

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            DoBaseRead(() => ((ICollection)ReadCollection).CopyTo(array, index));
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return DoBaseRead(() =>
                {
                    return ((ICollection)ReadCollection).IsSynchronized;
                });
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return DoBaseRead(() =>
                {
                    return ((ICollection)ReadCollection).SyncRoot;
                });
            }
        }

        #endregion 

        #region IList 

        int IList.Add(object value)
        {
            return DoBaseWrite(() =>
            {
                return ((IList)WriteCollection).Add(value);
            });
        }

        bool IList.Contains(object value)
        {
            return DoBaseRead(() =>
            {
                return ((IList)ReadCollection).Contains(value);
            });
        }

        int IList.IndexOf(object value)
        {
            return DoBaseRead(() =>
            {
                return ((IList)ReadCollection).IndexOf(value);
            });
        }

        void IList.Insert(int index, object value)
        {
            DoBaseWrite(() =>
            {
                ((IList)WriteCollection).Insert(index, value);
            });
        }

        bool IList.IsFixedSize
        {
            get
            {
                return DoBaseRead(() =>
                {
                    return ((IList)ReadCollection).IsFixedSize;
                });
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return DoBaseRead(() =>
                {
                    return ((IList)ReadCollection).IsReadOnly;
                });
            }
        }

        void IList.Remove(object value)
        {
            DoBaseWrite(() => ((IList)WriteCollection).Remove(value));
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

        #endregion

        #region Protected

        protected virtual void OnItemAdded(T Item)
        {
            if (this.ItemAdded != null)
                this.ItemAdded(this, new EventArgs<T>(Item));
        }

        protected virtual void OnItemsAdded(IEnumerable<T> Items)
        {
            if (this.ItemsAdded != null)
                this.ItemsAdded(this, new EventArgs<IEnumerable<T>>(Items));
        }

        protected virtual void OnItemsChanged()
        {
            this.Total = this.Count;
            this.IsEmpty = this.Total == 0;

            if (this.ItemsChanged != null)
                this.ItemsChanged(this, new EventArgs());
        }

        protected virtual void OnItemsCleared()
        {
            if (this.ItemsCleared != null)
                this.ItemsCleared(this, new EventArgs());
        }

        protected virtual void OnItemInserted(T Item, int Index)
        {
            if (this.ItemInserted != null)
                this.ItemInserted(this, new EventArgs<T>(Item, Index));
        }

        protected virtual void OnItemRemoved(T Item)
        {
            if (this.ItemRemoved != null)
                this.ItemRemoved(this, new EventArgs<T>(Item));
        }

        protected virtual void OnItemsRemoved(IEnumerable<T> OldItems)
        {
            if (this.ItemsRemoved != null)
                this.ItemsRemoved(this, new EventArgs<IEnumerable<T>>(OldItems));
        }

        #endregion

        #region Public

        #region ICollection<T>

        public void Add(T Item)
        {
            DoBaseWrite(() => WriteCollection.Add(Item));
            this.OnItemAdded(Item);
            this.OnItemsChanged();
        }

        public void Clear()
        {
            DoBaseClear(null);
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
                else Console.Out.WriteLine("ConcurrentObservableCollection attempting to copy into wrong sized array (array.Count < ReadCollection.Count)");
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
                return DoBaseRead(() =>
                {
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

        #region IHierarchialList<T>

        /// <summary>
        /// Add item A to B.
        /// </summary>
        public bool AddTo(T A, T B)
        {
            if (B.HasProperty("Items") && ((object)B.ToDynamic().Items).Is<IList<T>>())
            {
                B.ToDynamic().Items.Add(A);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add item A to B.
        /// </summary>
        public async Task<bool> BeginAddTo(T A, T B)
        {
            bool Result = false;
            await Task.Run(new Action(() => Result = AddTo(A, B)));
            return Result;
        }

        async Task<object> BeginGetParent(T ToEvaluate, object Parent, bool Start)
        {
            if (Start && ToEvaluate == null) return null;

            var Result = default(object);
            await Task.Run(new Action(async () =>
            {
                IList<T> ToEnumerate = Parent.Equals(this) ? this : Parent.HasProperty("Items") && ((object)Parent.ToDynamic().Items).Is<IList<T>>() ? (IList<T>)Parent.ToDynamic().Items : null;
                if (ToEnumerate == null) return;

                foreach (T i in ToEnumerate)
                {
                    if (i.Equals(ToEvaluate))
                    {
                        Result = ToEnumerate;
                        return;
                    }
                    else if ((Result = await this.BeginGetParent(ToEvaluate, i, false)) != null) return;
                }
            }));
            return Result;
        }

        public async Task<object> BeginGetParent(T Item)
        {
            return await this.BeginGetParent(Item, this, true);
        }

        /// <summary>
        /// Moves item up one level.
        /// </summary>
        public async Task<bool> BeginLevelUp(T Item)
        {
            if (Item.IsNull()) return false;

            var Parent = await this.BeginGetParent(Item);
            if (Parent.IsNull() || Parent.Equals(this)) return false;

            var ParentOfParent = await this.BeginGetParent(Parent.As<T>());
            if (ParentOfParent.IsNull()) return false;

            Parent.ToDynamic().Items.Remove(Item);

            IList<T> Collection = ParentOfParent.Equals(this) ? this : (ParentOfParent.HasProperty("Items") && ((object)ParentOfParent.ToDynamic().Items).Is<IList<T>>() ? (IList<T>)ParentOfParent.ToDynamic().Items : null);
            if (Collection != null)
            {
                Collection.Add(Item);
                return true;
            }
            return false;
        }

        public async Task<bool> BeginMove(T Item, Direction Direction)
        {
            var p = await this.BeginGetParent(Item);

            if (p == null) return false;

            var Parent = (dynamic)p;

            IList<T> Collection = Parent.Equals(this) ? this : (p.HasProperty("Items") && ((object)Parent.Items).Is<IList<T>>() ? (IList<T>)Parent.Items : null);

            int Index = Collection.IsNull() ? -1 : Collection.IndexOf(Item);
            if (Index <= -1) return false;

            var ToRemove = default(T);
            if (Direction == Direction.Up)
            {
                if ((Index - 1) >= 0)
                    ToRemove = Collection[Index - 1];
            }
            else if (Direction == Direction.Down)
            {
                if ((Index + 1) < Collection.Count)
                    ToRemove = Collection[Index + 1];
            }

            if (ToRemove != null)
            {
                Collection.Remove(ToRemove);
                Collection.Insert(Index, ToRemove);
                return true;
            }

            return false;
        }

        public async Task<bool> BeginMoveDown(T Item)
        {
            return await this.BeginMove(Item, Direction.Down);
        }

        public async Task<bool> BeginMoveUp(T Item)
        {
            return await this.BeginMove(Item, Direction.Up);
        }

        /// <summary>
        /// Removes given item recursively.
        /// </summary>
        public async Task<bool> BeginRemove(T Item, Action Callback = null)
        {
            if (Item.IsNull()) return false;

            var p = await this.BeginGetParent(Item);
            if (p == null) return false;

            var Parent = (dynamic)p;

            IList<T> Collection = Parent.Equals(this) ? this : (p.HasProperty("Items") && ((object)Parent.Items).Is<IList<T>>() ? (IList<T>)Parent.Items : null);
            if (Collection != null) Collection.Remove(Item);

            if (Callback != null)
                Callback.Invoke();

            this.OnItemRemoved(Item);
            this.OnItemsChanged();

            return true;
        }

        /// <summary>
        /// Wraps item in a virtual folder via recursive search (assumes Parent.Items exists).
        /// </summary>
        /// <param name="Folder">The container item.</param>
        /// <param name="Item">The item to wrap.</param>
        /// <returns>Result of wrap attempt; false if failure.</returns>
        public async Task<bool> BeginWrap(T Folder, T Item)
        {
            if (Item == null) return false;

            var p = await this.BeginGetParent(Item);
            if (p == null) return false;

            if (!Folder.HasProperty("Items") || !((object)Folder.ToDynamic().Items).Is<IHierarchialCollection<T>>())
                return false;

            IList<T> Collection = p.Equals(this) ? this : (p.HasProperty("Items") && ((object)(p.ToDynamic().Items)).Is<IList<T>>() ? (IList<T>)p.ToDynamic().Items : null);

            var Index = Collection.IndexOf(Item);

            Collection.Remove(Item);
            Folder.ToDynamic().Items.Insert(0, Item);
            Collection.Insert(Index, Folder);

            return false;
        }

        /// <summary>
        /// Removes all occurences of each item in the specified collection.
        /// </summary>
        public bool RemoveAll(IEnumerable<T> OldItems)
        {
            if (OldItems.IsNull()) return false;

            var Set = new HashSet<T>(OldItems);

            int i = 0;
            while (i < this.Count)
            {
                if (Set.Contains(this[i]))
                    this.RemoveAt(i);
                else i++;
            }
            this.OnItemsRemoved(OldItems);
            this.OnItemsChanged();

            return true;
        }

        #endregion

        #region IList<T> 

        public T this[int index]
        {
            get
            {
                return DoBaseRead(() =>
                {
                    return ReadCollection[index];
                });
            }
            set
            {
                DoBaseWrite(() => WriteCollection[index] = value);
            }
        }

        public int IndexOf(T item)
        {
            return DoBaseRead(() =>
            {
                return ReadCollection.IndexOf(item);
            });
        }

        public void Insert(int index, T item)
        {
            DoBaseWrite(() => WriteCollection.Insert(index, item));
            this.OnItemsChanged();
        }

        public void RemoveAt(int index)
        {
            DoBaseWrite(() => WriteCollection.RemoveAt(index));
            this.OnItemsChanged();
        }

        #endregion

        #region ITrackable<T>

        public void Add(IEnumerable<T> Items)
        {
            foreach (var i in Items)
                DoBaseWrite(() => WriteCollection.Add(i));
            this.OnItemsAdded(Items);
            this.OnItemsChanged();
        }

        public void Remove(IEnumerable<T> Items)
        {
            //foreach (var i in Items)
            //base.Remove(i);
            this.OnItemsRemoved(Items);
            this.OnItemsChanged();
        }

        public void RemoveAt(params int[] Indices)
        {
            var Items = new List<T>();
            var j = 0;
            foreach (var i in Indices)
            {
                Items.Add(this[i]);
                //base.Remove(Items[j]);
                j++;
            }
            this.OnItemsRemoved(Items);
            this.OnItemsChanged();
        }

        #endregion

        #endregion

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
