using System;
using System.Collections;

namespace Imagin.Common.Linq
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static object Last(this IEnumerable Source)
        {
            var Result = default(object);
            Source.ForEach(i => Result = i);
            return Result;
        }
    }
}
