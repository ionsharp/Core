using System;

namespace Imagin.Core.Linq;

public static class XDecimal
{
    public static decimal Clamp(this decimal value, decimal maximum, decimal minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);

    //Conversion

    public static double Double(this decimal input) => Convert.ToDouble(input);
}