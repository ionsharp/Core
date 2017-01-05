using Imagin.Common.Data;
using System;
using System.Linq;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Increment"></param>
        /// <returns></returns>
        public static int Add(this int Value, int Increment)
        {
            return Value + Increment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCoerce"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static int Coerce(this int ToCoerce, int Maximum, int Minimum = 0)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Divisor"></param>
        /// <returns></returns>
        public static int Divide(this int Value, int Divisor)
        {
            return Value / Divisor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int K(this int Value)
        {
            return Value * 1024;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int M(this int Value)
        {
            return Value * 1024 * 1024;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Scalar"></param>
        /// <returns></returns>
        public static int Multiply(this int Value, int Scalar)
        {
            return Value * Scalar;
        }

        /// <summary>
        /// Generates a random string with numeric length.
        /// </summary>
        public static string Random(this int Length)
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var Random = new Random();
            return new string(Enumerable.Repeat(Chars, Length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Converts int to byte array.
        /// </summary>
        public static byte[] SplitBytes(this int ToSplit)
        {
            string s = ToSplit.ToString();
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
        /// <param name="Value"></param>
        /// <param name="Decrement"></param>
        /// <returns></returns>
        public static int Subtract(this int Value, int Decrement)
        {
            return Value - Decrement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static byte ToByte(this int Value)
        {
            return Convert.ToByte(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double ToDouble(this int Value)
        {
            return Convert.ToDouble(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static long ToInt64(this int Value)
        {
            return Convert.ToInt64(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Bytes"></param>
        /// <param name="FileSizeFormat"></param>
        /// <param name="RoundTo"></param>
        /// <returns></returns>
        public static string ToFileSize(this int Bytes, FileSizeFormat FileSizeFormat = FileSizeFormat.BinaryUsingSI, int RoundTo = 2)
        {
            return Bytes.ToInt64().ToFileSize(FileSizeFormat, RoundTo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string ToOrdinal(this int Value)
        {
            var Result = string.Empty;
            switch (Value)
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
            return "{0}{1}".F(Value, Result);
        }

        /// <summary>
        /// Checks if given number is within given range.
        /// </summary>
        public static bool WithinRange(this int Number, int Min, int Max)
        {
            return Number >= Min && Number <= Max;
        }
    }
}
