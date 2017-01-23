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

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double L = ComponentValue.ToDouble() / 100.0;
                double U = SelectionPoint.X / 255.0;
                double V = 1.0 - SelectionPoint.Y / 255.0;
                return Luv.ToColor(L, U, V);
            }

            public override int GetValue(Color Color)
            {
                return (Luv.FromColor(Color).L * 100.0).Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Luv Luv = Luv.FromColor(Color);
                int x = (Luv.U * 255.0).ToInt32();
                int y = 255 - (Luv.V * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Luv.ToRgba(ComponentValue.ToDouble() / this.MaxValue.ToDouble(), RowColumn.Column - 1.0, RowColumn.Row - 1.0);
                }), new RowColumn(2.0, 2.0));
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double u = Luv.FromColor(Color).U, v = Luv.FromColor(Color).V;
                    return Luv.ToRgba(CurrentRow / this.MaxValue.ToDouble(), u, v);
                }));
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
                double l = SelectionPoint.X / 255.0;
                double u = ComponentValue.ToDouble() / 100.0;
                double v = 1.0 - SelectionPoint.Y / 255.0;
                return Luv.ToColor(l, u, v);
            }

            public override int GetValue(Color Color)
            {
                return ((Luv.FromColor(Color).U) * 100.0).ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Luv Luv = Luv.FromColor(Color);
                int x = (Luv.L * 255.0).ToInt32();
                int y = 255 - (Luv.V * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Luv.ToRgba(RowColumn.Column, ComponentValue.ToDouble() / this.MaxValue.ToDouble(), RowColumn.Row - 1.0);
                }), new RowColumn(2.0, 1.0));
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Luv.FromColor(Color).L, v = Luv.FromColor(Color).V;
                    return Luv.ToRgba(l, CurrentRow / this.MaxValue.ToDouble(), v);
                }));
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
                double l = SelectionPoint.X / 255.0;
                double u = 1.0 - SelectionPoint.Y / 255.0;
                double v = ComponentValue.ToDouble() / 100.0;
                return Luv.ToColor(l.Round(2), u.Round(2), v);
            }

            public override int GetValue(Color Color)
            {
                return (Luv.FromColor(Color).V * 100.0).ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Luv Luv = Luv.FromColor(Color);
                int x = (Luv.L * 255.0).ToInt32();
                int y = 255 - (Luv.U * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Luv.ToRgba(RowColumn.Column, RowColumn.Row - 1.0, ComponentValue.ToDouble()/ this.MaxValue.ToDouble());
                }), new RowColumn(2.0, 1.0));
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Luv.FromColor(Color).L, u = Luv.FromColor(Color).U;
                    return Luv.ToRgba(l, u, CurrentRow / this.MaxValue.ToDouble());
                }));
            }
        }
    }
}
