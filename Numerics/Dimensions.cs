using System;

namespace Imagin.Core.Numerics;

/// <summary>Specifies a dimension with <i>n</i> [1, 3] axes.</summary>
[Serializable]
public enum Dimensions
{
    /// <summary>Specifies a dimension with 1 axis (X).</summary>
    [DisplayName("1D")]
    One,
    /// <summary>Specifies a dimension with 2 axes (X|Y).</summary>
    [DisplayName("2D")]
    Two,
    /// <summary>Specifies a dimension with 3 axes (X|Y|Z).</summary>
    [DisplayName("3D")]
    Three
}