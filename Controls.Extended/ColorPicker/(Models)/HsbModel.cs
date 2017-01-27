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
    public class HsbModel : ColorSpaceModel<Hsb>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Hsb GetValue()
        {
            return new Hsb(Components[typeof(HComponent)].Value, Components[typeof(SComponent)].Value, Components[typeof(BComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public HsbModel() : base()
        {
            Components.Add(new HComponent(this));
            Components.Add(new SComponent(this));
            Components.Add(new BComponent(this));
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
                return Hsb.MaxValue.H;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Hsb.MinValue.H;
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
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                return new Hsb(ComponentValue.ToDouble(), SelectionPoint.X.Divide(255d).Multiply(Hsb.MaxValue.S), 1d.Subtract(SelectionPoint.Y / 255d).Multiply(Hsb.MaxValue.B)).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Hsb(Color).H;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var hsb = new Hsb(Color);
                return PointFromColor(hsb.S.Divide(Hsb.MaxValue.S), hsb.B.Divide(Hsb.MaxValue.B));
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
                    return new Hsb(ComponentValue.ToDouble(), RowColumn.Column, RowColumn.Row).ToRgba();
                }), new RowColumn(Hsb.MaxValue.B, Hsb.MaxValue.S));
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
                    return new Hsb(CurrentRow, Hsb.MaxValue.S, Hsb.MaxValue.B).ToRgba();
                }));
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
                return Hsb.MaxValue.S;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Hsb.MinValue.S;
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
                return new Hsb(SelectionPoint.X.Divide(255).Multiply(Hsb.MaxValue.H), ComponentValue.ToDouble(), 1d.Subtract(SelectionPoint.Y / 255d).Multiply(Hsb.MaxValue.B)).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Hsb(Color).S;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var hsb = new Hsb(Color);
                return PointFromColor(hsb.H.Divide(Hsb.MaxValue.H), hsb.B.Divide(Hsb.MaxValue.B));
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
                    return new Hsb(RowColumn.Column, ComponentValue.ToDouble(), RowColumn.Row).ToRgba();
                }), new RowColumn(Hsb.MaxValue.B, Hsb.MaxValue.H));
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
                    return new Hsb(Color).New(Hsb.Component.S, CurrentRow).ToRgba();
                }));
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
        public sealed class BComponent : SelectableComponentModel
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
            /// <returns></returns>
            protected sealed override double GetMaximum()
            {
                return Hsb.MaxValue.B;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected sealed override double GetMinimum()
            {
                return Hsb.MinValue.B;
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
                return new Hsb(SelectionPoint.X.Divide(255).Multiply(Hsb.MaxValue.H), 1d.Subtract(SelectionPoint.Y / 255d).Multiply(Hsb.MaxValue.S), ComponentValue.ToDouble()).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Hsb(Color).B;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var hsb = new Hsb(Color);
                return PointFromColor(hsb.H.Divide(Hsb.MaxValue.H), hsb.S.Divide(Hsb.MaxValue.S));
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
                    return new Hsb(RowColumn.Column, RowColumn.Row, ComponentValue.ToDouble()).ToRgba();
                }), new RowColumn(Hsb.MaxValue.S, Hsb.MaxValue.H));
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
                    return new Hsb(Color).New(Hsb.Component.B, CurrentRow).ToRgba();
                }));
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
