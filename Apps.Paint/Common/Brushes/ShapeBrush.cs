using Imagin.Common;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [Icon(App.ImagePath + "BrushShape.png")]
    [Serializable]
    public class ShapeBrush : Brush
    {
        [Featured]
        [ReadOnly]
        public override string Name => "Shape";

        Shape path = new();
        public Shape Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        //...

        public ShapeBrush() 
            : this("Untitled", new PointCollection()) { }

        public ShapeBrush(string name, IList<System.Drawing.Point> points) 
            : base(name) => Path.Points = new(points.Select(i => new Point(i.X, i.Y)));

        public ShapeBrush(string name, PointCollection points)
            : base(name) => Path.Points = points;

        public ShapeBrush(string name, Shape shape)
            : base(name) => Path.Points = new(shape.Points);

        //...

        public override Matrix<byte> GetBytes(int size)
        {
            var bitmap = BitmapFactory.New(size, size);

            var shape = new Shape(Path);
            shape.Scale(new(size, size));

            var points = Shape.From(shape.Points);

            bitmap.FillPolygon(points, Colors.Black);
            return GetBytes(bitmap);
        }

        /*
        public override Matrix<byte> GetBytes(int size)
        {
            var result = new Matrix<byte>(size.UInt32(), size.UInt32());
            if (Hardness < 1)
            {
                //Hardness can indicate the lowest opacity in the entire matrix; the opacity would then increase from there to 1

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
                        result.SetValue(y, x, 1);
                }
            }
            return result;
        }

        public override void Draw(WriteableBitmap target, Matrix<byte> source, Vector2<int> point, Color color, int size, BlendModes? mode)
        {
            var d = (size / 2.0).Round();
            double x1 = point.X - d, y1 = point.Y - d;
            double x2 = point.X + size - d, y2 = point.Y + size - d;

            if (Hardness < 1)
            {
                var bytes = GetBytes(size);
                target.FillRectangle(x1.Int32(), y1.Int32(), bytes.Transform(i => color.A(i)), mode);
                return;
            }

            //When hardness = 1, use faster method!
            target.FillRectangle(x1.Int32(), y1.Int32(), x2.Int32() - 1, y2.Int32() - 1, color, mode);
        }
        */

        public override Brush Clone() => new ShapeBrush()
        {
            Angle = Angle,
            Bytes = Bytes,
            Name = Name,
            Noise = Noise,
            Path = new(Path),
            Size = Size,
            XScale = XScale,
            YScale = YScale,
        };
    }
}