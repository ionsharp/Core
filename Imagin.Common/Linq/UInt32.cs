using System;

namespace Imagin.Common.Linq
{
    public static class XUInt32
    {
        public static uint Coerce(this uint value, uint maximum, uint minimum = 0)
            => Math.Max(Math.Min(value, maximum), minimum);

        public static decimal Decimal(this uint value)
            => Convert.ToDecimal(value);

        public static double Double(this uint value)
            => Convert.ToDouble(value);

        public static short Int16(this uint value)
            => Convert.ToInt16(value);

        public static int Int32(this uint value)
            => Convert.ToInt32(value);

        public static long Int64(this uint value)
            => Convert.ToInt64(value);

        public static uint Maximum(this uint input, uint maximum)
            => input.Coerce(maximum, uint.MinValue);

        public static uint Minimum(this uint input, uint minimum)
            => input.Coerce(uint.MaxValue, minimum);

        public static float Single(this uint value)
            => Convert.ToSingle(value);

        public static ushort UInt16(this uint value)
            => Convert.ToUInt16(value);

        public static ulong UInt64(this uint value)
            => Convert.ToUInt64(value);

        public static bool Within(this uint input, uint minimum, uint maximum) => input >= minimum && input <= maximum;
    }
}