using System.IO;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class BitmapImageExtensions
    {
        public static System.Drawing.Bitmap ToBitmap(this BitmapImage BitmapImage)
        {
            using (MemoryStream OutStream = new MemoryStream())
            {
                PngBitmapEncoder Encoder = new PngBitmapEncoder();
                Encoder.Frames.Add(BitmapFrame.Create(BitmapImage));
                Encoder.Save(OutStream);
                System.Drawing.Bitmap Bitmap = new System.Drawing.Bitmap(OutStream);
                return new System.Drawing.Bitmap(Bitmap);
            }
        }
    }
}
