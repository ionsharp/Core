using Imagin.Core.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Numerics;

public static class Random
{
    public static readonly System.Random Current = new System.Random();

    public static bool NextBoolean()
        => NextInt32(0, 2) == 1;

    public static bool NextBoolean(double probability)
        => NextDouble() < probability / 100.0;

    public static byte NextByte()
        => NextInt32(0, 256).Byte();

    public static double NextDouble()
        => Current.NextDouble();

    public static float NextSingle()
        => Current.NextDouble().Single();

    public static float NextSingle(float range)
        => (Current.NextDouble() * range).Single();

    public static float NextSingle(float min, float max)
        => ((Current.NextDouble() * (max - min)) + min).Single();

    public static int NextInt32(int min, int max)
        => Current.Next(min, max);

    public static int NextInt32(IEnumerable<int> values)
        => NextInt32(values.ToArray());

    public static int NextInt32(params int[] values)
        => values[NextInt32(0, values.Length)];

    public static ushort NextUInt16(ushort minimum, ushort maximum)
        => Current.Next(minimum.Int32(), maximum.Int32()).UInt16();

    public static uint NextUInt32(uint minimum, uint maximum)
        => Current.Next(minimum.Int32(), maximum.Int32()).UInt32();

    public static ulong NextUInt64(ulong minimum, ulong maximum)
        => Current.Next(minimum.Int32(), maximum.Int32()).UInt64();

    public static string String(string AllowedCharacters, int MinLength, int MaxLength)
    {
        char[] characters = new char[MaxLength];
        int setLength = AllowedCharacters.Length;

        int length = Current.Next(MinLength, MaxLength + 1);
        for (int i = 0; i < length; ++i)
            characters[i] = AllowedCharacters[Current.Next(setLength)];

        return new string(characters, 0, length);
    }
}