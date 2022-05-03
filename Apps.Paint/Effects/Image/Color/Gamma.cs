using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Gamma")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class GammaEffect : ImageEffect
    {
        int[] r = null, g = null, b = null;

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), typeof(double), typeof(GammaEffect), new FrameworkPropertyMetadata(1d, PixelShaderConstantCallback(1)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(GammaEffect), new FrameworkPropertyMetadata(1d, PixelShaderConstantCallback(0)));
        [Format(RangeFormat.Both)]
        [Range(0.2, 5.0, 0.01)]
        [Visible]
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set
            {
                SetValue(ValueProperty, value);
                Ramps(Value.Single(), out r, out g, out b);
            }
        }

        public GammaEffect() : base()
        {
            UpdateShaderValue(ScaleProperty);
            UpdateShaderValue(ValueProperty);
            Ramps(Value.Single(), out r, out g, out b);
        }

        public GammaEffect(double value) : this()
        {
            SetCurrentValue(ValueProperty, value);
        }

        static void Ramps(float input, out int[] r, out int[] g, out int[] b)
        {
            r = new int[256];
            g = new int[256];
            b = new int[256];
            for (int x = 0; x < 256; ++x)
            {
                r[x] = ((255.0 * Math.Pow(x / 255.0, 1.0 / input)) + 0.5).Round().Int32().Coerce(255);
                g[x] = ((255.0 * Math.Pow(x / 255.0, 1.0 / input)) + 0.5).Round().Int32().Coerce(255);
                b[x] = ((255.0 * Math.Pow(x / 255.0, 1.0 / input)) + 0.5).Round().Int32().Coerce(255);
            }
        }

        public override Color Apply(Color color, double opacity = 1)
            => Color.FromArgb(color.A, r[color.R].Byte(), g[color.G].Byte(), b[color.B].Byte());

        public override ImageEffect Copy()
        {
            return new GammaEffect()
            {
                Scale = Scale,
                Value = Value
            };
        }
    }
}