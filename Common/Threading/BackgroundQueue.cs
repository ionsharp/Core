using System;
using System.Threading;
using System.Threading.Tasks;

namespace Imagin.Common.Threading
{
    public class BackgroundQueue
    {
        #region Members

        Task PreviousTask = Task.FromResult(true);

        object Lock = new object();

        #endregion

        #region Methods

        public Task QueueTask(Action Action)
        {
            lock (Lock)
            {
                PreviousTask = PreviousTask.ContinueWith(t => Action(), CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
                return PreviousTask;
            }
        }

        public Task<T> QueueTask<T>(Func<T> Work)
        {
            lock (Lock)
            {
                var Task = PreviousTask.ContinueWith(t => Work(), CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
                PreviousTask = Task;
                return Task;
            }
        }

        #endregion

        #region BackgroundQueue

        public BackgroundQueue()
        {
        }

        #endregion
    }
}
