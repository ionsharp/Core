using System;

namespace Imagin.Core.Time;

/// <summary>Specifies a time before or after midday.</summary>
[Serializable]
public enum Meridiem
{
    /// <summary>Specifies a time BEFORE midday.</summary>
    AM,
    /// <summary>Specifies a time AFTER midday.</summary>
    PM
}