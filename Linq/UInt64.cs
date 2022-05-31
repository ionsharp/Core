using System;

namespace Imagin.Core.Linq;

public static class XUInt64
{
    public static ulong Clamp(this ulong value, ulong maximum, ulong minimum = 0L) 
        => Math.Max(Math.Min(value, maximum), minimum);

    //Conversion

    public static decimal Decimal(this ulong value)
        => Convert.ToDecimal(value);

    public static double Double(this ulong value)
        => Convert.ToDouble(value);

    public static short Int16(this ulong value)
        => Convert.ToInt16(value);

    public static int Int32(this ulong value)
        => Convert.ToInt32(value);

    public static long Int64(this ulong value)
        => Convert.ToInt64(value);

    public static float Single(this ulong value)
        => Convert.ToSingle(value);

    public static ushort UInt16(this ulong value)
        => Convert.ToUInt16(value);

    public static uint UInt32(this ulong value)
        => Convert.ToUInt32(value);
}