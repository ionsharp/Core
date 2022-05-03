using System.IO;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class XBitmapImage
    {
        public static System.Drawing.Bitmap Bitmap(this BitmapImage input)
        {
            using (var outStream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(input));
                encoder.Save(outStream);

                var result = new System.Drawing.Bitmap(outStream);
                return new System.Drawing.Bitmap(result);
            }
        }
    }
}