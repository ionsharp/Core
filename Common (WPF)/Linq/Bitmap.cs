using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class BitmapExtensions
    {
        [DllImport("user32.dll")]
        static extern IntPtr CreateIconIndirect([In] ref ICONINFO piconinfo);

        [DllImport("user32.dll")]
        static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            /// <summary>
            /// Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies an icon; FALSE specifies a cursor. 
            /// </summary>
            public bool fIcon;

            /// <summary>
            /// Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot spot is always in the center of the icon, and this member is ignored.
            /// </summary>
            public Int32 xHotspot;

            /// <summary>
            /// Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot spot is always in the center of the icon, and this member is ignored. 
            /// </summary>
            public Int32 yHotspot;

            /// <summary>
            /// (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, this bitmask is formatted so that the upper half is the icon AND bitmask and the lower half is the icon XOR bitmask. Under this condition, the height should be an even multiple of two. If this structure defines a color icon, this mask only defines the AND bitmask of the icon. 
            /// </summary>
            public IntPtr hbmMask;

            /// <summary>
            /// (HBITMAP) Handle to the icon color bitmap. This member can be optional if this structure defines a black and white icon. The AND bitmask of hbmMask is applied with the SRCAND flag to the destination; subsequently, the color bitmap is applied (using XOR) to the destination by using the SRCINVERT flag. 
            /// </summary>
            public IntPtr hbmColor;
        }

        public static BitmapSource ToBitmapSource(this Bitmap Bitmap)
        {
            if (Bitmap == null) return null;
            IntPtr Pointer = Bitmap.GetHbitmap();
            BitmapSource Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Pointer, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            Pointer.DeleteObject();
            return Source;
        }

        public static System.Windows.Forms.Cursor ToFormsCursor(this Bitmap Bitmap, int HotX, int HotY)
        {
            ICONINFO Info = new ICONINFO();
            IntPtr Handle = Bitmap.GetHicon();
            GetIconInfo(Handle, out Info);

            Info.xHotspot = HotX;
            Info.yHotspot = HotY;
            Info.fIcon = false;

            IntPtr h = CreateIconIndirect(ref Info);
            return new System.Windows.Forms.Cursor(h);
        }
    }
}
