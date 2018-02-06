using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Imagin.Common.Threading
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public sealed class TaskCompletionNotifier<TResult> : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the task being watched. This property never changes and is never <c>null</c>.
        /// </summary>
        public Task<TResult> Task
        {
            get; private set;
        }

        /// <summary>
        /// Gets the result of the task. Returns the default value of TResult if the task has not completed successfully.
        /// </summary>
        public TResult Result
        {
            get
            {
                return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult);
            }
        }

        /// <summary>
        /// Gets whether the task has completed.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return Task.IsCompleted;
            }
        }

        /// <summary>
        /// Gets whether the task has completed successfully.
        /// </summary>
        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task.Status == TaskStatus.RanToCompletion;
            }
        }

        /// <summary>
        /// Gets whether the task has been canceled.
        /// </summary>
        public bool IsCanceled
        {
            get
            { return Task.IsCanceled;
            }
        }

        /// <summary>
        /// Gets whether the task has faulted.
        /// </summary>
        public bool IsFaulted
        {
            get
            {
                return Task.IsFaulted;
            }
        }

        #endregion

        #region TaskCompletionNotifier

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public TaskCompletionNotifier(Task<TResult> task)
        {
            this.Task = task;
            if (task.IsCompleted) return;

            var Scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();
            task.ContinueWith(t =>
            {
                var p = PropertyChanged;
                if (p != null)
                {
                    p(this, new PropertyChangedEventArgs("IsCompleted"));
                    if (t.IsCanceled)
                        p(this, new PropertyChangedEventArgs("IsCanceled"));
                    else if (t.IsFaulted)
                    {
                        p(this, new PropertyChangedEventArgs("IsFaulted"));
                        p(this, new PropertyChangedEventArgs("ErrorMessage"));
                    }
                    else
                    {
                        p(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                        p(this, new PropertyChangedEventArgs("Result"));
                    }
                }
            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, Scheduler);
        }

        #endregion
    }
}
