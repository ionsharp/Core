using System;

namespace Imagin.Common.Analytics
{
    [Flags]
    [Serializable]
    public enum TraceLevel
    {
        [Hidden]
        None = 0,
        Low = 1,
        Normal = 2,
        High = 4,
        [Hidden]
        All = Low | Normal | High
    }
}