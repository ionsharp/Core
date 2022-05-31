using System.Collections.Generic;

namespace Imagin.Core.Linq
{
    public static class XStack
    {
        public static bool Any<T>(this Stack<T> input) => input.Count > 0;
    }
}