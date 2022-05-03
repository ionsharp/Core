namespace Imagin.Common.Media
{
    public enum GraphicUnit
    {
        /// <summary>
        /// Specifies a device pixel as the unit of measure.
        /// </summary>
        [Abbreviation("px")]
        Pixel,
        /// <summary>
        /// Specifies an inch as the unit of measure.
        /// </summary>
        [Abbreviation("in")]
        Inch,
        /// <summary>
        /// Specifies a centimeter (1/2.54 inch) as the unit of measure.
        /// </summary>
        [Abbreviation("cm")]
        Centimeter,
        /// <summary>
        /// Specifies a millimeter (1/25.4 inch) as the unit of measure.
        /// </summary>
        [Abbreviation("mm")]
        Millimeter,
        /// <summary>
        /// Specifies a printer's point (1/72 inch) as the unit of measure.
        /// </summary>
        [Abbreviation("pt")]
        Point,
        /// <summary>
        /// Specifies a pica (1/6 inch) as the unit of measure.
        /// </summary>
        [Abbreviation("pc")]
        Pica,
        /// <summary>
        /// Specifies a twip (1/1140 inch) as the unit of measure.
        /// </summary>
        [Abbreviation("tw")]
        Twip,
        /// <summary>
        /// Specifies a character (1/12 inch) as the unit of measure.
        /// </summary>
        [Abbreviation("ch")]
        Character,
        /// <summary>
        /// Specifies an en (1/144.54 inch) as the unit of measure.
        /// </summary>
        [Abbreviation("en")]
        En
    }
}