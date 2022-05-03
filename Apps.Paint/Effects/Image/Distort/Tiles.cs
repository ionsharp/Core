using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect mimics the look of glass tiles.</summary>
    [Category(ImageEffectCategory.Distort), DisplayName("Tiles")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class TilesEffect : ImageEffect
    {
        public static readonly DependencyProperty TilesProperty = DependencyProperty.Register("Tiles", typeof(double), typeof(TilesEffect), new FrameworkPropertyMetadata(((double)(5D)), PixelShaderConstantCallback(0)));
        /// <summary>The approximate number of tiles per row/column.</summary>
        [Hidden(false)]
        [Range(0.0, 20.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Tiles
        {
            get => (double)GetValue(TilesProperty);
            set => SetValue(TilesProperty, value);
        }

        public static readonly DependencyProperty BevelWidthProperty = DependencyProperty.Register("BevelWidth", typeof(double), typeof(TilesEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(0.0, 10.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double BevelWidth
        {
            get => (double)GetValue(BevelWidthProperty);
            set => SetValue(BevelWidthProperty, value);
        }
        
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(double), typeof(TilesEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));
        [Hidden(false)]
        [Range(0.0, 3.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
        
        public static readonly DependencyProperty GroutColorProperty = DependencyProperty.Register("GroutColor", typeof(Color), typeof(TilesEffect), new FrameworkPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(2)));
        [Hidden(false)]
        public Color GroutColor
        {
            get => (Color)GetValue(GroutColorProperty);
            set => SetValue(GroutColorProperty, value);
        }

        public TilesEffect() : base()
        {
            UpdateShaderValue(TilesProperty);
            UpdateShaderValue(BevelWidthProperty);
            UpdateShaderValue(OffsetProperty);
            UpdateShaderValue(GroutColorProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new TilesEffect()
            {
                Tiles = Tiles,
                BevelWidth = BevelWidth,
                Offset = Offset,
                GroutColor = GroutColor
            };
        }
    }
}
