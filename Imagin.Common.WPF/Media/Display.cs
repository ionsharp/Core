using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Imagin.Common.Media
{
    public class Display
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public static Color Color()
        {
            var location = new Point();
            GetCursorPos(ref location);

            Bitmap screenPixel = new(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        /// <summary>
        /// Gets the current <see cref="System.Windows.Input.Mouse"/> position relative to <see cref="System.Windows.Forms.Screen"/>.
        /// </summary>
        public static Point Mouse
        {
            get
            {
                var result = new Point();
                GetCursorPos(ref result);
                return result;
            }
        }
    }
}