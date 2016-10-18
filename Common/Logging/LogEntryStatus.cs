using System;

namespace Imagin.Common.Logging
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
