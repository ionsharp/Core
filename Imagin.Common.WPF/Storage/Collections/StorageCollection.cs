using Imagin.Common.Analytics;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Common.Storage
{
    public abstract class StorageCollection<T> : ConcurrentCollection<T>, ISubscribe, IUnsubscribe
    {
        public static Watcher DefaultWatcher 
            => new(NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Security);

        class Automator : List<DispatcherOperation>
        {
            void OnCompleted(object sender, EventArgs e)
            {
                var i = (DispatcherOperation)sender;
                i.Completed -= OnCompleted;
                Remove(i);
            }

            new public void Add(DispatcherOperation i)
            {
                base.Add(i);
                i.Completed += OnCompleted;
            }

            new public void Clear()
            {
                for (var i = Count - 1; i >= 0; i--)
                {
                    this[i].Completed -= OnCompleted;
                    this[i].Abort();
                    RemoveAt(i);
                }
            }
        }

        public delegate void StorageCollectionEventHandler(StorageCollection<T> sender);

        public event StorageCollectionEventHandler Refreshing;

        public event StorageCollectionEventHandler Refreshed;

        #region Properties

        readonly Automator automator = new();

        Watcher watcher = null;

        //...

        readonly CancelTask<string> refreshTask;

        //...

        protected bool subscribed = false;

        bool isRefreshing = false;
        public bool IsRefreshing
        {
            get => isRefreshing;
            private set => this.Change(ref isRefreshing, value);
        }

        Filter filter = Filter.Default;
        public Filter Filter
        {
            get => filter;
            set => filter = value ?? Filter.Default;
        }

        string path = string.Empty;
        public string Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        double progress = 0;
        public double Progress
        {
            get => progress;
            set => this.Change(ref progress, value);
        }

        #endregion

        #region StorageCollection

        public StorageCollection() : this(string.Empty, null) { }

        public StorageCollection(Filter filter) : this(string.Empty, filter) { }

        public StorageCollection(string path, Filter filter) : base()
        {
            refreshTask
                = new(RefreshSync, RefreshAsync, true);

            Path
                = path;
            Filter
                = filter;
        }

        //...

        protected abstract T this[string path] { get; }

        #endregion

        #region Methods

        IEnumerable<T> Query(string path, Filter filter)
        {
            if (filter.Types.HasFlag(ItemType.Drive))
            {
                if (path.NullOrEmpty() || path == StoragePath.Root)
                {
                    Try.Invoke(out IEnumerable<DriveInfo> drives, () => Computer.Drives);
                    if (drives is not null)
                    {
                        foreach (var i in drives)
                            yield return ToDrive(i);
                    }
                }
            }
            if (filter.Types.HasFlag(ItemType.Folder))
            {
                Try.Invoke(out IEnumerable<string> folders, () => Folder.Long.GetFolders(path).Where(j => j != ".."));
                if (folders is not null)
                {
                    foreach (var i in folders)
                        yield return ToFolder(i);
                }
            }
            if (filter.Types.HasFlag(ItemType.File))
            {
                Try.Invoke(out IEnumerable<string> files, () => Folder.Long.GetFiles(path));
                if (files is not null)
                {
                    foreach (var i in files)
                    {
                        if (filter.Evaluate(i, ItemType.File))
                            yield return ToFile(i);
                    }
                }
            }
        }

        //...

        void OnItemChanged(object sender, FileSystemEventArgs e)
        {
            var item = this[e.FullPath];
            if (item != null)
            {
                var changedProperty = GetChangedProperty(item);
                OnItemChanged(item, changedProperty);
            }
        }

        void OnItemCreated(object sender, FileSystemEventArgs e)
        {
            T i = default;

            var a = File.Long.Exists(e.FullPath);
            var b = Filter?.Evaluate(e.FullPath, ItemType.File) != false;

            if (a && b)
            {
                i = ToFile(e.FullPath);
            }
            else
            {
                a = Folder.Long.Exists(e.FullPath);
                b = Filter?.Evaluate(e.FullPath, ItemType.Folder) != false;

                if (a && b)
                    i = ToFolder(e.FullPath);
            }

            if (i != null)
            {
                OnItemCreated(i);
                Add(i);
            }
        }

        void OnItemDeleted(object sender, FileSystemEventArgs e)
        {
            var item = this[e.FullPath];
            if (item != null)
            {
                OnItemDeleted(item);
                Remove(item);
            }
        }

        void OnItemRenamed(object sender, RenamedEventArgs e) => OnItemRenamed(e);

        //...

        protected abstract ItemProperty GetChangedProperty(T input);

        //...

        protected abstract T ToDrive(DriveInfo input);

        protected abstract T ToFile(string input);

        protected abstract T ToFolder(string input);

        //...

        protected virtual void OnItemChanged(T i, ItemProperty property) { }

        protected virtual void OnItemCreated(T i) { }

        protected virtual void OnItemDeleted(T i) { }

        protected virtual void OnItemRenamed(RenamedEventArgs e) { }

        //...

        protected virtual void OnRefreshing() => Refreshing?.Invoke(this);

        protected virtual void OnRefreshed() 
        {
            if (subscribed)
                watcher.Enable(Path);

            Refreshed?.Invoke(this);
        }

        //...

        public override void Clear()
        {
            refreshTask.Cancel();
            InternalClear();
        }

        void InternalClear()
        {
            automator
                .Clear();
            base
                .Clear();
        }

        async Task InternalClearAsync()
        {
            InternalClear();
            await While.InvokeAsync(() => automator, i => 0 < i.Count && 0 < Count);
        }

        //...

        void Refresh(string path, Filter filter, CancellationToken token)
        {
            IEnumerable<T> items = null;
            Try.Invoke(() => items = Query(path, filter), e => Log.Write<ItemCollection>(new Error(e)));

            if (items is not null)
            {
                foreach (var i in items)
                {
                    if (token.IsCancellationRequested)
                        return;

                    automator.Add(Dispatch.InvokeReturn(() => Add(i)));
                }
            }
        }

        //...

        async Task RefreshAsync(string path, CancellationToken token)
        {
            IsRefreshing = true;

            Path = path;
            await InternalClearAsync();

            OnRefreshing();
            var filter = Filter;
            await Task.Run(() => Refresh(path, filter, token));
            OnRefreshed();

            IsRefreshing = false;
        }

        void RefreshSync(string path)
        {
            IsRefreshing = true;

            Path = path;
            InternalClear();

            OnRefreshing();
            Refresh(path, Filter, new(false));
            OnRefreshed();

            IsRefreshing = false;
        }

        //...

        public void Refresh() => Refresh(Path);

        public void Refresh(string path) => refreshTask.Start(path);

        //...

        public async Task RefreshAsync() => await RefreshAsync(Path);

        public async Task RefreshAsync(string path) => await refreshTask.StartAsync(path);

        //...

        public virtual void Subscribe()
        {
            if (!subscribed)
            {
                subscribed = true;

                watcher = DefaultWatcher;
                watcher.Subscribe();

                watcher.Changed += OnItemChanged;
                watcher.Created += OnItemCreated;
                watcher.Deleted += OnItemDeleted;
                watcher.Renamed += OnItemRenamed;

                if (!isRefreshing)
                    watcher.Enable(Path);
            }
        }

        public virtual void Unsubscribe()
        {
            if (watcher != null)
            {
                watcher.Unsubscribe();

                watcher.Changed -= OnItemChanged;
                watcher.Created -= OnItemCreated;
                watcher.Deleted -= OnItemDeleted;
                watcher.Renamed -= OnItemRenamed;

                watcher.Disable();
                watcher.Dispose();

                watcher = null;
            }
            subscribed = false;
        }

        //...

        ICommand refreshCommand;
        public ICommand RefreshCommand => refreshCommand ??= new RelayCommand(Refresh);

        ICommand refreshAsyncCommand;
        public ICommand RefreshAsyncCommand => refreshAsyncCommand ??= new RelayCommand(async () => await RefreshAsync());

        #endregion
    }
}