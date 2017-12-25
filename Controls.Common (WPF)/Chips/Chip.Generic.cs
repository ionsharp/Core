using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A chip for displaying and selecting a brush.
    /// </summary>
    public abstract class Chip<T> : ChipBase where T : Brush
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<T>> ValueChanged;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty<T, Chip<T>> ValueProperty = new DependencyProperty<T, Chip<T>>("Value", new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get
            {
                return ValueProperty.Get(this);
            }
            set
            {
                ValueProperty.Set(this, value);
            }
        }
        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<Chip<T>>().OnValueChanged((T)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public Chip() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnValueChanged(T Value)
        {
            ValueChanged?.Invoke(this, new EventArgs<T>(Value));
        }
    }
}
