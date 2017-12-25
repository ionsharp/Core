using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Imagin.Common
{
    /// <summary>
    /// A base for abstract objects (implements INotifyPropertyChanged).
    /// </summary>
    public abstract class BindableObject : INotifyPropertyChanged
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
        /// <typeparam name="T"></typeparam>
        /// <param name="selectorExpression"></param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");

            MemberExpression body = selectorExpression.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("The body must be a member expression");

            OnPropertyChanged(body.Member.Name);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected bool SetValue<T>(ref T field, T value, Expression<Func<T>> expression)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(expression);
            return true;
        }
    }
}