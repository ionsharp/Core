using System;

namespace Imagin.Common.Controls
{
    [Serializable]
    public enum CellFormats : int
    {
        None,
        Number,
        Date,
        Percentage,
        Fraction,
        Text,
        Time,
        TimeRelative,
        TimeRelativeDifference,
        Custom,
    }
}