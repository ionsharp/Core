using Imagin.Core.Numerics;

namespace Imagin.Core.Linq;

public static class XMatrix
{
    /// <summary>Converts all values into a range of [0, 1] based on the smallest and largest occurring value (the current range).</summary>
    public static Matrix Normalize(this IMatrix input) => input.Normalize(out Range<double> _);

    /// <summary>Converts all values into a range of [0, 1] based on the smallest and largest occurring value (the current range).</summary>
    public static Matrix Normalize(this IMatrix input, out Range<double> range)
    {
        var result = new Matrix(input.Values);

        var smallest = result.Smallest(); var largest = result.Largest();
        if (smallest == largest)
            smallest = largest - 1;

        var r = new DoubleRange(smallest, largest);

        int rows = result.Rows, columns = result.Columns;
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < columns; x++)
                input.Values[y][x] = r.Convert(0, 1, input.Values[y][x]);
        }

        range = new(smallest, largest);
        return result;
    }
}