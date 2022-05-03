using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Imagin.Common.Storage
{
    public class Watcher : Base, ISubscribe, IUnsubscribe
    {
        public event FileSystemEventHandler Changed;

        public event FileSystemEventHandler Created;

        public event FileSystemEventHandler Deleted;

        public event RenamedEventHandler Renamed;

        //...

        public event Analytics.ErrorEventHandler Failed;

        //...

        readonly FileSystemWatcher watcher;

        //...

        public NotifyFilters Filter
        {
            get => watcher.NotifyFilter;
            set => watcher.NotifyFilter = value;
        }

        public bool IncludeChildren
        {
            get => watcher.IncludeSubdirectories;
            set => watcher.IncludeSubdirectories = value;
        }

        public string Path
        {
            get => watcher.Path;
            private set => watcher.Path = value;
        }

        //...

        public Watcher() : base() => watcher = new FileSystemWatcher();

        public Watcher(NotifyFilters input) : this() => Filter = input;

        //...

        protected void Handle(FileSystemEventArgs e, Action handler, Action invoke)
        {
            var dispatcher = Application.Current?.Dispatcher;
            var checkAccess = dispatcher?.CheckAccess();

            if (checkAccess == false && handler is Action)
            {
                dispatcher?.Invoke(handler);
            }
            else if (checkAccess == true)
                invoke?.Invoke(); 
        }

        //...

        protected virtual void OnChanged(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnChanged(sender, e), () => Changed?.Invoke(this, e));
        }

        protected virtual void OnCreated(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnCreated(sender, e), () => Created?.Invoke(this, e));
        }

        protected virtual void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnDeleted(sender, e), () => Deleted?.Invoke(this, e));
        }

        protected virtual void OnRenamed(object sender, RenamedEventArgs e)
        {
            Handle(e, () => OnRenamed(sender, e), () => Renamed?.Invoke(this, e));
        }

        //...

        protected virtual void OnFailed(Error result) => Failed?.Invoke(this, new(result));

        //...

        public void Dispose() => watcher.Dispose();

        //...

        public virtual void Disable() => watcher.EnableRaisingEvents = false;

        public virtual void Enable(string path)
        {
            Path = path;
            Try.Invoke(() => watcher.EnableRaisingEvents = true, e => Log.Write<Watcher>(e));
        }

        //...

        public virtual void Subscribe()
        {
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
        }

        public virtual void Unsubscribe()
        {
            watcher.Changed -= OnChanged;
            watcher.Created -= OnCreated;
            watcher.Deleted -= OnDeleted;
            watcher.Renamed -= OnRenamed;
        }
    }
}