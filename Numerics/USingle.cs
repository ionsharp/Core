using Imagin.Core.Numerics;
using System;

namespace Imagin.Core;

/// <summary>Represents an unsigned single-precision floating-point number.</summary>
[Serializable]
public struct USingle
{
    /// <summary>
    /// Equivalent to <see cref="float.Epsilon"/>.
    /// </summary>
    public readonly static USingle Epsilon = float.Epsilon;

    /// <summary>
    /// Represents the largest possible value of <see cref="USingle"/> (equivalent to <see cref="float.MaxValue"/>).
    /// </summary>
    public readonly static USingle MaxValue = float.MaxValue;

    /// <summary>
    /// Represents the smallest possible value of <see cref="USingle"/> (0).
    /// </summary>
    public readonly static USingle MinValue = 0;

    /// <summary>
    /// Equivalent to <see cref="float.NaN"/>.
    /// </summary>
    public readonly static USingle NaN = float.NaN;

    /// <summary>
    /// Equivalent to <see cref="float.PositiveInfinity"/>.
    /// </summary>
    public readonly static USingle PositiveInfinity = float.PositiveInfinity;
    readonly float value;

    /// <summary>
    /// Initializes an instance of the <see cref="USingle"/> structure.
    /// </summary>
    /// <param name="input"></param>
    public USingle(float input)
    {
        if (float.IsNegativeInfinity(input))
            throw new NotSupportedException();

        value = M.Clamp(input, float.MaxValue);
    }

    public static implicit operator float(USingle d) => d.value;

    public static implicit operator USingle(float d) => new(d);

    public static bool operator <(USingle a, USingle b) => a.value < b.value;

    public static bool operator >(USingle a, USingle b) => a.value > b.value;

    public static bool operator ==(USingle a, USingle b) => a.value == b.value;

    public static bool operator !=(USingle a, USingle b) => a.value != b.value;

    public static bool operator <=(USingle a, USingle b) => a.value <= b.value;

    public static bool operator >=(USingle a, USingle b) => a.value >= b.value;

    public static USingle operator +(USingle a, USingle b) => a.value + b.value;

    public static USingle operator -(USingle a, USingle b) => a.value - b.value;

    public static USingle operator /(USingle a, USingle b) => a.value / b.value;

    public static USingle operator *(USingle a, USingle b) => a.value * b.value;

    public USingle Clamp(USingle maximum, USingle minimum = default) => value > maximum ? maximum : (value < minimum ? minimum : value);

    public override bool Equals(object a) => a is USingle b ? this == b : false;

    public override int GetHashCode() => value.GetHashCode();

    public static USingle Parse(string input) => float.Parse(input);

    public static bool TryParse(string input, out USingle result)
    {
        var r = float.TryParse(input, out float d);
        result = d;
        return r;
    }

    public string ToString(string format) => value.ToString(format);

    public override string ToString() => value.ToString();
}