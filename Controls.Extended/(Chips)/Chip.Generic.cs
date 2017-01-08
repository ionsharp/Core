using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Extended
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
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(Chip<T>), new PropertyMetadata(default(T), OnValueChanged));
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get
            {
                return (T)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
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
            if (ValueChanged != null)
                ValueChanged(this, new EventArgs<T>(Value));
        }
    }
}
