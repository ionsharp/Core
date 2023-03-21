using Imagin.Core.Linq;
using System;

namespace Imagin.Core;

[Serializable]
public struct Range<T> : IEquatable<Range<T>>, IRange<T>
{
    public static Range<T> Default = new(default, default);

    ///

    public readonly T Minimum;
    T IRange<T>.Minimum => Minimum;

    public readonly T Maximum;
    T IRange<T>.Maximum => Minimum;

    ///

    public Range(T minimum, T maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public override string ToString() => $"{nameof(Minimum)} = {Minimum}, {nameof(Maximum)} = {Maximum}";

    ///

    public static bool operator ==(Range<T> left, Range<T> right) => left.EqualsOverload(right);

    public static bool operator !=(Range<T> left, Range<T> right) => !(left == right);

    public bool Equals(Range<T> i) => this.Equals<Range<T>>(i) && Minimum.Equals(i.Minimum) && Maximum.Equals(i.Maximum);

    public override bool Equals(object i) => i is Range<T> j ? Equals(j) : false;

    public override int GetHashCode() => new { Minimum, Maximum }.GetHashCode();
}