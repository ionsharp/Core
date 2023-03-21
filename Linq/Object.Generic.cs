using System;
using System.Collections.Generic;

namespace Imagin.Core.Linq;

public static partial class XObject
{
    #region Equals

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

    #endregion
    
    ///

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
    public static void If<T>(this T input, Predicate<T> where, Action<T> isNotNull, Action isNull = null)
    {
        if (input is T i)
        {
            if (where(i))
                isNotNull(i);

            return;
        }
        isNull?.Invoke();
    }

    ///

    public static void If<T>(this T input, T ifEquals, Action thenDo, Action elseDo = null)
        => input.If(ifEquals, i => thenDo(), i => elseDo());

    public static void If<T>(this T input, T ifEquals, Action<T> thenDo, Action<T> elseDo = null)
    {
        if (EqualityComparer<T>.Default.Equals(input, ifEquals))
        {
            thenDo(input);
        }
        else elseDo(input);
    }

    ///

    /// <summary>Calls the given action if the object is <see langword="not null"/>.</summary>
    public static Output IfGet<Input, Output>(this Input input, Func<Input, Output> get)
    {
        if (input is Input i)
            return get(i);

        return default;
    }

    /// <summary>Calls the given action if the object satisfies the given predicate.</summary>
    public static Output IfGet<Input, Output>(this Input input, Predicate<Input> where, Func<Input, Output> get)
    {
        if (where(input))
            return get(input);

        return default;
    }
}