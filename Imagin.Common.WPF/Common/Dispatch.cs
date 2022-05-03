using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Imagin.Common
{
    public class Dispatch
    {
        public static void Invoke(Action action, DispatcherPriority priority = DispatcherPriority.Background)
            => Application.Current.Dispatcher.Invoke(action, priority);

        async public static Task InvokeAsync(Action action, DispatcherPriority priority = DispatcherPriority.Background)
            => await Application.Current.Dispatcher.BeginInvoke(action, priority);

        async public static Task InvokeAsync(DispatcherPriority priority, Delegate callback, object arguments)
            => await Application.Current.Dispatcher.BeginInvoke(priority, callback, arguments);

        public static DispatcherOperation InvokeReturn(Action action, DispatcherPriority priority = DispatcherPriority.Background)
            => Application.Current.Dispatcher.BeginInvoke(action, priority);
    }
}