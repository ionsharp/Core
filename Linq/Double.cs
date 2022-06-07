using System;

namespace Imagin.Core.Linq;

public static class XDouble
{
    public static double Abs(this double value)
        => System.Math.Abs(value);

    public static double NaN(this double input, double j) => double.IsNaN(input) ? j : input;

    public static double Round(this double value, int digits = 0)
        => System.Math.Round(value, digits);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of days.</summary>
    public static TimeSpan Days(this double input) => TimeSpan.FromDays(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of hours.</summary>
    public static TimeSpan Hours(this double input) => TimeSpan.FromHours(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of milliseconds.</summary>
    public static TimeSpan Milliseconds(this double input) => TimeSpan.FromMilliseconds(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of minutes.</summary>
    public static TimeSpan Minutes(this double input) => TimeSpan.FromMinutes(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of months.</summary>
    public static TimeSpan Months(this double input) => TimeSpan.FromDays(30 * input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of seconds.</summary>
    public static TimeSpan Seconds(this double input) => TimeSpan.FromSeconds(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of years.</summary>
    public static TimeSpan Years(this double input) => TimeSpan.FromDays(365 * input);
}