using Imagin.Common.Linq;
using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class List
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<T> New<T>(params T[] items)
        {
            var result = new List<T>();
            items.ForEach(i => result.Add(i));
            return result;
        }
    }
}
