using Imagin.Common.Input;
using System;
using System.Windows;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class BindingProxy : Freezable
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs<object>> DataChanged;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null, OnDataChanged));
        /// <summary>
        /// 
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="e"></param>
        static void OnDataChanged(DependencyObject element, DependencyPropertyChangedEventArgs e) => (element as BindingProxy).OnDataChanged(e.NewValue);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDataChanged(object Value) => DataChanged?.Invoke(this, new EventArgs<object>(Value));
    }
}
