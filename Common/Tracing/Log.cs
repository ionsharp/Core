using Imagin.Common.Config;
using System;
using System.Windows;

namespace Imagin.Common.Tracing
{
    /// <summary>
    /// A facility for accessing the log of an application (if it exists) globally; note, <see cref="Application.Current"/> must implement <see cref="IApp"/>.
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
            if (App != null)
                Action(App);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Kind"></param>
        public static void Write(string Message, LogEntryKind Kind)
        {
            Write((i) => i.Log.Write(Message, Kind));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Level"></param>
        /// <param name="Kind"></param>
        public static void Write(string Message, WarningLevel Level = WarningLevel.Moderate, LogEntryKind Kind = LogEntryKind.Info)
        {
            Write((i) => i.Log.Write(Message, Level, Kind));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Message"></param>
        /// <param name="Kind"></param>
        public static void Write(string Source, string Message, LogEntryKind Kind)
        {
            Write((i) => i.Log.Write(Source, Message, Kind));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Message"></param>
        /// <param name="Level"></param>
        /// <param name="Kind"></param>
        public static void Write(string Source, string Message, WarningLevel Level = WarningLevel.Moderate, LogEntryKind Kind = LogEntryKind.Info)
        {
            Write((i) => i.Log.Write(Source, Message, Level, Kind));
        }
    }
}
