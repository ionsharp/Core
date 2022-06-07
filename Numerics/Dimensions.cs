using System;

namespace Imagin.Core.Numerics;

[Serializable]
public enum Dimensions
{
    /// <summary>Specifies a dimension with 2 axes (X|Y).</summary>
    [DisplayName("2D")]
    Two,
    /// <summary>Specifies a dimension with 3 axes (X|Y|Z).</summary>
    [DisplayName("3D")]
    Three
}