using System;

namespace Imagin.Core.Numerics;

/// <summary>Specifies a unit of measurement unique to angles.</summary>
[Serializable]
public enum AngleUnit
{
    /// <summary>
    /// Specifies a degree as the unit of measure.
    /// </summary>
    [Abbreviation("°")]
    [Description("Specifies a degree as the unit of measure.")]
    Degree,
    /// <summary>
    /// Specifies a radian as the unit of measure.
    /// </summary>
    [Abbreviation("rad")]
    [Description("Specifies a radian as the unit of measure.")]
    Radian
}