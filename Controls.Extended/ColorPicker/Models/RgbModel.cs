using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
public class RgbModel : ColorSpaceModel
{
    public override Color GetColor()
    {
        return new Rgba(this.Components[typeof(RComponent)].CurrentValue.ToByte(), this.Components[typeof(GComponent)].CurrentValue.ToByte(), this.Components[typeof(BComponent)].CurrentValue.ToByte()).ToColor();
    }

    public RgbModel() : base()
    {
        this.Components.Add(new RComponent());
        this.Components.Add(new GComponent());
        this.Components.Add(new BComponent());
    }

    public sealed class RComponent : NormalComponentModel
    {
        public override string ComponentLabel
        {
            get
            {
                return "R";
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
                return 255;
            }
        }

        public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
        {
            var blue = SelectionPoint.X.Round().ToByte();
            var green = (255.0 - SelectionPoint.Y).Round().ToByte();
            var red = ComponentValue.ToByte();
            return Color.FromRgb(red, green, blue);
        }

        public override int GetValue(Color Color)
        {
            return Color.R;
        }

        public override Point PointFromColor(Color Color)
        {
            return new Point(Color.B, 255 - Color.G);
        }

        public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
        {
            base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
            {
                return new Rgba((255 - CurrentRow).ToByte(), Color.G, Color.B);
            }), true);
        }

        public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
        {
            base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
            {
                return new Rgba(ComponentValue.ToByte(), RowColumn.Row.ToByte(), RowColumn.Column.ToByte());
            }), new RowColumn(255, 255));
        }
    }

    public sealed class GComponent : NormalComponentModel
    {
        public override string ComponentLabel
        {
            get
            {
                return "G";
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
                return 255;
            }
        }

        public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
        {
            var blue = (byte)Math.Round(SelectionPoint.X);
            var green = (byte)ComponentValue;
            var red = (byte)Math.Round(255 - SelectionPoint.Y);
            return Color.FromRgb(red, green, blue);
        }

        public override int GetValue(Color Color)
        {
            return Color.G;
        }

        public override Point PointFromColor(Color Color)
        {
            return new Point(Color.B, 255 - Color.R);
        }

        public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
        {
            base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
            {
                return new Rgba(Color.R, (255.ToDouble() - CurrentRow).ToByte(), Color.B);
            }), true);
        }

        public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
        {
            base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
            {
                return new Rgba(RowColumn.Row.ToByte(), ComponentValue.ToByte(), RowColumn.Column.ToByte());
            }), new RowColumn(255, 255));
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

        public override int MaxValue
        {
            get
            {
                return 255;
            }
        }

        public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
        {
            var blue = (byte)ComponentValue;
            var green = (byte)Math.Round(255 - SelectionPoint.Y);
            var red = (byte)Math.Round(SelectionPoint.X);
            return Color.FromRgb(red, green, blue);
        }

        public override int GetValue(Color Color)
        {
            return Color.B;
        }

        public override Point PointFromColor(Color Color)
        {
            return new Point(Color.R, 255 - Color.G);
        }

        public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
        {
            base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
            {
                return new Rgba(Color.R, Color.G, (255.ToDouble() - CurrentRow).ToByte());
            }), true);
        }

        public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
        {
            base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
            {
                return new Rgba(RowColumn.Column.ToByte(), RowColumn.Row.ToByte(), ComponentValue.ToByte());
            }), new RowColumn(255, 255));
        }
    }
}
}
