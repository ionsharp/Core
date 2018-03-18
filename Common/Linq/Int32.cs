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
        public static int Add(this int value, int Increment) => value + Increment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Absolute(this int value) => Math.Abs(value);

        /// <summary>
        /// Coerces to the specified range.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static int Coerce(this int value, int maximum, int minimum = 0) => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static int Divide(this int value, int divisor) => value / divisor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int K(this int value) => value * 1024;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEven(this int value) => value == 0 ? true : value % 2 == 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOdd(this int value) => !IsEven(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int M(this int value) => value * 1024 * 1024;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static int Multiply(this int value, int scalar) => value * scalar;

        /// <summary>
        /// Generates a random string with numeric length.
        /// </summary>
        public static string Random(this int length)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var Random = new Random();
            return new string(Enumerable.Repeat(Chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Converts int to byte array.
        /// </summary>
        public static byte[] SplitBytes(this int input)
        {
            string s = input.ToString();
            byte[] Result = new byte[s.Length];
            int i = 0;
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
        public static int Subtract(this int value, int decrement) => value - decrement;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this int value) => Convert.ToByte(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this int value) => Convert.ToDouble(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToInt16(this int value) => Convert.ToInt16(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToInt64(this int value) => Convert.ToInt64(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToOrdinal(this int value)
        {
            var Result = string.Empty;
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
        public static float ToSingle(this int value) => Convert.ToSingle(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static bool WithinRange(this int value, int minimum, int maximum) => value >= minimum && value <= maximum;
    }
}
