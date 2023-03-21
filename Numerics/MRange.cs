using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class MRange<T> : Base, IRange<T>
{
    public T Minimum { get => Get<T>(); set => Set(value); }

    public T Maximum { get => Get<T>(); set => Set(value); }

    public MRange() : base() { }

    public MRange(T minimum, T maximum) : base()
    {
        Minimum = minimum;
        Maximum = maximum;
    }
}

[Serializable]
public class AngleRange : MRange<Angle>
{
    public AngleRange() : base() { }

    public AngleRange(Angle minimum, Angle maximum) : base(minimum, maximum) { }
}

[Serializable]
public class DoubleRange : MRange<double>
{
    public DoubleRange() : base() { }

    public DoubleRange(double minimum, double maximum) : base(minimum, maximum) { }
}