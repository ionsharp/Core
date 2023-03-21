using System;

namespace Imagin.Core.Analytics;

[Flags]
[Serializable]
public enum ResultTypes
{
    [Hide]
    None = 0,
    Error = 1,
    Message = 2,
    Success = 4,
    Warning = 8,
    [Hide]
    All = Error | Message | Success | Warning
}