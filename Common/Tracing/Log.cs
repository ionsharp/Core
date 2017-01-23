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

        public static void Write(string Message)
        {
            Write((i) => i.Log.Write(Message));
        }

        public static void Write(string Source, string Message)
        {
            Write((i) => i.Log.Write(Source, Message));
        }

        public static void Write(LogEntryStatus Status, string Source, string Message)
        {
            Write((i) => i.Log.Write(Status, Source, Message));
        }

        public static void Write(WarningLevel WarningLevel, LogEntryStatus Status, string Source, string Message)
        {
            Write((i) => i.Log.Write(WarningLevel, Status, Source, Message));
        }
    }
}
