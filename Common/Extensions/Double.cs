using System;

namespace Imagin.Common.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static double Add(this double ToAdd, double Increment)
        {
            return ToAdd + Increment;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static double Coerce(this double ToCoerce, double Maximum, double Minimum = 0.0)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static double Divide(this double ToDivide, double Divisor)
        {
            return ToDivide - Divisor;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static double Multiply(this double ToMultiply, double Scalar)
        {
            return ToMultiply - Scalar;
        }

        /// <summary>
        /// Imagin.Common: Rounds double to variable places.
        /// </summary>
        public static double Round(this double ToRound, int Digits = 0)
        {
            return Math.Round(ToRound, Digits);
        }

        /// <summary>
        /// Imagin.Common: Shifts decimal variable places.
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
        /// Imagin.Common: Shifts decimal point and rounds to variable places.
        /// </summary>
        public static double ShiftRound(this double ToShiftRound, int Shifts = 1, int Digits = 0, bool ShiftRight = true)
        {
            return ToShiftRound.Shift(Shifts, ShiftRight).Round(Digits);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static double Subtract(this double ToSubtract, double Decrement)
        {
            return ToSubtract - Decrement;
        }

        /// <summary>
        /// Imagin.Common: Converts double to byte.
        /// </summary>
        public static byte ToByte(this double ToConvert)
        {
            return Convert.ToByte(ToConvert);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string ToFileSize(this double Bytes)
        {
            const int byteConversion = 1024;
            if (Bytes >= Math.Pow(byteConversion, 3)) //GB Range
                return string.Concat(Math.Round(Bytes / Math.Pow(byteConversion, 3), 2), " GB");
            else if (Bytes >= Math.Pow(byteConversion, 2)) //MB Range
                return string.Concat(Math.Round(Bytes / Math.Pow(byteConversion, 2), 2), " MB");
            else if (Bytes >= byteConversion) //KB Range
                return string.Concat(Math.Round(Bytes / byteConversion, 2), " KB");
            else
                return string.Concat(Bytes, " B");
        }

        /// <summary>
        /// Imagin.Common: Converts double to int.
        /// </summary>
        public static int ToInt(this double ToConvert)
        {
            return Convert.ToInt32(ToConvert);
        }

        /// <summary>
        /// Imagin.Common: Converts angle to radians.
        /// </summary>
        public static double ToRadians(this double ToConvert)
        {
            return (Math.PI / 180.0) * ToConvert;
        }
    }
}
