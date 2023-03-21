using System;

namespace Imagin.Core.Analytics;

[Flags, Serializable]
public enum ResultLevel
{
    [Hide]
    None = 0,
    [Color("3A3")]
    Low = 1,
    [Color("FC0")]
    Normal = 2,
    [Color("C30")]
    High = 4,
    [Hide]
    All = Low | Normal | High
}