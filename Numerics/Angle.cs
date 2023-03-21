using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public struct Angle : IEquatable<Angle>
{
    /// <summary>Specifies the largest possible value (359).</summary>
    public readonly static double MaxValue = 359;

    /// <summary>Specifies the smallest possible value (-359).</summary>
    public readonly static double MinValue = -359;

    readonly double Value;

    ///

    /// <summary>
    /// Initializes an instance of the <see cref="Angle"/> structure.
    /// </summary>
    /// <param name="input"></param>
    public Angle(double input)
    {
        if (input < MinValue || input > MaxValue)
            throw new NotSupportedException();

        Value = input;
    }

    ///

    public static implicit operator double(Angle i) => i.Value;

    public static implicit operator Angle(double i) => new(i);

    ///

    public static bool operator <(Angle a, Angle b) => a.Value < b.Value;

    public static bool operator >(Angle a, Angle b) => a.Value > b.Value;

    public static bool operator <=(Angle a, Angle b) => a.Value <= b.Value;

    public static bool operator >=(Angle a, Angle b) => a.Value >= b.Value;

    public static Angle operator +(Angle a, Angle b) => a.Value + b.Value;

    public static Angle operator -(Angle a, Angle b) => a.Value - b.Value;

    public static Angle operator /(Angle a, Angle b) => a.Value / b.Value;

    public static Angle operator *(Angle a, Angle b) => a.Value * b.Value;

    ///

    public static bool operator ==(Angle a, Angle b) => a.EqualsOverload(b);

    public static bool operator !=(Angle a, Angle b) => !(a == b);

    ///

    public bool Equals(Angle i)
        => this.Equals<Angle>(i) && Value.Equals(i.Value);

    public override bool Equals(object i)
        => Equals((Angle)i);

    public override int GetHashCode() => Value.GetHashCode();

    ///

    public static Angle Parse(string input) => double.Parse(input);

    public static bool TryParse(string input, out Angle result)
    {
        var r = double.TryParse(input, out double d);
        result = d;
        return r;
    }

    ///

    public string ToString(string format) => Value.ToString(format);

    public override string ToString() => Value.ToString();

    ///

    public static double GetDegree(double radian) => radian * (180.0 / Math.PI);

    public static double GetRadian(double degree) => (Math.PI / 180.0) * degree;

    ///

    public static double NormalizeDegree(double degree)
    {
        var result = degree % 360.0;
        return result >= 0 ? result : (result + 360.0);
    }
}