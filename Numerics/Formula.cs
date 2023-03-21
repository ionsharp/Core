using Imagin.Apps.Desktop;
using Imagin.Core.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Numerics;

public static class Formula
{
    #region Private

    static T Calculate<T>(this IEnumerable<T> i, Formulas formula)
    => formula switch
    {
        Formulas.Largest
            => i.Largest(),
        Formulas.Mean
            => i.Mean(),
        Formulas.Median
            => i.Median(),
        Formulas.Mode
            => i.Mode(),
        Formulas.Smallest
            => i.Smallest(),
        Formulas.StandardDeviation
            => i.StandardDeviation(),
        Formulas.Variance
            => i.Variance(),
        _ => default
    };

    ///

    static T Largest<T>(this IEnumerable<T> input)
    {
        var type = NumberType.Get<T>();

        T result = type.Minimum;
        foreach (var i in input)
        {
            if (type.GreaterThan(i, result))
                result = i;
        }
        return result;
    }

    static T Mean<T>(this IEnumerable<T> input)
    {
        var type = NumberType.Get<T>();

        uint count = 0;

        T result = default;
        foreach (var i in input)
        {
            result = type.Add(result, i);
            count++;
        }

        return type.DivideByUInt32(result, count);
    }

    static T Median<T>(this IEnumerable<T> input)
    {
        var type = NumberType.Get<T>();

        var sorted = input.OrderBy(i => i).ToArray();

        int size = sorted.Length, mid = size / 2;
        return (size % 2 != 0) ? sorted[mid] : type.FromDouble(type.ToDouble(type.Add(sorted[mid], sorted[mid - 1])) / 2.0);
    }

    static T Mode<T>(this IEnumerable<T> input)
    {
        if (input.GroupBy(i => i).OrderByDescending(i => i.Count()).ThenBy(i => i.Key).FirstOrDefault() is IGrouping<T, T> grouping)
            return grouping.Key;

        return input.First();
    }

    static T Smallest<T>(this IEnumerable<T> input)
    {
        var type = NumberType.Get<T>();

        T result = type.Maximum;
        foreach (var i in input)
        {
            if (type.LessThan(i, result))
                result = i;
        }
        return result;
    }

    static T Variance<T>(this IEnumerable<T> input)
    {
        var type = NumberType.Get<T>();

        T mean = input.Mean(), variance = default;

        var count = input.Count();
        for (int i = 0; i < count; i++)
            variance = type.Add(variance, type.FromDouble(Math.Pow(type.ToDouble(type.Subtract(input.ElementAt(i), mean)), 2)));

        return type.DivideByUInt32(variance, (uint)count);
    }

    static T StandardDeviation<T>(this IEnumerable<T> input)
    {
        var type = NumberType.Get<T>();

        if (input.Count() == 0)
            return default;

        return type.FromDouble(Math.Sqrt(type.ToDouble(input.Variance())));
    }

    #endregion

    #region Public

    public static Byte Calculate(this IEnumerable<Byte> input, Formulas formula) 
        => input.Calculate<Byte>(formula);

    public static Decimal Calculate(this IEnumerable<Decimal> input, Formulas formula) 
        => input.Calculate<Decimal>(formula);

    public static Double Calculate(this IEnumerable<Double> input, Formulas formula) 
        => input.Calculate<Double>(formula);

    public static Int16 Calculate(this IEnumerable<Int16> input, Formulas formula)        
        => input.Calculate<Int16>(formula);

    public static Int32 Calculate(this IEnumerable<Int32> input, Formulas formula) 
        => input.Calculate<Int32>(formula);

    public static Int64 Calculate(this IEnumerable<Int64> input, Formulas formula) 
        => input.Calculate<Int64>(formula);

    public static Single Calculate(this IEnumerable<Single> input, Formulas formula) 
        => input.Calculate<Single>(formula);

    public static UDouble Calculate(this IEnumerable<UDouble> input, Formulas formula) 
        => input.Calculate<UDouble>(formula);

    public static UInt16 Calculate(this IEnumerable<UInt16> input, Formulas formula) 
        => input.Calculate<UInt16>(formula);

    public static UInt32 Calculate(this IEnumerable<UInt32> input, Formulas formula) 
        => input.Calculate<UInt32>(formula);

    public static UInt64 Calculate(this IEnumerable<UInt64> input, Formulas formula)
        => input.Calculate<UInt64>(formula);

    public static USingle Calculate(this IEnumerable<USingle> input, Formulas formula)
        => input.Calculate<USingle>(formula);

    #endregion
}