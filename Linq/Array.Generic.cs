using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Linq;

public static partial class XArray
{
    public static T[] Add<T>(this T[] input, params T[] elements)
    {
        var length = input.Length;
        System.Array.Resize(ref input, length + elements.Length);

        for (int j = 0, count = elements.Length; j < count; j++)
            input[length + j] = elements[j];

        return input;
    }

    ///

    public static T[,] Duplicate<T>(this T[,] input)
    {
        int rows = input.GetLength(0), columns = input.GetLength(1);

        var result = new T[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
                result[row, column] = input[row, column];
        }
        return result;
    }

    public static T[][] Duplicate<T>(this T[][] input)
    {
        int rows = input.Length, columns = input[0].Length;

        var result = new T[rows][];
        for (int row = 0; row < rows; row++)
        {
            result[row] = new T[columns];
            for (int column = 0; column < columns; column++)
                result[row][column] = input[row][column];
        }
        return result;
    }

    ///

    public static T[] Empty<T>() => new T[0];

    ///

    public static int IndexOf<T>(this T[] input, T element)
    {
        for (int i = 0, length = input.Length; i < length; i++)
        {
            if (input[i].Equals(element))
                return i;
        }
        return -1;
    }

    ///

    public static T[] New<T>(params T[] input) => input ?? new T[0];

    public static T[] New<T>(IEnumerable<T> input)
    {
        var count = input?.Count() ?? 0;
        var result = new T[count];

        if (count == 0)
            return result;

        for (var i = 0; i < count; i++)
            result[i] = input.ElementAtOrDefault(i);

        return result;
    }

    ///

    public static T[][] Project<T>(this T[,] input)
    {
        int rows = input.GetLength(0), columns = input.GetLength(1);

        var result = new T[rows][];
        for (int row = 0; row < rows; row++)
        {
            result[row] = new T[columns];
            for (int column = 0; column < columns; column++)
                result[row][column] = input[row, column];
        }

        return result;
    }

    public static T[,] Project<T>(this T[][] input)
    {
        int rows = input.Length, columns = input[0].Length;

        var result = new T[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
                result[row, column] = input[row][column];
        }

        return result;
    }

    ///

    public static void Remove<T>(this T[] input, params T[] elements)
    {
        foreach (var i in elements)
            input.RemoveAt(input.IndexOf(i));
    }

    public static void RemoveAt<T>(this T[] input, int index)
    {
        var result = new T[input.Length - 1];

        if (index > 0)
            System.Array.Copy(input, 0, result, 0, index);

        if (index < input.Length - 1)
            System.Array.Copy(input, index + 1, result, index, input.Length - index - 1);

        input = result;
    }
}