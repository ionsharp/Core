using Imagin.Common;
using Imagin.Common.Drawing;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class HslModel : ColorSpaceModel<Hsl>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Hsl GetValue()
        {
            return new Hsl(Components[typeof(HComponent)].Value, Components[typeof(SComponent)].Value, Components[typeof(LComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public HslModel() : base()
        {
            Components.Add(new HComponent(this));
            Components.Add(new SComponent(this));
            Components.Add(new LComponent(this));
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : SelectableComponentModel
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
            public override bool IsUniform
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMaximum()
            {
                return Hsl.MaxValue.H;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Hsl.MinValue.H;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                return new Hsl(ComponentValue.ToDouble(), SelectionPoint.X.Divide(255d).Multiply(Hsl.MaxValue.S), 1d.Subtract(SelectionPoint.Y / 255d).Multiply(Hsl.MaxValue.L)).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Hsl(Color);
                return new Hsl(NewComponentValue.ToDouble(), c.S, c.L).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Hsl(Color).H;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var hsl = new Hsl(Color);
                return PointFromColor(hsl.S.Divide(Hsl.MaxValue.S), hsl.L.Divide(Hsl.MaxValue.L));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Hue";
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
                    return new Hsl(ComponentValue.ToDouble(), RowColumn.Column, RowColumn.Row).ToRgba();
                }), new RowColumn(Hsl.MaxValue.L, Hsl.MaxValue.S));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            /// <param name="Orientation"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false, Orientation Orientation = Orientation.Vertical)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Hsl(CurrentRow, Hsl.MaxValue.S, Hsl.MaxValue.L / 2).ToRgba();
                }), false, Orientation);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public HComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "S";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMaximum()
            {
                return Hsl.MaxValue.S;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Hsl.MinValue.S;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string UnitLabel
            {
                get
                {
                    return "%";
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
                return new Hsl(SelectionPoint.X.Divide(255).Multiply(Hsl.MaxValue.H), ComponentValue.ToDouble(), 1d.Subtract(SelectionPoint.Y / 255d).Multiply(Hsl.MaxValue.L)).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Hsl(Color);
                return new Hsl(c.H, NewComponentValue.ToDouble(), c.L).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Hsl(Color).S;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var hsl = new Hsl(Color);
                return PointFromColor(hsl.H.Divide(Hsl.MaxValue.H), hsl.L.Divide(Hsl.MaxValue.L));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Saturation";
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
                    return new Hsl(RowColumn.Column, ComponentValue.ToDouble(), RowColumn.Row).ToRgba();
                }), new RowColumn(Hsl.MaxValue.L, Hsl.MaxValue.H));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            /// <param name="Orientation"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false, Orientation Orientation = Orientation.Vertical)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Hsl(Color).New(Hsl.Component.S, CurrentRow).ToRgba();
                }), false, Orientation);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public SComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "L";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMaximum()
            {
                return Hsl.MaxValue.L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Hsl.MinValue.L;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string UnitLabel
            {
                get
                {
                    return "%";
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
                return new Hsl(SelectionPoint.X.Divide(255).Multiply(Hsl.MaxValue.H), 1d.Subtract(SelectionPoint.Y / 255d).Multiply(Hsl.MaxValue.S), ComponentValue.ToDouble()).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Hsl(Color);
                return new Hsl(c.H, c.S, NewComponentValue.ToDouble()).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Hsl(Color).L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var hsl = new Hsl(Color);
                return PointFromColor(hsl.H.Divide(Hsl.MaxValue.H), hsl.S.Divide(Hsl.MaxValue.S));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Lightness";
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
                    return new Hsl(RowColumn.Column, RowColumn.Row, ComponentValue.ToDouble()).ToRgba();
                }), new RowColumn(Hsl.MaxValue.S, Hsl.MaxValue.H));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            /// <param name="Orientation"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false, Orientation Orientation = Orientation.Vertical)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Hsl(Color).New(Hsl.Component.L, CurrentRow).ToRgba();
                }), false, Orientation);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public LComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }
    }
}
