using Imagin.Core.Time;
using System;
using System.Globalization;

namespace Imagin.Core.Linq
{
    public static class XDateTime
    {
        public static DateTime Coerce(this DateTime input, DateTime maximum, DateTime minimum = default) => input > maximum ? maximum : (input < minimum ? minimum : input);

        public static Meridiem Meridiem(this DateTime input)
        {
            switch (input.ToString("tt", CultureInfo.InvariantCulture).ToLowerInvariant())
            {
                case "am":
                    return Time.Meridiem.AM;
                case "pm":
                    return Time.Meridiem.PM;
            }
            return default;
        }

        public static DateTime Milliseconds(this DateTime input, int milliseconds) => new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, milliseconds);

        public static string Relative(this DateTime input)
        {
            const int second
                = 1;
            const int minute
                = 60 * second;
            const int hour
                = 60 * minute;
            const int day
                = 24 * hour;
            const int month
                = 30 * day;

            var span = default(TimeSpan);
            var delta = 0d;

            var now = DateTime.Now;
            var suffix = string.Empty;

            //It's in the future
            if (input > now)
            {
                span = new TimeSpan(input.Ticks - DateTime.Now.Ticks);
                delta = Math.Abs(span.TotalSeconds);
                suffix = " from now";
            }
            //It's in the past
            else if (input < now)
            {
                span = new TimeSpan(DateTime.Now.Ticks - input.Ticks);
                delta = Math.Abs(span.TotalSeconds);
                suffix = " ago";
            }
            //It's now
            else return "now";

            var result = string.Empty;

            if (delta < 1 * minute)
            {
                switch (span.Seconds)
                {
                    case 0:
                    case 1:
                        result = $"a second";
                        break;
                    default:
                        result = $"{span.Seconds} seconds";
                        break;
                }
            }

            else if (delta < 2 * minute)
                result = $"a minute";

            else if (delta < 45 * minute)
                result = $"{span.Minutes} minutes";

            else if (delta < 120 * minute)
                result = $"an hour";

            else if (delta < 24 * hour)
                result = $"{span.Hours} hours";

            else if (delta < 48 * hour)
            {
                if (input < now)
                {
                    return "yesterday";
                }
                else if (input > now)
                    return "tomorrow";
            }

            else if (delta < 30 * day)
                result = $"{span.Days} days";

            else if (delta < 12 * month)
            {
                var months = Convert.ToInt32(Math.Floor((double)span.Days / 30));

                if (months <= 1)
                {
                    result = $"a month";
                }
                else result = $"{months} months";
            }
            else
            {
                var years = Convert.ToInt32(Math.Floor((double)span.Days / 365));

                if (years <= 1)
                {
                    result = $"a year";
                }
                else result = $"{years} years";
            }
            return $"{result}{suffix}";
        }

        public static string Relative(this DateTime? input)
        {
            if (input == null)
                return "never";

            return Relative(input.Value);
        }

        public static string RelativeDifference(this DateTime input, int format)
        {
            var result = string.Empty;

            var difference = input - DateTime.Now;
            var _difference = difference.Duration();

            double d = _difference.Days;

            var y1 = d / 365.0;
            var y2 = y1.Floor();

            if (y2 != 0)
                d = (y1 - y2) * 365.0;

            var h = _difference.Hours;
            var m = _difference.Minutes;
            var s = _difference.Seconds;

            switch (format)
            {
                case 0:
                    if (y2 != 0)
                        result += "{0}Y ".F(y2);

                    if (d != 0)
                        result += "{0}D ".F(d);

                    if (h != 0)
                        result += "{0}H ".F(h);

                    if (m != 0)
                        result += "{0}M ".F(m);

                    if (s != 0)
                        result += "{0}S ".F(s);
                    break;
                case 1:
                    if (y2 != 0)
                        result += $"{y2.ToString().PadLeft(2, '0')}:";

                    result += $"{d.ToString().PadLeft(3, '0')}:";
                    result += $"{h.ToString().PadLeft(2, '0')}:";
                    result += $"{m.ToString().PadLeft(2, '0')}:";
                    result += $"{s.ToString().PadLeft(2, '0')}";
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (difference.Seconds < 0)
                result = "-{0}".F(result);

            return result;
        }

        public static string RelativeDifference(this DateTime? input, int format)
        {
            if (input == null)
                return string.Empty;

            return RelativeDifference(input.Value, format);
        }

        public static bool Today(this DateTime input) => input.Date == DateTime.Now.Date;
    }
}