using System;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public class MRange<T> : Base
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

        public double Convert(double newMinimum, double newMaximum, double value)
        {
            var scale = (newMaximum - newMinimum) / (Maximum - Minimum);
            return newMinimum + ((value - Minimum) * scale);
        }
    }
}