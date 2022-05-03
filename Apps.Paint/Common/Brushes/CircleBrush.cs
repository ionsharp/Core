using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [Icon(App.ImagePath + "BrushCircle.png")]
    [Serializable]
    public class CircleBrush : HardBrush
    {
        [Featured, ReadOnly]
        public override string Name => "Circle";

        public override Matrix<byte> GetBytes(int size)
        {
            var result = new Matrix<byte>(size.UInt32(), size.UInt32());
            WriteableBitmap bitmap = null;

            if (Hardness < 1)
            {
                bitmap = BitmapFactory.New(size, size);

                int columns = size, rows = size;
                int x1 = 0, y1 = 0;

                double a = Hardness;

                uint ring = 0;
                int rings = GetRings(columns, rows);

                var total = 1 - Hardness;
                var i = total / rings.Double();

                while (columns > 1 && rows > 1)
                {
                    bitmap.FillEllipse(x1, y1, x1 + columns, y1 + rows, Colors.White.A(a.Multiply(255)), null);
                    a += i;

                    x1++;
                    y1++;

                    ring++;
                    columns -= 2;
                    rows -= 2;
                }

                bitmap.ForEach((x2, y2, color) =>
                {
                    result.SetValue(y2.UInt32(), x2.UInt32(), color.A);
                    return color;
                });
            }
            else
            {
                bitmap = BitmapFactory.New(result.Columns.Int32(), result.Rows.Int32());
                bitmap.FillEllipse(0, 0, result.Columns.Int32() - 1, result.Rows.Int32() - 1, Colors.Black);

                for (uint x = 0; x < result.Columns; x++)
                {
                    for (uint y = 0; y < result.Rows; y++)
                        result.SetValue(y, x, bitmap.GetPixel(x.Int32(), y.Int32()).A);
                }
            }
            return result;
        }

        public override Brush Clone() => new CircleBrush()
        {
            Angle = Angle,
            Bytes = Bytes,
            Hardness = Hardness,
            Name = Name,
            Noise = Noise,
            Size = Size,
            XScale = XScale,
            YScale = YScale
        };
    }
}