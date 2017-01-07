using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public class LabModel : ColorSpaceModel
    {
        public override Color GetColor()
        {
            return Lab.ToColor(this.Components[typeof(LComponent)].CurrentValue, this.Components[typeof(AComponent)].CurrentValue, this.Components[typeof(BComponent)].CurrentValue);
        }

        public LabModel() : base()
        {
            this.Components.Add(new LComponent());
            this.Components.Add(new AComponent());
            this.Components.Add(new BComponent());
        }

        public sealed class LComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "L";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = ComponentValue.ToDouble();
                double b = 127.0 - SelectionPoint.Y;
                double a = SelectionPoint.X - 128.0;
                return Lab.ToColor(l, a, b);
            }

            public override int GetValue(Color Color)
            {
                return Lab.FromColor(Color).L.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                int x = 128 + Lab.A.ToInt32();
                int y = 128 - Lab.B.ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double a = Lab.FromColor(Color).A, b = Lab.FromColor(Color).B;
                    return Lab.ToRgba(CurrentRow, a, b);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lab.ToRgba(ComponentValue.ToDouble(), RowColumn.Column - 128.0, RowColumn.Row - 128.0);
                }), new RowColumn(256.0, 256.0));
            }
        }

        public sealed class AComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "A";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "";
                }
            }

            public override int MinValue
            {
                get
                {
                    return -128;
                }
            }

            public override int MaxValue
            {
                get
                {
                    return 127;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = 100.0 - SelectionPoint.Y * 100.0 / 256.0;
                double a = ComponentValue.ToDouble();
                double b = SelectionPoint.X - 128.0;
                return Lab.ToColor(l, a, b);
            }

            public override int GetValue(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                return Lab.A.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                int x = 128 + Lab.B.ToInt32();
                int y = 100 - Lab.L.ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Lab.FromColor(Color).L, b = Lab.FromColor(Color).B;
                    return Lab.ToRgba(l, CurrentRow, b);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lab.ToRgba(RowColumn.Row, ComponentValue.ToDouble(), RowColumn.Column - 128.0);
                }), new RowColumn(100.0, 256.0));
            }
        }

        public sealed class BComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "B";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "";
                }
            }

            public override int MinValue
            {
                get
                {
                    return -128;
                }
            }

            public override int MaxValue
            {
                get
                {
                    return 127;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = 100.0 - SelectionPoint.Y * 100.0 / 256.0;
                double a = SelectionPoint.X - 128.0;
                double b = ComponentValue.ToDouble();
                return Lab.ToColor(l, a, b);
            }

            public override int GetValue(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                return Lab.B.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                int x = 128 + Lab.A.ToInt32();
                int y = 100 - Lab.L.ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Lab.FromColor(Color).L, a = Lab.FromColor(Color).A;
                    return Lab.ToRgba(l, a, CurrentRow);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lab.ToRgba(RowColumn.Row, RowColumn.Column - 128.0, ComponentValue.ToDouble());
                }), new RowColumn(100.0, 256.0));
            }
        }
    }
}
