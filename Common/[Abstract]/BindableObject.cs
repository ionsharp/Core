using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Imagin.Common
{
    /// <summary>
    /// A base for abstract objects (implements INotifyPropertyChanged).
    /// </summary>
    public abstract class BindableObject : INotifyPropertyChanged, IPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public BindableObject()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        public void OnPropertyChanged<TValue>(Expression<Func<TValue>> expression)
        {
            Property.OnChanged(this, expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected bool SetValue<T>(ref T field, T value, Expression<Func<T>> expression)
        {
            return Property.Set(this, ref field, value, expression);
        }
    }
}