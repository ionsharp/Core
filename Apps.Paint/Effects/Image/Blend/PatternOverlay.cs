using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint.Effects
{

    [Category(ImageEffectCategory.Blend), DisplayName("Pattern overlay")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class PatternOverlayEffect : BlendImageEffect
    {
        public static readonly DependencyProperty PatternProperty = RegisterPixelShaderSamplerProperty(nameof(Pattern), typeof(PatternOverlayEffect), 1);
        [Hidden]
        public System.Windows.Media.Brush Pattern
        {
            get => (System.Windows.Media.Brush)GetValue(PatternProperty);
            set => SetValue(PatternProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), typeof(double), typeof(PatternOverlayEffect), new FrameworkPropertyMetadata(1d, PixelShaderConstantCallback(2)));
        [Format(Common.RangeFormat.Both)]
        [Range(0.0, 10.0, 0.01)]
        [Visible]
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public PatternOverlayEffect() : base()
        {
            UpdateShaderValue(PatternProperty);
            UpdateShaderValue(ScaleProperty);
            Pattern = new ImageBrush(new ImageSourceConverter().ConvertFromString(Resources.ProjectUri("Images/Pattern1.png").OriginalString).As<ImageSource>().Bitmap(ImageExtensions.Png).WriteableBitmap());
        }

        protected Matrix<Color> Render(WriteableBitmap input)
        {
            return default;
            /*
            Pattern = new(new ImageSourceConverter().ConvertFromString(Resources.ProjectUri("Images/Pattern1.png").OriginalString).As<ImageSource>().Bitmap(ImageExtensions.Png).WriteableBitmap().Resize((Pattern.Rows * Scale).Int32(), (Pattern.Columns * Scale).Int32(), Interpolations.NearestNeighbor));
            var newPattern = Pattern;

            //Top left of bounds = non transparent pixel with smallest x and y
            System.Drawing.Point? topLeft = null;
            //Bottom right of bounds = non transparent pixel with largest x and y
            System.Drawing.Point? bottomRight = null;

            var sx = int.MaxValue;
            var sy = int.MaxValue;

            var lx = int.MinValue;
            var ly = int.MinValue;

            input.ForEach((xa, ya, a) =>
            {
                if (a.A > 0)
                {
                    if (xa < sx)
                        sx = xa;

                    if (ya < sy)
                        sy = ya;

                    if (xa > lx)
                        lx = xa;

                    if (ya > ly)
                        ly = ya;

                }
                return a;
            });

            topLeft = new(sx, sy);
            bottomRight = new(lx, ly);

            var bounds = new Int32Region(topLeft.Value.X, topLeft.Value.Y, bottomRight.Value.X - topLeft.Value.X, bottomRight.Value.Y - topLeft.Value.Y);

            var overlay = new ColorMatrix((uint)bounds.Height, (uint)bounds.Width);
            for (uint x = 0; x < overlay.Columns; x++)
            {
                for (uint y = 0; y < overlay.Rows; y++)
                    overlay[y, x] = newPattern[y % newPattern.Rows, x % newPattern.Columns];
            }

            input.ForEach((xa, ya, a) =>
            {
                if (xa >= topLeft.Value.X && xa < topLeft.Value.X + bounds.Width)
                {
                    if (ya >= topLeft.Value.Y && ya < topLeft.Value.Y + bounds.Height)
                    {
                        if (a.A > 0)
                            return a.Blend(overlay[(uint)(ya - topLeft.Value.Y), (uint)(xa - topLeft.Value.X)], BlendMode, Opacity);
                    }
                }
                return a;
            });
            */
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy() => new PatternOverlayEffect();
    }
}