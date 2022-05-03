using Imagin.Common.Numbers;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class ComponentSlider : BaseComponentSlider
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(One), typeof(ComponentSlider), new FrameworkPropertyMetadata((One)0));
        public One X
        {
            get => (One)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(One), typeof(ComponentSlider), new FrameworkPropertyMetadata((One)0));
        public One Y
        {
            get => (One)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(nameof(Z), typeof(One), typeof(ComponentSlider), new FrameworkPropertyMetadata(One.Zero, OnZChanged));
        public One Z
        {
            get => (One)GetValue(ZProperty);
            set => SetValue(ZProperty, value);
        }
        static void OnZChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ComponentSlider).OnZChanged(new Value<One>(e));

        public ComponentSlider() : base() { }

        protected override void Mark() => ArrowPosition.Y = ((1 - Z) * ActualHeight) - 8;

        protected override void OnMouseChanged(Vector2<One> input)
        {
            base.OnMouseChanged(input);
            SetCurrentValue(ZProperty, input.Y);
        }

        protected virtual void OnZChanged(Value<One> input)
        {
            Mark();
        }
    }
}