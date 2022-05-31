namespace Imagin.Core.Linq;

public static partial class XArray
{
    /// <summary>Gets the largest value.</summary>
    public static long Largest(this long[] input)
    {
        var result = long.MinValue;
        foreach (var i in input)
        {
            if (i > result)
                result = i;
        }
        return result;
    }

    /// <summary>Gets the smallest value.</summary>
    public static long Smallest(this long[] input)
    {
        var result = long.MaxValue;
        foreach (var i in input)
        {
            if (i < result)
                result = i;
        }
        return result;
    }
}