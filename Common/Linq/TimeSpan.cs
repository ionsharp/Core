using System;

namespace Imagin.Common.Linq
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan Coerce(this TimeSpan Value, TimeSpan Maximum, TimeSpan Minimum = default(TimeSpan))
        {
            var minimum = Minimum == default(TimeSpan) ? TimeSpan.Zero : Minimum;
            return Value > Maximum ? Maximum : (Value < minimum ? minimum : Value);
        }

        public static int Months(this TimeSpan Value)
        {
            return Value.Days.ToDouble().Divide(30d).Floor().ToInt32();
            /*
            Days = Days - (30 * Months);
            Days = Days < 0 ? 0 : Days;
            */
        }

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
