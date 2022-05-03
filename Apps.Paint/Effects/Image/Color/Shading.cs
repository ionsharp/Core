using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Shading")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ShadingEffect : ImageEffect
    {
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(ShadingEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(ShadingEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(ShadingEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        public ShadingEffect() : base()
        {
            UpdateShaderValue(RedProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(BlueProperty);
        }

        public ShadingEffect(double red, double green, double blue) : this()
        {
            SetCurrentValue(RedProperty, red);
            SetCurrentValue(GreenProperty, green);
            SetCurrentValue(BlueProperty, blue);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            var r = (color.R.Double() * (Red.Double() / 100.0)).Round().Int32().Coerce(255);
            var g = (color.G.Double() * (Green.Double() / 100.0)).Round().Int32().Coerce(255);
            var b = (color.B.Double() * (Blue.Double() / 100.0)).Round().Int32().Coerce(255);
            return Color.FromArgb(color.A, r.Byte(), g.Byte(), b.Byte());
        }

        public override ImageEffect Copy()
        {
            return new ShadingEffect()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }
}