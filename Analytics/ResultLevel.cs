using System;

namespace Imagin.Core.Analytics
{
    [Flags]
    [Serializable]
    public enum ResultLevel
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