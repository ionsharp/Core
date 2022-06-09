using Imagin.Core.Linq;
using System;
using static System.Math;

namespace Imagin.Core.Numerics;

/// <summary>An extension of <see cref="Math"/>.</summary>
public static class M
{
    public const double PI2 = 2 * PI;

    public const double PI3 = 3 * PI;

    //...

    /// <summary>a ≤ <see cref="AB(double, double, double)">input</see> ≤ b</summary>
    public static bool AB(double input, double a, double b)
        => input >= a && input <= b;

    /// <summary>a ≤ <see cref="Ab(double, double, double)">input</see> <![CDATA[ < ]]> b</summary>
    public static bool Ab(double input, double a, double b)
        => input >= a && input < b;

    /// <summary>a <![CDATA[ < ]]> <see cref="aB(double, double, double)">input</see> ≤ b</summary>
    public static bool aB(double input, double a, double b)
        => input > a && input <= b;

    //...

    public static byte Clamp(byte i, byte m, byte n = 0) => Max(Min(i, m), n);

    public static decimal Clamp(decimal i, decimal m, decimal n = 0) => Max(Min(i, m), n);

    public static double Clamp(double i, double m, double n = 0) => Max(Min(i, m), n);

    public static short Clamp(short i, short m, short n = 0) => Max(Min(i, m), n);

    public static int Clamp(int i, int m, int n = 0) => Max(Min(i, m), n);

    public static long Clamp(long i, long m, long n = 0) => Max(Min(i, m), n);

    public static float Clamp(float i, float m, float n = 0) => Max(Min(i, m), n);

    public static ushort Clamp(ushort i, ushort m, ushort n = 0) => Max(Min(i, m), n);

    public static uint Clamp(uint i, uint m, uint n = 0) => Max(Min(i, m), n);

    public static ulong Clamp(ulong i, ulong m, ulong n = 0) => Max(Min(i, m), n);

    //...

    /// <summary>Compute cosine of angle in degrees.</summary>
    public static double CosDeg(in double x)
    {
        var x_rad = Angle.GetRadian(x);
        var y = Cos(x_rad);
        return y;
    }

    /// <summary>Compute sine of angle in degrees.</summary>
    public static double SinDeg(in double x)
    {
        var x_rad = Angle.GetRadian(x);
        var y = Sin(x_rad);
        return y;
    }

    //...

    public static bool Even(in double input) => input == 0 ? true : input % 2 == 0;

    public static bool Odd(in double input) => !Even(input);

    //...

    public static double GetDistance(double x, double y) => GetDistance(0.5, x, y);

    public static double GetDistance(double x1y1, double x2, double y2) => GetDistance(x1y1, x1y1, x2, y2);

    public static double GetDistance(double x1, double y1, double x2, double y2) => Sqrt(Pow(Abs(x1 - x2), 2) + Pow(Abs(y1 - y2), 2));

    //...

    public static double Modulo(double input, double left, double? right = null)
    {
        //Detect single-argument case
        if (right == null)
        {
            right = left;
            left = 0;
        }

        //Swap frame order
        if (left > right)
        {
            var tmp = right.Value;
            right = left;
            left = tmp;
        }

        var frame = right.Value - left;
        input = ((input + left) % frame) - left;

        if (input < left)
            input += frame;

        if (input > right)
            input -= frame;

        return input;
    }

    public static double NearestFactor(in double input, in double factor)
        => Round(input / factor, MidpointRounding.AwayFromZero) * factor;

    //...

    public static double Normalize(in byte i) => i.Double() / 255;

    public static Vector Normalize(in Vector input, Vector min, Vector max)
    {
        var result = new double[input.Length];
        for (var i = 0; i < input.Length; i++)
            result[i] = new DoubleRange(min[i], max[i]).Convert(0, 1, input[i]);

        return new(result);
    }

    public static Vector2 Normalize(in Vector2 input, Vector2 min, Vector2 max)
    {
        var result = new double[2];
        for (var i = 0; i < 2; i++)
            result[i] = new DoubleRange(min[i], max[i]).Convert(0, 1, input[i]);

        return new(result[0], result[1]);
    }

    public static Vector3 Normalize(in Vector3 input, Vector3 min, Vector3 max)
    {
        var result = new double[3];
        for (var i = 0; i < 3; i++)
            result[i] = new DoubleRange(min[i], max[i]).Convert(0, 1, input[i]);

        return new(result[0], result[1], result[2]);
    }

    public static Vector4 Normalize(in Vector4 input, Vector4 min, Vector4 max)
    {
        var result = new double[4];
        for (var i = 0; i < 4; i++)
            result[i] = new DoubleRange(min[i], max[i]).Convert(0, 1, input[i]);

        return new(result[0], result[1], result[2], result[3]);
    }

    //...

    public static byte Denormalize(in double i) => Clamp((i * 255).Round(), 255).Byte();

    public static Vector Denormalize(in Vector input, Vector min, Vector max)
    {
        var result = new double[input.Length];
        for (var i = 0; i < input.Length; i++)
            result[i] = new DoubleRange(0, 1).Convert(min[i], max[i], input[i]);

        return new(result);
    }

    public static Vector2 Denormalize(in Vector2 input, Vector2 min, Vector2 max)
    {
        var result = new double[2];
        for (var i = 0; i < 2; i++)
            result[i] = new DoubleRange(0, 1).Convert(min[i], max[i], input[i]);

        return new(result[0], result[1]);
    }

    public static Vector3 Denormalize(in Vector3 input, Vector3 min, Vector3 max)
    {
        var result = new double[3];
        for (var i = 0; i < 3; i++)
            result[i] = new DoubleRange(0, 1).Convert(min[i], max[i], input[i]);

        return new(result[0], result[1], result[2]);
    }

    public static Vector4 Denormalize(in Vector4 input, Vector4 min, Vector4 max)
    {
        var result = new double[4];
        for (var i = 0; i < 4; i++)
            result[i] = new DoubleRange(0, 1).Convert(min[i], max[i], input[i]);

        return new(result[0], result[1], result[2], result[3]);
    }

    //...

    /// <summary>Compute x^2.</summary>
    public static double Pow2(in double x) => x * x;

    /// <summary>Compute x^3.</summary>
    public static double Pow3(in double x) => x * x * x;

    /// <summary>Compute x^4.</summary>
    public static double Pow4(in double x) => x * x * (x * x);

    /// <summary>Compute x^7.</summary>
    public static double Pow7(in double x) => x * x * x * (x * x * x) * x;

    //...

    /// <summary>Compute x^1/y.</summary>
    public static double Root(in double x, in double y) => Pow(x, (1.0 / y));

    //...

    /// <summary>Compute x^1/3.</summary>
    public static double Cbrt(in double x) => Root(x, 3);

    /// <summary>Compute x^1/4.</summary>
    public static double Qdrt(in double x) => Root(x, 4);

    //...

    public static double Shift(this double input, int shifts = 1)
    {
        var leftOrRight = shifts < 0;

        for (var i = 0; i < Abs(shifts); i++)
            input = leftOrRight ? input / 10 : input * 10;

        return input;
    }
}