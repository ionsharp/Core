using System;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public struct Angle
    {
        /// <summary>
        /// Represents the largest possible value of <see cref="Angle"/> (359).
        /// </summary>
        public readonly static double MaxValue = 359;

        /// <summary>
        /// Represents the smallest possible value of <see cref="Angle"/> (-359).
        /// </summary>
        public readonly static double MinValue = -359;

        readonly double value;

        /// <summary>
        /// Initializes an instance of the <see cref="Angle"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public Angle(double input)
        {
            if (input < MinValue || input > MaxValue)
                throw new NotSupportedException();

            value = input;
        }

        public static implicit operator double(Angle d) => d.value;

        public static implicit operator Angle(double d) => new Angle(d);

        public static bool operator <(Angle a, Angle b) => a.value < b.value;

        public static bool operator >(Angle a, Angle b) => a.value > b.value;

        public static bool operator ==(Angle a, Angle b) => a.value == b.value;

        public static bool operator !=(Angle a, Angle b) => a.value != b.value;

        public static bool operator <=(Angle a, Angle b) => a.value <= b.value;

        public static bool operator >=(Angle a, Angle b) => a.value >= b.value;

        public static Angle operator +(Angle a, Angle b) => a.value + b.value;

        public static Angle operator -(Angle a, Angle b) => a.value - b.value;

        public static Angle operator /(Angle a, Angle b) => a.value / b.value;

        public static Angle operator *(Angle a, Angle b) => a.value * b.value;

        public override bool Equals(object a) => a is Angle b ? this == b : false;

        public override int GetHashCode() => value.GetHashCode();

        public static Angle Parse(string input) => double.Parse(input);

        public static bool TryParse(string input, out Angle result)
        {
            var r = double.TryParse(input, out double d);
            result = d;
            return r;
        }

        public string ToString(string format) => value.ToString(format);

        public override string ToString() => value.ToString();

        //...

        public static double GetDegree(double radian) => radian * (180.0 / Numbers.PI);

        public static double GetRadian(double degree) => (Numbers.PI / 180.0) * degree;

        public static double NormalizeDegree(double degree)
        {
            var result = degree % 360.0;
            return result >= 0 ? result : (result + 360.0);
        }
    }
}