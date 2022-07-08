using Imagin.Core.Linq;
using System;
using System.Globalization;

namespace Imagin.Core.Numerics;

[Serializable]
public struct ByteVector4 : IEquatable<ByteVector4>
{
    public const uint Length = 4;

    readonly Guid id = Guid.NewGuid();

    #region Properties

    public static ByteVector4 Black => new(0, 0, 0, 255);

    public static ByteVector4 White => new(255);

    public static ByteVector4 Transparent => new(0);

    //...

    public byte X { get; private set; }

    public byte Y { get; private set; }

    public byte Z { get; private set; }

    public byte W { get; private set; }

    [Hidden]
    public Vector3 XYZ => new(X, Y, Z);

    /// <summary>(0) <see cref="R"/> = <see cref="X"/></summary>
    [Hidden]
    public byte R => X;

    /// <summary>(1) <see cref="G"/> = <see cref="Y"/></summary>
    [Hidden]
    public byte G => Y;

    /// <summary>(2) <see cref="B"/> = <see cref="Z"/></summary>
    [Hidden]
    public byte B => Z;

    /// <summary>(3) <see cref="A"/> = <see cref="W"/></summary>
    [Hidden]
    public byte A => W;

    public byte this[int index] => new byte[] { X, Y, Z, W }[index];

    #endregion

    #region ByteVector4

    public ByteVector4() : this(0) { }

    public ByteVector4(byte xyzw) : this(xyzw, xyzw, xyzw, xyzw) { }

    public ByteVector4(byte x, byte y, byte z, byte w)
    {
        X = x; Y = y; Z = z; W = w;
    }

    public ByteVector4(string hexadecimal) : this(0) 
    {
        if (hexadecimal.Length > 8)
            throw new ArgumentOutOfRangeException(nameof(hexadecimal));

        switch (hexadecimal.Length)
        {
            case 3:
                hexadecimal = "{0}{1}{2}{3}".F("FF", new string(hexadecimal[0], 2), new string(hexadecimal[1], 2), new string(hexadecimal[2], 2));
                break;
            case 6:
                hexadecimal = "{0}{1}".F("FF", hexadecimal);
                break;
            default:
                hexadecimal = string.Concat(hexadecimal, new string('0', 8 - hexadecimal.Length));
                break;
        }

        X = Parse(hexadecimal.Substring(2, 2));
        Y = Parse(hexadecimal.Substring(4, 2));
        Z = Parse(hexadecimal.Substring(6, 2));
        W = Parse(hexadecimal.Substring(0, 2));
    }

    //...

    public static implicit operator byte[](ByteVector4 input)
        => XArray.New(input.X, input.Y, input.Z, input.W);

    public static implicit operator string(ByteVector4 input) => input.ToString();

    #endregion

    #region Methods

    public override string ToString()
        => A.ToString("X2") + R.ToString("X2") + G.ToString("X2") + B.ToString("X2");

    public string ToString(bool alpha)
        => alpha ? ToString() : R.ToString("X2") + G.ToString("X2") + B.ToString("X2");

    //...

    public static byte Parse(string input) => int.Parse(input, NumberStyles.HexNumber).Byte();

    #endregion

    #region ==

    public static bool operator ==(ByteVector4 a, ByteVector4 b)
        => a.EqualsOverload(b);

    public static bool operator !=(ByteVector4 a, ByteVector4 b)
        => !(a == b);

    public bool Equals(ByteVector4 i)
        => this.Equals<ByteVector4>(i) && X.Equals(i.X) && Y.Equals(i.Y) && Z.Equals(i.Z) && W.Equals(i.W);

    public override bool Equals(object i)
        => i is ByteVector4 j && Equals(j);

    public override int GetHashCode()
        => id.GetHashCode();

    #endregion
}