using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Imagin.Common
{
    /// <summary>
    /// A utility for setting properties of objects that implement both <see cref="INotifyPropertyChanged"/> and <see cref="IPropertyChanged"/>.
    /// </summary>
    public static class Property
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="expression"></param>
        public static void Raise<TSource, TValue>(TSource source, Expression<Func<TValue>> expression) where TSource : IPropertyChanged
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
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        public static void Raise<TSource>(TSource source, [CallerMemberName] string propertyName = "") where TSource : IPropertyChanged
        {
            source.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool Set<TSource, TValue>(TSource source, ref TValue field, TValue value, Expression<Func<TValue>> expression) where TSource : IPropertyChanged
        {
            if (EqualityComparer<TValue>.Default.Equals(field, value)) return false;

            field = value;
            Raise(source, expression);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool Set<TSource, TValue>(TSource source, ref TValue field, TValue value, [CallerMemberName] string propertyName = "") where TSource : IPropertyChanged
        {
            if (EqualityComparer<TValue>.Default.Equals(field, value)) return false;

            field = value;
            Raise(source, propertyName);
            return true;
        }
    }
}