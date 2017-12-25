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
        /// <typeparam name="TObject"></typeparam>
        /// <param name="Items"></param>
        /// <returns></returns>
        public static List<TObject> New<TObject>(params TObject[] Items)
        {
            var Result = new List<TObject>();
            Items.ForEach(i => Result.Add(i));
            return Result;
        }
    }
}
