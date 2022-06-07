using System;
using System.Threading.Tasks;
using static System.Math;

namespace Imagin.Core.Linq;

public static class XTimeSpan
{
    public static TimeSpan Clamp(this TimeSpan input, TimeSpan maximum, TimeSpan minimum = default)
    {
        var result = minimum == default ? TimeSpan.Zero : minimum;
        return input > maximum ? maximum : (input < result ? result : input);
    }

    public static TimeSpan Left(this TimeSpan input, long current, long total)
    {
        var lines = (current.Double() / total.Double()) * 100;
        lines = lines == 0.0 ? 1.0 : lines;
        var seconds = (input.TotalSeconds / lines) * (100.0 - lines);
        return TimeSpan.FromSeconds(double.IsNaN(seconds) ? 0 : seconds);
    }

    public static int Months(this TimeSpan input) => Floor(input.Days.Double() / 30).Int32();

    public static string ShortTime(this TimeSpan input, bool daysOnly)
    {
        if (input.TotalSeconds == 0)
            return string.Empty;

        string result = string.Empty;
        if (input.Days > 0)
            result += string.Format("{0}d", input.Days.ToString());

        if (!daysOnly || input.Days == 0)
        {
            if (input.Hours > 0)
                result += string.Format(" {0}h", input.Hours.ToString());

            if (input.Minutes > 0)
                result += string.Format(" {0}m", input.Minutes.ToString());

            if (input.Seconds > 0)
                result += string.Format(" {0}s", input.Seconds.ToString());
        }

        return result;
    }

    async public static Task TrySleep(this TimeSpan input) => await Task.Run(() => Try.Invoke(() => System.Threading.Thread.Sleep(input.TotalMilliseconds.Int32()), e => Analytics.Log.Write<TimeSpan>(e)));

    public static int Years(this TimeSpan input)
    {
        var result = input.Months();
        return result >= 12 ? Floor(result.Double() / 12).Int32() : 0;
    }
}