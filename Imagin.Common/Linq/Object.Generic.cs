using System;
using System.Reflection;

namespace Imagin.Common.Linq
{
    public static partial class XObject
    {
        /* Remarks

        To implement equality in a class, define the following methods and tweak accordingly...

        A. public static bool operator ==(T a, T b) => a.EqualsOverload(b);
        B. public static bool operator !=(T a, T b) => !(a == b);
        C. public override bool Equals(object b) => Equals(b as T);
        D. public bool Equals(T b) => this.Equals<T>(b) && ...;
        E. public override int GetHashCode() => ...;

        */

        /// <summary>
        /// Implements <see cref="IEquatable{T}.Equals(T)"/> for any given <see langword="class"/> that implements <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals<T>(this T a, T b)
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
        /// Implements the <see langword="=="/> operator for any given <see langword="class"/> or <see langword="struct"/>. Example: <see langword="public static bool operator ==(left, right) => left.EqualsEquals(right);"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualsOverload<T>(this T a, T b)
        {
            if (ReferenceEquals(a, null))
            {
                //null == null = true
                if (ReferenceEquals(b, null))
                    return true;

                //Only the left side is null
                return false;
            }
            return a.Equals(b);
        }

        //...

        /// <summary>
        /// Gets whether or not <see cref="object"/> is equal to any given <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool EqualsAny<T>(this T input, params T[] values)
        {
            foreach (var i in values)
            {
                if (input.Equals(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Calls the given action if the object is not <see langword="null"/>.
        /// </summary>
        public static void If<T>(this T input, Action<T> isNotNull, Action isNull = null)
        {
            if (input is T i)
                isNotNull(i);

            else isNull?.Invoke();
        }

        /// <summary>
        /// Calls the given action if the object is <see langword="not null"/> and satisfies the given predicate.
        /// </summary>
        public static void If<T>(this T input, Predicate<T> predicate, Action<T> isNotNull, Action isNull = null)
        {
            if (input is T i)
            {
                if (predicate(i))
                    isNotNull(i);

                return;
            }
            isNull?.Invoke();
        }

        /// <summary>
        /// Calls the given action if the object is <see langword="not null"/>.
        /// </summary>
        public static Output IfThen<Input, Output>(this Input input, Func<Input, Output> action)
        {
            if (input is Input i)
                return action(i);

            return default;
        }

        /// <summary>
        /// Calls the given action if the object satisfies the given predicate.
        /// </summary>
        public static Output IfThen<Input, Output>(this Input input, Predicate<Input> predicate, Func<Input, Output> action)
        {
            if (predicate(input))
                return action(input);

            return default;
        }

        /// <summary>
        /// Gets whether or not the <see cref="object"/> is <see cref="System.Nullable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Nullable<T>(this T input) => input.GetType().IsNullable();

        /// <summary>
        /// (Recursive) Uses reflection to clone the given instance (using <see cref="BindingFlags.Instance"/> and <see cref="BindingFlags.Public"/>). If an instance member implements <see cref="ICloneable"/>, <see cref="ICloneable.Clone"/> is returned;
        /// if it is otherwise still a <see langword="class"/>, <see cref="SmartClone{T}(T)"/> is attempted (recursively).
        /// </summary>
        public static T SmartClone<T>(this T input) where T : class
        {
            if (input == null)
                return null;

            if (input is ICloneable a)
                return (T)a.Clone();

            if (!input.GetType().IsClass)
                return input;

            T result = default;
            Try.Invoke(() =>
            {
                result = typeof(T).Create<T>();
                foreach (var i in typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public, MemberTypes.Field | MemberTypes.Property, null, true, true))
                {
                    object CloneValue(object x)
                    {
                        if (x is ICloneable y)
                            x = y.Clone();

                        else if (x?.GetType().IsClass == true)
                            x = x.SmartClone();

                        return x;
                    }

                    if (i is FieldInfo field)
                    {
                        var value = field.GetValue(input);
                        value = CloneValue(value);

                        field.SetValue(result, value);
                    }
                    else if (i is PropertyInfo property)
                    {
                        var value = property.GetValue(input);
                        value = CloneValue(value);

                        property.SetValue(result, value);
                    }
                }
            });
            return result;
        }
    }
}