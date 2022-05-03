using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that turns the input into blocky pixels.</summary>
    [Category(ImageEffectCategory.Distort), DisplayName("Pixelate")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class PixelateEffect : ImageEffect
    {
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(nameof(Offset), typeof(double), typeof(PixelateEffect), new FrameworkPropertyMetadata(0d, PixelShaderConstantCallback(0)));
        /// <summary>The amount to shift alternate rows (use 1 to get a brick wall look).</summary>
        [Format(RangeFormat.Both)]
        [Range(0.0, 1.0, 0.01)]
        [Visible]
        public double Offset
        {
            get => (double)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(Size), typeof(PixelateEffect), new FrameworkPropertyMetadata(new Size(60d, 40d), PixelShaderConstantCallback(1)));
        /// <summary>The number of horizontal and vertical pixel blocks.</summary>
        [Hidden]
        public Size Size
        {
            get => (Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty PixelHeightProperty = DependencyProperty.Register(nameof(PixelHeight), typeof(double), typeof(PixelateEffect), new FrameworkPropertyMetadata(60d, OnSizeChanged));
        /// <summary>The number of horizontal and vertical pixel blocks.</summary>
        [DisplayName("Height"), Visible]
        [Format(RangeFormat.Both)]
        [Range(1.0, 1024.0, 1.0)]
        public double PixelHeight
        {
            get => (double)GetValue(PixelHeightProperty);
            set => SetValue(PixelHeightProperty, value);
        }

        public static readonly DependencyProperty PixelWidthProperty = DependencyProperty.Register(nameof(PixelWidth), typeof(double), typeof(PixelateEffect), new FrameworkPropertyMetadata(40d, OnSizeChanged));
        /// <summary>The number of horizontal and vertical pixel blocks.</summary>
        [DisplayName("Width"), Visible]
        [Format(RangeFormat.Both)]
        [Range(1.0, 1024.0, 1.0)]
        public double PixelWidth
        {
            get => (double)GetValue(PixelWidthProperty);
            set => SetValue(PixelWidthProperty, value);
        }

        static void OnSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.If<PixelateEffect>(i => i.Size = new(i.PixelWidth, i.PixelHeight));
        
        public PixelateEffect() : base()
        {
            UpdateShaderValue(OffsetProperty);
            UpdateShaderValue(SizeProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new PixelateEffect()
            {
                Offset = Offset,
                Size = Size
            };
        }
    }
}
