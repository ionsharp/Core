using System;

namespace Imagin.Common.Tracing
{
    [Flags]
    [Serializable]
    public enum LogEntryStatus
    {
        Error,
        Success,
        Warning,
        Info
    }
}
