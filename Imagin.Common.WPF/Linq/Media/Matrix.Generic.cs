using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class XMatrix
    {
        public static WriteableBitmap Convert(this Matrix<Argb> input)
        {
            if (input != null)
            {
                var result = Media.BitmapFactory.New(input.Columns.Int32(), input.Rows.Int32());
                input.Each((y, x, i) =>
                {
                    result.SetPixel(x, y, System.Windows.Media.Color.FromArgb(i.A, i.R, i.G, i.B));
                    return i;
                });
                return result;
            }
            return null;
        }
    }
}