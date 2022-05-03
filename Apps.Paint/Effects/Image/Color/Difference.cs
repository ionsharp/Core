using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Difference")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class DifferenceEffect : ImageEffect
    {
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register("Red", typeof(double), typeof(DifferenceEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(0.0, 255.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register("Green", typeof(double), typeof(DifferenceEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(0.0, 255.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register("Blue", typeof(double), typeof(DifferenceEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(0.0, 255.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        public DifferenceEffect() : base()
        {
            UpdateShaderValue(RedProperty);
            UpdateShaderValue(GreenProperty);
            UpdateShaderValue(BlueProperty);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            int ob = color.B, og = color.G, or = color.R;
            int nr = or, ng = og, nb = ob;

            nr = Red > or
                ? Red.Int32() - or
                : or - Red.Int32();
            ng = Green > og
                ? Green.Int32() - og
                : og - Green.Int32();
            nb = Blue > ob
                ? Blue.Int32() - ob
                : ob - Blue.Int32();

            return Color.FromArgb(color.A, nr.Coerce(255).Byte(), ng.Coerce(255).Byte(), nb.Coerce(255).Byte());
        }

        public override ImageEffect Copy()
        {
            return new DifferenceEffect()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }
}