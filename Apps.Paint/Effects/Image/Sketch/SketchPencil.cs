using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>A pencil stroke effect.</summary>
    [Category(ImageEffectCategory.Sketch), DisplayName("Sketch (pencil)")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class SketchPencilEffect : ImageEffect
    {
        public static readonly DependencyProperty BrushSizeProperty = DependencyProperty.Register("BrushSize", typeof(double), typeof(SketchPencilEffect), new FrameworkPropertyMetadata(((double)(0.005D)), PixelShaderConstantCallback(0)));
        /// <summary>The brush size of the sketch effect.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double BrushSize
        {
            get => (double)GetValue(BrushSizeProperty);
            set => SetValue(BrushSizeProperty, value);
        }

        public SketchPencilEffect() : base()
        {
            UpdateShaderValue(BrushSizeProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new SketchPencilEffect()
            {
                BrushSize = BrushSize
            };
        }
    }
}
