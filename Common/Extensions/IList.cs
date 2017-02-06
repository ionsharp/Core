using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static bool Any(this IList Source) 
        {
            if (Source != null)
            {
                foreach (var i in Source)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Action"></param>
        public static void ForEach(this IList Source, Action<object> Action)
        {
            foreach (var i in Source)
                Action(i);
        }
    }
}
