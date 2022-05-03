using System;

namespace Imagin.Common.Linq
{
    public static class XInt16
    {
        public static short Absolute(this short input) => Math.Abs(input);

        public static short Coerce(this short input, short maximum, short minimum = 0) => Math.Max(Math.Min(input, maximum), minimum);

        public static double Double(this short input) => Convert.ToDouble(input);

        public static int Int32(this short input) => Convert.ToInt32(input);

        public static short Maximum(this short input, short maximum) => input.Coerce(maximum, short.MinValue);

        public static short Minimum(this short input, short minimum) => input.Coerce(short.MaxValue, minimum);

        public static bool Within(this short input, short minimum, short maximum) => input >= minimum && input <= maximum;
    }
}