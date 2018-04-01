using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// A chip for displaying and selecting a <see cref="Brush"/>.
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
        public static readonly DependencyProperty<T, Chip<T>> ValueProperty = new DependencyProperty<T, Chip<T>>(nameof(Value), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get => ValueProperty.Get(this);
            set => ValueProperty.Set(this, value);
        }
        static void OnValueChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
            => element.As<Chip<T>>().OnValueChanged((T)e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        public Chip() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnValueChanged(T Value)
            => ValueChanged?.Invoke(this, new EventArgs<T>(Value));
    }
}