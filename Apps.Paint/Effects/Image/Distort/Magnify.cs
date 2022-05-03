using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that magnifies a circular region with a smooth boundary.</summary>
    [Category(ImageEffectCategory.Distort), DisplayName("Magnify")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class MagnifyEffect : ImageEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(MagnifyEffect), new FrameworkPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));
        /// <summary>The center point of the magnified region.</summary>
        [Hidden(false)]
        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(MagnifyEffect), new FrameworkPropertyMetadata(((double)(0.2D)), PixelShaderConstantCallback(1)));
        /// <summary>The inner radius of the magnified region.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double InnerRadius
        {
            get => (double)GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register("OuterRadius", typeof(double), typeof(MagnifyEffect), new FrameworkPropertyMetadata(((double)(0.4D)), PixelShaderConstantCallback(2)));
        /// <summary>The outer radius of the magnified region.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double OuterRadius
        {
            get => (double)GetValue(OuterRadiusProperty);
            set => SetValue(OuterRadiusProperty, value);
        }

        public static readonly DependencyProperty MagnificationProperty = DependencyProperty.Register("Magnification", typeof(double), typeof(MagnifyEffect), new FrameworkPropertyMetadata(((double)(2D)), PixelShaderConstantCallback(3)));
        /// <summary>The magnification factor.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Magnification
        {
            get => (double)GetValue(MagnificationProperty);
            set => SetValue(MagnificationProperty, value);
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(MagnifyEffect), new FrameworkPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(4)));
        /// <summary>The aspect ratio (width / height) of the input.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double AspectRatio
        {
            get => (double)GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }

        public MagnifyEffect() : base()
        {
            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(InnerRadiusProperty);
            UpdateShaderValue(OuterRadiusProperty);
            UpdateShaderValue(MagnificationProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new MagnifyEffect()
            {
                Center = Center,
                InnerRadius = InnerRadius,
                OuterRadius = OuterRadius,
                Magnification = Magnification,
                AspectRatio = AspectRatio
            };
        }
    }
}
