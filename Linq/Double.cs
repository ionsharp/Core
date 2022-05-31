using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Linq;

public static class XDouble
{
    public static double Absolute(this double value)
        => System.Math.Abs(value);

    public static double Ceiling(this double value)
        => System.Math.Ceiling(value);

    public static double Clamp(this double i, double maximum, double minimum = 0)
        => System.Math.Max(System.Math.Min(i, maximum), minimum);

    /// <summary>Converts the given value from <see cref="Unit"/> (a) to <see cref="Unit"/> (b).</summary>
    public static double Convert(this double value, Unit a, Unit b, double ppi = 72.0)
    {
        var pixels = 0d;
        switch (a)
        {
            case Unit.Pixel:
                pixels = System.Math.Round(value, 0);
                break;
            case Unit.Inch:
                pixels = System.Math.Round(value * ppi, 0);
                break;
            case Unit.Centimeter:
                pixels = System.Math.Round((value * ppi) / 2.54, 0);
                break;
            case Unit.Millimeter:
                pixels = System.Math.Round((value * ppi) / 25.4, 0);
                break;
            case Unit.Point:
                pixels = System.Math.Round((value * ppi) / 72, 0);
                break;
            case Unit.Pica:
                pixels = System.Math.Round((value * ppi) / 6, 0);
                break;
            case Unit.Twip:
                pixels = System.Math.Round((value * ppi) / 1140, 0);
                break;
            case Unit.Character:
                pixels = System.Math.Round((value * ppi) / 12, 0);
                break;
            case Unit.En:
                pixels = System.Math.Round((value * ppi) / 144.54, 0);
                break;
        }

        var inches = pixels / ppi;
        var result = pixels;

        switch (b)
        {
            case Unit.Inch:
                result = inches;
                break;
            case Unit.Centimeter:
                result = inches * 2.54;
                break;
            case Unit.Millimeter:
                result = inches * 25.4;
                break;
            case Unit.Point:
                result = inches * 72.0;
                break;
            case Unit.Pica:
                result = inches * 6.0;
                break;
            case Unit.Twip:
                result = inches * 1140.0;
                break;
            case Unit.Character:
                result = inches * 12.0;
                break;
            case Unit.En:
                result = inches * 144.54;
                break;
        }

        return result;
    }

    public static double Floor(this double value)
        => System.Math.Floor(value);

    public static double ReplaceNaN(this double input, double j) => double.IsNaN(input) ? j : input;

    public static double Round(this double value, int digits = 0)
        => System.Math.Round(value, digits);

    //Time

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of days.</summary>
    public static TimeSpan Days(this double input) 
        => TimeSpan.FromDays(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of hours.</summary>
    public static TimeSpan Hours(this double input) 
        => TimeSpan.FromHours(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of milliseconds.</summary>
    public static TimeSpan Milliseconds(this double input) 
        => TimeSpan.FromMilliseconds(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of minutes.</summary>
    public static TimeSpan Minutes(this double input) 
        => TimeSpan.FromMinutes(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of months.</summary>
    public static TimeSpan Months(this double input) 
        => TimeSpan.FromDays(30 * input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of seconds.</summary>
    public static TimeSpan Seconds(this double input) 
        => TimeSpan.FromSeconds(input);

    /// <summary>Gets a <see cref="TimeSpan"/> representing the given number of years.</summary>
    public static TimeSpan Years(this double input) 
        => TimeSpan.FromDays(365 * input);

    //Conversion

    /// <summary>Converts to <see cref="byte"/>.</summary>
    public static byte Byte(this double value) 
        => System.Convert.ToByte(value);

    /// <summary>Converts to <see cref="decimal"/>.</summary>
    public static decimal Decimal(this double value)
        => System.Convert.ToDecimal(value);

    /// <summary>Converts to <see cref="short"/>.</summary>
    public static short Int16(this double value)
        => System.Convert.ToInt16(value);

    /// <summary>Converts to <see cref="int"/>.</summary>
    public static int Int32(this double value)
        => System.Convert.ToInt32(value);

    /// <summary>Converts to <see cref="long"/>.</summary>
    public static long Int64(this double value)
        => System.Convert.ToInt64(value);

    /// <summary>Converts to <see cref="float"/>.</summary>
    public static float Single(this double value)
        => System.Convert.ToSingle(value);

    /// <summary>Converts to <see cref="ushort"/>.</summary>
    public static ushort UInt16(this double value)
        => System.Convert.ToUInt16(value);

    /// <summary>Converts to <see cref="uint"/>.</summary>
    public static uint UInt32(this double value) 
        => System.Convert.ToUInt32(value);

    /// <summary>Converts to <see cref="ulong"/>.</summary>
    public static ulong UInt64(this double value)
        => System.Convert.ToUInt64(value);
}