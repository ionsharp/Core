using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public class HslModel : ColorSpaceModel
    {
        public override Color GetColor()
        {
            return Hsl.ToColor(this.Components[typeof(HComponent)].CurrentValue / 360.0, this.Components[typeof(SComponent)].CurrentValue / 100.0, this.Components[typeof(LComponent)].CurrentValue / 100.0);
        }

        public HslModel() : base()
        {
            this.Components.Add(new HComponent());
            this.Components.Add(new SComponent());
            this.Components.Add(new LComponent());
        }

        public sealed class HComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "H";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "°";
                }
            }

            public override bool IsNormalIndependantOfColor
            {
                get
                {
                    return true;
                }
            }

            public override int MaxValue
            {
                get
                {
                    return 359;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double h = ComponentValue.ToDouble() / 359.0;
                double s = SelectionPoint.X / 255.0;
                double l = 1.0 - SelectionPoint.Y / 255.0;
                return Hsl.ToColor(h, s, l);
            }

            public override int GetValue(Color Color)
            {
                return (Hsl.FromColor(Color).H * this.MaxValue.ToDouble()).Round().ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsl Hsl = Hsl.FromColor(Color);
                int x = (Hsl.S * 255.0).ToInt();
                int y = 255 - (Hsl.L * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return Hsl.ToRgba(CurrentRow / this.MaxValue.ToDouble(), 1.0, 0.5);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Hsl.ToRgba(ComponentValue.ToDouble() / this.MaxValue.ToDouble(), RowColumn.Column, RowColumn.Row);
                }), new RowColumn(1.0, 1.0));
            }
        }

        public sealed class SComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "S";
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
                double h = SelectionPoint.X / 255.0;
                double s = ComponentValue.ToDouble() / 100.0;
                double l = 1.0 - SelectionPoint.Y / 255.0;
                return Hsl.ToColor(h, s, l);
            }

            public override int GetValue(Color Color)
            {
                return ((Hsl.FromColor(Color).S) * this.MaxValue.ToDouble()).ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsl Hsl = Hsl.FromColor(Color);
                int x = (Hsl.H * 255.0).ToInt();
                int y = 255 - (Hsl.L * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Hsl Hsl = Hsl.FromColor(Color);
                    return Hsl.ToRgba(Hsl.H, CurrentRow / this.MaxValue.ToDouble(), Hsl.L);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Hsl.ToRgba(RowColumn.Column, ComponentValue.ToDouble() / this.MaxValue.ToDouble(), RowColumn.Row);
                }), new RowColumn(1.0, 1.0));
            }
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
                double h = SelectionPoint.X / 255.0;
                double s = 1.0 - SelectionPoint.Y / 255.0;
                double l = ComponentValue.ToDouble() / 100.0;
                return Hsl.ToColor(h.Round(2), s.Round(2), l);
            }

            public override int GetValue(Color Color)
            {
                return (Hsl.FromColor(Color).L * this.MaxValue.ToDouble()).ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsl Hsl = Hsl.FromColor(Color);
                int x = (Hsl.H * 255.0).ToInt();
                int y = 255 - (Hsl.S * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Hsl Hsl = Hsl.FromColor(Color);
                    return Hsl.ToRgba(Hsl.H, Hsl.S, CurrentRow / this.MaxValue.ToDouble());
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Hsl.ToRgba(RowColumn.Column, RowColumn.Row, ComponentValue.ToDouble() / this.MaxValue.ToDouble());
                }), new RowColumn(1.0, 1.0));
            }
        }
    }
}
