using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static TimeSpan Coerce(this TimeSpan Value, TimeSpan Maximum, TimeSpan Minimum = default(TimeSpan))
        {
            var minimum = Minimum == default(TimeSpan) ? TimeSpan.Zero : Minimum;
            return Value > Maximum ? Maximum : (Value < minimum ? minimum : Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int Months(this TimeSpan Value)
        {
            return Value.Days.ToDouble().Divide(30d).Floor().ToInt32();
            /*
            Days = Days - (30 * Months);
            Days = Days < 0 ? 0 : Days;
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int Years(this TimeSpan Value)
        {
            var Months = Value.Months();
            return Months >= 12 ? Months.ToDouble().Divide(12d).Floor().ToInt32() : 0;
            /*
            Months = Months - (12 * Years);
            Months = Months < 0 ? 0 : Months;
            */
        }
    }
}
