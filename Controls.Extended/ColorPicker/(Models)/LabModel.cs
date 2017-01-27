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
    public class LabModel : ColorSpaceModel<Lab>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Lab GetValue()
        {
            return new Lab(Components[typeof(LComponent)].Value, Components[typeof(AComponent)].Value, Components[typeof(BComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public LabModel() : base()
        {
            Components.Add(new LComponent(this));
            Components.Add(new AComponent(this));
            Components.Add(new BComponent(this));
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
            protected override double GetMaximum()
            {
                return Lab.MaxValue.L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Lab.MinValue.L;
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
                double l = ComponentValue.ToDouble();
                double a = SelectionPoint.X - 128d;
                double b = 127d - SelectionPoint.Y;
                return new Lab(l, a, b, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Lab(Color, Observer, Illuminant).L;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var lab = new Lab(Color, Observer, Illuminant);
                int x = 128 + lab.A.ToInt32();
                int y = 128 - lab.B.ToInt32();
                return new Point(x, y);
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
                    return new Lab(ComponentValue.ToDouble(), RowColumn.Column - 128.0, RowColumn.Row - 128.0, Observer, Illuminant).ToRgba();
                }), new RowColumn(Lab.Range.A, Lab.Range.B));
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
                    return new Lab(Color, Observer, Illuminant).New(Lab.Component.L, CurrentRow).ToRgba();
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
        public sealed class AComponent : SelectableComponentModel
        {
            /// <summary>
            /// 
            /// </summary>
            public override string ComponentLabel
            {
                get
                {
                    return "A";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMaximum()
            {
                return Lab.MaxValue.A;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Lab.MinValue.A;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = 100.0 - SelectionPoint.Y * 100.0 / 256.0;
                double a = ComponentValue.ToDouble();
                double b = SelectionPoint.X - 128.0;
                return new Lab(l, a, b, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Lab(Color, Observer, Illuminant).A;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var lab = new Lab(Color, Observer, Illuminant);
                int x = 128 + lab.B.ToInt32();
                int y = 100 - lab.L.ToInt32();
                return new Point(x, y);
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
                    return new Lab(RowColumn.Row, ComponentValue.ToDouble(), RowColumn.Column - 128.0, Observer, Illuminant).ToRgba();
                }), new RowColumn(Lab.Range.L, Lab.Range.B));
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
                    return new Lab(Color, Observer, Illuminant).New(Lab.Component.A, CurrentRow).ToRgba();
                }));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ColorSpace"></param>
            public AComponent(ColorSpaceModel ColorSpace) : base(ColorSpace)
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
            protected override double GetMaximum()
            {
                return Lab.MaxValue.B;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected override double GetMinimum()
            {
                return Lab.MinValue.B;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="SelectionPoint"></param>
            /// <param name="ComponentValue"></param>
            /// <returns></returns>
            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = 100.0 - SelectionPoint.Y * 100.0 / 256.0;
                double a = SelectionPoint.X - 128.0;
                double b = ComponentValue.ToDouble();
                return new Lab(l, a, b, Observer, Illuminant).Color;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override double GetValue(Color Color)
            {
                return new Lab(Color, Observer, Illuminant).B;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Color"></param>
            /// <returns></returns>
            public override Point PointFromColor(Color Color)
            {
                var lab = new Lab(Color, Observer, Illuminant);
                int x = 128 + lab.A.ToInt32();
                int y = 100 - lab.L.ToInt32();
                return new Point(x, y);
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
                    return new Lab(RowColumn.Row, RowColumn.Column - 128.0, ComponentValue.ToDouble(), Observer, Illuminant).ToRgba();
                }), new RowColumn(Lab.Range.L, Lab.Range.A));
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
                    return new Lab(Color, Observer, Illuminant).New(Lab.Component.B, CurrentRow).ToRgba();
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
