using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An paper sketch effect.</summary>
    [Category(ImageEffectCategory.Sketch), DisplayName("Sketch (granite)")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class SketchGraniteEffect : ImageEffect
    {
        public static readonly DependencyProperty BrushSizeProperty = DependencyProperty.Register("BrushSize", typeof(double), typeof(SketchGraniteEffect), new FrameworkPropertyMetadata(((double)(0.003D)), PixelShaderConstantCallback(0)));
        /// <summary>The brush size of the sketch effect.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double BrushSize
        {
            get => (double)GetValue(BrushSizeProperty);
            set => SetValue(BrushSizeProperty, value);
        }

        public SketchGraniteEffect() : base()
        {
            UpdateShaderValue(BrushSizeProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new SketchGraniteEffect()
            {
                BrushSize = BrushSize
            };
        }
    }
}
