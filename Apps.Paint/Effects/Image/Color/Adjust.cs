using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>Pixel shader which produces random scratches, noise and other FX like an old projector</summary>
    [Category(ImageEffectCategory.Color), DisplayName("Adjust")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class AdjustEffect : ImageEffect
    {
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(nameof(Amount), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(1d, PixelShaderConstantCallback(9)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Amount
        {
            get => (double)GetValue(AmountProperty);
            set => SetValue(AmountProperty, value);
        }

        public static readonly DependencyProperty X0Property = DependencyProperty.Register(nameof(X0), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.393, PixelShaderConstantCallback(0)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double X0
        {
            get => (double)GetValue(X0Property);
            set => SetValue(X0Property, value);
        }

        public static readonly DependencyProperty Y0Property = DependencyProperty.Register(nameof(Y0), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.769, PixelShaderConstantCallback(1)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 0.95, 0.01)]
        [Visible]
        public double Y0
        {
            get => (double)GetValue(Y0Property);
            set => SetValue(Y0Property, value);
        }

        public static readonly DependencyProperty Z0Property = DependencyProperty.Register(nameof(Z0), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.189, PixelShaderConstantCallback(2)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 0.82, 0.01)]
        [Visible]
        public double Z0
        {
            get => (double)GetValue(Z0Property);
            set => SetValue(Z0Property, value);
        }

        public static readonly DependencyProperty X1Property = DependencyProperty.Register(nameof(X1), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.349, PixelShaderConstantCallback(3)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double X1
        {
            get => (double)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        public static readonly DependencyProperty Y1Property = DependencyProperty.Register(nameof(Y1), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.686, PixelShaderConstantCallback(4)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 0.95, 0.01)]
        [Visible]
        public double Y1
        {
            get => (double)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public static readonly DependencyProperty Z1Property = DependencyProperty.Register(nameof(Z1), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.168, PixelShaderConstantCallback(5)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 0.82, 0.01)]
        [Visible]
        public double Z1
        {
            get => (double)GetValue(Z1Property);
            set => SetValue(Z1Property, value);
        }

        public static readonly DependencyProperty X2Property = DependencyProperty.Register(nameof(X2), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.272, PixelShaderConstantCallback(6)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double X2
        {
            get => (double)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public static readonly DependencyProperty Y2Property = DependencyProperty.Register(nameof(Y2), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.534, PixelShaderConstantCallback(7)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 0.95, 0.01)]
        [Visible]
        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        public static readonly DependencyProperty Z2Property = DependencyProperty.Register(nameof(Z2), typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0.131, PixelShaderConstantCallback(8)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 0.82, 0.01)]
        [Visible]
        public double Z2
        {
            get => (double)GetValue(Z2Property);
            set => SetValue(Z2Property, value);
        }

        /*
        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register("Frame", typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(4)));
        /// <summary>The current frame.</summary>
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Frame
        {
            get => (double)GetValue(FrameProperty);
            set => SetValue(FrameProperty, value);
        }

        public static readonly DependencyProperty NoiseAmountProperty = DependencyProperty.Register("NoiseAmount", typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double NoiseAmount
        {
            get => (double)GetValue(NoiseAmountProperty);
            set => SetValue(NoiseAmountProperty, value);
        }

        public static readonly DependencyProperty NoiseSamplerProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("NoiseSampler", typeof(AdjustEffect), 1);
        [Visible]
        public System.Windows.Media.Brush NoiseSampler
        {
            get => (System.Windows.Media.Brush)GetValue(NoiseSamplerProperty);
            set => SetValue(NoiseSamplerProperty, value);
        }

        public static readonly DependencyProperty RandomCoord1Property = DependencyProperty.Register("RandomCoord1", typeof(Point), typeof(AdjustEffect), new FrameworkPropertyMetadata(new Point(0D, 0D), PixelShaderConstantCallback(2)));
        /// <summary>The random coordinate 1 that is used for lookup in the noise texture.</summary>
        [Visible]
        public Point RandomCoord1
        {
            get => (Point)GetValue(RandomCoord1Property);
            set => SetValue(RandomCoord1Property, value);
        }

        public static readonly DependencyProperty RandomCoord2Property = DependencyProperty.Register("RandomCoord2", typeof(Point), typeof(AdjustEffect), new FrameworkPropertyMetadata(new Point(0D, 0D), PixelShaderConstantCallback(3)));
        /// <summary>The random coordinate 2 that is used for lookup in the noise texture.</summary>
        [Visible]
        public Point RandomCoord2
        {
            get => (Point)GetValue(RandomCoord2Property);
            set => SetValue(RandomCoord2Property, value);
        }

        public static readonly DependencyProperty ScratchAmountProperty = DependencyProperty.Register("ScratchAmount", typeof(double), typeof(AdjustEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double ScratchAmount
        {
            get => (double)GetValue(ScratchAmountProperty);
            set => SetValue(ScratchAmountProperty, value);
        }
        */

        public AdjustEffect() : base()
        {
            UpdateShaderValue(AmountProperty);

            UpdateShaderValue(X0Property);
            UpdateShaderValue(Y0Property);
            UpdateShaderValue(Z0Property);

            UpdateShaderValue(X1Property);
            UpdateShaderValue(Y1Property);
            UpdateShaderValue(Z1Property);

            UpdateShaderValue(X2Property);
            UpdateShaderValue(Y2Property);
            UpdateShaderValue(Z2Property);
        }

        public override Color Apply(Color color, double opacity = 1)
        {
            double r = color.R, g = color.G, b = color.B;
            var nr = ((r * 0.393) + (g * 0.769) + (b * 0.189)).Coerce(255).Byte();
            var ng = ((r * 0.349) + (g * 0.686) + (b * 0.168)).Coerce(255).Byte();
            var nb = ((r * 0.272) + (g * 0.534) + (b * 0.131)).Coerce(255).Byte();
            return Color.FromArgb(color.A, nr, ng, nb);
        }

        public override ImageEffect Copy() => new AdjustEffect() { };
    }
}
