using System.Windows;

namespace Imagin.Common.Data
{
    public class DoubleReference : Freezable
    {
        public static readonly DependencyProperty FirstProperty = DependencyProperty.Register(nameof(First), typeof(object), typeof(DoubleReference), new FrameworkPropertyMetadata(null));
        public object First
        {
            get => GetValue(FirstProperty);
            set => SetValue(FirstProperty, value);
        }

        public static readonly DependencyProperty SecondProperty = DependencyProperty.Register(nameof(Second), typeof(object), typeof(DoubleReference), new FrameworkPropertyMetadata(null));
        public object Second
        {
            get => GetValue(SecondProperty);
            set => SetValue(SecondProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new DoubleReference();

        public DoubleReference() : base() { }
    }
}