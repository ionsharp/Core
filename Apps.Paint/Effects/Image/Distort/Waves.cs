using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that applies a wave pattern to the input.</summary>
    [Category(ImageEffectCategory.Distort), DisplayName("Waves")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class WavesEffect : ImageEffect
    {
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(WavesEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        [Range(0.0, 2048.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Time
        {
            get => (double)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }
        
        public static readonly DependencyProperty WaveSizeProperty = DependencyProperty.Register("WaveSize", typeof(double), typeof(WavesEffect), new FrameworkPropertyMetadata(((double)(64D)), PixelShaderConstantCallback(1)));
        /// <summary>The distance between waves. (the higher the value the closer the waves are to their neighbor).</summary>
        [Hidden(false)]
        [Range(32.0, 256.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double WaveSize
        {
            get => (double)GetValue(WaveSizeProperty);
            set => SetValue(WaveSizeProperty, value);
        }

        public WavesEffect() : base()
        {
            UpdateShaderValue(TimeProperty);
            UpdateShaderValue(WaveSizeProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new WavesEffect()
            {
                Time = Time,
                WaveSize = WaveSize
            };
        }
    }
}
