using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Tint")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class TintEffect : ImageEffect
    {
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(TintEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(TintEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(TintEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        public TintEffect() : base()
        {
            UpdateShaderValue(RedProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(BlueProperty);
        }

        public TintEffect(double red, double green, double blue) : this()
        {
            SetCurrentValue(RedProperty, red);
            SetCurrentValue(GreenProperty, green);
            SetCurrentValue(BlueProperty, blue);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            int b = color.B, g = color.G, r = color.R, a = color.A;
            return Color.FromArgb(a.Byte(), (r + (255 - r) * (Red / 100.0)).Round().Int32().Coerce(255).Byte(), (g + (255 - g) * (Green / 100.0)).Round().Int32().Coerce(255).Byte(), (b + (255 - b) * (Blue / 100.0)).Round().Int32().Coerce(255).Byte());
        }

        public override ImageEffect Copy()
        {
            return new TintEffect()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }
}