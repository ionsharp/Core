using Imagin.Common.Data;
using System;

namespace Imagin.Common.Extensions
{
    public static class DoubleExtensions
    {
        public static double Add(this double ToAdd, double Increment)
        {
            return ToAdd + Increment;
        }

        public static double Coerce(this double ToCoerce, double Maximum, double Minimum = 0.0)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        public static double Coerce(this double ToCoerce, double MinMax, bool MinOrMax)
        {
            return MinOrMax ? (ToCoerce < MinMax ? MinMax : ToCoerce) : (ToCoerce > MinMax ? MinMax : ToCoerce);
        }

        public static double Divide(this double ToDivide, double Divisor)
        {
            return ToDivide / Divisor;
        }

        public static TimeSpan GetRemaining(this TimeSpan Elapsed, long TotalBytes, long ProcessedBytes)
        {
            var Lines = (ProcessedBytes.ToDouble() / TotalBytes.ToDouble()).Multiply(100.0);
            Lines = Lines == 0.0 ? 1.0 : Lines;
            return TimeSpan.FromSeconds(Elapsed.TotalSeconds.Divide(Lines).Multiply(100.0.Subtract(Lines)));
        }

        public static double Multiply(this double ToMultiply, double Scalar)
        {
            return ToMultiply * Scalar;
        }

        public static double NearestFactor(this double Value, double Factor)
        {
            return Math.Round((Value / Factor), MidpointRounding.AwayFromZero) * Factor;
        }

        /// <summary>
        /// Rounds double to variable places.
        /// </summary>
        public static double Round(this double ToRound, int Digits = 0)
        {
            return Math.Round(ToRound, Digits);
        }

        /// <summary>
        /// Shifts decimal variable places.
        /// </summary>
        public static double Shift(this double ToShift, int Shifts = 1, bool ShiftRight = true)
        {
            if (Shifts <= 0) Shifts = 1;
            for (int i = 0; i < Shifts; i++)
            {
                if (ShiftRight) ToShift *= 10;
                else ToShift /= 10;
            }
            return ToShift;
        }

        /// <summary>
        /// Shifts decimal point and rounds to variable places.
        /// </summary>
        public static double ShiftRound(this double ToShiftRound, int Shifts = 1, int Digits = 0, bool ShiftRight = true)
        {
            return ToShiftRound.Shift(Shifts, ShiftRight).Round(Digits);
        }

        public static double Subtract(this double ToSubtract, double Decrement)
        {
            return ToSubtract - Decrement;
        }

        /// <summary>
        /// Converts double to byte.
        /// </summary>
        public static byte ToByte(this double ToConvert)
        {
            return Convert.ToByte(ToConvert);
        }

        public static string ToFileSize(this double Bytes, FileSizeFormat FileSizeFormat = FileSizeFormat.BinaryUsingSI, int RoundTo = 2)
        {
            if (FileSizeFormat == FileSizeFormat.Bytes)
                return Bytes.ToString();

            var x = FileSizeFormat == FileSizeFormat.BinaryUsingSI || FileSizeFormat == FileSizeFormat.IECBinary ? 1024 : 1000;

            var a = new string[]
            {
                "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB",
            };
            var b = new string[]
            {
                "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB",
            };
            var c = FileSizeFormat == FileSizeFormat.BinaryUsingSI || FileSizeFormat == FileSizeFormat.DecimalUsingSI ? b : a;

            var f = new Func<double, string, string>((Value, Suffix) =>
            {
                return string.Format("{0} {1}", Value, Suffix);
            });

            for (var i = 8; i >= 0; i--)
            {
                var e = c[i];

                if (i == 0)
                    return f(Bytes, e);
                else if (i == 1 && Bytes >= x)
                    return f((Bytes / x).Round(RoundTo), e);
                else if (Bytes >= Math.Pow(x, i))
                    return f((Bytes / Math.Pow(x, i)).Round(RoundTo), e);
            }

            return string.Empty;
        }

        /// <summary>
        /// Converts double to int.
        /// </summary>
        public static int ToInt(this double ToConvert)
        {
            return Convert.ToInt32(ToConvert);
        }

        /// <summary>
        /// Converts double to long.
        /// </summary>
        public static long ToLong(this double ToConvert)
        {
            return Convert.ToInt64(ToConvert);
        }

        /// <summary>
        /// Converts angle to radians.
        /// </summary>
        public static double ToRadians(this double ToConvert)
        {
            return (Math.PI / 180.0) * ToConvert;
        }
    }
}
