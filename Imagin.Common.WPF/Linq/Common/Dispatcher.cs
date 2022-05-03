using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Imagin.Common.Linq
{
    public static class XDispatcher
    {
        public static async Task BeginInvoke(this Dispatcher input, Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
            => await input.BeginInvoke(dispatcherPriority, action);

        public static async Task<T> BeginInvoke<T>(this Dispatcher input, Func<T> action, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
        {
            var result = default(T);
            await input.BeginInvoke(new Action(() => result = action()));
            return result;
        }

        public static object Invoke(this Dispatcher input, Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Background)
            => input.Invoke(dispatcherPriority, action);
    }
}