using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common
{
    public class DependencyValue : DependencyObject
    {
        public event DefaultEventHandler<Value<object>> ValueChanged;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(DependencyValue), new FrameworkPropertyMetadata(null, OnValueChanged));
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.If<DependencyValue>(i => i.ValueChanged?.Invoke(i, new(e)));
    }
}