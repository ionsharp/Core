using Imagin.Common.Config;
using System;
using System.Windows;

namespace Imagin.Common.Tracing
{
    /// <summary>
    /// A facility to access the log owned by the current application; <see cref="Application.Current"/> must implement <see cref="IApp"/>.
    /// </summary>
    public static class Log
    {
        static IApp App
        {
            get
            {
                return Application.Current as IApp;
            }
        }

        static void Write(Action<IApp> Action)
        {
            if (App != null) Action(App);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        public static void Write(string Message, LogEntryType Type = LogEntryType.Message)
        {
            Write((i) => i.Log.Write(Message, Type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Source"></param>
        /// <param name="Type"></param>
        public static void Write(string Message, object Source, LogEntryType Type = LogEntryType.Message)
        {
            Write((i) => i.Log.Write(Message, Source, Type));
        }
    }
}
