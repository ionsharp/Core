using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Sketch), DisplayName("Oil painting")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class OilPaintingEffect : ImageEffect
    {
        public static readonly DependencyProperty LevelsProperty = DependencyProperty.Register(nameof(Levels), typeof(double), typeof(OilPaintingEffect), new FrameworkPropertyMetadata(1d, PixelShaderConstantCallback(0)));
        [Format(RangeFormat.Both)]
        [Range(1d, 100d, 1d)]
        [Visible]
        public double Levels
        {
            get => (double)GetValue(LevelsProperty);
            set => SetValue(LevelsProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(double), typeof(OilPaintingEffect), new FrameworkPropertyMetadata(3d, PixelShaderConstantCallback(1)));
        [Format(RangeFormat.Both)]
        [Range(3d, 27d, 2d)]
        [Visible]
        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public OilPaintingEffect() : base()
        {
            UpdateShaderValue(LevelsProperty);
            UpdateShaderValue(SizeProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public Color Apply(ColorMatrix colors, int x, int y, Color color)
        {
            int filterOffset = ((int)Size - 1) / 2;

            int currentIntensity = 0;
            int maxIndex = 0, maxIntensity = 0;

            double red = 0, green = 0, blue = 0;

            int[] intensityBin
                = new int[(int)Levels];
            int[] blueBin
                = new int[(int)Levels];
            int[] greenBin
                = new int[(int)Levels];
            int[] redBin
                = new int[(int)Levels];

            for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
            {
                for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                {
                    var x2 = x + filterX;
                    var y2 = y + filterY;

                    if (x2 >= 0 && x2 < colors.Columns && y2 >= 0 && y2 < colors.Rows)
                    {
                        var c = colors.GetValue(y2.UInt32(), x2.UInt32());
                        currentIntensity = (int)((((c.R.Double() + c.G.Double() + c.B.Double()) / 3.0) * Levels) / 255.0);
                        intensityBin[currentIntensity]++;

                        redBin[currentIntensity] += c.R;
                        greenBin[currentIntensity] += c.G;
                        blueBin[currentIntensity] += c.B;

                        if (intensityBin[currentIntensity] > maxIntensity)
                        {
                            maxIntensity = intensityBin[currentIntensity];
                            maxIndex = currentIntensity;
                        }
                    }
                }
            }

            var mid = maxIntensity.Double();
            red = redBin[maxIndex].Double() / mid;
            green = greenBin[maxIndex].Double() / mid;
            blue = blueBin[maxIndex].Double() / mid;

            return Color.FromArgb(color.A, red.Coerce(255).Byte(), green.Coerce(255).Byte(), blue.Coerce(255).Byte());
        }

        public override ImageEffect Copy() => new OilPaintingEffect()
        {
            Levels = Levels,
            Size = Size
        };
    }
}