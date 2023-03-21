namespace Imagin.Core.Linq;

public static partial class XRange
{
    /// <summary>Converts the given value to a new range based on the current range.</summary>
    public static double Convert(this IRange<double> input, in IRange<double> range, in double value) => input.Convert(range.Minimum, range.Maximum, value);

    /// <summary>Converts the given value to a new range based on the current range.</summary>
    public static double Convert(this IRange<double> input, in double min, in double max, in double value)
    {
        var n = (max - min) / (input.Maximum - input.Minimum);
        return min + ((value - input.Minimum) * n);
    }

    /// <summary>Converts the given value to a new range based on the current range.</summary>
    public static float Convert(this IRange<float> input, in float min, in float max, in float value)
    {
        var n = (max - min) / (input.Maximum - input.Minimum);
        return min + ((value - input.Minimum) * n);
    }
}