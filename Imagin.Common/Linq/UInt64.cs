using Imagin.Common.Data;
using System;

namespace Imagin.Common.Linq
{
    public static class XUInt64
    {
        public static ulong Coerce(this ulong value, ulong maximum, ulong minimum = 0L) 
            => Math.Max(Math.Min(value, maximum), minimum);

        public static string FileSize(this ulong value, FileSizeFormat format, int round = 1)
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

            var result = string.Format("{0:n" + round + "}", a);

            var j = result.Length;
            for (var i = result.Length - 1; i >= 0; i--)
            {
                if (result[i] == '.')
                {
                    j--;
                    break;
                }
                if (result[i] == '0')
                {
                    j--;
                }
                else break;
            }

            return $"{result.Substring(0, j)} {Labels[m]}"; ;
        }

        public static decimal Decimal(this ulong value)
            => Convert.ToDecimal(value);

        public static double Double(this ulong value)
            => Convert.ToDouble(value);

        public static short Int16(this ulong value)
            => Convert.ToInt16(value);

        public static int Int32(this ulong value)
            => Convert.ToInt32(value);

        public static long Int64(this ulong value)
            => Convert.ToInt64(value);

        public static ulong Maximum(this ulong input, ulong maximum)
            => input.Coerce(maximum, ulong.MinValue);

        public static ulong Minimum(this ulong input, ulong minimum)
            => input.Coerce(ulong.MaxValue, minimum);

        public static float Single(this ulong value)
            => Convert.ToSingle(value);

        public static ushort UInt16(this ulong value)
            => Convert.ToUInt16(value);

        public static uint UInt32(this ulong value)
            => Convert.ToUInt32(value);

        public static bool Within(this ulong input, ulong minimum, ulong maximum) => input >= minimum && input <= maximum;
    }
}