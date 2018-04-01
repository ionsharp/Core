using System;
using System.Linq;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Increment"></param>
        /// <returns></returns>
        public static Int32 Add(this Int32 value, Int32 Increment) => value + Increment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 Absolute(this Int32 value) => Math.Abs(value);

        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static Int32 Coerce(this Int32 value, Int32 maximum, Int32 minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static Int32 Divide(this Int32 value, Int32 divisor) => value / divisor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 K(this Int32 value) => value * 1024;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsEven(this Int32 value) => value == 0 ? true : value % 2 == 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsOdd(this Int32 value) => !IsEven(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 M(this Int32 value) => value * 1024 * 1024;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Int32 Multiply(this Int32 value, Int32 scalar) => value * scalar;

        /// <summary>
        /// 
        /// </summary>
        public static Byte[] SplitBytes(this Int32 input)
        {
            String s = input.ToString();
            Byte[] Result = new Byte[s.Length];
            Int32 i = 0;
            foreach (char c in s)
            {
                Result[i] = c.ToString().ToByte();
                i++;
            }
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decrement"></param>
        /// <returns></returns>
        public static Int32 Subtract(this Int32 value, Int32 decrement) => value - decrement;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Byte ToByte(this Int32 value) => Convert.ToByte(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this Int32 value)
            => Convert.ToDecimal(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double ToDouble(this Int32 value) => Convert.ToDouble(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this Int32 value) => Convert.ToInt16(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this Int32 value) => Convert.ToInt64(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToOrdinal(this Int32 value)
        {
            var Result = String.Empty;
            switch (value)
            {
                case 1:
                    Result = "st";
                    break;
                case 2:
                    Result = "nd";
                    break;
                case 3:
                    Result = "rd";
                    break;
                default:
                    Result = "th";
                    break;
            }
            return "{0}{1}".F(value, Result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Single ToSingle(this Int32 value) => Convert.ToSingle(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static Boolean WithinRange(this Int32 value, Int32 minimum, Int32 maximum) => value >= minimum && value <= maximum;
    }
}