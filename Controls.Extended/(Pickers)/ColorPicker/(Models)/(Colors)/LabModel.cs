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
    public class LabModel : ColorSpaceModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Color GetColor()
        {
            return Lab.ToColor(Components[typeof(LComponent)].Value, Components[typeof(AComponent)].Value, Components[typeof(BComponent)].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public LabModel() : base()
        {
            Components.Add(new LComponent());
            Components.Add(new AComponent());
            Components.Add(new BComponent());
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LComponent : NormalComponentModel
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
            public override string UnitLabel
            {
                get
                {
                    return "%";
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = ComponentValue.ToDouble();
                double b = 127.0 - SelectionPoint.Y;
                double a = SelectionPoint.X - 128.0;
                return Lab.ToColor(l, a, b);
            }

            public override int GetValue(Color Color)
            {
                return Lab.FromColor(Color).L.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                int x = 128 + Lab.A.ToInt32();
                int y = 128 - Lab.B.ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double a = Lab.FromColor(Color).A, b = Lab.FromColor(Color).B;
                    return Lab.ToRgba(CurrentRow, a, b);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lab.ToRgba(ComponentValue.ToDouble(), RowColumn.Column - 128.0, RowColumn.Row - 128.0);
                }), new RowColumn(256.0, 256.0));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AComponent : NormalComponentModel
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
            public override string UnitLabel
            {
                get
                {
                    return "";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override int Minimum
            {
                get
                {
                    return -128;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override int Maximum
            {
                get
                {
                    return 127;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = 100.0 - SelectionPoint.Y * 100.0 / 256.0;
                double a = ComponentValue.ToDouble();
                double b = SelectionPoint.X - 128.0;
                return Lab.ToColor(l, a, b);
            }

            public override int GetValue(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                return Lab.A.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                int x = 128 + Lab.B.ToInt32();
                int y = 100 - Lab.L.ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Lab.FromColor(Color).L, b = Lab.FromColor(Color).B;
                    return Lab.ToRgba(l, CurrentRow, b);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lab.ToRgba(RowColumn.Row, ComponentValue.ToDouble(), RowColumn.Column - 128.0);
                }), new RowColumn(100.0, 256.0));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class BComponent : NormalComponentModel
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
            public override string UnitLabel
            {
                get
                {
                    return "";
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override int Minimum
            {
                get
                {
                    return -128;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override int Maximum
            {
                get
                {
                    return 127;
                }
            }

            public override Color ColorAtPoint(Point SelectionPoint, int ComponentValue)
            {
                double l = 100.0 - SelectionPoint.Y * 100.0 / 256.0;
                double a = SelectionPoint.X - 128.0;
                double b = ComponentValue.ToDouble();
                return Lab.ToColor(l, a, b);
            }

            public override int GetValue(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                return Lab.B.Round().ToInt32();
            }

            public override Point PointFromColor(Color Color)
            {
                Lab Lab = Lab.FromColor(Color);
                int x = 128 + Lab.A.ToInt32();
                int y = 100 - Lab.L.ToInt32();
                return new Point(x, y);
            }

            public override void UpdateSlider(WriteableBitmap Bitmap, Color Color, Func<Color, double, Rgba> Action = null, bool Reverse = false)
            {
                base.UpdateSlider(Bitmap, Color, new Func<Color, double, Rgba>((c, CurrentRow) =>
                {
                    double l = Lab.FromColor(Color).L, a = Lab.FromColor(Color).A;
                    return Lab.ToRgba(l, a, CurrentRow);
                }));
            }

            public override void UpdatePlane(WriteableBitmap Bitmap, int ComponentValue, Func<RowColumn, int, Rgba> Action = null, RowColumn? Unit = null)
            {
                base.UpdatePlane(Bitmap, ComponentValue, new Func<RowColumn, int, Rgba>((RowColumn, Value) =>
                {
                    return Lab.ToRgba(RowColumn.Row, RowColumn.Column - 128.0, ComponentValue.ToDouble());
                }), new RowColumn(100.0, 256.0));
            }
        }
    }
}
