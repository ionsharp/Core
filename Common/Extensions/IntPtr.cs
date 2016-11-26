using Imagin.Common.Native;
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
            return NativeUtilities.DeleteObject(Ptr);
        }
    }
}
