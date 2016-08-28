using System;

namespace Imagin.Common.Extensions
{
    public static class IntPtrExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static bool DeleteObject(this IntPtr Ptr)
        {
            return NativeMethods.DeleteObject(Ptr);
        }
    }
}
