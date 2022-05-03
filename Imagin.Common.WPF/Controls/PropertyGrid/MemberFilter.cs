using System;

namespace Imagin.Common.Controls
{
    [Flags]
    [Serializable]
    public enum MemberFilter
    {
        [Hidden]
        None = 0,
        Entry = 1,
        Field = 2,
        Property = 4,
        [Hidden]
        All = Entry | Field | Property
    }
}