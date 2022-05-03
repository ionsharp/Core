using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Threshold")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ThresholdEffect : ImageEffect
    {
        public static readonly DependencyProperty Color1Property = DependencyProperty.Register("Color1", typeof(Color), typeof(ThresholdEffect), new FrameworkPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        public Color Color1
        {
            get => (Color)GetValue(Color1Property);
            set => SetValue(Color1Property, value);
        }

        public static readonly DependencyProperty Color2Property = DependencyProperty.Register("Color2", typeof(Color), typeof(ThresholdEffect), new FrameworkPropertyMetadata(Color.FromArgb(255, 1, 1, 1), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        public Color Color2
        {
            get => (Color)GetValue(Color2Property);
            set => SetValue(Color2Property, value);
        }

        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level", typeof(double), typeof(ThresholdEffect), new FrameworkPropertyMetadata(100.0, PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(1.0, 255.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Level
        {
            get => (double)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        public ThresholdEffect() : base()
        {
            UpdateShaderValue(Color1Property);
            UpdateShaderValue(Color2Property);
            UpdateShaderValue(LevelProperty);
        }

        public sealed override Color Apply(Color color, double opacity = 1)
        {
            var brightness = color.Int32().GetBrightness();
            return brightness > Level.Double() / 255.0 ? Color1 : Color2;
        }

        public override ImageEffect Copy()
        {
            return new ThresholdEffect()
            {
                Color1 = Color1,
                Color2 = Color2,
                Level = Level
            };
        }
    }
}