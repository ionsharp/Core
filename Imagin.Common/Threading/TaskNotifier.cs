using System.Threading;
using System.Threading.Tasks;

namespace Imagin.Common.Threading
{
    public sealed class TaskNotifier<T> : Base
    {
        public string ErrorMessage
            => Task.Exception?.Message;

        public bool IsCompleted 
            => Task.IsCompleted;

        public bool IsSuccessfullyCompleted 
            => Task.Status == TaskStatus.RanToCompletion;

        public bool IsCanceled 
            => Task.IsCanceled;

        public bool IsFaulted
            => Task.IsFaulted;

        //...

        public T Result
            => (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default;

        public Task<T> Task { get; private set; }

        //...

        public TaskNotifier(Task<T> task)
        {
            Task = task;
            if (task.IsCompleted) return;

            var scheduler 
                = (SynchronizationContext.Current == null) 
                ? TaskScheduler.Current 
                : TaskScheduler.FromCurrentSynchronizationContext();

            task.ContinueWith(i =>
            {
                this.Changed(() => IsCompleted);
                if (i.IsCanceled)
                    this.Changed(() => IsCanceled);

                else if (i.IsFaulted)
                {
                    this.Changed(() => ErrorMessage);
                    this.Changed(() => IsFaulted);
                }
                else
                {
                    this.Changed(() => IsSuccessfullyCompleted);
                    this.Changed(() => Result);
                }
            }, 
            CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, scheduler);
        }
    }
}