using System;

namespace Imagin.Common.Linq
{
    public static class XIntPtr
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        public static bool Dispose(this IntPtr value) => DeleteObject(value);
    }
}