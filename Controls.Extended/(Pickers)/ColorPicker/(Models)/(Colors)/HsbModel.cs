using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended.Primitives;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public class HsbModel : ColorSpaceModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Color GetColor()
        {
            return Hsb.ToColor(Components[typeof(HComponent)].Value / 360d, Components[typeof(SComponent)].Value / 100d, Components[typeof(BComponent)].Value / 100d);
        }

        /// <summary>
        /// 
        /// </summary>
        public HsbModel() : base()
        {
            Components.Add(new HComponent());
            Components.Add(new SComponent());
            Components.Add(new BComponent());
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : NormalComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "H";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override string UnitLabel
            {
                get
                {
                    return "°";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override bool IsNormalIndependantOfColor
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override int Maximum
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
                return (Hsb.FromColor(Color).H * Maximum.ToDouble()).Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsb Hsb = Hsb.FromColor(Color);
                int x = (Hsb.S * 255.0).ToInt32();
                int y = 255 - (Hsb.B * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) => Hsb.ToRgba(CurrentRow / Maximum.ToDouble(), 1d, 1d)));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) => Hsb.ToRgba(ComponentValue.ToDouble() / Maximum.ToDouble(), RowColumn.Column, RowColumn.Row)), new RowColumn(1d, 1d));
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                return Hsb.FromColor(Color).S.Shift(2).ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsb Hsb = Hsb.FromColor(Color);
                int x = (Hsb.H * 255.0).ToInt32();
                int y = 255 - (Hsb.B * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Hsb Hsb = Hsb.FromColor(Color);
                    return Hsb.ToRgba(Hsb.H, CurrentRow / Maximum.ToDouble(), Hsb.B);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) => Hsb.ToRgba(RowColumn.Column, ComponentValue.ToDouble() / Maximum.ToDouble(), RowColumn.Row)), new RowColumn(1d, 1d));
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                return Hsb.FromColor(Color).B.Shift(2).ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Hsb Hsb = Hsb.FromColor(Color);
                int x = (Hsb.H * 255.0).ToInt32();
                int y = 255 - (Hsb.S * 255.0).ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    Hsb Hsb = Hsb.FromColor(Color);
                    return Hsb.ToRgba(Hsb.H, Hsb.S, CurrentRow / Maximum.ToDouble());
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) => Hsb.ToRgba(RowColumn.Column, RowColumn.Row, ComponentValue.ToDouble() / Maximum.ToDouble())), new RowColumn(1d, 1d));
            }
        }
    }
}
