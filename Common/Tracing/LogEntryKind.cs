using System;

namespace Imagin.Common.Tracing
{
    [Flags]
    [Serializable]
    public enum LogEntryKind
    {
        Error,
        Success,
        Warning,
        Info
    }
}
