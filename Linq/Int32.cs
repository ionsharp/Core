using System;

namespace Imagin.Core.Linq;

public static class XInt32
{
    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of days.</summary>
    public static TimeSpan Days(this int input) => TimeSpan.FromDays(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of hours.</summary>
    public static TimeSpan Hours(this int input) => TimeSpan.FromHours(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of milliseconds.</summary>
    public static TimeSpan Milliseconds(this int input) => TimeSpan.FromMilliseconds(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of minutes.</summary>
    public static TimeSpan Minutes(this int input) => TimeSpan.FromMinutes(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of months.</summary>
    public static TimeSpan Months(this int input) => TimeSpan.FromDays(30 * input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of seconds.</summary>
    public static TimeSpan Seconds(this int input) => TimeSpan.FromSeconds(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of years.</summary>
    public static TimeSpan Years(this int input) => TimeSpan.FromDays(365 * input);
}