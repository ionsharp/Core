using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public abstract class ParseBox<T> : TextBox
    {
        public static readonly DependencyProperty<T, ParseBox<T>> ValueProperty = new(nameof(Value), new FrameworkPropertyMetadata(default(T), OnValueChanged, OnValueCoerced));
        public T Value
        {
            get => ValueProperty.Get(this);
            set => ValueProperty.Set(this, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ParseBox<T>>().OnValueChanged(new Value<T>(e));
        static object OnValueCoerced(DependencyObject i, object value) => i.As<ParseBox<T>>().OnValueCoerced(value);

        public ParseBox() : base() 
        {
            var converter = new SimpleConverter<T, string>(i => ToString(i), i => GetValue(i));
            this.Bind(TextProperty, new PropertyPath("(0)", ValueProperty.Property), this, BindingMode.TwoWay, converter);
        }

        protected abstract T GetValue(string value);

        protected abstract string ToString(T Value);

        protected virtual void OnValueChanged(Value<T> input) { }

        protected virtual object OnValueCoerced(object input) => input;
    }
}