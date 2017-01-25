using System;
using System.Collections;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Action"></param>
        public static void ForEach(this IEnumerable Source, Action<object> Action)
        {
            foreach (var i in Source)
                Action(i);
        }
    }
}
