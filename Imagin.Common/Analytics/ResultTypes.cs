using System;

namespace Imagin.Common.Analytics
{
    [Flags]
    [Serializable]
    public enum ResultTypes
    {
        [Hidden]
        None = 0,
        Error = 1,
        Message = 2,
        Success = 4,
        Warning = 8,
        [Hidden]
        All = Error | Message | Success | Warning
    }
}