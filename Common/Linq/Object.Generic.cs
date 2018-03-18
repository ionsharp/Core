using System;

namespace Imagin.Common.Linq
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Implements standard behavior of the <see cref="IEquatable{T}.Equals(T)"/> method of a <see langword="class"/> or <see langword="struct"/>; use in conjunction with native behavior. For example: <see langword="public bool Equals(TSource o) => this.Equals[TSource](o) AND ();"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals<TSource>(this TSource a, TSource b)
        {
            if (ReferenceEquals(b, null))
                return false;

            if (ReferenceEquals(a, b))
                return true;

            if (a.GetType() != b.GetType())
                return false;

            return true;
        }

        /// <summary>
        /// Implements standard behavior of the <see langword="=="/> operator overload of a <see langword="class"/> or <see langword="struct"/>. For example: <see langword="public static bool operator ==(left, right) => left.Equals_(right);"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals_<TSource>(this TSource a, TSource b)
        {
            if (!ReferenceEquals(a, null))
                return a.Equals(b);

            if (ReferenceEquals(b, null))
                return true;

            return false;
        }

        /// <summary>
        /// Check if object is equal to any given object.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool EqualsAny<TSource>(this TSource source, params TSource[] values)
        {
            foreach (var i in values)
            {
                if (source.Equals(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Executes the given <see cref="Action"/> if the given object is not <see langword="null"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void IfNotNull<TSource>(this TSource source, Action<TSource> action)
        {
            if (source != null)
                action(source);
        }

        /// <summary>
        /// Gets whether or not the <see cref="object"/> is <see cref="Nullable"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsNullable<TSource>(this TSource Value) => Value.GetType().IsNullable();
    }
}
