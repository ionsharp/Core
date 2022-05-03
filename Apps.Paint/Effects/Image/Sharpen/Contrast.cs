using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Sharpen), DisplayName("Contrast")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ContrastEffect : ImageEffect
    {
        public static readonly DependencyProperty ContrastProperty = DependencyProperty.Register(nameof(Contrast), typeof(double), typeof(ContrastEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(-128.0, 128.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Contrast
        {
            get => (double)GetValue(ContrastProperty);
            set => SetValue(ContrastProperty, value);
        }

        public ContrastEffect() : base()
        {
            UpdateShaderValue(ContrastProperty);
        }

        public ContrastEffect(double contrast) : this()
        {
            SetCurrentValue(ContrastProperty, contrast);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            int nr = color.R, ng = color.G, nb = color.B;
            nr = ((Contrast * (nr - 128)) + 128).Coerce(255).Round().Int32();
            ng = ((Contrast * (ng - 128)) + 128).Coerce(255).Round().Int32();
            nb = ((Contrast * (nb - 128)) + 128).Coerce(255).Round().Int32();

            return Color.FromArgb(color.A, nr.Coerce(255).Byte(), ng.Coerce(255).Byte(), nb.Coerce(255).Byte());
        }

        public override ImageEffect Copy() => new ContrastEffect() { Contrast = Contrast };
    }
}