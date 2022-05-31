using System;

namespace Imagin.Core.Linq;

public static class XByte
{
    public static byte Clamp(this byte input, byte maximum, byte minimum = 0) => Math.Max(Math.Min(input, maximum), minimum);

    //Conversion

    public static decimal Decimal(this byte input)
        => Convert.ToDecimal(input);

    public static double Double(this byte input)
        => Convert.ToDouble(input);

    public static int Int32(this byte input)
        => Convert.ToInt32(input);

    public static float Single(this byte input)
        => Convert.ToSingle(input);
}