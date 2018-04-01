using System;
using System.Collections.Generic;
using System.Threading;

namespace Imagin.Common.Input
{
    /// <summary>
    /// 
    /// </summary>
    public static class WeakEventHandlerManager
    {
        static readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

        static void CallHandler(object sender, EventHandler eventHandler)
        {
            if (eventHandler != null)
            {
                if (synchronizationContext != null)
                {
                    synchronizationContext.Post((o) => eventHandler(sender, EventArgs.Empty), null);
                }
                else eventHandler(sender, EventArgs.Empty);
            }
        }

        static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler[] callees, int count)
        {
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = handlers[i];
                EventHandler handler = reference.Target as EventHandler;

                if (handler == null)
                {
                    handlers.RemoveAt(i);
                }
                else
                {
                    callees[count] = handler;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="handler"></param>
        /// <param name="defaultListSize"></param>
        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handler == null)

            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="handlers"></param>
        public static void CallWeakReferenecHandlers(object sender, List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                var calllees = new EventHandler[handlers.Count];
                int count = 0;

                count = CleanupOldHandlers(handlers, calllees, count);

                for (int i = 0; i < count; i++)
                {
                    CallHandler(sender, calllees[i]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="handler"></param>
        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers != null)
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    EventHandler existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler))
                    {
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
    }
}
