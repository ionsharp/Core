using Imagin.Common.Analytics;
using System;

namespace Imagin.Common.Configuration
{
    public enum UnhandledExceptions
    {
        AppDomain,
        Dispatcher,
        TaskScheduler
    }

    public delegate void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e);

    public class UnhandledExceptionEventArgs : EventArgs
    {
        public readonly Error Error;

        public readonly UnhandledExceptions Type;

        public UnhandledExceptionEventArgs(UnhandledExceptions type, Error error) : base()
        {
            Type = type;
            Error = error;
        }
    }
}