using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// Stores a single instance of any <see cref="Type"/> indefinitely. 
    /// </summary>
    public static class Get
    {
        static readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();

        //...

        public static T Create<T>(params object[] parameters)
        {
            var result = typeof(T).Create<T>(parameters);
            Register<T>(result);
            return result;
        }

        //...

        public static void Register(Type type, object instance)
        {
            if (instances.ContainsKey(type))
            {
                instances[type] = instance;
                return;
            }
            instances.Add(type, instance);
        }

        public static void Register<T>(object instance) => Register(typeof(T), instance);

        //...

        public static T Current<T>()
        {
            if (instances.ContainsKey(typeof(T)))
                return (T)instances[typeof(T)];

            return default;
        }

        public static T Where<T>() where T : class
        {
            foreach (var i in instances)
            {
                if (i.Key.Inherits(typeof(T), true) || (typeof(T).IsInterface && i.Key.Implements<T>()))
                    return (T)i.Value;
            }

            return default;
        }

        //...

        public static bool Exists<T>() => Current<T>() != null;
    }
}