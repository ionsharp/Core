using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace Imagin.Common.Collections
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
        ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The underlying base enumerable that is used to store the items,
        /// used for creating an immutable collection from which an enumerator
        /// can be obtained.
        /// </summary>
        ObservableCollection<T> baseCollection;

        /// <summary>
        /// Flag indicating that a write has occured, so anything that depends on
        /// taking a snapshot of the collection needs to be updated.
        /// </summary>
        bool newSnapshotRequired = false;

        /// <summary>
        /// The enumerable lock to prevent threading conflicts on allocating
        /// the enumerable of the fixed collection
        /// </summary>
        ReaderWriterLockSlim snapshotLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The collection used for generating an enumerable that iterates
        /// over a snapshot of the base collection
        /// </summary>
        ImmutableCollectionBase<T> baseSnapshot;

        /// <summary>
        /// A list of observers
        /// </summary>
        Dictionary<int, IObserver<NotifyCollectionChangedEventArgs>> subscribers;

        /// <summary>
        /// The key for new observers, incremented with each new observer
        /// </summary>
        int subscriberKey;

        /// <summary>
        /// Flag indicating this collection is disposed
        /// </summary>
        bool isDisposed;

        #endregion

        #region Public 

        /// <summary>
        /// Gets an immutable snapshot of the collection
        /// </summary>
        public ImmutableCollectionBase<T> Snapshot
        {
            get
            {
                return DoBaseRead(() => {
                    UpdateSnapshot();
                    return baseSnapshot;
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
                return baseCollection;
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
        protected ConcurrentObservableBase(IEnumerable<T> enumerable)
        {
            subscribers = new Dictionary<int, IObserver<NotifyCollectionChangedEventArgs>>();

            baseCollection = new ObservableCollection<T>(enumerable);
            baseSnapshot = new ImmutableCollection<T>(enumerable);

            // subscribers must be initialized befor calling this as it may
            // subscribe immediately
            viewModel = new ObservableCollectionViewModel<T>(this);

            // Handle when the base collection changes. Event will be passed through
            // the IObservable.OnNext method.
            baseCollection.CollectionChanged += HandleBaseCollectionChanged;

            // Bubble up the notify collection changed event from the view model
            viewModel.CollectionChanged += (sender, e) => {
            if(CollectionChanged != null) {
                CollectionChanged(sender, e);
            }
            };
        }

        ~ConcurrentObservableBase() {
            Dispose(false);
        }

        #endregion

        #region Methods

        #region Protected 

        /// <summary>
        /// Removes all items from the ICollection<T>.
        /// </summary>
        protected void DoBaseClear(Action action) {
            // Don't use BaseCollection.Clear(), it causes problems because it
            // sends a reset event, and then the collection needs to be read out through
            // an enumerator. Use RemoveAt instead until the collection is empty.
            // Using remove from end after testing with this speed test:
            //
            //  // speed test for using RemoveAt(int index);
            //  using System;
            //  using System.Collections.Generic;
            //  using System.Collections.ObjectModel;
            //  using System.Diagnostics;
            //
            //  namespace ConsoleApplication1 {
            //    class Program {
            //      static void Main(string[] args) {
            //        var coll = new Collection<int>();
            //        for (int ix = 0; ix < 100000; ++ix) coll.Add(ix);
            //        var sw = Stopwatch.StartNew();
            //        while (coll.Count > 0) coll.RemoveAt(0);
            //        sw.Stop();
            //        Console.WriteLine("Removed from start {0}ms",sw.ElapsedMilliseconds);
            //        for (int ix = 0; ix < 100000; ++ix) coll.Add(ix);
            //        sw = Stopwatch.StartNew();
            //        while (coll.Count > 0) coll.RemoveAt(coll.Count - 1);
            //        Console.WriteLine("Removed from end {0}ms",sw.ElapsedMilliseconds);
            //        Console.ReadLine();
            //      }
            //    }
            //  }
            //
            //  Output: 
            //  Removed from start 4494ms
            //  Removed from end 3ms

            // Need a special case of DoBaseWrite for a set changes to make sure that nothing else does a change
            // while we are in the middle of doing a collection of changes.
            readWriteLock.TryEnterUpgradeableReadLock(Timeout.Infinite);
            try
            {
                readWriteLock.TryEnterWriteLock(Timeout.Infinite);
                action();
                while(WriteCollection.Count > 0)
                {
                    newSnapshotRequired = true;
                    WriteCollection.RemoveAt(WriteCollection.Count - 1);
                }
            }
            finally
            {
                if(readWriteLock.IsWriteLockHeld)
                    readWriteLock.ExitWriteLock();
                readWriteLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Handles when the base collection changes. Pipes the event through IObservable.OnNext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HandleBaseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {

            // As this is a concurrent collection we don't want a change event to result in the listener
            // later coming back to enumerate over the whole collection again, possible before the listener
            // gets other changed events, but after the collection has been added to.
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
            if(newSnapshotRequired)
            {
                snapshotLock.TryEnterWriteLock(Timeout.Infinite);
                if(newSnapshotRequired)
                {
                    baseSnapshot = new ImmutableCollection<T>(baseCollection);
                    newSnapshotRequired = false;
                }
                snapshotLock.ExitWriteLock();
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
        protected TResult DoBaseRead<TResult>(Func<TResult> readFunc)
        {
            if(IsDispatcherThread)
                return readFunc();
            readWriteLock.TryEnterReadLock(Timeout.Infinite);
            try
            {
                return readFunc();
            }
            finally
            {
                readWriteLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Calls the read function passed in, and if it returns true,
        /// then calls the next read function, else calls the write
        /// function.
        /// </summary>
        protected TResult DoBaseReadWrite<TResult>(Func<bool> readFuncTest, Func<TResult> readFunc, Func<TResult> writeFunc) {
            readWriteLock.TryEnterUpgradeableReadLock(Timeout.Infinite);
            try
            {
                if(readFuncTest()) {
                    return readFunc();
                }
                else
                {
                    readWriteLock.TryEnterWriteLock(Timeout.Infinite);
                    try
                    {
                        newSnapshotRequired = true;
                        TResult returnValue = writeFunc();
                        return returnValue;
                    }
                    finally
                    {
                        if(readWriteLock.IsWriteLockHeld)
                            readWriteLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                readWriteLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Calls the read function passed in, and if it returns true,
        /// then calls the next read function, else unlocks the collection,
        /// calls the pre-write function, then chains to DoBaseReadWrite
        /// calls the write
        /// function.
        /// </summary>
        protected TResult DoBaseReadWrite<TResult>(Func<bool> readFuncTest, Func<TResult> readFunc, Action preWriteFunc, Func<TResult> writeFunc)
        {
            readWriteLock.TryEnterReadLock(Timeout.Infinite);
            try
            {
                if (readFuncTest())
                    return readFunc();
            }
            finally
            {
                readWriteLock.ExitReadLock();
            }
            preWriteFunc();
            return DoBaseReadWrite(readFuncTest, readFunc, writeFunc);
        }

        /// <summary>
        /// Handles write access to the base collection
        /// </summary>
        /// <param name="writeFunc"></param>
        protected void DoBaseWrite(Action WriteAction) {
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
        protected TResult DoBaseWrite<TResult>(Func<TResult> writeFunc)
        {
            readWriteLock.TryEnterUpgradeableReadLock(Timeout.Infinite);
            try
            {
                readWriteLock.TryEnterWriteLock(Timeout.Infinite);
                newSnapshotRequired = true;
                return writeFunc();
            }
            finally
            {
                if(readWriteLock.IsWriteLockHeld)
                    readWriteLock.ExitWriteLock();
                readWriteLock.ExitUpgradeableReadLock();
            }
        }

        #endregion

        #endregion

        #region IObservable

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
                GC.SuppressFinalize(this);
            OnCompleted();
            isDisposed = true;
        }

        protected void OnNext(NotifyCollectionChangedEventArgs value)
        {
            if(isDisposed)
                throw new ObjectDisposedException("Observable<T>");
            foreach (IObserver<NotifyCollectionChangedEventArgs> observer in subscribers.Select(kv => kv.Value))
                observer.OnNext(value);
        }

        protected void OnError(Exception exception)
        {
            if(isDisposed)
                throw new ObjectDisposedException("Observable<T>");
            if(exception == null)
                throw new ArgumentNullException("exception");
            foreach(IObserver<NotifyCollectionChangedEventArgs> observer in subscribers.Select(kv => kv.Value))
                observer.OnError(exception);
        }

        protected void OnCompleted()
        {
            if(isDisposed)
                throw new ObjectDisposedException("Observable<T>");
            foreach (IObserver<NotifyCollectionChangedEventArgs> observer in subscribers.Select(kv => kv.Value))
                observer.OnCompleted();
        }

        public IDisposable Subscribe(IObserver<NotifyCollectionChangedEventArgs> observer)
        {
            if(observer == null) 
                throw new ArgumentNullException("observer");
            return DoBaseWrite(() => 
            {
                int key = subscriberKey++;
                subscribers.Add(key, observer);
                UpdateSnapshot();
                foreach(var item in baseSnapshot) 
                    observer.OnNext(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
                return new DoDispose(() => 
                {
                    DoBaseWrite(() => 
                    {
                        subscribers.Remove(key);
                    });
                });
            });
        }

        /// <summary>
        /// Used as the return IDisposable from Subscribe()
        /// </summary>
        private class DoDispose : IDisposable
        {
            private Action doDispose;

            public DoDispose(Action DoDispose)
            {
                this.doDispose = DoDispose;
            }

            public void Dispose()
            {
                doDispose();
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
            if(IsDispatcherThread)
                return viewModel.GetEnumerator();
            else
                return Snapshot.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for a snapshot of the collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}
