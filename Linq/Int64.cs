using System;

namespace Imagin.Core.Linq;

public static class XInt64
{
    public static long Absolute(this long value) => Math.Abs(value);

    public static long Clamp(this long value, long maximum, long minimum = 0L) => Math.Max(Math.Min(value, maximum), minimum);

    //Conversion

    public static double Double(this long value) => Convert.ToDouble(value);

    public static short Int16(this long value) => Convert.ToInt16(value);

    public static int Int32(this long value) => Convert.ToInt32(value);

    public static float Single(this long value) => Convert.ToSingle(value);

    public static ushort UInt16(this long value) => Convert.ToUInt16(value);

    public static uint UInt32(this long value) => Convert.ToUInt32(value);

    public static ulong UInt64(this long value) => Convert.ToUInt64(value);
}