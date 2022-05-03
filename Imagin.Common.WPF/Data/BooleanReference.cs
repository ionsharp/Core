using System.Windows;

namespace Imagin.Common.Data
{
    public class BooleanReference : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(bool), typeof(BooleanReference), new FrameworkPropertyMetadata(false));
        public bool Data
        {
            get => (bool)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new Reference();

        public BooleanReference() : base() { }

        public BooleanReference(bool data) => SetCurrentValue(DataProperty, data);
    }
}