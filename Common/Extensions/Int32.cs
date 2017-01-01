using Imagin.Common.Data;
using System;
using System.Linq;

namespace Imagin.Common.Extensions
{
    public static class Int32Extensions
    {
        public static int Add(this int ToAdd, int Increment)
        {
            return ToAdd + Increment;
        }

        public static int Coerce(this int ToCoerce, int Maximum, int Minimum = 0)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        public static int Divide(this int ToDivide, int Divisor)
        {
            return ToDivide / Divisor;
        }

        public static int Multiply(this int ToMultiply, int Scalar)
        {
            return ToMultiply * Scalar;
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

        public static int Subtract(this int ToSubtract, int Decrement)
        {
            return ToSubtract - Decrement;
        }

        public static byte ToByte(this int ToConvert)
        {
            return Convert.ToByte(ToConvert);
        }

        public static double ToDouble(this int ToConvert)
        {
            return Convert.ToDouble(ToConvert);
        }

        public static string ToFileSize(this int Bytes, FileSizeFormat FileSizeFormat = FileSizeFormat.BinaryUsingSI, int RoundTo = 2)
        {
            return Convert.ToDouble(Bytes).ToFileSize(FileSizeFormat, RoundTo);
        }

        public static string ToOrdinal(this int Number)
        {
            string Result = Number.ToString();
            if (Number == 1)
                Result += "st";
            else if (Number == 2)
                Result += "nd";
            else if (Number == 3)
                Result += "rd";
            else
                Result += "th";
            return Result;
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
