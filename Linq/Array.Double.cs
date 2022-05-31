namespace Imagin.Core.Linq;

public static partial class XArray
{
    /// <summary>Gets the largest value.</summary>
    public static double Largest(this double[] input)
    {
        var result = double.MinValue;
        foreach (var i in input)
        {
            if (i > result)
                result = i;
        }
        return result;
    }

    /// <summary>Gets the smallest value.</summary>
    public static double Smallest(this double[] input)
    {
        var result = double.MaxValue;
        foreach (var i in input)
        {
            if (i < result)
                result = i;
        }
        return result;
    }
}