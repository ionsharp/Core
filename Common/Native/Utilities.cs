using System;

namespace Imagin.Common.Native
{
    public static class NativeUtilities
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}
