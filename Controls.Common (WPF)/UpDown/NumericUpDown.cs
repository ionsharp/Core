using Imagin.Common.Linq;
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
        public virtual Regex Expression
        {
            get
            {
                return new Regex("^[0-9]?$");
            }
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
        public static readonly DependencyProperty<string, NumericUpDown<T>> StringFormatProperty = new DependencyProperty<string, NumericUpDown<T>>("StringFormat", new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnStringFormatChanged));
        /// <summary>
        /// 
        /// </summary>
        public string StringFormat
        {
            get
            {
                return StringFormatProperty.Get(this);
            }
            set
            {
                StringFormatProperty.Set(this, value);
            }
        }
        static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<NumericUpDown<T>>().OnStringFormatChanged((string)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public NumericUpDown() : base()
        {
            SetCurrentValue(IncrementProperty.Property, DefaultIncrement);
        }

        /// <summary>
        /// Occurs when the increment value changes.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnIncrementChanged(T value)
        {
        }

        /// <summary>
        /// Occurs when <see cref="StringFormat"/> changes.
        /// </summary>
        protected virtual void OnStringFormatChanged(string value)
        {
            OnValueChanged(Value);
        }
    }
}
