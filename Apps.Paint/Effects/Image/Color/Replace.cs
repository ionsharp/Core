using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that makes pixels of a particular color another color.</summary>
    [Category(ImageEffectCategory.Color), DisplayName("Replace")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ReplaceEffect : ImageEffect
    {
        public static readonly DependencyProperty Color1Property = DependencyProperty.Register("Color1", typeof(Color), typeof(ReplaceEffect), new FrameworkPropertyMetadata(Color.FromArgb(255, 0, 128, 0), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        public Color Color1
        {
            get => (Color)GetValue(Color1Property);
            set => SetValue(Color1Property, value);
        }

        public static readonly DependencyProperty Color2Property = DependencyProperty.Register("Color2", typeof(Color), typeof(ReplaceEffect), new FrameworkPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        public Color Color2
        {
            get => (Color)GetValue(Color2Property);
            set => SetValue(Color2Property, value);
        }

        public static readonly DependencyProperty ToleranceProperty = DependencyProperty.Register("Tolerance", typeof(double), typeof(ReplaceEffect), new FrameworkPropertyMetadata(((double)(0.9D)), PixelShaderConstantCallback(2)));
        /// <summary>The tolerance in color differences.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Tolerance
        {
            get => (double)GetValue(ToleranceProperty);
            set => SetValue(ToleranceProperty, value);
        }

        public ReplaceEffect() : base()
        {
            UpdateShaderValue
                (Color1Property);
            UpdateShaderValue
                (Color2Property);
            UpdateShaderValue
                (ToleranceProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color == Color1 ? Color2 : color;

        public override ImageEffect Copy()
        {
            return new ReplaceEffect()
            {
                Color1
                    = Color1,
                Color2 
                    = Color2,
                Tolerance 
                    = Tolerance
            };
        }
    }
}