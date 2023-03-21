using Imagin.Core.Reflection;
using System;

namespace Imagin.Core.Numerics;

/// <summary>Specifies a dimension with <i>n</i> [1, 3] axes.</summary>
[Serializable]
public enum Dimensions
{
    /// <summary>Specifies a dimension with 1 axis (X).</summary>
    [Image("Dimension1.png", AssemblyType.Core), Name("1D")]
    One,
    /// <summary>Specifies a dimension with 2 axes (X|Y).</summary>
    [Image("Dimension2.png", AssemblyType.Core), Name("2D")]
    Two,
    /// <summary>Specifies a dimension with 3 axes (X|Y|Z).</summary>
    [Image("Dimension3.png", AssemblyType.Core), Name("3D")]
    Three
}