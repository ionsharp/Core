using System;
using System.Text;

namespace Imagin.Core.Linq;

public static class XInt32
{
    public static int Absolute(this int input) => Math.Abs(input);

    public static int Clamp(this int input, int maximum, int minimum = 0) => Math.Max(Math.Min(input, maximum), minimum);

    //...

    public static string Letter(this int index)
    {
        const int columns = 26;
        //ceil(log26(Int32.Max))
        const int digitMaximum = 7;

        const string digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        if (index <= 0)
            throw new IndexOutOfRangeException("index must be a positive number");

        if (index <= columns)
            return digits[index - 1].ToString();

        var result = new StringBuilder().Append(' ', digitMaximum);

        var current = index;
        var offset = digitMaximum;
        while (current > 0)
        {
            result[--offset] = digits[--current % columns];
            current /= columns;
        }

        return result.ToString(offset, digitMaximum - offset);
    }

    public static string Ordinal(this int input)
    {
        var result = string.Empty;
        switch (input)
        {
            case 1:
                result = "st";
                break;
            case 2:
                result = "nd";
                break;
            case 3:
                result = "rd";
                break;
            default:
                result = "th";
                break;
        }
        return $"{input}{result}";
    }

    public static string Roman(this int index)
    {
        if ((index < 0) || (index > 3999))
            throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");

        if (index < 1)
            return string.Empty;

        if (index >= 1000)
            return "M" + Roman(index - 1000);

        if (index >= 900)
            return "CM" + Roman(index - 900);

        if (index >= 500)
            return "D" + Roman(index - 500);

        if (index >= 400)
            return "CD" + Roman(index - 400);

        if (index >= 100)
            return "C" + Roman(index - 100);

        if (index >= 90)
            return "XC" + Roman(index - 90);

        if (index >= 50)
            return "L" + Roman(index - 50);

        if (index >= 40)
            return "XL" + Roman(index - 40);

        if (index >= 10)
            return "X" + Roman(index - 10);

        if (index >= 9)
            return "IX" + Roman(index - 9);

        if (index >= 5)
            return "V" + Roman(index - 5);

        if (index >= 4)
            return "IV" + Roman(index - 4);

        if (index >= 1)
            return "I" + Roman(index - 1);

        throw new ArgumentOutOfRangeException();
    }

    //Time

    /// <summary>
    /// Gets a <see cref="TimeSpan"/> representing the given number of days.
    /// </summary>
    public static TimeSpan Days(this int input) => TimeSpan.FromDays(input);

    /// <summary>
    /// Gets a <see cref="TimeSpan"/> representing the given number of hours.
    /// </summary>
    public static TimeSpan Hours(this int input) => TimeSpan.FromHours(input);

    /// <summary>
    /// Gets a <see cref="TimeSpan"/> representing the given number of milliseconds.
    /// </summary>
    public static TimeSpan Milliseconds(this int input) => TimeSpan.FromMilliseconds(input);

    /// <summary>
    /// Gets a <see cref="TimeSpan"/> representing the given number of minutes.
    /// </summary>
    public static TimeSpan Minutes(this int input) => TimeSpan.FromMinutes(input);

    /// <summary>
    /// Gets a <see cref="TimeSpan"/> representing the given number of months.
    /// </summary>
    public static TimeSpan Months(this int input) => TimeSpan.FromDays(30 * input);

    /// <summary>
    /// Gets a <see cref="TimeSpan"/> representing the given number of seconds.
    /// </summary>
    public static TimeSpan Seconds(this int input) => TimeSpan.FromSeconds(input);

    /// <summary>
    /// Gets a <see cref="TimeSpan"/> representing the given number of years.
    /// </summary>
    public static TimeSpan Years(this int input) => TimeSpan.FromDays(365 * input);

    //Conversion

    public static byte Byte(this int input) => Convert.ToByte(input);

    public static decimal Decimal(this int input) => Convert.ToDecimal(input);

    public static double Double(this int input) => Convert.ToDouble(input);

    public static short Int16(this int input) => Convert.ToInt16(input);

    public static long Int64(this int input) => Convert.ToInt64(input);

    public static float Single(this int input) => Convert.ToSingle(input);

    public static ushort UInt16(this int input) => Convert.ToUInt16(input);

    public static uint UInt32(this int input) => Convert.ToUInt32(input);

    public static ulong UInt64(this int input) => Convert.ToUInt64(input);
}