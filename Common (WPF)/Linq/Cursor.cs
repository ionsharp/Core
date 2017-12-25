using Microsoft.Win32.SafeHandles;

namespace Imagin.Common.Linq
{
    public static class CursorExtensions
    {
        public static System.Windows.Input.Cursor ToInputCursor(this System.Windows.Forms.Cursor Cursor)
        {
            SafeFileHandle h = new SafeFileHandle(Cursor.Handle, false);
            return System.Windows.Interop.CursorInteropHelper.Create(h);
        }
    }
}
