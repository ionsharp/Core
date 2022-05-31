using Imagin.Core.Numerics;
using System;

namespace Imagin.Core.Linq;

public static class XVector
{
    /// <summary>Gets an absolute <see cref="Vector"/>.</summary>
    public static Vector Absolute(this IVector input) => input.Transform((i, j) => j.Absolute());

    /// <summary>Coerces the range of the <see cref="Vector"/> based on the specified range.</summary>
    public static Vector Clamp(this IVector input, double minimum, double maximum) => input.Transform((i, j) => j.Clamp(maximum, minimum));

    /// <summary>Coerces the range of the <see cref="Vector"/> based on the specified range.</summary>
    public static Vector Clamp(this IVector input, Vector minimum, Vector maximum)
    {
        if (minimum.Length != input.Length)
            throw new ArgumentOutOfRangeException(nameof(minimum));

        if (maximum.Length != input.Length)
            throw new ArgumentOutOfRangeException(nameof(maximum));

        return input.Transform((index, value) => value.Clamp(maximum[index], minimum[index]));
    }

    /// <summary>Performs dot multiplication on both given vectors.</summary>
    public static double Dot(this IVector left, IVector right)
    {
        double result = 0;
        left.Each((i, j) => result += j * right.Values[i]);
        return result;
    }

    /// <summary>Calls the given action for each value in the vector.</summary>
    public static void Each(this IVector input, Action<int, double> action)
    {
        for (int i = 0, count = input.Values.Length; i < count; i++)
            action(i, input.Values[i]);
    }

    /// <summary>Gets the largest value in the vector.</summary>
    public static double Largest(this IVector input) => input.Values.Largest();

    /// <summary>Gets a rounded copy.</summary>
    public static Vector Round(this IVector input) => input.Transform((i, j) => j.Round());

    /// <summary>Gets the smallest value in the vector.</summary>
    public static double Smallest(this IVector input) => input.Values.Smallest();

    /// <summary>Gets the sum of all values in the vector.</summary>
    public static double Sum(this IVector input)
    {
        double result = 0;
        input.Values.ForEach(i => result += i);
        return result;
    }
}