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
    public class LchModel : ColorSpaceModel<Lch>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Lch GetValue()
        {
            return new Lch(Components[typeof(LComponent)].Value, Components[typeof(CComponent)].Value, Components[typeof(HComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public LchModel() : base()
        {
            Components.Add(new LComponent(this));
            Components.Add(new CComponent(this));
            Components.Add(new HComponent(this));
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Lch.MaxValue.L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Lch.MinValue.L;
            }

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
                var l = ComponentValue.ToDouble();
                var c = (1d - SelectionPoint.Y / 255d) * Lch.MaxValue.C;
                var h = (SelectionPoint.X / 255d) * Lch.MaxValue.H;
                return new Lch(l, c, h, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Lch(Color);
                return new Lch(NewComponentValue.ToDouble(), c.C, c.H).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Lch(Color, Observer, Illuminant).L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var lch = new Lch(Color, Observer, Illuminant);
                int x = ((lch.C / Lch.MaxValue.C) * 255.0).ToInt32();
                int y = 255 - ((lch.H / Lch.MaxValue.H) * 255.0).ToInt32();
                return new Point(x, y);
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
                    return new Lch(ComponentValue.ToDouble(), RowColumn.Row, RowColumn.Column, Observer, Illuminant).ToRgba();
                }), new RowColumn(100d, 359d));
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
                    return new Lch(Color, Observer, Illuminant).New(Lch.Component.L, CurrentRow).ToRgba();
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

        /// <summary>
        /// 
        /// </summary>
        public sealed class CComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Lch.MaxValue.C;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Lch.MinValue.C;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "C";
                }
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
                var l = (1d - SelectionPoint.Y / 255d) * Lch.MaxValue.L;
                var c = ComponentValue.ToDouble();
                var h = (SelectionPoint.X / 255.0) * Lch.MaxValue.H;
                return new Lch(l, c, h, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Lch(Color);
                return new Lch(c.L, NewComponentValue.ToDouble(), c.H).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Lch(Color, Observer, Illuminant).C;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var lch = new Lch(Color, Observer, Illuminant);
                var x = ((lch.L / Lch.MaxValue.L) * 255d).ToInt32();
                var y = 255 - ((lch.H / Lch.MaxValue.H) * 255d).ToInt32();
                return new Point(x, y);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Chroma";
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
                    return new Lch(RowColumn.Row, ComponentValue.ToDouble(), RowColumn.Column, Observer, Illuminant).ToRgba();
                }), new RowColumn(Lch.MaxValue.L, Lch.MaxValue.H));
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
                    return new Lch(Color, Observer, Illuminant).New(Lch.Component.C, CurrentRow).ToRgba();
                }), false, Orientation);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public CComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class HComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Lch.MaxValue.H;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Lch.MinValue.H;
            }

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
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                var l = (1d - SelectionPoint.Y / 255d).Shift(2);
                var c = (SelectionPoint.X / 255d).Shift(2);
                var h = ComponentValue.ToDouble();
                return new Lch(l, c, h, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Lch(Color);
                return new Lch(c.L, c.C, NewComponentValue.ToDouble()).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Lch(Color, Observer, Illuminant).H;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var lch = new Lch(Color, Observer, Illuminant);
                var x = ((lch.C / Lch.MaxValue.C) * 255d).ToInt32();
                var y = 255 - ((lch.L / Lch.MaxValue.L) * 255d).ToInt32();
                return new Point(x, y);
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
                    return new Lch(RowColumn.Row, RowColumn.Column, ComponentValue.ToDouble(), Observer, Illuminant).ToRgba();
                }), new RowColumn(Lch.MaxValue.L, Lch.MaxValue.C));
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
                    return new Lch(50d, 100d, CurrentRow, Observer, Illuminant).ToRgba();
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
    }
}
