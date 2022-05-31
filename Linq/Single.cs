using System;

namespace Imagin.Core.Linq
{
    public static class XSingle
    {
        public static float Absolute(this float value) => Math.Abs(value);

        public static float Ceiling(this float value) => Math.Ceiling(value).Single();

        public static float Clamp(this float value, float maximum, float minimum = 0f) => Math.Max(Math.Min(value, maximum), minimum);

        public static float Floor(this float value) => Math.Floor(value).Single();

        public static float Round(this float value, int digits = 0)
            => Math.Round(value, digits).Single();

        //Conversion

        public static decimal Decimal(this float value) => Convert.ToDecimal(value);

        public static double Double(this float value) => Convert.ToDouble(value);

        public static short Int16(this float value) => Convert.ToInt16(value);

        public static int Int32(this float value) => Convert.ToInt32(value);

        public static long Int64(this float value) => Convert.ToInt64(value);

        public static ushort UInt16(this float value) => Convert.ToUInt16(value);

        public static uint UInt32(this float value) => Convert.ToUInt32(value);

        public static ulong UInt64(this float value) => Convert.ToUInt64(value);
    }
}