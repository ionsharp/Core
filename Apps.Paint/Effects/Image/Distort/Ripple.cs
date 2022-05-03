using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that superimposes rippling waves upon the input.</summary>
    [Category(ImageEffectCategory.Distort), DisplayName("Ripple")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class RippleEffect : ImageEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(RippleEffect), new FrameworkPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));
        /// <summary>The center point of the ripples.</summary>
        [Hidden(false)]
        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public static readonly DependencyProperty AmplitudeProperty = DependencyProperty.Register("Amplitude", typeof(double), typeof(RippleEffect), new FrameworkPropertyMetadata(((double)(0.1D)), PixelShaderConstantCallback(1)));
        /// <summary>The amplitude of the ripples.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Amplitude
        {
            get => (double)GetValue(AmplitudeProperty);
            set => SetValue(AmplitudeProperty, value);
        }

        public static readonly DependencyProperty FrequencyProperty = DependencyProperty.Register("Frequency", typeof(double), typeof(RippleEffect), new FrameworkPropertyMetadata(((double)(70D)), PixelShaderConstantCallback(2)));
        /// <summary>The frequency of the ripples.</summary>
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Frequency
        {
            get => (double)GetValue(FrequencyProperty);
            set => SetValue(FrequencyProperty, value);
        }

        public static readonly DependencyProperty PhaseProperty = DependencyProperty.Register("Phase", typeof(double), typeof(RippleEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        /// <summary>The phase of the ripples.</summary>
        [Hidden(false)]
        [Range(-20.0, 20.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Phase
        {
            get => (double)GetValue(PhaseProperty);
            set => SetValue(PhaseProperty, value);
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(RippleEffect), new FrameworkPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(4)));
        /// <summary>The aspect ratio (width / height) of the input.</summary>
        [Hidden(false)]
        [Range(0.5, 2.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double AspectRatio
        {
            get => (double)GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }

        public RippleEffect() : base()
        {
            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(AmplitudeProperty);
            UpdateShaderValue(FrequencyProperty);
            UpdateShaderValue(PhaseProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new RippleEffect()
            {
                Center = Center,
                Amplitude = Amplitude,
                Frequency = Frequency,
                Phase = Phase,
                AspectRatio = AspectRatio,
            };
        }
    }
}