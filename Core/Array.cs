using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core;

public static class Array<T>
{
    public static T[] New(IEnumerable<T> input) => input.ToArray();

    public static T[] New(params T[] input) => input;
}