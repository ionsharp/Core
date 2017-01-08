using Imagin.Common.Attributes;
using System;

namespace Imagin.Common.Drawing
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
        Degree,
        /// <summary>
        /// Specifies a radian as the unit of measure.
        /// </summary>
        [Abbreviation("rad")]
        Radian
    }
}
