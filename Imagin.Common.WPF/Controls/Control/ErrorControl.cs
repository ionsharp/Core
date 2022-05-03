using Imagin.Common.Analytics;
using Imagin.Common.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class ErrorControl : Control
    {
        public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register(nameof(Error), typeof(Error), typeof(ErrorControl), new FrameworkPropertyMetadata(null, OnErrorChanged));
        [TypeConverter(typeof(ErrorTypeConverter))]
        public Error Error
        {
            get => (Error)GetValue(ErrorProperty);
            set => SetValue(ErrorProperty, value);
        }
        static void OnErrorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ErrorControl control)
            {
                if (e.NewValue is Error error)
                {
                    if (error.Inner != null)
                        control.InternalError = error.Inner;
                }
            }
        }

        static readonly DependencyPropertyKey InternalErrorKey = DependencyProperty.RegisterReadOnly(nameof(InternalError), typeof(Error), typeof(ErrorControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty InternalErrorProperty = InternalErrorKey.DependencyProperty;
        public Error InternalError
        {
            get => (Error)GetValue(InternalErrorProperty);
            private set => SetValue(InternalErrorKey, value);
        }

        public static readonly DependencyProperty WrapProperty = DependencyProperty.Register(nameof(Wrap), typeof(TextWrapping), typeof(ErrorControl), new FrameworkPropertyMetadata(TextWrapping.Wrap));
        public TextWrapping Wrap
        {
            get => (TextWrapping)GetValue(WrapProperty);
            set => SetValue(WrapProperty, value);
        }

        public ErrorControl() : base() { }
    }
}