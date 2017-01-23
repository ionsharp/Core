using Imagin.Common.Native;
using System;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IntPtrExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ptr"></param>
        /// <returns></returns>
        public static bool DeleteObject(this IntPtr Ptr)
        {
            return Utilities.DeleteObject(Ptr);
        }
    }
}
