using Imagin.Common.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// This class provides the base for concurrent collections that can be bound to user interface elements
    /// </summary>
    /// <notes>
    /// Could do with a more performant enumerable implementation
    /// but this is what I have so far. I create a snapshot of the collection
    /// and use the enumerable from that. When the collection is updated
    /// I set a flag indicating that a new snapshot is required.
    /// </notes>
    [Serializable]
    public abstract class BaseConcurrentCollection<T> : IDisposable, IEnumerable<T>, INotifyCollectionChanged, IObservable<NotifyCollectionChangedEventArgs>
    {
        #region Properties

        /// <summary>
        /// Gets if the calling thread is the same as the dispatcher thread
        /// </summary>
        protected static bool IsDispatcherThread => DispatcherQueueProcessor.Instance.IsDispatcherThread;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #region Private

        /// <summary>
        /// The lock that controls read/write access to the base collection
        /// </summary>
        readonly ReaderWriterLockSlim ReadWriteLock = new();

        /// <summary>
        /// The underlying base enumerable that is used to store the items,
        /// used for creating an immutable collection from which an enumerator
        /// can be obtained.
        /// </summary>
        readonly System.Collections.ObjectModel.ObservableCollection<T> BaseCollection;

        /// <summary>
        /// Flag indicating that a write has occured, so anything that depends on
        /// taking a snapshot of the collection needs to be updated.
        /// </summary>
        bool NewSnapshotRequired = false;

        /// <summary>
        /// The enumerable lock to prevent threading conflicts on allocating
        /// the enumerable of the fixed collection
        /// </summary>
        readonly ReaderWriterLockSlim SnapshotLock = new();

        /// <summary>
        /// The collection used for generating an enumerable that iterates
        /// over a snapshot of the base collection
        /// </summary>
        BaseImmutableCollection<T> BaseSnapshot;

        /// <summary>
        /// A list of observers
        /// </summary>
        readonly Dictionary<int, IObserver<NotifyCollectionChangedEventArgs>> Subscribers;

        /// <summary>
        /// The key for new observers, incremented with each new observer
        /// </summary>
        int SubscriberKey;

        /// <summary>
        /// Flag indicating this collection is disposed
        /// </summary>
        bool IsDisposed;

        #endregion

        #region Public 

        /// <summary>
        /// Gets an immutable snapshot of the collection
        /// </summary>
        public BaseImmutableCollection<T> Snapshot
        {
            get
            {
                return DoBaseRead(() => 
                {
                    UpdateSnapshot();
                    return BaseSnapshot;
                });
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// 
        /// </summary>
        protected System.Collections.ObjectModel.ObservableCollection<T> ReadCollection
        {
            get
            {
                if (IsDispatcherThread)
                {
                    return ViewModel;
                }
                else
                {
                    return WriteCollection;
                }
            }
        }

        /// <summary>
        /// The view model that is used to allow this collection to be bound to the UI.
        /// Relevant methods determine if they are being called on the UI thread, and if
        /// so then the view model is used.
        /// </summary>
        readonly ConcurrentCollectionViewModel<T> viewModel;
        /// <summary>
        /// Access this directly if getting the error "An ItemsControl is inconsistent with its items source".
        /// </summary>
        protected ConcurrentCollectionViewModel<T> ViewModel
        {
            get
            {
                return viewModel;
            }
        }

        /// <summary>
        /// Gets the base collection that holds the values
        /// </summary>
        protected System.Collections.ObjectModel.ObservableCollection<T> WriteCollection
        {
            get
            {
                return BaseCollection;
            }
        }

        #endregion

        #endregion

        #region ConcurrentCollectionBase

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected BaseConcurrentCollection() : this(new T[]{})
        {
        }

        /// <summary>
        /// Constructor that takes an eumerable
        /// </summary>
        protected BaseConcurrentCollection(IEnumerable<T> Items)
        {
            Subscribers = new Dictionary<int, IObserver<NotifyCollectionChangedEventArgs>>();

            BaseCollection = new System.Collections.ObjectModel.ObservableCollection<T>(Items);
            BaseSnapshot = new ImmutableCollection<T>(Items);

            // subscribers must be initialized befor calling this as it may
            // subscribe immediately
            viewModel = new ConcurrentCollectionViewModel<T>(this);

            // Handle when the base collection changes. Event will be passed through
            // the IObservable.OnNext method.
            BaseCollection.CollectionChanged += HandleBaseCollectionChanged;

            // Bubble up the notify collection changed event from the view model
            viewModel.CollectionChanged += (sender, e) => CollectionChanged?.Invoke(sender, e);
        }

        ~BaseConcurrentCollection() {
            Dispose(false);
        }

        #endregion

        #region Methods

        #region IDisposable

        protected virtual void Dispose(bool Disposing)
        {
            if (Disposing)
                GC.SuppressFinalize(this);

            OnCompleted();
            IsDisposed = true;
        }

        public virtual void Dispose() => Dispose(true);

        #endregion

        #region IObservable

        protected void OnNext(NotifyCollectionChangedEventArgs value)
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Observable<T>");

            foreach (var i in Subscribers.Select(kv => kv.Value))
                i.OnNext(value);
        }

        protected void OnError(Exception Exception)
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Observable<T>");

            if (Exception == null)
                throw new ArgumentNullException("Exception");

            foreach (var i in Subscribers.Select(kv => kv.Value))
                i.OnError(Exception);
        }

        protected void OnCompleted()
        {
            if (IsDisposed)
                throw new ObjectDisposedException("Observable<T>");

            foreach (var i in Subscribers.Select(kv => kv.Value))
                i.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<NotifyCollectionChangedEventArgs> Observer)
        {
            if (Observer == null)
                throw new ArgumentNullException("observer");

            return DoBaseWrite(() =>
            {
                var Key = SubscriberKey++;

                Subscribers.Add(Key, Observer);
                UpdateSnapshot();

                foreach (var i in BaseSnapshot)
                    Observer.OnNext(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, i));

                return new DisposeDelegate(() => DoBaseWrite(() => Subscribers.Remove(Key)));
            });
        }

        /// <summary>
        /// Result returned from <see cref="Subscribe"/> method.
        /// </summary>
        class DisposeDelegate : IDisposable
        {
            readonly Action dispose;

            public DisposeDelegate(Action Dispose)
            {
                dispose = Dispose;
            }

            public void Dispose()
            {
                dispose();
            }
        }

        #endregion

        #region IEnumerable<T>

        /// <summary>
        /// Gets the enumerator for a snapshot of the collection
        /// </summary>
        /// <remarks>
        /// Note that the Enumerator should really only be used on the Dispatcher thread,
        /// if not then should enumerate over the Snapshot instead.
        /// 
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            return IsDispatcherThread ? viewModel.GetEnumerator() : Snapshot.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for a snapshot of the collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private

        /// <summary>
        /// Updates the snapshot that is used to generate an Enumerator
        /// </summary>
        void UpdateSnapshot()
        {
            if (NewSnapshotRequired)
            {
                SnapshotLock.TryEnterWriteLock(Timeout.Infinite);
                if (NewSnapshotRequired)
                {
                    BaseSnapshot = new ImmutableCollection<T>(BaseCollection);
                    NewSnapshotRequired = false;
                }
                SnapshotLock.ExitWriteLock();
            }
        }

        #endregion

        #region Protected

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <remarks>
        /// Don't use BaseCollection.Clear(), it causes problems because it
        /// sends a reset event, and then the collection needs to be read out through
        /// an enumerator. Use RemoveAt instead until the collection is empty.
        /// Using remove from end after testing with this speed test:
        /// </remarks>
        protected void DoBaseClear(Action Action = null)
        {
            // Need a special case of DoBaseWrite for a set changes to make sure that nothing else does a change
            // while we are in the middle of doing a collection of changes.
            ReadWriteLock.TryEnterUpgradeableReadLock(Timeout.Infinite);
            try
            {
                ReadWriteLock.TryEnterWriteLock(Timeout.Infinite);
                Action?.Invoke();
                while (WriteCollection.Count > 0)
                {
                    NewSnapshotRequired = true;
                    WriteCollection.RemoveAt(WriteCollection.Count - 1);
                }
            }
            finally
            {
                if(ReadWriteLock.IsWriteLockHeld)
                    ReadWriteLock.ExitWriteLock();
                ReadWriteLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Handles when the base collection changes. Pipes the event through IObservable.OnNext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// As this is a concurrent collection we don't want a change event to result in the listener
        /// later coming back to enumerate over the whole collection again, possible before the listener
        /// gets other changed events, but after the collection has been added to.
        /// </remarks>
        protected void HandleBaseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool actionTypeIsOk = e.Action != NotifyCollectionChangedAction.Reset;
            System.Diagnostics.Debug.Assert(actionTypeIsOk, "Reset called on concurrent observable collection. This shouldn't happen");
            OnNext(e);
        }

        /// <summary>
        /// Handles read access from the base collection
        /// </summary>
        /// <param name="readFunc"></param>
        protected void DoBaseRead(Action readFunc)
        {
            DoBaseRead<object>(() => 
            {
                readFunc();
                return null;
            });
        }

        /// <summary>
        /// Handles read access from the base collection when a return value is required
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="ReadAction"></param>
        /// <returns></returns>
        protected TResult DoBaseRead<TResult>(Func<TResult> ReadAction)
        {
            if (IsDispatcherThread)
                return ReadAction();

            ReadWriteLock.TryEnterReadLock(Timeout.Infinite);

            try
            {
                return ReadAction();
            }
            finally
            {
                ReadWriteLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Calls the read function passed in, and if it returns true,
        /// then calls the next read function, else calls the write
        /// function.
        /// </summary>
        protected TResult DoBaseReadWrite<TResult>(Func<bool> ReadFuncTest, Func<TResult> ReadFunc, Func<TResult> WriteFunc) {
            ReadWriteLock.TryEnterUpgradeableReadLock(Timeout.Infinite);
            try
            {
                if(ReadFuncTest())
                    return ReadFunc();
                else
                {
                    ReadWriteLock.TryEnterWriteLock(Timeout.Infinite);
                    try
                    {
                        NewSnapshotRequired = true;
                        TResult returnValue = WriteFunc();
                        return returnValue;
                    }
                    finally
                    {
                        if(ReadWriteLock.IsWriteLockHeld)
                            ReadWriteLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                ReadWriteLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Calls the read function passed in, and if it returns true,
        /// then calls the next read function, else unlocks the collection,
        /// calls the pre-write function, then chains to DoBaseReadWrite
        /// calls the write
        /// function.
        /// </summary>
        protected TResult DoBaseReadWrite<TResult>(Func<bool> ReadFuncTest, Func<TResult> ReadFunc, Action PreWriteFunc, Func<TResult> WriteFunc)
        {
            ReadWriteLock.TryEnterReadLock(Timeout.Infinite);
            try
            {
                if (ReadFuncTest())
                    return ReadFunc();
            }
            finally
            {
                ReadWriteLock.ExitReadLock();
            }
            PreWriteFunc();
            return DoBaseReadWrite(ReadFuncTest, ReadFunc, WriteFunc);
        }

        /// <summary>
        /// Handles write access to the base collection
        /// </summary>
        /// <param name="WriteAction"></param>
        protected void DoBaseWrite(Action WriteAction)
        {
            DoBaseWrite<object>(() => 
            {
                WriteAction();
                return null;
            });
        }

        /// <summary>
        /// Handles write access to the base collection when a return value is required
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="WriteFunc"></param>
        /// <returns></returns>
        protected TResult DoBaseWrite<TResult>(Func<TResult> WriteFunc)
        {
            ReadWriteLock.TryEnterUpgradeableReadLock(Timeout.Infinite);
            try
            {
                ReadWriteLock.TryEnterWriteLock(Timeout.Infinite);
                NewSnapshotRequired = true;
                return WriteFunc();
            }
            finally
            {
                if(ReadWriteLock.IsWriteLockHeld)
                    ReadWriteLock.ExitWriteLock();
                ReadWriteLock.ExitUpgradeableReadLock();
            }
        }

        #endregion

        #endregion
    }
}