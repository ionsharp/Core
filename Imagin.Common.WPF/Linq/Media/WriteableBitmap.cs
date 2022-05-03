using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class XWriteableBitmap
    {
        public static Bitmap Bitmap<T>(this WriteableBitmap input) where T : BitmapEncoder, new()
        {
            Bitmap result = default;
            using (var stream = new MemoryStream())
            {
                T encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(input));
                encoder.Save(stream);
                result = new Bitmap(stream);
            }
            return result;
        }

        public static WriteableBitmap Clone(this WriteableBitmap input)
        {
            var result = new WriteableBitmap(input.PixelWidth, input.PixelHeight, input.DpiX, input.DpiY, input.Format, null);
            input.ForEach((x, y, color) => { result.SetPixel(x, y, color.A, color.R, color.G, color.B); return color; });
            return result;
        }

        public static Matrix<Argb> Convert(this WriteableBitmap input)
        {
            if (input != null)
            {
                var result = new Matrix<Argb>(input.PixelHeight.UInt32(), input.PixelWidth.UInt32());
                input.ForEach((x, y, color) =>
                {
                    result.SetValue(y.UInt32(), x.UInt32(), new Argb(color.A, color.R, color.G, color.B));
                    return color;
                });
                return result;
            }
            return null;
        }

        public static WriteableBitmap Resize(this WriteableBitmap bitmap, double scale)
        {
            var s = new System.Windows.Media.ScaleTransform(scale, scale);

            var result = new TransformedBitmap(bitmap, s);

            WriteableBitmap a(BitmapSource b)
            {
                // Calculate stride of source
                int stride = b.PixelWidth * (b.Format.BitsPerPixel / 8);

                // Create data array to hold source pixel data
                byte[] data = new byte[stride * b.PixelHeight];

                // Copy source image pixels to the data array
                b.CopyPixels(data, stride, 0);

                // Create WriteableBitmap to copy the pixel data to.      
                WriteableBitmap target = new(b.PixelWidth, b.PixelHeight, b.DpiX, b.DpiY, b.Format, null);

                // Write the pixel data to the WriteableBitmap.
                target.WritePixels(new Int32Rect(0, 0, b.PixelWidth, b.PixelHeight), data, stride, 0);

                return target;
            }

            return a(result);
        }
    }
}