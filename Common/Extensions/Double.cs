using Imagin.Common.Drawing;
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
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double Abs(this double Value)
        {
            return Math.Abs(Value);
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
        /// <param name="Value"></param>
        /// <param name="Digits"></param>
        /// <returns></returns>
        public static double Round(this double Value, int Digits = 0)
        {
            return Math.Round(Value, Digits);
        }

        /// <summary>
        /// Shifts decimal variable places. <para>If negative, shift left; otherwise, shift right.</para>
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Shifts"></param>
        /// <returns></returns>
        public static double Shift(this double Value, int Shifts = 1)
        {
            var LeftOrRight = Shifts < 0;
            for (var i = 0; i < Shifts.Abs(); i++)
                Value = LeftOrRight ? Value / 10 : Value * 10;
            return Value;
        }

        /// <summary>
        /// Shifts decimal point and rounds to variable places. <para>If negative, shift left; otherwise, shift right.</para>
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Shifts"></param>
        /// <param name="Digits"></param>
        /// <returns></returns>
        public static double ShiftRound(this double Value, int Shifts = 1, int Digits = 0)
        {
            return Value.Shift(Shifts).Round(Digits);
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
        /// Convert given radians to angle.
        /// </summary>
        public static double ToAngle(this double Radians)
        {
            return Radians.Multiply(180d / Math.PI);
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
            return (Math.PI / 180d) * Value;
        }

        /// <summary>
        /// Converts a graphical unit value to another graphical unit value.
        /// </summary>
        /// <param name="Value">Number of units.</param>
        /// <param name="From">The unit to convert from.</param>
        /// <param name="To">The unit to convert to.</param>
        /// <param name="Ppi">Pixels per inch.</param>
        /// <param name="RoundTo">Decimal places to round to.</param>
        public static double ToUnit(this double Value, GraphicalUnit From, GraphicalUnit To, double Ppi = 72.0)
        {
            //Convert to pixels
            double Pixels = 0.0;
            switch (From)
            {
                case GraphicalUnit.Pixel:
                    Pixels = Math.Round(Value, 0);
                    break;
                case GraphicalUnit.Inch:
                    Pixels = Math.Round(Value * Ppi, 0);
                    break;
                case GraphicalUnit.Centimeter:
                    Pixels = Math.Round((Value * Ppi) / 2.54, 0);
                    break;
                case GraphicalUnit.Millimeter:
                    Pixels = Math.Round((Value * Ppi) / 25.4, 0);
                    break;
                case GraphicalUnit.Point:
                    Pixels = Math.Round((Value * Ppi) / 72, 0);
                    break;
                case GraphicalUnit.Pica:
                    Pixels = Math.Round((Value * Ppi) / 6, 0);
                    break;
                case GraphicalUnit.Twip:
                    Pixels = Math.Round((Value * Ppi) / 1140, 0);
                    break;
                case GraphicalUnit.Character:
                    Pixels = Math.Round((Value * Ppi) / 12, 0);
                    break;
                case GraphicalUnit.En:
                    Pixels = Math.Round((Value * Ppi) / 144.54, 0);
                    break;
            }

            double Inches = Pixels / Ppi;

            double Result = Pixels;

            //Convert to target unit
            switch (To)
            {
                case GraphicalUnit.Inch:
                    Result = Inches;
                    break;
                case GraphicalUnit.Centimeter:
                    Result = Inches * 2.54;
                    break;
                case GraphicalUnit.Millimeter:
                    Result = Inches * 25.4;
                    break;
                case GraphicalUnit.Point:
                    Result = Inches * 72.0;
                    break;
                case GraphicalUnit.Pica:
                    Result = Inches * 6.0;
                    break;
                case GraphicalUnit.Twip:
                    Result = Inches * 1140.0;
                    break;
                case GraphicalUnit.Character:
                    Result = Inches * 12.0;
                    break;
                case GraphicalUnit.En:
                    Result = Inches * 144.54;
                    break;
            }

            return Result;
        }
    }
}
