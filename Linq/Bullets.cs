using Imagin.Core.Numerics;
using Imagin.Core.Text;
using System;

namespace Imagin.Core.Linq;

public static class XBullets
{
    public static object ToString(this Bullets input, double index)
    {
        Try.Invoke(out object result, () =>
        {
            var i = index.Int32();
            return input switch
            {
                Bullets.LetterUpperPeriod
                    => $"{Number.Convert(i, Numerics.NumberStyle.Letter)}.".ToUpper(),
                Bullets.LetterUpperParenthesis
                    => $"{Number.Convert(i, Numerics.NumberStyle.Letter)})".ToUpper(),
                Bullets.LetterLowerPeriod
                    => $"{Number.Convert(i, Numerics.NumberStyle.Letter)}.".ToLower(),
                Bullets.LetterLowerParenthesis
                    => $"{Number.Convert(i, Numerics.NumberStyle.Letter)})".ToLower(),
                Bullets.NumberPeriod
                    => $"{index}.",
                Bullets.NumberParenthesis
                    => $"{index})",
                Bullets.RomanNumberUpperPeriod
                    => $"{Number.Convert(i, Numerics.NumberStyle.Roman)}.".ToUpper(),
                Bullets.RomanNumberUpperParenthesis
                    => $"{Number.Convert(i, Numerics.NumberStyle.Roman)})".ToUpper(),
                Bullets.RomanNumberLowerPeriod
                    => $"{Number.Convert(i, Numerics.NumberStyle.Roman)}.".ToLower(),
                Bullets.RomanNumberLowerParenthesis
                    => $"{Number.Convert(i, Numerics.NumberStyle.Roman)})".ToLower(),
                _ => throw new NotSupportedException(),
            };
        });
        return result;
    }
}