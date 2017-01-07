using Imagin.Common.Extensions;
using Imagin.Common.Input;
using Imagin.Common.Web;
using Imagin.Common.Web.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Imagin.Common.Collections.Concurrent
{
    public abstract class ServerObjectCollection<T> : ConcurrentObservableCollection<T> where T : IServerObject
    {
        #region Properties

        #region Events

        public event EventHandler<EventArgs> Refreshed;

        public event EventHandler<EventArgs> Searched;

        public event EventHandler<EventArgs<SearchInfo>> Searching;

        #endregion

        #region Public

        List<DispatcherOperation> automator = new List<DispatcherOperation>();
        public List<DispatcherOperation> Automator
        {
            get
            {
                return automator;
            }
            private set
            {
                automator = value;
            }
        }
        
        /// <summary>
        /// The cancellation token source used to cancel major asynchronous operations.
        /// </summary>
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public CancellationTokenSource CancellationTokenSource
        {
            get
            {
                return new CancellationTokenSource();
            }
        }

        string[] extensions = null;
        public string[] Extensions
        {
            get
            {
                return extensions;
            }
            protected set
            {
                extensions = value;
                OnPropertyChanged("Extensions");
            }
        }

        bool isLogEnabled = false;
        public bool IsLogEnabled
        {
            get
            {
                return isLogEnabled;
            }
            protected set
            {
                isLogEnabled = value;
                OnPropertyChanged("IsLogEnabled");
            }
        }

        bool isRefreshing = false;
        /// <summary>
        /// Indicates whether or not the in the process of refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            protected set
            {
                isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        bool isSearching = false;
        /// <summary>
        /// Indicates whether or not in the process of searching.
        /// </summary>
        public bool IsSearching
        {
            get
            {
                return isSearching;
            }
            protected set
            {
                isSearching = value;
                OnPropertyChanged("IsSearching");
            }
        }

        string path = string.Empty;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                OnPropertyChanged("Path");
                this.OnPathChanged(value);
            }
        }

        SearchHelper searchHelper = new SearchHelper();
        /// <summary>
        /// Utility to search file system.
        /// </summary>
        public SearchHelper SearchHelper
        {
            get
            {
                return searchHelper;
            }
            set
            {
                searchHelper = value;
                OnPropertyChanged("SearchHelper");
            }
        }

        /// <summary>
        /// Total length in bytes.
        /// </summary>
        long size = 0L;
        public long Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        ServerObjectType types = ServerObjectType.All;
        public ServerObjectType Types
        {
            get
            {
                return types;
            }
            set
            {
                types = value;
                OnPropertyChanged("Types");
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Abstract

        protected abstract IClient GetClient();

        /// <summary>
        /// Perform asynchronous refresh (cancellation is supported).
        /// </summary>
        public abstract Task BeginRefresh(IClient Client, string Path);

        /// <summary>
        /// Begin asynchronous, recursive search (cancellation is supported).
        /// </summary>
        public abstract Task BeginSearch(IClient Client, string Path, SearchOptions SearchOptions);
        
        /// <summary>
        /// Executes a refresh.
        /// </summary>
        protected abstract void ExecuteRefresh(IClient Client, string Path, ServerObjectType Types, bool IsLoggingEnabled, CancellationTokenSource CancellationTokenSource);

        /// <summary>
        /// Executes a search.
        /// </summary>
        protected abstract Task ExecuteSearch(IClient Client, string Path, SearchOptions Options, CancellationTokenSource CancellationTokenSource);

        #endregion

        #region Virtual

        protected virtual void OnPathChanged(string Value)
        {
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnRefreshed()
        {
            this.IsRefreshing = false;

            if (this.Refreshed != null)
                this.Refreshed(this, new EventArgs());
        }

        protected virtual void OnRefreshing(string Path)
        {
            this.Path = Path;
            this.IsRefreshing = true;

            if (0 < this.Count)
            {
                this.Size = 0L;
                this.Clear();
            }
        }

        protected virtual void OnSearched()
        {
            this.IsSearching = false;

            SearchHelper.Stopwatch.Stop();
            SearchHelper.Stopwatch.Reset();
            SearchHelper.Timer.Stop();

            if (this.Searched != null)
                this.Searched(this, new EventArgs());
        }

        protected virtual void OnSearching()
        {
            SearchHelper.SearchInfo.Total++;
            if (this.Searching != null)
                this.Searching(this, new EventArgs<SearchInfo>(SearchHelper.SearchInfo));
        }

        /// <summary>
        /// Perform slow (synchronous) refresh (cancellation not supported).
        /// </summary>
        public virtual void Refresh(string Path)
        {
            this.OnRefreshing(Path);
            this.ExecuteRefresh(GetClient(), Path, Types, IsLogEnabled, CancellationTokenSource);
            this.OnRefreshed();
        }

        #endregion

        #region Protected

        /// <summary>
        /// Abort current operations; optionally, clear.
        /// </summary>
        protected void Abort(bool Clear)
        {
            if (this.CancellationTokenSource.Token.CanBeCanceled)
                this.CancellationTokenSource.Cancel();

            int Count = this.Automator.Count;
            for (int i = 0; i < Count; i++)
            {
                this.Automator[0].Abort();
                this.Automator.RemoveAt(0);
            }

            if (Clear) this.Clear();
        }

        /// <summary>
        /// Get item with specified path.
        /// </summary>
        protected T GetFirst(string Path) 
        {
            var Result = this.Where(x => (x as IServerObject).Path == Path);
            return Result.Count() > 0 ? Result.First().As<T>() : default(T);
        }

        #endregion

        #region Public

        /// <summary>
        /// Queue task of adding a newly created item.
        /// </summary>
        /// <param name="Item"></param>
        public void Automate(T Item)
        {
            var Operation = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Add(Item)));
            Operation.Completed += (a, b) => Automator.Remove(a.As<DispatcherOperation>());
            Automator.Add(Operation);
        }

        /// <summary>
        /// Perform asynchronous refresh (cancellation is supported).
        /// </summary>
        public async Task BeginRefresh()
        {
            await this.BeginRefresh(this.Path);
        }

        /// <summary>
        /// Perform asynchronous refresh (cancellation is supported).
        /// </summary>
        public async Task BeginRefresh(string Path)
        {
            await BeginRefresh(GetClient(), Path);
        }

        /// <summary>
        /// Begin asynchronous, recursive search (cancellation is supported).
        /// </summary>
        public async Task BeginSearch(SearchOptions SearchOptions)
        {
            await this.BeginSearch(Path, SearchOptions);
        }

        /// <summary>
        /// Begin asynchronous, recursive search (cancellation is supported).
        /// </summary>
        public async Task BeginSearch(string Path, SearchOptions SearchOptions)
        {
            await BeginSearch(GetClient(), Path, SearchOptions);
        }

        /// <summary>
        /// Cancel current refresh operation.
        /// </summary>
        public void CancelRefresh()
        {
            Abort(true);
        }

        /// <summary>
        /// Cancel current search operation.
        /// </summary>
        public void CancelSearch()
        {
            this.Abort(false);
        }

        /// <summary>
        /// Perform slow (synchronous) refresh (cancellation not supported).
        /// </summary>
        public void Refresh()
        {
            this.Refresh(this.Path);
        }

        #endregion

        #endregion

        #region ServerObjectCollection

        public ServerObjectCollection() : base()
        {
            this.OnInitialized();
        }

        public ServerObjectCollection(string Path, ServerObjectType Types = ServerObjectType.All, string[] Extensions = null, bool IsLoggingEnabled = false) : base()
        {
            this.Path = Path;
            this.Types = Types;
            this.Extensions = Extensions;
            this.IsLogEnabled = IsLogEnabled;

            this.OnInitialized();
        }

        #endregion
    }
}
