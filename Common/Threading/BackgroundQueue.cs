using System;
using System.Threading;
using System.Threading.Tasks;
using Imagin.Common.Input;

namespace Imagin.Common.Threading
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultEventArgs : EventArgs
    {
        readonly object result;
        /// <summary>
        /// 
        /// </summary>
        public object Result
        {
            get
            {
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Result"></param>
        public ResultEventArgs(object Result) : base()
        {
            result = Result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BackgroundQueue
    {
        object Lock = new object();

        Task previousTask = Task.FromResult(true);

        CancellationTokenSource tokenSource = new CancellationTokenSource();

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
        public event EventHandler<ResultEventArgs> TaskCompleted;

        int count = 0;
        /// <summary>
        /// Gets the number of tasks.
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        bool isCancellationRequested = false;
        /// <summary>
        /// Gets whether or not cancellation was requested.
        /// </summary>
        public bool IsCancellationRequested
        {
            get
            {
                return isCancellationRequested;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Action"></param>
        /// <returns></returns>
        public Task Add(Action Action)
        {
            lock (Lock)
            {
                count++;
                previousTask = previousTask.ContinueWith(t =>
                {
                    Action();
                    OnTaskCompleted(default(object));
                }, tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Action"></param>
        /// <returns></returns>
        public Task Add(Action<CancellationToken> Action)
        {
            lock (Lock)
            {
                count++;
                previousTask = previousTask.ContinueWith(t =>
                {
                    Action(tokenSource.Token);
                    OnTaskCompleted(default(object));
                }, tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Action"></param>
        /// <returns></returns>
        public Task Add<T>(Func<T> Action)
        {
            lock (Lock)
            {
                previousTask = previousTask.ContinueWith(t =>
                {
                    var Result = Action();
                    OnTaskCompleted(Result);
                }, tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Action"></param>
        /// <returns></returns>
        public Task Add<T>(Func<CancellationToken, T> Action)
        {
            lock (Lock)
            {
                count++;
                previousTask = previousTask.ContinueWith(t =>
                {
                    var Result = Action(tokenSource.Token);
                    OnTaskCompleted(Result);
                }, tokenSource.Token, TaskContinuationOptions.NotOnCanceled, TaskScheduler.Default);
                return previousTask;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BackgroundQueue()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnCancelled()
        {
            count = 0;
            previousTask = Task.FromResult(true);
            tokenSource = new CancellationTokenSource();

            Cancelled?.Invoke(this, new EventArgs());
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnTaskCompleted<T>(T Result)
        {
            count--;

            TaskCompleted?.Invoke(this, new ResultEventArgs(Result));

            if (count == 0)
                OnCompleted();
        }

        /// <summary>
        /// Cancels all tasks.
        /// </summary>
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
