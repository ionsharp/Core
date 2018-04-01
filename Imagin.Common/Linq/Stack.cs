using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Any<TObject>(this Stack<TObject> Value)
        {
            return Value.Count > 0;
        }
    }
}
