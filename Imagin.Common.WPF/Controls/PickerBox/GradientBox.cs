using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class GradientBox : PickerBox<Gradient, GradientWindow>
    {
        protected override Gradient DefaultValue => Gradient.Default;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Gradient), typeof(GradientBox), new FrameworkPropertyMetadata(null, OnValueChanged));
        public Gradient Value
        {
            get => (Gradient)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<GradientBox>().OnValueChanged(e);

        public GradientBox() : base() { }

        protected override Gradient GetValue() => Value?.Clone() ?? DefaultValue;

        protected override void SetValue(Gradient i) => Value.CopyFrom(i);
    }
}