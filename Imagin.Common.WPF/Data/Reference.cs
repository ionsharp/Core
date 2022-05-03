using System.Windows;

namespace Imagin.Common.Data
{
    public class Reference : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(Reference), new FrameworkPropertyMetadata(null));
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new Reference();

        public Reference() : base() { }

        public Reference(object data) => SetCurrentValue(DataProperty, data);
    }
}