using Imagin.Core.Input;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Imagin.Core.Threading
{
    public class TaskQueue
    {
        readonly object Lock = new object();

        Task previousTask = Task.FromResult(true);

        CancellationTokenSource tokenSource = new CancellationTokenSource();

        //...

        /// <summary>
        /// Occurs when all tasks have cancelled.
        /// </summary>
        public event EventHandler<EventArgs> Cancelled;

        /// <summary>
        /// Occurs when all tasks have completed.
        /// </summary>
        public event EventHandler<EventArgs> Completed;

        /// <summary>
        /// Occurs when a task has completed.
        /// </summary>
        public event GenericEventHandler<object> TaskCompleted;

        //...

        int count = 0;
        public int Count
        {
            get
            {
                return count;
            }
        }

        bool isCancellationRequested = false;
        public bool IsCancellationRequested
        {
            get
            {
                return isCancellationRequested;
            }
        }

        //...

        public TaskQueue()
        {
        }

        //...

        public Task Add(Action Action)
        {
            lock (Lock)
            {
                count++;
                previousTask = previousTask.ContinueWith(i =>
                {
                    Action();
                    OnTaskCompleted(default(object));
                }, 
                tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        public Task Add(Action<CancellationToken> Action)
        {
            lock (Lock)
            {
                count++;
                previousTask = previousTask.ContinueWith(i =>
                {
                    Action(tokenSource.Token);
                    OnTaskCompleted(default(object));
                }, 
                tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        public Task Add<T>(Func<T> Action)
        {
            lock (Lock)
            {
                previousTask = previousTask.ContinueWith(i =>
                {
                    var result = Action();
                    OnTaskCompleted(result);
                },
                tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        public Task Add<T>(Func<CancellationToken, T> Action)
        {
            lock (Lock)
            {
                count++;
                previousTask = previousTask.ContinueWith(i =>
                {
                    var result = Action(tokenSource.Token);
                    OnTaskCompleted(result);
                },
                tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        //...

        protected virtual void OnCancelled()
        {
            count = 0;
            previousTask = Task.FromResult(true);
            tokenSource = new CancellationTokenSource();

            Cancelled?.Invoke(this, new EventArgs());
        }
        
        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this, new EventArgs());
        }

        protected virtual void OnTaskCompleted<T>(T input)
        {
            count--;
            TaskCompleted?.Invoke(this, new EventArgs<object>(input));
            if (count == 0)
                OnCompleted();
        }

        //...

        public void CancelAll()
        {
            if (!IsCancellationRequested)
            {
                isCancellationRequested = true;
                tokenSource?.Cancel();

                try
                {
                    previousTask?.Wait();
                }
                catch
                {
                    //System.Diagnostics.Debug.WriteLine("Cancelled");
                }
                finally
                {
                    OnCancelled();
                }
                isCancellationRequested = false;
            }
        }
    }
}