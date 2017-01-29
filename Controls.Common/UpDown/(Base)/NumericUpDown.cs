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
        public static readonly DependencyProperty<T, NumericUpDown<T>> IncrementProperty = new DependencyProperty<T, NumericUpDown<T>>("Increment", new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIncrementChanged));
        /// <summary>
        /// 
        /// </summary>
        public T Increment
        {
            get
            {
                return IncrementProperty.Get(this);
            }
            set
            {
                IncrementProperty.Set(this, value);
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
            e.Handled = CaretIndex == 0 && e.Text == "-" ? Text.Contains("-") : !Expression.IsMatch(e.Text);
        }

        /// <summary>
        /// Occurs when the increment value changes.
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnIncrementChanged(T Value)
        {
        }
    }
}
