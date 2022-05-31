namespace Imagin.Core.Linq;

public static partial class XArray
{
    /// <summary>
    /// Gets the largest value.
    /// </summary>
    public static short Largest(this short[] input)
    {
        var result = short.MinValue;
        foreach (var i in input)
        {
            if (i > result)
                result = i;
        }
        return result;
    }

    /// <summary>
    /// Gets the smallest value.
    /// </summary>
    public static short Smallest(this short[] input)
    {
        var result = short.MaxValue;
        foreach (var i in input)
        {
            if (i < result)
                result = i;
        }
        return result;
    }
}