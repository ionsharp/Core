using System;
using System.Globalization;
using Imagin.Common.Globalization;

namespace Imagin.Common.Linq
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Maximum"></param>
        /// <param name="Minimum"></param>
        /// <returns></returns>
        public static DateTime Coerce(this DateTime Value, DateTime Maximum, DateTime Minimum = default(DateTime))
        {
            return Value > Maximum ? Maximum : (Value < Minimum ? Minimum : Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static int CurrentMonth
        {
            get
            {
                return DateTime.Today.Month;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int CurrentDay
        {
            get
            {
                return DateTime.Today.Day;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int CurrentYear
        {
            get
            {
                return DateTime.Today.Year;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Localizer"></param>
        /// <returns></returns>
        public static string GetRelative(this DateTime Value, ILocalizer Localizer)
        {
            const int Second = 1;
            const int Minute = 60 * Second;
            const int Hour = 60 * Minute;
            const int Day = 24 * Hour;
            const int Month = 30 * Day;

            var span = default(TimeSpan);
            var delta = 0d;

            var Now = DateTime.Now;

            var Label = new Func<string, string>(i => Localizer.GetValue(i));

            var Suffix = string.Empty;

            //It's in the future
            if (Value > Now)
            {
                span = new TimeSpan(Value.Ticks - DateTime.Now.Ticks);
                delta = Math.Abs(span.TotalSeconds);

                Suffix = Label("FromNow").ToLower();
            }
            //It's in the past
            else if (Value < Now)
            {
                span = new TimeSpan(DateTime.Now.Ticks - Value.Ticks);
                delta = Math.Abs(span.TotalSeconds);

                Suffix = Label("Ago").ToLower();
            }
            //It's now
            else return Label("Now");

            if (delta < 1 * Minute)
            {
                switch (span.Seconds)
                {
                    case 0:
                        return Label("Now");
                    case 1:
                        return "{0} {1}".F(Label("ASecond"), Suffix);
                    default:
                        return "{0} {1} {2}".F(span.Seconds, Label("Seconds").ToLower(), Suffix);
                }
            }

            if (delta < 2 * Minute)
                return "{0} {1}".F(Label("AMinute"), Suffix);

            if (delta < 45 * Minute)
                return "{0} {1} {2}".F(span.Minutes, Label("Minutes").ToLower(), Suffix);

            if (delta < 90 * Minute)
                return "{0} {1}".F(Label("AnHour"), Suffix);

            if (delta < 24 * Hour)
                return "{0} {1} {2}".F(span.Hours, Label("Hours").ToLower(), Suffix);

            if (delta < 48 * Hour)
            {
                if (Value < Now)
                {
                    return Label("Yesterday");
                }
                else if (Value > Now)
                    return Label("Tomorrow");
            }

            if (delta < 30 * Day)
                return "{0} {1} {2}".F(span.Days, Label("Days").ToLower(), Suffix);

            if (delta < 12 * Month)
            {
                var months = Convert.ToInt32(Math.Floor((double)span.Days / 30));

                if (months <= 1)
                {
                    return "{0} {1}".F(Label("AMonth"), Suffix);
                }
                else return "{0} {1} {2}".F(months, Label("Months").ToLower(), Suffix);
            }
            else
            {
                var years = Convert.ToInt32(Math.Floor((double)span.Days / 365));

                if (years <= 1)
                {
                    return "{0} {1}".F(Label("AYear"), Suffix);
                }
                else return "{0} {1} {2}".F(years, Label("Years").ToLower(), Suffix);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Localizer"></param>
        /// <returns></returns>
        public static string GetRelative(this DateTime? Value, ILocalizer Localizer)
        {
            if (Value == null)
                return Localizer.GetValue("Never");

            return GetRelative(Value.Value, Localizer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetRelativeDifference(this DateTime Value)
        {
            var Result = string.Empty;

            var Difference = Value - DateTime.Now;
            if (Difference.TotalSeconds != 0)
            {
                Result = "{0}d {1}h {2}m {3}s".F
                (
                    Difference.Days.ToString("N0"),
                    Difference.Hours,
                    Difference.Minutes,
                    Difference.Seconds
                );
            }
            else Result = "0";

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetRelativeDifference(this DateTime? Value)
        {
            if (Value == null)
                return string.Empty;

            return GetRelativeDifference(Value.Value);
        }

        /// <summary>
        /// Checks if month, day, and year are identical to that of today (ignores time).
        /// </summary>
        public static bool IsToday(this DateTime Value)
        {
            return Value.Date == DateTime.Now.Date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Meridiem Meridiem(this DateTime Value)
        {
            switch (Value.ToString("tt", CultureInfo.InvariantCulture).ToLowerInvariant())
            {
                case "am":
                    return Common.Meridiem.Ante;
                case "pm":
                    return Common.Meridiem.Post;
            }
            return Common.Meridiem.Unspecified;
        }

        /// <summary>
        /// Gets whether or not both <see cref="Nullable{DateTime}"/> values have the same date (time is ignored).
        /// </summary>
        public static bool SameDate(this DateTime? First, DateTime? Second)
        {
            if (First != null && Second != null)
                return First.Value.Date == Second.Value.Date;

            return (First.Value.Date == Second.Value.Date);
        }

        /// <summary>
        /// Gets whether or not both <see cref="DateTime"/> values have the same date (time is ignored).
        /// </summary>
        public static bool SameDate(this DateTime First, DateTime Second)
        {
            return First.Date == Second.Date;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DateTime Tomorrow
        {
            get
            {
                return DateTime.Today.AddDays(1.0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static DateTime TrimMilliseconds(this DateTime Value)
        {
            return new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second, 0);
        }

        public static DateTime Yesterday
        {
            get
            {
                return DateTime.Today.AddDays(-1.0);
            }
        }
    }
}
