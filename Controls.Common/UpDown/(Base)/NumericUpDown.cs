using Imagin.Common.Extensions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NumericUpDown<T> : UpDown<T>
    {
        /// <summary>
        /// The default value to increment by.
        /// </summary>
        public abstract T DefaultIncrement
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract Regex Expression
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(T), typeof(NumericUpDown<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIncrementChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public NumericUpDown() : base()
        {
            Increment = DefaultIncrement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            OnPreviewTextInput(e, Expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnIncrementChanged(T Value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="Expression"></param>
        protected virtual void OnPreviewTextInput(TextCompositionEventArgs e, Regex Expression)
        {
            e.Handled = CaretIndex == 0 && e.Text == "-" ? Text.Contains("-") : !Expression.IsMatch(e.Text);
        }
    }
}
