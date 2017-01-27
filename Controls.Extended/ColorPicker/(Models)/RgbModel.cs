using Imagin.Common;
using Imagin.Common.Drawing;
using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RgbaModel : ColorSpaceModel<Rgba>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Rgba GetValue()
        {
            return new Rgba(Components[typeof(RComponent)].Value, Components[typeof(GComponent)].Value, Components[typeof(BComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public RgbaModel() : base()
        {
            Components.Add(new RComponent(this));
            Components.Add(new GComponent(this));
            Components.Add(new BComponent(this));
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract class RgbComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMaximum()
            {
                return Rgba.MaxValue.ToDouble();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Rgba.MinValue.ToDouble();
            }

            /// <summary>
            /// 
            /// </summary>
            public sealed override string UnitLabel
            {
                get
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public RgbComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class RComponent : RgbComponent
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "R";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                var blue = SelectionPoint.X.Round().ToByte();
                var green = (255.0 - SelectionPoint.Y).Round().ToByte();
                var red = ComponentValue.ToByte();
                return Color.FromRgb(red, green, blue);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return Color.R;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                return new Point(Color.B, 255 - Color.G);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="ComponentValue"></param>
            /// <param name="Action"></param>
            /// <param name="Unit"></param>
            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return new Rgba(ComponentValue.ToByte(), RowColumn.Row.ToByte(), RowColumn.Column.ToByte());
                }), new RowColumn(255, 255));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Rgba((255 - CurrentRow).ToByte(), Color.G, Color.B);
                }), true);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public RComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class GComponent : RgbComponent
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "G";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                var blue = (byte)Math.Round(SelectionPoint.X);
                var green = (byte)ComponentValue;
                var red = (byte)Math.Round(255 - SelectionPoint.Y);
                return Color.FromRgb(red, green, blue);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return Color.G;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                return new Point(Color.B, 255 - Color.R);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="ComponentValue"></param>
            /// <param name="Action"></param>
            /// <param name="Unit"></param>
            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return new Rgba(RowColumn.Row.ToByte(), ComponentValue.ToByte(), RowColumn.Column.ToByte());
                }), new RowColumn(255, 255));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Rgba(Color.R, (255.ToDouble() - CurrentRow).ToByte(), Color.B);
                }), true);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public GComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class BComponent : RgbComponent
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "B";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                var blue = (byte)ComponentValue;
                var green = (byte)Math.Round(255 - SelectionPoint.Y);
                var red = (byte)Math.Round(SelectionPoint.X);
                return Color.FromRgb(red, green, blue);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return Color.B;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                return new Point(Color.R, 255 - Color.G);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="ComponentValue"></param>
            /// <param name="Action"></param>
            /// <param name="Unit"></param>
            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return new Rgba(RowColumn.Column.ToByte(), RowColumn.Row.ToByte(), ComponentValue.ToByte());
                }), new RowColumn(255, 255));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Rgba(Color.R, Color.G, (255.ToDouble() - CurrentRow).ToByte());
                }), true);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public BComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }
    }
}
