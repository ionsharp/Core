using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public class LchModel : ColorSpaceModel
    {
        public override Color GetColor()
        {
            return Lch.ToColor(this.Components[typeof(LComponent)].CurrentValue, this.Components[typeof(CComponent)].CurrentValue, this.Components[typeof(HComponent)].CurrentValue);
        }

        public LchModel() : base()
        {
            this.Components.Add(new LComponent());
            this.Components.Add(new CComponent());
            this.Components.Add(new HComponent());
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
                double c = (1.0 - SelectionPoint.Y / 255.0).Shift(2);
                double h = (SelectionPoint.X / 255.0) * 359.0;
                return Lch.ToColor(l, c, h);
            }

            public override int GetValue(Color Color)
            {
                return Lch.FromColor(Color).L.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lch Lch = Lch.FromColor(Color);
                int x = ((Lch.C / Lch.MaxValue.C) * 255.0).ToInt32();
                int y = 255 - ((Lch.H / Lch.MaxValue.H) * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Lch Lch = Lch.FromColor(Color);
                    return Lch.ToRgba(CurrentRow, Lch.C, Lch.H);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lch.ToRgba(ComponentValue.ToDouble(), RowColumn.Row, RowColumn.Column);
                }), new RowColumn(100.0, 359.0));
            }
        }

        public sealed class CComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "C";
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
                double l = (1.0 - SelectionPoint.Y / 255.0).Shift(2);
                double c = ComponentValue.ToDouble();
                double h = (SelectionPoint.X / 255.0) * 359.0;
                return Lch.ToColor(l, c, h);
            }

            public override int GetValue(Color Color)
            {
                return Lch.FromColor(Color).C.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lch Lch = Lch.FromColor(Color);
                int x = ((Lch.L / Lch.MaxValue.L) * 255.0).ToInt32();
                int y = 255 - ((Lch.H / Lch.MaxValue.H) * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Lch Lch = Lch.FromColor(Color);
                    return Lch.ToRgba(Lch.L, CurrentRow, Lch.H);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lch.ToRgba(RowColumn.Row, ComponentValue.ToDouble(), RowColumn.Column);
                }), new RowColumn(100.0, 359.0));
            }
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
                double l = (1.0 - SelectionPoint.Y / 255.0).Shift(2);
                double c = SelectionPoint.X / 255.0;
                double h = ComponentValue.ToDouble();
                return Lch.ToColor(l, c, h);
            }

            public override int GetValue(Color Color)
            {
                Lch Lch = Lch.FromColor(Color);
                return Lch.H.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lch Lch = Lch.FromColor(Color);
                int x = ((Lch.L / Lch.MaxValue.L) * 255.0).ToInt32();
                int y = 255 - ((Lch.C / Lch.MaxValue.C) * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Lch Lch = Lch.FromColor(Color);
                    return Lch.ToRgba(Lch.L, Lch.C, CurrentRow);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lch.ToRgba(RowColumn.Row, RowColumn.Column, ComponentValue.ToDouble());
                }), new RowColumn(100.0, 100.0));
            }
        }
    }
}
