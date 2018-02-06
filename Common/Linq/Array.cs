using System;
using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Element"></param>
        /// <returns></returns>
        public static int IndexOf<TElement>(this TElement[] Source, TElement Element)
        {
            for (var i = 0; i < Source.Length; i++)
            {
                if (Source[i].Equals(Element))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Predicate"></param>
        /// <returns></returns>
        public static IEnumerable<object> Where(this System.Array Value, Func<object, bool> Predicate)
        {
            foreach (var i in Value)
            {
                if (Predicate(i))
                    yield return i;
            }
            yield break;
        }
    }
}
