using System;

namespace Imagin.Common.Numbers
{
    /// <summary>
    /// Specifies an angular unit.
    /// </summary>
    [Serializable]
    public enum AngularUnit
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
}