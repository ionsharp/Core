using Imagin.Common.Data;
using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Increment"></param>
        /// <returns></returns>
        public static double Add(this double Value, double Increment)
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
        public static double Coerce(this double ToCoerce, double Maximum, double Minimum = 0.0)
        {
            return ToCoerce > Maximum ? Maximum : (ToCoerce < Minimum ? Minimum : ToCoerce);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCoerce"></param>
        /// <param name="MinMax"></param>
        /// <param name="MinOrMax"></param>
        /// <returns></returns>
        public static double Coerce(this double ToCoerce, double MinMax, bool MinOrMax)
        {
            return MinOrMax ? (ToCoerce < MinMax ? MinMax : ToCoerce) : (ToCoerce > MinMax ? MinMax : ToCoerce);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Divisor"></param>
        /// <returns></returns>
        public static double Divide(this double Value, double Divisor)
        {
            return Value / Divisor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Elapsed"></param>
        /// <param name="TotalBytes"></param>
        /// <param name="ProcessedBytes"></param>
        /// <returns></returns>
        public static TimeSpan GetRemaining(this TimeSpan Elapsed, long TotalBytes, long ProcessedBytes)
        {
            var Lines = (ProcessedBytes.ToDouble() / TotalBytes.ToDouble()).Multiply(100.0);
            Lines = Lines == 0.0 ? 1.0 : Lines;
            return TimeSpan.FromSeconds(Elapsed.TotalSeconds.Divide(Lines).Multiply(100.0.Subtract(Lines)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Scalar"></param>
        /// <returns></returns>
        public static double Multiply(this double Value, double Scalar)
        {
            return Value * Scalar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Factor"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Decrement"></param>
        /// <returns></returns>
        public static double Subtract(this double Value, double Decrement)
        {
            return Value - Decrement;
        }

        /// <summary>
        /// Converts double to byte.
        /// </summary>
        public static byte ToByte(this double Value)
        {
            return Convert.ToByte(Value);
        }

        /// <summary>
        /// Converts double to int.
        /// </summary>
        public static int ToInt32(this double Value)
        {
            return Convert.ToInt32(Value);
        }

        /// <summary>
        /// Converts double to long.
        /// </summary>
        public static long ToInt64(this double Value)
        {
            return Convert.ToInt64(Value);
        }

        /// <summary>
        /// Converts angle to radians.
        /// </summary>
        public static double ToRadians(this double Value)
        {
            return (Math.PI / 180.0) * Value;
        }
    }
}
