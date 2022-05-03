using System;

namespace Imagin.Common.Linq
{
    public static class XDecimal
    {
        public static decimal Coerce(this decimal value, decimal maximum, decimal minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);

        public static double Double(this decimal input) => Convert.ToDouble(input);

        public static bool Within(this decimal input, decimal minimum, decimal maximum) => input >= minimum && input <= maximum;
    }
}