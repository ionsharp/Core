using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Imagin.Common
{
    /// <summary>
    /// A utility for setting properties of objects that implement <see cref="INotifyPropertyChanged"/> and <see cref="IPropertyChanged"/>.
    /// </summary>
    public static class Property
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="expression"></param>
        public static void OnChanged<TObject, TValue>(TObject source, Expression<Func<TValue>> expression) where TObject : INotifyPropertyChanged, IPropertyChanged
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var body = expression.Body as MemberExpression;

            if (body == null)
                throw new ArgumentException("The body must be a member expression.");

            source.OnPropertyChanged(body.Member.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool Set<TObject, TValue>(TObject source, ref TValue field, TValue value, Expression<Func<TValue>> expression) where TObject : INotifyPropertyChanged, IPropertyChanged
        {
            if (EqualityComparer<TValue>.Default.Equals(field, value))
                return false;

            field = value;

            OnChanged(source, expression);
            return true;
        }
    }
}