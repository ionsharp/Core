using System;

namespace Imagin.Common.Extensions
{
    public static class RandomExtensions
    {
        static string GetString(this Random Random, string AllowedCharacters, int MinLength, int MaxLength)
        {
            char[] Characters = new char[MaxLength];
            int SetLength = AllowedCharacters.Length;

            int length = Random.Next(MinLength, MaxLength + 1);
            for (int i = 0; i < length; ++i)
                Characters[i] = AllowedCharacters[Random.Next(SetLength)];
            return new string(Characters, 0, length);
        }
    }
}
