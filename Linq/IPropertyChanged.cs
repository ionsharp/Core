using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Imagin.Core
{
    public static class XPropertyChanged
    {
        public static void Changed<T>(this IPropertyChanged input, Expression<Func<T>> propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            var body = propertyName.Body as MemberExpression;

            if (body == null)
                throw new ArgumentNullException($"{nameof(propertyName)}.{nameof(propertyName.Body)}");

            input.OnPropertyChanged(body.Member.Name);
        }

        public static void Changed(this IPropertyChanged input, [CallerMemberName] string propertyName = "") => input.OnPropertyChanged(propertyName);

        public static bool Change<T>(this IPropertyChanged input, ref T field, T value, Expression<Func<T>> propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            input.Changed(propertyName);
            return true;
        }

        public static bool Change<T>(this IPropertyChanged input, ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            input.Changed(propertyName);
            return true;
        }
    }
}