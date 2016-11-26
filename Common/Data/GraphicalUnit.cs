using Imagin.Common.Attributes;
using System;

namespace Imagin.Common.Measurement
{
    [Serializable]
    public enum GraphicalUnit
    {
        [Abbreviation("px")]
        Pixels,
        [Abbreviation("in")]
        Inches,
        [Abbreviation("cm")]
        Centimeters,
        [Abbreviation("mm")]
        Millimeters,
        [Abbreviation("pt")]
        Points,
        [Abbreviation("pc")]
        Picas,
        [Abbreviation("tw")]
        Twips,
        [Abbreviation("ch")]
        Characters,
        [Abbreviation("en")]
        Ens
    }
}
