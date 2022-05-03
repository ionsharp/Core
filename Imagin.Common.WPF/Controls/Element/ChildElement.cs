using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Specifies a single element.
    /// </summary>
    [ContentProperty(nameof(Child))]
    public class ChildElement : FrameworkElement
    {
        static readonly FrameworkPropertyMetadata ChildMetadata = new(null, FrameworkPropertyMetadataOptions.AffectsParentArrange, new PropertyChangedCallback(OnChildChanged));
        public static readonly DependencyProperty ChildProperty = DependencyProperty.RegisterAttached(nameof(Child), typeof(UIElement), typeof(ChildElement), ChildMetadata);
        public UIElement Child
        {
            get => (UIElement)GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }
        static void OnChildChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ChildElement view)
            {
                Visual oldChild = e.OldValue as Visual;
                if (oldChild != null)
                {
                    view.RemoveVisualChild(oldChild);
                    view.RemoveLogicalChild(oldChild);
                }

                Visual newChild = e.NewValue as Visual;
                if (newChild != null)
                {
                    view.AddVisualChild(newChild);
                    view.AddLogicalChild(newChild);

                    view.OnChildChanged(new(e));
                }
            }
        }

        public static readonly DependencyProperty ImageForegroundProperty = ImageElement.ForegroundProperty.AddOwner(typeof(ChildElement), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));
        public Brush ImageForeground
        {
            get => (Brush)GetValue(ImageForegroundProperty);
            set => SetValue(ImageForegroundProperty, value);
        }

        //...

        protected override int VisualChildrenCount => GetValue(ChildProperty) is not null ? 1 : 0;

        protected override Visual GetVisualChild(int index) => (UIElement)GetValue(ChildProperty);

        //...

        public ChildElement() : base() { }

        //...

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (GetValue(ChildProperty) is UIElement child)
                child.Arrange(new Rect(new Point(0, 0), finalSize));

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (GetValue(ChildProperty) is UIElement child)
            {
                child.Measure(availableSize);
                return child.DesiredSize;
            }
            return new Size(0, 0);
        }

        protected virtual void OnChildChanged(Value<Visual> input) { }
    }
}