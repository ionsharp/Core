using Imagin.Common.Data;
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
        /// <param name="value"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static ulong Coerce(this ulong value, ulong maximum, ulong minimum = 0L) => Math.Max(Math.Min(value, maximum), minimum);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string ToFileSize(this ulong value, FileSizeFormat format, int round = 1)
        {
            if (format == FileSizeFormat.Bytes)
                return value.ToString();

            var Labels = new string[]
            {
                "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"
            };

            if (format == FileSizeFormat.BinaryUsingSI || format == FileSizeFormat.DecimalUsingSI)
            {
                Labels = new string[]
                {
                    "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
                };
            }

            if (value == 0)
                return "0 B";

            var f = format == FileSizeFormat.BinaryUsingSI || format == FileSizeFormat.IECBinary ? (ulong)1024 : 1000;

            var m = (int)Math.Log(value, f);
            var a = (decimal)value / (1L << (m * 10));

            if (Math.Round(a, round) >= 1000)
            {
                m += 1;
                a /= f;
            }

            return string.Format("{0:n" + round + "} {1}", a, Labels[m]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToInt64(this ulong value)
        {
            return Convert.ToInt64(value);
        }
    }
}
