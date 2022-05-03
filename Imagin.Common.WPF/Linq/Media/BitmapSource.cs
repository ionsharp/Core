using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public enum ImageExtensions { Jpg, Png, }

    public static class XBitmapSource
    {
        public static BitmapImage BitmapImage(this BitmapSource input)
        {
            if (input == null)
                return null;

            var e = new JpegBitmapEncoder();
            var s = new MemoryStream();
            var m = new BitmapImage();

            e.Frames.Add(BitmapFrame.Create(input));
            e.Save(s);

            m.BeginInit();
            m.StreamSource = new MemoryStream(s.ToArray());
            m.EndInit();

            s.Close();
            return m;
        }

        public static System.Drawing.Bitmap Bitmap(this BitmapSource input, ImageExtensions extension = ImageExtensions.Jpg)
        {
            if (input == null)
                return null;

            System.Drawing.Bitmap result;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder encoder 
                    = extension == ImageExtensions.Jpg 
                    ? new JpegBitmapEncoder() 
                    : new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(input));
                encoder.Save(outStream);
                result = new System.Drawing.Bitmap(outStream);
            }
            return result;
        }

        public static byte[] Bytes(this BitmapSource input)
        {
            var stream = ((BitmapImage)(input as ImageSource)).StreamSource;
            byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using (var reader = new BinaryReader(stream))
                    buffer = reader.ReadBytes((int)stream.Length);
            }
            return buffer;
        }
    }
}