using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public class LuvModel : ColorSpaceModel
    {
        public override Color GetColor()
        {
            return Luv.ToColor(this.Components[typeof(LComponent)].CurrentValue, this.Components[typeof(UComponent)].CurrentValue, this.Components[typeof(VComponent)].CurrentValue);
        }

        public LuvModel() : base()
        {
            this.Components.Add(new LComponent());
            this.Components.Add(new UComponent());
            this.Components.Add(new VComponent());
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

            public override bool IsNormalIndependantOfColor
            {
                get
                {
                    return true;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double L = ComponentValue.ToDouble() / 100.0;
                double U = SelectionPoint.X / 255.0;
                double V = 1.0 - SelectionPoint.Y / 255.0;
                return Luv.ToColor(L, U, V);
            }

            public override int GetValue(Color Color)
            {
                return (Luv.FromColor(Color).L * 100.0).Round().ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Luv Luv = Luv.FromColor(Color);
                int x = (Luv.U * 255.0).ToInt();
                int y = 255 - (Luv.V * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double u = Luv.FromColor(Color).U, v = Luv.FromColor(Color).V;
                    return Luv.ToRgba(CurrentRow / 100.0, u, v);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Luv.ToRgba(ComponentValue.ToDouble() / 100.0, RowColumn.Column, RowColumn.Row);
                }), new RowColumn(1.0, 1.0));
            }
        }

        public sealed class UComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "U";
                }
            }

            public override int MinValue
            {
                get
                {
                    return -100;
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "";
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double h = SelectionPoint.X / 255.0;
                double s = ComponentValue.ToDouble() / 100.0;
                double l = 1.0 - SelectionPoint.Y / 255.0;
                return Hsl.ToColor(h, s, l);
            }

            public override int GetValue(Color Color)
            {
                return ((Hsl.FromColor(Color).S) * 100.0).ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Luv Luv = Luv.FromColor(Color);
                int x = (Luv.L * 255.0).ToInt();
                int y = 255 - (Luv.V * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Luv.FromColor(Color).L, v = Luv.FromColor(Color).V;
                    return Luv.ToRgba(l, CurrentRow, v);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Luv.ToRgba(RowColumn.Column, ComponentValue.ToDouble(), RowColumn.Row - 100.0);
                }), new RowColumn(200.0, 100.0));
            }
        }

        public sealed class VComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "V";
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
                    return -100;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double h = SelectionPoint.X / 255.0;
                double s = 1.0 - SelectionPoint.Y / 255.0;
                double l = ComponentValue.ToDouble() / 100.0;
                return Hsl.ToColor(h.Round(2), s.Round(2), l);
            }

            public override int GetValue(Color Color)
            {
                return (Hsl.FromColor(Color).L * 100.0).ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Luv Luv = Luv.FromColor(Color);
                int x = (Luv.L * 255.0).ToInt();
                int y = 255 - (Luv.U * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Luv.FromColor(Color).L, u = Luv.FromColor(Color).U;
                    return Luv.ToRgba(l, u, CurrentRow);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Luv.ToRgba(RowColumn.Column, RowColumn.Row - 100.0, ComponentValue.ToDouble());
                }), new RowColumn(200.0, 100.0));
            }
        }
    }
}
