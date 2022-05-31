using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class NRange<T> : Base, IRange<T>
{
    T minimum = default;
    public T Minimum
    {
        get => minimum;
        set => this.Change(ref minimum, value);
    }

    T maximum = default;
    public T Maximum
    {
        get => maximum;
        set => this.Change(ref maximum, value);
    }

    public NRange() : base() { }

    public NRange(T minimum, T maximum) : base()
    {
        Minimum = minimum;
        Maximum = maximum;
    }
}

[Serializable]
public class AngleRange : NRange<Angle>
{
    public AngleRange() : base() { }

    public AngleRange(Angle minimum, Angle maximum) : base(minimum, maximum) { }
}

[Serializable]
public class DoubleRange : NRange<double>
{
    public DoubleRange() : base() { }

    public DoubleRange(double minimum, double maximum) : base(minimum, maximum) { }
}