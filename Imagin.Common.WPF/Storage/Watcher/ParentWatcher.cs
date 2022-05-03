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
    /// <summary>
    /// Does not yet properly subscribe and unsubscribe when used with <see cref="StorageCollection{T}"/>.
    /// </summary>
    [Obsolete]
    public class ParentWatcher : Watcher
    {
        readonly List<FileSystemWatcher> parents = new();

        bool subscribed = false;

        //...

        IEnumerable<string> GetParents()
        {
            var i = System.IO.Path.GetDirectoryName(Path);
            while (!i.NullOrEmpty())
            {
                yield return i;
                i = System.IO.Path.GetDirectoryName(i);
            }
        }

        //...

        void OnParentDeleted(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnParentDeleted(sender, e), () =>
            {
                if (parents.Select(i => i.Path).Contains(e.FullPath) || Path == e.FullPath)
                {
                    Unsubscribe();
                    OnFailed(new Error());
                }
            });
        }

        void OnParentRenamed(object sender, RenamedEventArgs e)
        {
            Handle(e, () => OnParentRenamed(sender, e), () =>
            {
                if (e.FullPath != e.OldFullPath && (parents.Select(i => i.Path).Contains(e.OldFullPath) || Path == e.OldFullPath))
                {
                    Unsubscribe();
                    OnFailed(new Error());
                }
            });
        }

        //...

        public override void Disable()
        {
            base.Disable();
            foreach (var i in parents)
            {
                i.Deleted -= OnParentDeleted;
                i.Renamed -= OnParentRenamed;
                i.EnableRaisingEvents = false;
                i.Dispose();
            }
            parents.Clear();
        }

        public override void Enable(string path)
        {
            base.Enable(path);
            foreach (var i in GetParents())
            {
                var j = new FileSystemWatcher()
                {
                    IncludeSubdirectories = false,
                    Path = i
                };
                parents.Add(j);

                if (subscribed)
                {
                    j.Deleted += OnParentDeleted;
                    j.Renamed += OnParentRenamed;
                }

                j.EnableRaisingEvents = true;
            }
        }

        //...

        public override void Subscribe()
        {
            subscribed = true;
            base.Subscribe();
            foreach (var i in parents)
            {
                i.Deleted += OnParentDeleted;
                i.Renamed += OnParentRenamed;
            }
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            foreach (var i in parents)
            {
                i.Deleted -= OnParentDeleted;
                i.Renamed -= OnParentRenamed;
            }
            subscribed = false;
        }
    }
}