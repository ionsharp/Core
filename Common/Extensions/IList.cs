using System;
using System.Collections;

namespace Imagin.Common.Extensions
{
    public static class IListExtensions
    {
        public static void ForEach(this IList Source, Action<object> Action)
        {
            foreach (var i in Source)
                Action(i);
        }
    }
}
