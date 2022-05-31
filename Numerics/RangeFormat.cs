using System;

namespace Imagin.Core.Numerics;

/// <summary>Specifies the format of a range.</summary>
[Serializable]
public enum RangeFormat
{
    /// <summary>Specifies a range of [0, 1].</summary>
    Nominal,
    /// <summary>Specifies a standard range.</summary>
    Standard,
    /// <summary>Specifies a zero-based range.</summary>
    ZeroBased,
}