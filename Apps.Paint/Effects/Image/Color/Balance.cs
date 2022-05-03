using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Balance")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class BalanceEffect : ImageEffect
    {
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(BalanceEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(BalanceEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(BalanceEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        public static readonly DependencyProperty RangeProperty = DependencyProperty.Register("Range", typeof(double), typeof(BalanceEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        [Hidden(false)]
        public ColorRanges Range
        {
            get => (ColorRanges)(int)(double)GetValue(RangeProperty);
            set => SetValue(RangeProperty, (double)(int)value);
        }

        public BalanceEffect() : base()
        {
            UpdateShaderValue(RedProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(BlueProperty);
            UpdateShaderValue(RangeProperty);
        }

        public BalanceEffect(double red, double green, double blue) : this()
        {
            SetCurrentValue(RedProperty, red);
            SetCurrentValue(GreenProperty, green);
            SetCurrentValue(BlueProperty, blue);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            int b = color.B, g = color.G, r = color.R;

            var d = color.Int32();
            var l = d.GetBrightness();

            Color result(int r0, int g0, int b0) => Color.FromArgb(color.A, (r + r0).Coerce(255).Byte(), (g + g0).Coerce(255).Byte(), (b + b0).Coerce(255).Byte());

            //Highlight
            if (Range == ColorRanges.Highlights && l > 0.66)
                return result(Red.Int32(), Green.Int32(), Blue.Int32());

            //Midtone
            else if (Range == ColorRanges.Midtones && l > 0.33)
                return result(Red.Int32(), Green.Int32(), Blue.Int32());

            //Shadow
            else if (Range == ColorRanges.Shadows && l <= 0.33)
                return result(Red.Int32(), Green.Int32(), Blue.Int32());

            return result((Red.Double() * l).Round().Int32(), (Green.Double() * l).Round().Int32(), (Blue.Double() * l).Round().Int32());
        }

        public override ImageEffect Copy()
        {
            return new BalanceEffect()
            {
                Red = Red,
                Green = Green,
                Blue = Blue,
                Range = Range
            };
        }
    }
}