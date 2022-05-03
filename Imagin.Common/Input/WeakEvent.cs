using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Imagin.Common.Input
{
    public class WeakEvent<TEventArgs>
    {
        readonly List<WeakDelegate> handlers;

        public WeakEvent() => handlers = new List<WeakDelegate>();

        public void Raise(object sender, TEventArgs e)
        {
            lock (handlers)
            {
                handlers.RemoveAll(h => !h.Invoke(sender, e));
            }
        }

        public void Subscribe(EventHandler<TEventArgs> handler)
        {
            var weakHandlers = handler
                .GetInvocationList()
                .Select(d => new WeakDelegate(d))
                .ToList();

            lock (handlers)
            {
                handlers.AddRange(weakHandlers);
            }
        }

        public void Unsubscribe(EventHandler<TEventArgs> handler)
        {
            lock (handlers)
            {
                int index = handlers.FindIndex(h => h.IsMatch(handler));
                if (index >= 0)
                    handlers.RemoveAt(index);
            }
        }

        class WeakDelegate
        {
            #region Open handler generation and cache

            delegate void OpenEventHandler(object target, object sender, TEventArgs e);

            // ReSharper disable once StaticMemberInGenericType (by design)
            static readonly ConcurrentDictionary<MethodInfo, OpenEventHandler> _openHandlerCache =
                new ConcurrentDictionary<MethodInfo, OpenEventHandler>();

            static OpenEventHandler CreateOpenHandler(MethodInfo method)
            {
                var target = Expression.Parameter(typeof(object), "target");
                var sender = Expression.Parameter(typeof(object), "sender");
                var e = Expression.Parameter(typeof(TEventArgs), "e");

                if (method.IsStatic)
                {
                    var expr = Expression.Lambda<OpenEventHandler>(
                        Expression.Call(
                            method,
                            sender, e),
                        target, sender, e);
                    return expr.Compile();
                }
                else
                {
                    var expr = Expression.Lambda<OpenEventHandler>(
                        Expression.Call(
                            Expression.Convert(target, method.DeclaringType),
                            method,
                            sender, e),
                        target, sender, e);
                    return expr.Compile();
                }
            }

            #endregion

            readonly MethodInfo method;

            readonly OpenEventHandler openHandler;

            readonly WeakReference weakTarget;

            public WeakDelegate(Delegate handler)
            {
                weakTarget = handler.Target != null ? new WeakReference(handler.Target) : null;
                method = handler.GetMethodInfo();
                openHandler = _openHandlerCache.GetOrAdd(method, CreateOpenHandler);
            }

            public bool Invoke(object sender, TEventArgs e)
            {
                object target = null;
                if (weakTarget != null)
                {
                    target = weakTarget.Target;
                    if (target == null)
                        return false;
                }
                openHandler(target, sender, e);
                return true;
            }

            public bool IsMatch(EventHandler<TEventArgs> handler)
            {
                return ReferenceEquals(handler.Target, weakTarget?.Target)
                    && handler.GetMethodInfo().Equals(method);
            }
        }
    }
}