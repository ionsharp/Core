using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    public class XyzModel : ColorSpaceModel
    {
        public override Color GetColor()
        {
            return Xyz.ToColor(this.Components[typeof(XComponent)].CurrentValue, this.Components[typeof(YComponent)].CurrentValue, this.Components[typeof(ZComponent)].CurrentValue);
        }

        public XyzModel() : base()
        {
            this.Components.Add(new XComponent());
            this.Components.Add(new YComponent());
            this.Components.Add(new ZComponent());
        }

        public sealed class XComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "X";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "";
                }
            }

            public override int MaxValue
            {
                get
                {
                    return 95;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double x = ComponentValue / Xyz.MaxValue.X.Shift(2);
                double y = SelectionPoint.X / 255d;
                double z = 1d - (SelectionPoint.Y / 255d);
                return Xyz.ToColor(x, y, z);
            }

            public override int GetValue(Color Color)
            {
                return (Xyz.FromColor(Color).X * Xyz.MaxValue.X.Shift(2)).Round().ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Xyz Xyz = Xyz.FromColor(Color);
                int x = (Xyz.Y * 255.0).ToInt();
                int y = 255 - (Xyz.Z * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Xyz Xyz = Xyz.FromColor(Color);
                    return Xyz.ToRgba(CurrentRow / Xyz.MaxValue.X.Shift(2), Xyz.Y, Xyz.Z);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Xyz.ToRgba(ComponentValue.ToDouble() / Xyz.MaxValue.X.Shift(2), RowColumn.Column, RowColumn.Row);
                }), new RowColumn(Xyz.MaxValue.Z, Xyz.MaxValue.Y));
            }
        }

        public sealed class YComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "Y";
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
                double x = SelectionPoint.X / 255.0;
                double y = ComponentValue.ToDouble() / Xyz.MaxValue.Y.Shift(2);
                double z = 1d - (SelectionPoint.Y / 255.0);
                return Xyz.ToColor(x, y, z);
            }

            public override int GetValue(Color Color)
            {
                return (Xyz.FromColor(Color).Y * Xyz.MaxValue.Y.Shift(2)).Round().ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Xyz Xyz = Xyz.FromColor(Color);
                int x = (Xyz.X * 255.0).ToInt();
                int y = 255 - (Xyz.Z * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Xyz Xyz = Xyz.FromColor(Color);
                    return Xyz.ToRgba(Xyz.X, CurrentRow / Xyz.MaxValue.Y.Shift(2), Xyz.Z);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Xyz.ToRgba(RowColumn.Column, ComponentValue.ToDouble() / Xyz.MaxValue.Y.Shift(2), RowColumn.Row);
                }), new RowColumn(Xyz.MaxValue.Z, Xyz.MaxValue.X));
            }
        }

        public sealed class ZComponent : NormalComponentModel
        {
            public override string ComponentLabel
            {
                get
                {
                    return "Z";
                }
            }

            public override string UnitLabel
            {
                get
                {
                    return "";
                }
            }

            public override int MaxValue
            {
                get
                {
                    return 108;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double x = SelectionPoint.X / 255.0;
                double y = 1.0 - (SelectionPoint.Y / 255.0);
                double z = ComponentValue.ToDouble() / Xyz.MaxValue.Z.Shift(2);
                return Xyz.ToColor(x, y, z);
            }

            public override int GetValue(Color Color)
            {
                return (Xyz.FromColor(Color).Z * Xyz.MaxValue.Z.Shift(2)).Round().ToInt();
            }

            public override Point PointFromColor(Color Color)
            {
                Xyz Xyz = Xyz.FromColor(Color);
                int x = (Xyz.X * 255.0).ToInt();
                int y = 255 - (Xyz.Y * 255.0).ToInt();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Xyz Xyz = Xyz.FromColor(Color);
                    return Xyz.ToRgba(Xyz.X, Xyz.Y, CurrentRow / Xyz.MaxValue.Z.Shift(2));
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Xyz.ToRgba(RowColumn.Column, RowColumn.Row, ComponentValue.ToDouble() / Xyz.MaxValue.Z.Shift(2));
                }), new RowColumn(Xyz.MaxValue.Y, Xyz.MaxValue.X));
            }
        }
    }
}
