using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class UInt64Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static ulong Coerce(this ulong Value, ulong Maximum, ulong Minimum = 0L)
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="FileSizeFormat"></param>
        /// <param name="RoundTo"></param>
        /// <returns></returns>
        public static string ToFileSize(this ulong Value, FileSizeFormat FileSizeFormat, int RoundTo = 1)
        {
            if (FileSizeFormat == FileSizeFormat.Bytes)
                return Value.ToString();

            var Labels = new string[]
            {
                "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"
            };

            if (FileSizeFormat == FileSizeFormat.BinaryUsingSI || FileSizeFormat == FileSizeFormat.DecimalUsingSI)
            {
                Labels = new string[]
                {
                    "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
                };
            }

            if (Value == 0)
                return "0 B";

            var f = FileSizeFormat == FileSizeFormat.BinaryUsingSI || FileSizeFormat == FileSizeFormat.IECBinary ? (ulong)1024 : 1000;

            var m = (int)Math.Log(Value, f);
            var a = (decimal)Value / (1L << (m * 10));

            if (Math.Round(a, RoundTo) >= 1000)
            {
                m += 1;
                a /= f;
            }

            return string.Format("{0:n" + RoundTo + "} {1}", a, Labels[m]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static long ToInt64(this ulong Value)
        {
            return Convert.ToInt64(Value);
        }
    }
}
