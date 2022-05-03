using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Imagin.Common.Threading
{
    public delegate Task AsyncDelegate<T>(T input, CancellationToken token);

    public delegate void SyncDelegate<T>(T input);

    //...

    public delegate Task AsyncDelegate(CancellationToken token);

    public delegate void SyncDelegate(CancellationToken token);

    //...

    public delegate void TaskCompletedEventHandler(CancelTask task);

    //...

    /// <summary>
    /// Specifies a task that cancels with either a synchronous or asynchronous action.
    /// </summary>
    [Serializable]
    public class CancelTask : Base
    {
        [field: NonSerialized]
        public event TaskCompletedEventHandler Completed;

        //...

        [field: NonSerialized]
        readonly AsyncDelegate AsyncAction;

        [field: NonSerialized]
        readonly SyncDelegate SyncAction;

        //...

        [field: NonSerialized]
        protected CancellationTokenSource tokenSource;

        [Hidden]
        [XmlIgnore]
        public bool IsCancelled => tokenSource.IsCancellationRequested;

        [field: NonSerialized]
        protected bool restart = false;

        protected bool restarts;
        public bool Restarts
        {
            get => restarts;
            set => this.Change(ref restarts, value);
        }

        [field: NonSerialized]
        protected bool started = false;
        [Hidden]
        [XmlIgnore]
        public bool Started
        {
            get => started;
            set => this.Change(ref started, value);
        }

        //...

        CancelTask() : base() { }

        protected CancelTask(bool restarts) : this() 
            => Restarts = restarts;

        public CancelTask(AsyncDelegate action, bool restarts = false) : this(restarts)
        {
            AsyncAction = action;
        }

        public CancelTask(SyncDelegate action, bool restarts = false) : this(restarts)
        {
            SyncAction = action;
        }

        //...

        protected virtual void OnCompleted() => Completed?.Invoke(this);

        //...

        public void Cancel() => tokenSource?.Cancel();

        async public Task CancelAsync()
        {
            Cancel();
            await Task.Run(() => { while (started) { } });
        }

        public async Task Start()
        {
            if (started)
            {
                restart = restarts;
                Cancel();
                return;
            }

            Started = true;

            tokenSource = new CancellationTokenSource();
            if (AsyncAction != null)
                await AsyncAction.Invoke(tokenSource.Token);

            if (SyncAction != null)
                await Task.Run(() => SyncAction.Invoke(tokenSource.Token), tokenSource.Token);

            Started = false;
            OnCompleted();

            if (restart)
            {
                restart = false;
                await Start();
            }
        }
    }

    public enum TaskExecution
    {
        Async,
        Sync,
    }

    /// <summary>
    /// Specifies a task that cancels with both a synchronous and asynchronous action, and a parameter of type <see cref="{T}"/>.
    /// </summary>
    public class CancelTask<T> : CancelTask
    {
        class Parameter
        {
            public readonly TaskExecution Execution;

            public readonly T Value;

            public Parameter(T i, TaskExecution e)
            {
                Value
                    = i;
                Execution
                    = e;
            }
        }

        //...

        [field: NonSerialized]
        readonly AsyncDelegate<T> AsyncAction;

        [field: NonSerialized]
        readonly SyncDelegate<T> SyncAction;

        Parameter parameter;

        //...

        public CancelTask(SyncDelegate<T> syncAction, AsyncDelegate<T> asyncAction, bool restarts = false) : base(restarts)
        {
            SyncAction 
                = syncAction;
            AsyncAction 
                = asyncAction;
        }

        //...

        bool CheckStart(T input, TaskExecution e)
        {
            if (started)
            {
                restart
                    = restarts;
                parameter
                    = new Parameter(input, e);

                Cancel();
                return false;
            }
            return true;
        }

        void CheckRestart()
        {
            if (restart)
            {
                restart = false;

                var temp = parameter;
                parameter = default;

                switch (temp.Execution)
                {
                    case TaskExecution.Async:
                        _ = StartAsync(temp.Value);
                        break;
                    case TaskExecution.Sync:
                        Start(temp.Value);
                        break;
                }
            }
        }

        //...

        public void Start(T input)
        {
            if (CheckStart(input, TaskExecution.Sync))
            {
                Started = true;
                SyncAction.Invoke(input);
                Started = false;

                OnCompleted();
                CheckRestart();
            }
        }

        public async Task StartAsync(T input)
        {
            if (CheckStart(input, TaskExecution.Async))
            {
                Started = true;

                tokenSource = new CancellationTokenSource();
                await AsyncAction.Invoke(input, tokenSource.Token);

                Started = false;

                OnCompleted();
                CheckRestart();
            }
        }
    }
}