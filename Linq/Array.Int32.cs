namespace Imagin.Core.Linq;

public static partial class XArray
{
    /// <summary>Gets the largest value.</summary>
    public static int Largest(this int[] input)
    {
        var result = int.MinValue;
        foreach (var i in input)
        {
            if (i > result)
                result = i;
        }
        return result;
    }

    /// <summary>Gets the smallest value.</summary>
    public static int Smallest(this int[] input)
    {
        var result = int.MaxValue;
        foreach (var i in input)
        {
            if (i < result)
                result = i;
        }
        return result;
    }
}