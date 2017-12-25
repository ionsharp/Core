using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using System.Linq;
using Imagin.Common.Linq;

namespace Imagin.Common.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Item"></param>
    public delegate void StorageChangedEventHandler(SystemItem Item);

    /// <summary>
    /// 
    /// </summary>
    public class FileSystemWatcher
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> ContentsChanged;

        /// <summary>
        /// 
        /// </summary>
        public event StorageChangedEventHandler FileChanged;
        
        /// <summary>
        /// 
        /// </summary>
        public event StorageChangedEventHandler FileMovedOrCreated;

        /// <summary>
        /// 
        /// </summary>
        public event StorageChangedEventHandler FileMovedOrDeleted;

        /// <summary>
        /// 
        /// </summary>
        public event StorageChangedEventHandler FolderChanged;

        /// <summary>
        /// 
        /// </summary>
        public event StorageChangedEventHandler FolderMovedOrCreated;

        /// <summary>
        /// 
        /// </summary>
        public event StorageChangedEventHandler FolderMovedOrDeleted;

        /// <summary>
        /// 
        /// </summary>
        List<SystemItem> Contents { get; set; } = new List<SystemItem>();

        /// <summary>
        /// 
        /// </summary>
        StorageFolderQueryResult Query { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public StorageFolder Folder { get; private set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public FolderDepth FolderDepth { get; set; } = FolderDepth.Deep;
        
        /// <summary>
        /// 
        /// </summary>
        public NotifyFilters NotifyFilters { get; set; } = NotifyFilters.All;

        #endregion

        #region FileSystemWatcher

        /// <summary>
        /// 
        /// </summary>
        public FileSystemWatcher() : this(NotifyFilters.All)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notifyFilters"></param>
        public FileSystemWatcher(NotifyFilters notifyFilters)
        {
            NotifyFilters = notifyFilters;
        }

        #endregion

        #region Methods

        async Task CrossReferenceAsync(List<SystemItem> OldContents, List<SystemItem> NewContents)
        {
            foreach (var i in OldContents)
            {
                var j = NewContents.WhereFirst(k => i.Path == k.Path);
                if (j == null)
                {
                    //The item used to, but no longer, exists
                    if (i is SystemFile)
                    {
                        OnFileMovedOrDeleted(i as SystemFile);
                    }
                    else if (i is SystemFolder)
                        OnFolderMovedOrDeleted(i as SystemFolder);
                }
                else
                {
                    //The item existed before and does now
                    if 
                    (
                        (i.Attributes != j.Attributes)
                        ||
                        (i.DateCreated != j.DateCreated)
                    )
                    {
                        //The item's attributes have changed
                        if (i is SystemFile)
                        {
                            OnFileChanged(i as SystemFile);
                        }
                        else if (i is SystemFolder)
                        {
                            OnFolderChanged(i as SystemFolder);
                        }
                    }
                }
            }

            foreach (var i in NewContents)
            {
                if (!OldContents.Contains(j => i.Path == j.Path))
                {
                    //The item exists now, but didn't before
                    if (i is SystemFile)
                    {
                        OnFileMovedOrCreated(i as SystemFile);
                    }
                    else if (i is SystemFolder)
                        OnFolderMovedOrCreated(i as SystemFolder);
                }
            }
        }

        async Task<List<SystemItem>> TryGetContentsAsync(StorageFolder RootFolder)
        {
            var Result = new List<SystemItem>();

            var Contents = default(IReadOnlyList<IStorageItem>);
            try
            {
                Contents = await RootFolder.GetItemsAsync();
            }
            catch
            {
                return Result;
            }

            foreach (var i in Contents)
            {
                if (i is StorageFolder)
                {
                    Result.Add(new SystemFolder(i as StorageFolder));
                    (await TryGetContentsAsync(i as StorageFolder)).ForEach(j => Result.Add(j));
                }
                else if (i is StorageFile)
                    Result.Add(new SystemFile(i as StorageFile));
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async virtual void OnContentsChanged(IStorageQueryResultBase sender, object e)
        {
            ContentsChanged?.Invoke(this, new EventArgs());
            //await CrossReferenceAsync(Contents, await TryGetContentsAsync(Folder));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnFileChanged(SystemFile Value)
        {
            FileChanged?.Invoke(Value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnFileMovedOrCreated(SystemFile Value)
        {
            FileMovedOrCreated?.Invoke(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnFileMovedOrDeleted(SystemFile Value)
        {
            FileMovedOrDeleted?.Invoke(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnFolderChanged(SystemFolder Value)
        {
            FolderChanged?.Invoke(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnFolderMovedOrDeleted(SystemFolder Value)
        {
            FolderMovedOrDeleted?.Invoke(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnFolderMovedOrCreated(SystemFolder Value)
        {
            FolderMovedOrCreated?.Invoke(Value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task AbortAsync()
        {
            if (Query != null)
            {
                Query.ContentsChanged -= OnContentsChanged;
                Query = null;
            }

            Folder = null;
            Contents.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public async Task WatchAsync(StorageFolder folder)
        {
            Query = folder?.CreateFolderQueryWithOptions(new QueryOptions()
            {
                FolderDepth = FolderDepth
            });

            var Result = true;

            try
            {
                await Query?.GetFoldersAsync(0, 1);
            }
            catch
            {
                Result = false;
            }

            if (Result)
            {
                Folder = folder;
                Query.ContentsChanged += OnContentsChanged;

                Contents.Clear();
                (await TryGetContentsAsync(Folder)).ForEach(i => Contents.Add(i));
            }
        }

        #endregion
    }
}
