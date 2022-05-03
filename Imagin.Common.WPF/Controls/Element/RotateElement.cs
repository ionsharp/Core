using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Specifies a single element that rotates.
    /// </summary>
    public class RotateElement : ChildElement
    {
        RotateTransform transform => Child?.RenderTransform as RotateTransform;

        public static readonly DependencyProperty RotateProperty = DependencyProperty.Register(nameof(Rotate), typeof(bool), typeof(RotateElement), new FrameworkPropertyMetadata(false));
        public bool Rotate
        {
            get => (bool)GetValue(RotateProperty);
            set => SetValue(RotateProperty, value);
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof(Rotation), typeof(AngleRange), typeof(RotateElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        [TypeConverter(typeof(AngleRangeTypeConverter))]
        public AngleRange Rotation
        {
            get => (AngleRange)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        public static readonly DependencyProperty RotationScaleProperty = DependencyProperty.Register(nameof(RotationScale), typeof(double), typeof(RotateElement), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsRender));
        public double RotationScale
        {
            get => (double)GetValue(RotationScaleProperty);
            set => SetValue(RotationScaleProperty, value);
        }

        //...

        protected override int VisualChildrenCount => GetValue(ChildProperty) is not null ? 1 : 0;

        protected override Visual GetVisualChild(int index) => (UIElement)GetValue(ChildProperty);

        //...

        public RotateElement() : base() { }

        //...

        protected override void OnChildChanged(Value<Visual> input)
        {
            base.OnChildChanged(input);
            input.New.SetCurrentValue(RenderTransformProperty,
                new RotateTransform() { Angle = Rotation?.Minimum ?? 0 });
            input.New.SetCurrentValue(RenderTransformOriginProperty,
                new Point(0.5, 0.5));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            transform.If(i => i is not null && Rotation is not null, i => i.Angle = Rotation.Minimum + ((Rotation.Maximum - Rotation.Minimum) * RotationScale));
        }
    }
}