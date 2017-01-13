using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    public abstract class NumericUpDown<T> : UpDown<T>
    {
        /// <summary>
        /// The default value to increment by.
        /// </summary>
        public abstract T DefaultIncrement
        {
            get;
        }

        public abstract Regex Expression
        {
            get;
        }

        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(T), typeof(NumericUpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIncrementChanged));
        public T Increment
        {
            get
            {
                return (T)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }
        static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<NumericUpDown<T>>().OnIncrementChanged((T)e.NewValue);
        }

        public NumericUpDown() : base()
        {
            Increment = DefaultIncrement;
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            OnPreviewTextInput(e, Expression);
        }

        protected virtual void OnIncrementChanged(T Value)
        {
        }

        protected virtual void OnPreviewTextInput(TextCompositionEventArgs e, Regex Expression)
        {
            e.Handled = CaretIndex == 0 && e.Text == "-" ? Text.Contains("-") : !Expression.IsMatch(e.Text);
        }
    }
}
