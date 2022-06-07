using System;
using System.Collections.Generic;

namespace Imagin.Core.Linq;

public static partial class XArray
{
    public static T Compare<T>(this T[] input, Func<T, T, bool> action, T origin)
    {
        T result = origin;
        foreach (var i in input)
        {
            if (action(i, result))
                result = i;
        }
        return result;
    }

    //...

    /// <summary>Gets the largest value.</summary>
    public static decimal Largest(this decimal[] input) => Compare(input, (i, j) => i > j, decimal.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static double Largest(this double[] input) => Compare(input, (i, j) => i > j, double.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static short Largest(this short[] input) => Compare(input, (i, j) => i > j, short.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static int Largest(this int[] input) => Compare(input, (i, j) => i > j, int.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static long Largest(this long[] input) => Compare(input, (i, j) => i > j, long.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static ushort Largest(this ushort[] input) => Compare(input, (i, j) => i > j, ushort.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static uint Largest(this uint[] input) => Compare(input, (i, j) => i > j, uint.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static ulong Largest(this ulong[] input) => Compare(input, (i, j) => i > j, ulong.MinValue);

    /// <summary>Gets the largest value.</summary>
    public static float Largest(this float[] input) => Compare(input, (i, j) => i > j, float.MinValue);

    //...

    /// <summary>Gets the largest value.</summary>
    public static decimal Smallest(this decimal[] input) => Compare(input, (i, j) => i < j, decimal.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static double Smallest(this double[] input) => Compare(input, (i, j) => i < j, double.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static short Smallest(this short[] input) => Compare(input, (i, j) => i < j, short.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static int Smallest(this int[] input) => Compare(input, (i, j) => i < j, int.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static long Smallest(this long[] input) => Compare(input, (i, j) => i < j, long.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static ushort Smallest(this ushort[] input) => Compare(input, (i, j) => i < j, ushort.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static uint Smallest(this uint[] input) => Compare(input, (i, j) => i < j, uint.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static ulong Smallest(this ulong[] input) => Compare(input, (i, j) => i < j, ulong.MaxValue);

    /// <summary>Gets the largest value.</summary>
    public static float Smallest(this float[] input) => Compare(input, (i, j) => i < j, float.MaxValue);

    //...

    public static string SplitWith(this Array input, string separator)
    {
        var result = string.Empty;

        var k = 0;
        foreach (var j in input)
        {
            result += $"{j}";

            if (k < input.Length - 1)
                result += $"{separator}";

            k++;
        }

        return result;
    }

    public static IEnumerable<object> Where(this Array value, Predicate<object> predicate)
    {
        foreach (var i in value)
        {
            if (predicate(i))
                yield return i;
        }
        yield break;
    }
}