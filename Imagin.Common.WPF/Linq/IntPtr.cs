using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class IntPtrExtensions
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Dispose(this IntPtr value)
        {
            return DeleteObject(value);
        }
    }
}
