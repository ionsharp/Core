using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    public static class XByte
    {
        public static ImageSource ImageSource(this byte[] input, int width, int height, System.Drawing.Imaging.PixelFormat Format)
        {
            var bitmap = new Bitmap(width, height, Format);

            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, Format);
            Marshal.Copy(input, 0, bitmapData.Scan0, input.Length);

            bitmap.UnlockBits(bitmapData);
            return bitmap.BitmapSource() as ImageSource;
        }
    }
}