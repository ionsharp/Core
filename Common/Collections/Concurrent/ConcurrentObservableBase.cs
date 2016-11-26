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
    /// This class provides the base for concurrent collections that 
    /// can be bound to user interface elements
    /// </summary>
    /// <notes>
    /// Could do with a more performant enumerable implementation
    /// but this is what I have so far. I create a snapshot of the collection
    /// and use the enumerable from that. When the collection is updated
    /// I set a flag indicating that a new snapshot is required.
    /// </notes>
    [Serializable]
    public abstract class ConcurrentObservableBase<T> : IObservable<NotifyCollectionChangedEventArgs>, INotifyCollectionChanged, IEnumerable<T>, IDisposable
    {
        #region Properties

        #region Static

        /// <summary>
        /// Gets if the calling thread is the same as the dispatcher thread
        /// </summary>
        protected static bool IsDispatcherThread
        {
            get
            {
                return DispatcherQueueProcessor.Instance.IsDispatcherThread;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// The lock that controls read/write access to the base collection
        /// </summary>
        ReaderWriterLockSlim ReadWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The underlying base enumerable that is used to store the items,
        /// used for creating an immutable collection from which an enumerator
        /// can be obtained.
        /// </summary>
        ObservableCollection<T> BaseCollection;

        /// <summary>
        /// Flag indicating that a write has occured, so anything that depends on
        /// taking a snapshot of the collection needs to be updated.
        /// </summary>
        bool NewSnapshotRequired = false;

        /// <summary>
        /// The enumerable lock to prevent threading conflicts on allocating
        /// the enumerable of the fixed collection
        /// </summary>
        ReaderWriterLockSlim SnapshotLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The collection used for generating an enumerable that iterates
        /// over a snapshot of the base collection
        /// </summary>
        ImmutableCollectionBase<T> BaseSnapshot;

        /// <summary>
        /// A list of observers
        /// </summary>
        Dictionary<int, IObserver<NotifyCollectionChangedEventArgs>> Subscribers;

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
        public ImmutableCollectionBase<T> Snapshot
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

        protected ObservableCollection<T> ReadCollection
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
        ObservableCollectionViewModel<T> viewModel;
        /// <summary>
        /// Access this directly if getting the error "An ItemsControl is inconsistent with its items source".
        /// </summary>
        protected ObservableCollectionViewModel<T> ViewModel
        {
            get
            {
                return viewModel;
            }
        }

        /// <summary>
        /// Gets the base collection that holds the values
        /// </summary>
        protected ObservableCollection<T> WriteCollection
        {
            get
            {
                return BaseCollection;
            }
        }

        #endregion

        #endregion

        #region ConcurrentObservableBase

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected ConcurrentObservableBase() : this(new T[]{})
        {
        }

        /// <summary>
        /// Constructor that takes an eumerable
        /// </summary>
        protected ConcurrentObservableBase(IEnumerable<T> Items)
        {
            Subscribers = new Dictionary<int, IObserver<NotifyCollectionChangedEventArgs>>();

            BaseCollection = new ObservableCollection<T>(Items);
            BaseSnapshot = new ImmutableCollection<T>(Items);

            // subscribers must be initialized befor calling this as it may
            // subscribe immediately
            viewModel = new ObservableCollectionViewModel<T>(this);

            // Handle when the base collection changes. Event will be passed through
            // the IObservable.OnNext method.
            BaseCollection.CollectionChanged += HandleBaseCollectionChanged;

            // Bubble up the notify collection changed event from the view model
            viewModel.CollectionChanged += (sender, e) => 
            {
                if(CollectionChanged != null)
                    CollectionChanged(sender, e);
            };
        }

        ~ConcurrentObservableBase() {
            Dispose(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes all items from the ICollection<T>.
        /// </summary>
        /// <remarks>
        /// Don't use BaseCollection.Clear(), it causes problems because it
        /// sends a reset event, and then the collection needs to be read out through
        /// an enumerator. Use RemoveAt instead until the collection is empty.
        /// Using remove from end after testing with this speed test:
        /// </remarks>
        /// <test for="RemoveAt(int index)">
        ///
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Collections.ObjectModel;
        /// using System.Diagnostics;
        ///
        /// namespace ConsoleApplication1 
        /// {
        ///     class Program 
        ///     {
        ///         static void Main(string[] args) 
        ///         {
        ///             var coll = new Collection<int>();
        ///             for (int ix = 0; ix < 100000; ++ix) coll.Add(ix);
        ///             var sw = Stopwatch.StartNew();
        ///             while (coll.Count > 0) coll.RemoveAt(0);
        ///             sw.Stop();
        ///             Console.WriteLine("Removed from start {0}ms",sw.ElapsedMilliseconds);
        ///             for (int ix = 0; ix < 100000; ++ix) coll.Add(ix);
        ///             sw = Stopwatch.StartNew();
        ///             while (coll.Count > 0) coll.RemoveAt(coll.Count - 1);
        ///             Console.WriteLine("Removed from end {0}ms",sw.ElapsedMilliseconds);
        ///             Console.ReadLine();
        ///         }
        ///     }
        /// }
        ///
        ///  Output: 
        ///  Removed from start 4494ms
        ///  Removed from end 3ms
        /// 
        /// </test>
        protected void DoBaseClear(Action Action = null)
        {
            // Need a special case of DoBaseWrite for a set changes to make sure that nothing else does a change
            // while we are in the middle of doing a collection of changes.
            ReadWriteLock.TryEnterUpgradeableReadLock(Timeout.Infinite);
            try
            {
                ReadWriteLock.TryEnterWriteLock(Timeout.Infinite);
                if (Action != null) Action();
                while(WriteCollection.Count > 0)
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
        /// Updates the snapshot that is used to generate an Enumerator
        /// </summary>
        /// <param name="forceUpdate"></param>
        private void UpdateSnapshot()
        {
            if(NewSnapshotRequired)
            {
                SnapshotLock.TryEnterWriteLock(Timeout.Infinite);
                if(NewSnapshotRequired)
                {
                    BaseSnapshot = new ImmutableCollection<T>(BaseCollection);
                    NewSnapshotRequired = false;
                }
                SnapshotLock.ExitWriteLock();
            }
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
        /// <param name="readFunc"></param>
        /// <returns></returns>
        protected TResult DoBaseRead<TResult>(Func<TResult> ReadFunc)
        {
            if(IsDispatcherThread)
                return ReadFunc();
            ReadWriteLock.TryEnterReadLock(Timeout.Infinite);
            try
            {
                return ReadFunc();
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
        /// <param name="writeFunc"></param>
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
        /// <param name="writeFunc"></param>
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

        #region IObservable

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool Disposing)
        {
            if(Disposing) GC.SuppressFinalize(this);
            OnCompleted();
            IsDisposed = true;
        }

        protected void OnNext(NotifyCollectionChangedEventArgs value)
        {
            if(IsDisposed)
                throw new ObjectDisposedException("Observable<T>");
            foreach (IObserver<NotifyCollectionChangedEventArgs> Observer in Subscribers.Select(kv => kv.Value))
                Observer.OnNext(value);
        }

        protected void OnError(Exception Exception)
        {
            if(IsDisposed)
                throw new ObjectDisposedException("Observable<T>");
            if(Exception == null)
                throw new ArgumentNullException("Exception");
            foreach(IObserver<NotifyCollectionChangedEventArgs> Observer in Subscribers.Select(kv => kv.Value))
                Observer.OnError(Exception);
        }

        protected void OnCompleted()
        {
            if(IsDisposed)
                throw new ObjectDisposedException("Observable<T>");
            foreach (IObserver<NotifyCollectionChangedEventArgs> Observer in Subscribers.Select(kv => kv.Value))
                Observer.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<NotifyCollectionChangedEventArgs> observer)
        {
            if(observer == null) 
                throw new ArgumentNullException("observer");
            return DoBaseWrite(() => 
            {
                int Key = SubscriberKey++;

                Subscribers.Add(Key, observer);
                UpdateSnapshot();

                foreach(var item in BaseSnapshot) 
                    observer.OnNext(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

                return new DisposeDelegate(() => DoBaseWrite(() => Subscribers.Remove(Key)));
            });
        }

        /// <summary>
        /// Result returned from "Subscribe" method.
        /// </summary>
        class DisposeDelegate : IDisposable
        {
            private Action dispose;

            public DisposeDelegate(Action Dispose)
            {
                this.dispose = Dispose;
            }

            public void Dispose()
            {
                dispose();
            }
        }

        #endregion

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

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
    }
}
