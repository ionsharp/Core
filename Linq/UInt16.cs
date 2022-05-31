using System;

namespace Imagin.Core.Linq;

public static class XUInt16
{
    public static ushort Clamp(this ushort value, ushort maximum, ushort minimum = 0) 
        => Math.Max(Math.Min(value, maximum), minimum);

    //Conversion

    public static decimal Decimal(this ushort value)
        => Convert.ToDecimal(value);

    public static double Double(this ushort value)
        => Convert.ToDouble(value);

    public static short Int16(this ushort value)
        => Convert.ToInt16(value);

    public static int Int32(this ushort value)
        => Convert.ToInt32(value);

    public static long Int64(this ushort value)
        => Convert.ToInt64(value);

    public static float Single(this ushort value)
        => Convert.ToSingle(value);

    public static uint UInt32(this ushort value)
        => Convert.ToUInt32(value);

    public static ulong UInt64(this ushort value)
        => Convert.ToUInt64(value);
}