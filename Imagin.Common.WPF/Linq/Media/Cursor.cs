using Microsoft.Win32.SafeHandles;

namespace Imagin.Common.Linq
{
    public static class XCursor
    {
        public static System.Windows.Input.Cursor Convert(this System.Windows.Forms.Cursor Cursor)
        {
            SafeFileHandle h = new(Cursor.Handle, false);
            return System.Windows.Interop.CursorInteropHelper.Create(h);
        }
    }
}