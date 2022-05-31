using System;

namespace Imagin.Core.Linq
{
    public static class XInt16
    {
        public static short Absolute(this short input) => Math.Abs(input);

        public static short Clamp(this short input, short maximum, short minimum = 0) => Math.Max(Math.Min(input, maximum), minimum);

        //Conversion

        public static double Double(this short input) => Convert.ToDouble(input);

        public static int Int32(this short input) => Convert.ToInt32(input);
    }
}