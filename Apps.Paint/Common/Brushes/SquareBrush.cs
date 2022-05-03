using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;

namespace Imagin.Apps.Paint
{
    [Icon(App.ImagePath + "BrushSquare.png")]
    [Serializable]
    public class SquareBrush : HardBrush
    {
        [Featured, ReadOnly]
        public override string Name => "Square";

        public override Matrix<byte> GetBytes(int size)
        {
            var result = new Matrix<byte>(size.UInt32(), size.UInt32());
            if (Hardness < 1)
            {
                uint columns = result.Columns, rows = result.Rows;
                uint x = 0, y = 0;

                var total = 1 - Hardness;
                double a = Hardness;

                uint ring = 0;
                int rings = GetRings(columns.Int32(), rows.Int32());
                var increment = total / rings.Double();

                while (columns > 1 && rows > 1)
                {
                    for (y = ring; y < rows; y++)
                    {
                        for (x = ring; x < columns; x++)
                        {
                            result.SetValue(y, x, a.Multiply(255));
                        }
                    }

                    a += increment;

                    ring++;
                    columns -= 1;
                    rows -= 1;
                }
            }
            else
            {
                for (uint x = 0; x < result.Columns; x++)
                {
                    for (uint y = 0; y < result.Rows; y++)
                        result[y, x] = 255;
                }
            }
            return result;
        }

        public override Brush Clone() => new SquareBrush()
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