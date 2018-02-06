using System;

namespace Imagin.Common.Native
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utilities
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}
