using System;

namespace Imagin.Common.Linq
{
    public static class XUInt16
    {
        public static ushort Coerce(this ushort value, ushort maximum, ushort minimum = 0) 
            => Math.Max(Math.Min(value, maximum), minimum);

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

        public static ushort Maximum(this ushort input, ushort maximum)
            => input.Coerce(maximum, ushort.MinValue);

        public static ushort Minimum(this ushort input, ushort minimum)
            => input.Coerce(ushort.MaxValue, minimum);

        public static float Single(this ushort value)
            => Convert.ToSingle(value);

        public static uint UInt32(this ushort value)
            => Convert.ToUInt32(value);

        public static ulong UInt64(this ushort value)
            => Convert.ToUInt64(value);

        public static bool Within(this ushort input, ushort minimum, ushort maximum) => input >= minimum && input <= maximum;
    }
}