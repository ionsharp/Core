using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an old and new value.
    /// </summary>
    public class Value<T>
    {
        public readonly T Old = default;

        public readonly T New = default;

        public Value(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is T i)
                Old = i;

            if (e.NewValue is T j)
                New = j;
        }

        public Value(T @new) : this(default, @new) { }

        public Value(T old, T @new)
        {
            Old = old;
            New = @new;
        }

        public static implicit operator Value<T>(DependencyPropertyChangedEventArgs i) => new(i);
    }

    /// <inheritdoc />
    public class Value : Value<object>
    {
        public Value(DependencyPropertyChangedEventArgs e) : base(e) { }

        public Value(object old, object @new) : base(old, @new) { }
    }
}