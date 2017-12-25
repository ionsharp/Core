using Imagin.Common;
using Imagin.Common.Drawing;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class LuvModel : ColorSpaceModel<Luv>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Luv GetValue()
        {
            return new Luv(Components[typeof(LComponent)].Value, Components[typeof(UComponent)].Value, Components[typeof(VComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public LuvModel() : base()
        {
            Components.Add(new LComponent(this));
            Components.Add(new UComponent(this));
            Components.Add(new VComponent(this));
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
                return Luv.MaxValue.L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Luv.MinValue.L;
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
                var L = ComponentValue.ToDouble();
                var U = (1d - SelectionPoint.Y / 255d) * Luv.Range.U;
                var V = (SelectionPoint.X / 255d) * Luv.Range.V;
                return new Luv(L, U, V, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Luv(Color, Observer, Illuminant).L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var luv = new Luv(Color);
                return PointFromColor(luv.U.Divide(Luv.MaxValue.U), luv.V.Divide(Luv.MaxValue.V));
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
                    return new Luv(ComponentValue.ToDouble(), RowColumn.Row.Add(Luv.MinValue.U), RowColumn.Column.Add(Luv.MinValue.V), Observer, Illuminant).ToRgba();
                }), new RowColumn(Luv.Range.U, Luv.Range.V));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Luv(Color, Observer, Illuminant).New(Luv.Component.L, CurrentRow).ToRgba();
                }));
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
        public sealed class UComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Luv.MaxValue.U;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Luv.MinValue.U;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "U";
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
                var l = (1d - SelectionPoint.Y / 255d) * Luv.MaxValue.L;
                var u = ComponentValue.ToDouble();
                var v = (SelectionPoint.X / 255.0) * Luv.MaxValue.V;
                return new Luv(l, u, v, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Luv(Color, Observer, Illuminant).U;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var luv = new Luv(Color);
                return PointFromColor(luv.L.Divide(Luv.MaxValue.L), luv.V.Divide(Luv.MaxValue.V));
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
                    return new Luv(RowColumn.Row.Add(Luv.MinValue.L), ComponentValue.ToDouble(), RowColumn.Column.Add(Luv.MinValue.V), Observer, Illuminant).ToRgba();
                }), new RowColumn(Luv.Range.L, Luv.Range.V));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Luv(Color, Observer, Illuminant).New(Luv.Component.U, CurrentRow).ToRgba();
                }));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public UComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class VComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Luv.MaxValue.V;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Luv.MinValue.V;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "V";
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
                var l = (1d - SelectionPoint.Y / 255d) * Luv.MaxValue.L;
                var u = (SelectionPoint.X / 255d) * Luv.MaxValue.U;
                var v = ComponentValue.ToDouble();
                return new Luv(l, u, v, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Luv(Color, Observer, Illuminant).V;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var luv = new Luv(Color);
                return PointFromColor(luv.U.Divide(Luv.MaxValue.U), luv.L.Divide(Luv.MaxValue.L));
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
                    return new Luv(RowColumn.Row.Add(Luv.MinValue.L), RowColumn.Column.Add(Luv.MinValue.U), ComponentValue.ToDouble(), Observer, Illuminant).ToRgba();
                }), new RowColumn(Luv.Range.L, Luv.Range.U));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Bitmap"></param>
            /// <param name="Color"></param>
            /// <param name="Action"></param>
            /// <param name="Reverse"></param>
            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    return new Luv(Color, Observer, Illuminant).New(Luv.Component.V, CurrentRow).ToRgba();
                }));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public VComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
            {
            }
        }
    }
}
