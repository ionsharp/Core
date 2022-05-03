using System.Windows;
using System;

namespace Imagin.Common.Linq
{
    public static partial class XObject
    {
        public static T FindParent<T>(this object input, Predicate<T> predicate = null) where T : DependencyObject
        {
            if (input is DependencyObject i)
                return i.FindParent<T>(predicate);

            return default;
        }

        public static bool HasParent<T>(this object input, Predicate<T> predicate = null) where T : DependencyObject
            => input.FindParent(predicate) is not null;
    }
}