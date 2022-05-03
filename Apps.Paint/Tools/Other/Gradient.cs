using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [DisplayName("Gradient")]
    [Icon(Images.Gradient)]
    [Serializable]
    public class GradientTool : RulerTool
    {
        #region Properties

        [Hidden]
        public override double? Angle { get => base.Angle; set => base.Angle = value; }

        BlendModes mode = BlendModes.Normal;
        [Featured]
        public BlendModes Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        [Hidden]
        public CardinalDirection Direction
        {
            get
            {
                if (DifferenceX > DifferenceY)
                {
                    if (MouseMove.Value.X > MouseDown.Value.X)
                        return CardinalDirection.E;

                    return CardinalDirection.W;
                }
                if (DifferenceY > DifferenceX)
                {
                    if (MouseMove.Value.Y > MouseDown.Value.Y)
                        return CardinalDirection.S;

                    return CardinalDirection.N;
                }
                return CardinalDirection.S;
            }
        }

        [Hidden]
        public bool Reverse => Direction is CardinalDirection i && (i == CardinalDirection.N || i == CardinalDirection.W);

        [field: NonSerialized]
        Gradient gradient = Gradient.Default;
        [Index(0)]
        public Gradient Gradient
        {
            get => gradient;
            set => this.Change(ref gradient, value);
        }

        [Hidden]
        public override double? Height { get => base.Height; set => base.Height = value; }

        [Hidden]
        public override Uri Icon => Resources.InternalImage(Images.Gradient);

        [Hidden]
        public override double? Length { get => base.Length; set => base.Length = value; }

        double opacity = 1;
        [Range(0.0, 1.0, 0.01)]
        [Format(Common.RangeFormat.Both)]
        public double Opacity
        {
            get => opacity;
            set => this.Change(ref opacity, value);
        }

        bool reflect = false;
        public bool Reflect
        {
            get => reflect;
            set => this.Change(ref reflect, value);
        }

        public double Scale 
            => Angle != null && Math.Abs(Angle.Value) is double i && i >= 45 && i <= 135 
            ? Length.Value / Document.Height : Length.Value / Document.Width;

        [Index(100)]
        [Range(1.0, 90.0, 1.0)]
        [Format(Common.RangeFormat.Both)]
        public override double Snap { get => base.Snap; set => base.Snap = value; }

        GradientType type = GradientType.Linear;
        [Index(1)]
        public GradientType Type
        {
            get => type;
            set => this.Change(ref type, value);
        }

        [Hidden]
        public override double? Width { get => base.Width; set => base.Width = value; }

        [Hidden]
        public override double? X { get => base.X; set => base.X = value; }

        [Hidden]
        public override double? Y { get => base.Y; set => base.Y = value; }

        [Hidden]
        public override double X2 { get => base.X2; set => base.X2 = value; }

        [Hidden]
        public override double Y2 { get => base.Y2; set => base.Y2 = value; }

        #endregion

        #region Methods

        //...

        void Render(PixelLayer input)
        {
            if (gradient == null || gradient.Steps.Count == 0)
                return;

            Matrix<Color> result = default;
            Try.Invoke(() =>
            {
                var gradient = new Gradient(GetSteps(Gradient, reflect, Reverse));

                switch (Type)
                {
                    case GradientType.Angle:
                        result = RenderAngle(gradient, input.Size * Scale, Angle.Value);
                        break;

                    case GradientType.Circle:
                        result = RenderCircle(gradient, input.Size * Scale, Angle.Value);
                        break;

                    case GradientType.Diamond:
                        result = RenderDiamond(gradient, input.Size * Scale, Angle.Value);
                        break;

                    case GradientType.Linear:
                        result = RenderLinear(gradient, input.Size * Scale, Angle.Value);
                        break;
                }
            },
            e => Log.Write<GradientTool>(e));
            input.Pixels.Blend(Mode, result, 0, 0, Opacity, true);
        }

        //...

        protected override bool AssertLayer() => AssertLayer<PixelLayer>();

        public override void OnMouseUp(Point point)
        {
            if (TargetLayer is PixelLayer layer)
                Render(layer);

            X = Y = X2 = Y2 = 0;
            base.OnMouseUp(point);
        }

        //...

        //...

        static float FLerp(float norm, float min, float max)
            => (max - min) * norm + min;

        static void GetColors(Gradient input, double t, ref GradientStep a, ref GradientStep b)
        {
            var count = input.Steps.Count;
            for (var i = count - 1; i >= 0; i--)
            {
                if (i - 1 >= 0)
                {
                    if (t > input.Steps[i - 1].Offset && t <= input.Steps[i].Offset)
                    {
                        b = input.Steps[i - 1];
                        a = input.Steps[i];
                        break;
                    }
                }
            }
        }

        static float GetDistance(float x1, float y1, float x2, float y2)
            => (float)(Math.Sqrt(Math.Pow(Math.Abs(x1 - x2), 2) + Math.Pow(Math.Abs(y1 - y2), 2)));

        //...

        public static GradientStep[] GetSteps(Gradient gradient, bool reflect, bool reverse)
        {
            var result = new List<GradientStep>();
            gradient.Steps.ForEach(i => result.Add(new(i.Offset, i.Color)));

            if (reverse)
            {
                result.Reverse();
                for (var i = 0; i < result.Count; i++)
                    result[i].Offset = gradient.Steps[i].Offset;
            }

            if (reflect)
            {
                var oldRange = new DoubleRange(0, 1);
                foreach (var i in result)
                    i.Offset = oldRange.Convert(0, 0.5, i.Offset);

                for (var i = result.Count - 1; i >= 0; i--)
                {
                    var j = new GradientStep(1 - result[i].Offset, result[i].Color);
                    result.Add(j);
                }
            }
            return result.ToArray();
        }

        //...

        public static Matrix<Color> RenderAngle(Gradient input, Int32Size size, double angle)
        {
            int cx = size.Width / 2, cy = size.Height / 2;

            GradientStep a = null, b = null;

            var result = new Matrix<Color>((uint)size.Height, (uint)size.Width);
            for (var y = 0; y < size.Height; y++)
            {
                for (var x = 0; x < size.Width; x++)
                {
                    var r = 2 * Math.PI;

                    var t = Math.Atan2(y - cy, x - cx) + angle.GetRadian();
                    t = t + Math.PI;
                    if (t > r)
                        t = t - r;

                    t = t / r;

                    GetColors(input, t, ref a, ref b);
                    if (a == null || b == null)
                        continue;

                    var p = 1 - ((t - a.Offset) / (b.Offset - a.Offset));
                    result[(uint)y, (uint)x] = Color.Add(Color.Multiply(a.Color.Value, p.Single()), Color.Multiply(b.Color.Value, (1 - p).Single())).A(255);
                }
            }
            return result;
        }

        /// <remarks>To do: Observe angle.</remarks>
        public static Matrix<Color> RenderCircle(Gradient input, Int32Size size, double angle)
        {
            float cx = size.Width * 0.5f, cy = size.Height * 0.5f;

            GradientStep aStep = null, bStep = null;

            var result = new Matrix<Color>((uint)size.Height, (uint)size.Width);
            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    float md = GetDistance(cx, cy, size.Width, size.Height);
                    float nd = Math.Min(GetDistance(cx, cy, x, y) / md, 1);

                    var s = Convert.ToByte(FLerp(nd, 255, 0));
                    var t = s.Double() / 255d;

                    GetColors(input, t, ref aStep, ref bStep);
                    if (aStep == null || bStep == null)
                        continue;

                    var p = (t - aStep.Offset) / (bStep.Offset - aStep.Offset);
                    result[(uint)y, (uint)x] = aStep.Color.Value.Blend(bStep.Color.Value.A((p * 255d).Byte()));
                }
            }
            return result;
        }

        /// <remarks>To do: Observe angle.</remarks>
        public static Matrix<Color> RenderDiamond(Gradient input, Int32Size size, double angle)
        {
            int cx = size.Width / 2, cy = size.Height / 2;

            GradientStep aStep = null, bStep = null;

            int x0 = cx, y0 = cy;
            int h = 1, w = 1;

            Color Blend(double p)
                => aStep.Color.Value.Blend(bStep.Color.Value.A((p * 255d).Byte()));

            double P()
            {
                float dm = GetDistance(cx, cy, size.Width, size.Height);
                float dn = Math.Min(GetDistance(x0, y0, cx, cy) / dm, 1);

                var t = Convert.ToByte(FLerp(dn, 255, 0)).Double() / 255d;
                GetColors(input, t, ref aStep, ref bStep);
                if (aStep == null || bStep == null)
                    return 0;

                return (t - aStep.Offset) / (bStep.Offset - aStep.Offset);
            }

            var result = new Matrix<Color>((uint)size.Height, (uint)size.Width);

            //Top
            while (y0 >= 0)
            {
                if (y0 < 0) break;
                var p = P();

                var x1 = x0 + w;
                for (var x2 = x0; x2 < x1; x2++)
                {
                    if (x2 < 0 || x2 >= size.Width)
                        continue;

                    result[(uint)y0, (uint)x2] = Blend(p);
                }

                x0--; y0--;
                w += 2;

                if (y0 == 0)
                    break;
            }

            x0 = cx; y0 = cy;
            w = 1;

            //Bottom
            while (y0 <= size.Height - 1)
            {
                if (y0 >= size.Height) break;
                var p = P();

                var x1 = x0 + w;
                for (var x2 = x0; x2 < x1; x2++)
                {
                    if (x2 < 0 || x2 >= size.Width)
                        continue;

                    result[(uint)y0, (uint)x2] = Blend(p);
                }

                x0--; y0++;
                w += 2;

                if (y0 == size.Height - 1)
                    break;
            }

            x0 = cx; y0 = cy;

            //Right
            while (x0 <= size.Width - 1)
            {
                if (x0 >= size.Width) break;
                var p = P();

                var y1 = y0 + h;
                for (var y2 = y0; y2 < y1; y2++)
                {
                    if (y2 < 0 || y2 >= size.Height)
                        continue;

                    result[(uint)y2, (uint)x0] = Blend(p);
                }

                x0++; y0--;
                h += 2;

                if (x0 == size.Width - 1)
                    break;
            }

            x0 = cx; y0 = cy;
            h = 1;

            //Left
            while (x0 >= 0)
            {
                if (x0 < 0) break;
                var p = P();

                var y1 = y0 + h;
                for (var y2 = y0; y2 < y1; y2++)
                {
                    if (y2 < 0 || y2 >= size.Height)
                        continue;

                    result[(uint)y2, (uint)x0] = Blend(p);
                }

                x0--; y0--;
                h += 2;

                if (x0 == 0)
                    break;
            }

            return result;
        }

        /// <remarks>To do: Observe angle.</remarks>
        public static Matrix<Color> RenderLinear(Gradient input, Int32Size size, double angle)
        {
            GradientStep aStep = null, bStep = null;

            var result = new Matrix<Color>((uint)size.Height, (uint)size.Width);
            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    var t = x.Double() / size.Width.Double();

                    var count = input.Steps.Count;
                    for (var i = count - 1; i >= 0; i--)
                    {
                        if (i - 1 >= 0)
                        {
                            if (t > input.Steps[i - 1].Offset && t <= input.Steps[i].Offset)
                            {
                                aStep = input.Steps[i - 1];
                                bStep = input.Steps[i];
                                break;
                            }
                        }
                    }

                    if (aStep == null || bStep == null)
                        continue;

                    var q = bStep.Offset - aStep.Offset;
                    if (q == 0)
                        continue;

                    var p = (t - aStep.Offset) / q;

                    var color = aStep.Color.Value.Blend(bStep.Color.Value.A((p * 255d).Coerce(255).Byte()));
                    result[(uint)y, (uint)x] = color;
                }
            }
            return result;
        }

        //...

        ICommand exportCommand;
        [DisplayName("Export")]
        [Index(998)]
        public virtual ICommand ExportCommand => exportCommand ??= new RelayCommand(() => _ = Get.Current<Options>().Gradients.Export(), () => true);

        ICommand importCommand;
        [DisplayName("Import")]
        [Index(999)]
        public virtual ICommand ImportCommand => importCommand ??= new RelayCommand(() => Get.Current<Options>().Gradients.Import(), () => true);
        
        #endregion
    }
}