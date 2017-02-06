using Imagin.Common.Data;
using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int64Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Increment"></param>
        /// <returns></returns>
        public static long Add(this long Value, long Increment)
        {
            return Value + Increment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static long Abs(this long Value)
        {
            return Math.Abs(Value);
        }

        /// <summary>
        /// Coerces <see cref="long"/> to given maximum and minimum.
        /// </summary>
        /// <param name="Value">The value to coerce.</param>
        /// <param name="Maximum">The maximum to coerce to.</param>
        /// <param name="Minimum">The minimum to coerce to.</param>
        /// <returns></returns>
        public static long Coerce(this long Value, long Maximum, long Minimum = 0L)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }

        /// <summary>
        /// Coerces <see cref="long"/> to given limit, which can be minimal or maximal.
        /// </summary>
        /// <param name="Value">The value to coerce.</param>
        /// <param name="Limit">The minimum or maximum to coerce to.</param>
        /// <param name="MinimumOrMaximum">Whether or not to coerce to minimum or maximum.</param>
        /// <returns></returns>
        public static long Coerce(this long Value, long Limit, bool MinimumOrMaximum)
        {
            if ((MinimumOrMaximum && Value < Limit) || (!MinimumOrMaximum && Value > Limit))
                return Limit;

            return Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToDivide"></param>
        /// <param name="Divisor"></param>
        /// <returns></returns>
        public static long Divide(this long ToDivide, long Divisor)
        {
            return ToDivide / Divisor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static long K(this long Value)
        {
            return Value * 1024L;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static long M(this long Value)
        {
            return Value * 1024L * 1024L;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Scalar"></param>
        /// <returns></returns>
        public static long Multiply(this long Value, long Scalar)
        {
            return Value * Scalar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Decrement"></param>
        /// <returns></returns>
        public static long Subtract(this long Value, long Decrement)
        {
            return Value - Decrement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double ToDouble(this long Value)
        {
            return Convert.ToDouble(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double ToInt32(this long Value)
        {
            return Convert.ToInt32(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Bytes"></param>
        /// <param name="FileSizeFormat"></param>
        /// <param name="RoundTo"></param>
        /// <returns></returns>
        public static string ToFileSize(this long Bytes, FileSizeFormat FileSizeFormat = FileSizeFormat.BinaryUsingSI, int RoundTo = 2)
        {
            if (FileSizeFormat == FileSizeFormat.Bytes)
                return Bytes.ToString();

            var Format = FileSizeFormat == FileSizeFormat.BinaryUsingSI || FileSizeFormat == FileSizeFormat.IECBinary ? 1024L : 1000L;

            var Labels = new string[,]
            {
                { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" },
                { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" },
            };

            var f = FileSizeFormat == FileSizeFormat.BinaryUsingSI || FileSizeFormat == FileSizeFormat.DecimalUsingSI ? 1 : 0;

            var Result = -1d;
            for (var i = 8; i >= 0; i--)
            {
                if (i == 0)
                {
                    Result = Bytes;
                }
                else if (i == 1 && Bytes >= Format)
                {
                    Result = Bytes / Format;
                }
                else
                {
                    var k = Math.Pow(Format, i);
                    Result = Bytes >= k ? Bytes / k : Result;
                }

                if (Result >= 0)
                    return "{0} {1}".F(Result.Round(RoundTo), Labels[f, i]);
            }

            return string.Empty;
        }
    }
}
