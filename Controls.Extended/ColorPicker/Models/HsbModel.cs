using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public class HsbModel : ColorSpaceModel
    {
        public override Color GetColor()
        {
            return Hsb.ToColor(this.Components[typeof(HComponent)].CurrentValue, this.Components[typeof(SComponent)].CurrentValue / 100.0, this.Components[typeof(BComponent)].CurrentValue / 100.0);
        }

        public HsbModel() : base()
        {
            this.Components.Add(new HComponent());
            this.Components.Add(new SComponent());
            this.Components.Add(new BComponent());
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
                double b = 1.0 - SelectionPoint.Y / 255.0;
                return Hsb.ToColor(h, s, b);
            }

            public override int GetValue(Color Color)
            {
                return (Hsb.FromColor(Color).H * this.MaxValue.ToDouble()).Round().ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsb Hsb = Hsb.FromColor(Color);
                int x = (Hsb.S * 255.0).ToInt();
                int y = 255 - (Hsb.B * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return Hsb.ToRgba(CurrentRow / this.MaxValue.ToDouble(), 1.0, 1.0);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Hsb.ToRgba(ComponentValue.ToDouble() / this.MaxValue.ToDouble(), RowColumn.Column, RowColumn.Row);
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
                double b = 1.0 - SelectionPoint.Y / 255.0;
                return Hsb.ToColor(h, s, b);
            }

            public override int GetValue(Color Color)
            {
                return Hsb.FromColor(Color).S.Shift(2).ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsb Hsb = Hsb.FromColor(Color);
                int x = (Hsb.H * 255.0).ToInt();
                int y = 255 - (Hsb.B * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Hsb Hsb = Hsb.FromColor(Color);
                    return Hsb.ToRgba(Hsb.H, CurrentRow / this.MaxValue.ToDouble(), Hsb.B);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Hsb.ToRgba(RowColumn.Column, ComponentValue.ToDouble() / this.MaxValue.ToDouble(), RowColumn.Row);
                }), new RowColumn(1.0, 1.0));
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
                    return "%";
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double h = SelectionPoint.X / 255.0;
                double s = 1.0 - SelectionPoint.Y / 255.0;
                double b = ComponentValue.ToDouble() / 100.0;
                return Hsb.ToColor(h, s, b);
            }

            public override int GetValue(Color Color)
            {
                return Hsb.FromColor(Color).B.Shift(2).ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsb Hsb = Hsb.FromColor(Color);
                int x = (Hsb.H * 255.0).ToInt();
                int y = 255 - (Hsb.S * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Hsb Hsb = Hsb.FromColor(Color);
                    return Hsb.ToRgba(Hsb.H, Hsb.S, CurrentRow / this.MaxValue.ToDouble());
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Hsb.ToRgba(RowColumn.Column, RowColumn.Row, ComponentValue.ToDouble() / this.MaxValue.ToDouble());
                }), new RowColumn(1.0, 1.0));
            }
        }
    }
}
