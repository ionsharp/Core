using Imagin.Core.Analytics;
using System;

namespace Imagin.Core.Linq;

public static class XTimeZone
{
    /// <summary>
    /// Gets the current time based on the given time zone.
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    public static DateTime Now(this Time.TimeZone input)
    {
        var result = DateTime.UtcNow;
        result = TimeZoneInfo.ConvertTimeFromUtc(result, TimeZoneInfo.FindSystemTimeZoneById($"{input}".SplitCamel()));
        return result;
    }

    /// <summary>
    /// Tries to get the current time based on the given time zone.
    /// </summary>
    public static Result TryNow(this Time.TimeZone input, out DateTime result)
    {
        DateTime internalResult = default;
        var finalResult = Try.Invoke(() => internalResult = input.Now(), i => Log.Write<Time.TimeZone>(i));
        result = internalResult;
        return finalResult;
    }
}