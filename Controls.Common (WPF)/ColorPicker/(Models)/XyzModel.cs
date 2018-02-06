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
    public class XyzModel : ColorSpaceModel<Xyz>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Xyz GetValue()
        {
            return new Xyz(Components[typeof(XComponent)].Value, Components[typeof(YComponent)].Value, Components[typeof(ZComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public XyzModel() : base()
        {
            Components.Add(new XComponent(this));
            Components.Add(new YComponent(this));
            Components.Add(new ZComponent(this));
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class XComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Xyz.Max[Xyz.Component.X, Observer, Illuminant].Shift(2);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Xyz.MinValue.X;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "X";
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
                var x = ComponentValue / Xyz.Max[Xyz.Component.X, Observer, Illuminant].Shift(2);
                var y = SelectionPoint.X / 255.0;
                var z = 1.0 - (SelectionPoint.Y / 255.0);
                return new Xyz(x, y, z, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Xyz(Color);
                return new Xyz(NewComponentValue.ToDouble(), c.Y, c.Z).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Xyz(Color, Observer, Illuminant).X.Multiply(Xyz.Max[Xyz.Component.X, Observer, Illuminant].Shift(2));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var xyz = new Xyz(Color, Observer, Illuminant);
                return PointFromColor(xyz.Y.Divide(Xyz.Max[Xyz.Component.Y, Observer, Illuminant]), xyz.Z.Divide(Xyz.Max[Xyz.Component.Z, Observer, Illuminant]));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "X";
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
                    return new Xyz(ComponentValue.ToDouble() / Xyz.Max[Xyz.Component.X, Observer, Illuminant].Shift(2), RowColumn.Column, RowColumn.Row, Observer, Illuminant).ToRgba();
                }), new RowColumn(Xyz.Max[Xyz.Component.Z, Observer, Illuminant], Xyz.Max[Xyz.Component.Y, Observer, Illuminant]));
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
                    return new Xyz(Color, Observer, Illuminant).New(Xyz.Component.X, CurrentRow.Shift(-2)).ToRgba();
                }), Reverse, Orientation);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public XComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class YComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Xyz.Max[Xyz.Component.Y, Observer, Illuminant].Shift(2);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Xyz.MinValue.Y;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "Y";
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
                double x = SelectionPoint.X / 255.0;
                double y = ComponentValue.ToDouble() / Xyz.Max[Xyz.Component.Y, Observer, Illuminant].Shift(2);
                double z = 1.0 - (SelectionPoint.Y / 255.0);
                return new Xyz(x, y, z, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Xyz(Color);
                return new Xyz(c.X, NewComponentValue.ToDouble(), c.Z).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Xyz(Color, Observer, Illuminant).Y.Multiply(Xyz.Max[Xyz.Component.Y, Observer, Illuminant].Shift(2));
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var xyz = new Xyz(Color, Observer, Illuminant);
                return PointFromColor(xyz.X.Divide(Xyz.Max[Xyz.Component.X, Observer, Illuminant]), xyz.Z.Divide(Xyz.Max[Xyz.Component.Z, Observer, Illuminant]));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Y";
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
                    return new Xyz(RowColumn.Column, ComponentValue.ToDouble() / Xyz.Max[Xyz.Component.Y, Observer, Illuminant].Shift(2), RowColumn.Row, Observer, Illuminant).ToRgba();
                }), new RowColumn(Xyz.Max[Xyz.Component.Z, Observer, Illuminant], Xyz.Max[Xyz.Component.X, Observer, Illuminant]));
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
                    return new Xyz(Color, Observer, Illuminant).New(Xyz.Component.Y, CurrentRow.Shift(-2)).ToRgba();
                }), Reverse, Orientation);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public YComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class ZComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Xyz.Max[Xyz.Component.Z, Observer, Illuminant].Shift(2);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Xyz.MinValue.Z;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "Z";
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
                double x = SelectionPoint.X / 255.0;
                double y = 1.0 - (SelectionPoint.Y / 255.0);
                double z = ComponentValue.ToDouble() / Xyz.Max[Xyz.Component.Z, Observer, Illuminant].Shift(2);
                return new Xyz(x, y, z, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <param name="NewComponentValue"></param>
            /// <returns></returns>
            public override Color ColorFrom(Color Color, int NewComponentValue)
            {
                var c = new Xyz(Color);
                return new Xyz(c.X, c.Y, NewComponentValue.ToDouble()).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Xyz(Color, Observer, Illuminant).Z.Multiply(Xyz.Max[Xyz.Component.Z, Observer, Illuminant].Shift(2));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var xyz = new Xyz(Color, Observer, Illuminant);
                return PointFromColor(xyz.X.Divide(Xyz.Max[Xyz.Component.X, Observer, Illuminant]), xyz.Y.Divide(Xyz.Max[Xyz.Component.Y, Observer, Illuminant]));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Z";
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
                    return new Xyz(RowColumn.Column, RowColumn.Row, ComponentValue.ToDouble() / Xyz.Max[Xyz.Component.Z, Observer, Illuminant].Shift(2), Observer, Illuminant).ToRgba();
                }), new RowColumn(Xyz.Max[Xyz.Component.Y, Observer, Illuminant], Xyz.Max[Xyz.Component.X, Observer, Illuminant]));
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
                    return new Xyz(Color, Observer, Illuminant).New(Xyz.Component.Z, CurrentRow.Shift(-2)).ToRgba();
                }), Reverse, Orientation);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public ZComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }
    }
}
