using System;
using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<object> Where(this Array value, Predicate<object> predicate)
        {
            foreach (var i in value)
            {
                if (predicate(i))
                    yield return i;
            }
            yield break;
        }
    }
}
