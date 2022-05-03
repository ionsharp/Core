using System;

namespace Imagin.Common.Linq
{
    public static class XByte
    {
        public static byte Coerce(this byte input, byte maximum, byte minimum = 0) => Math.Max(Math.Min(input, maximum), minimum);

        public static decimal Divide(this byte a, decimal b)
            => a.Decimal() / b;

        public static double Divide(this byte a, double b)
            => a.Double() / b;

        public static float Divide(this byte a, float b)
            => a.Single() / b;

        public static decimal Decimal(this byte input)
            => Convert.ToDecimal(input);

        public static double Double(this byte input)
            => Convert.ToDouble(input);

        public static int Int32(this byte input)
            => Convert.ToInt32(input);

        public static float Single(this byte input)
            => Convert.ToSingle(input);

        public static bool Within(this byte input, byte minimum, byte maximum) => input >= minimum && input <= maximum;
    }
}
