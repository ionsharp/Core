using Imagin.Core.Numerics;
using Imagin.Core.Text;
using System;

namespace Imagin.Core.Linq;

public static class XBullets
{
    public static object ToString(this Bullets input, double index)
    {
        object result = null;
        Try.Invoke(out result, () =>
        {
            var i = index.Int32();
            return input switch
            {
                Bullets.LetterUpperPeriod 
                    => $"{Number.Convert(i, NumberStyle.Letter)}.".ToUpper(),
                Bullets.LetterUpperParenthesis 
                    => $"{Number.Convert(i, NumberStyle.Letter)})".ToUpper(),
                Bullets.LetterLowerPeriod 
                    => $"{Number.Convert(i, NumberStyle.Letter)}.".ToLower(),
                Bullets.LetterLowerParenthesis 
                    => $"{Number.Convert(i, NumberStyle.Letter)})".ToLower(),
                Bullets.NumberPeriod 
                    => $"{index}.",
                Bullets.NumberParenthesis 
                    => $"{index})",
                Bullets.RomanNumberUpperPeriod 
                    => $"{Number.Convert(i, NumberStyle.Roman)}.".ToUpper(),
                Bullets.RomanNumberUpperParenthesis 
                    => $"{Number.Convert(i, NumberStyle.Roman)})".ToUpper(),
                Bullets.RomanNumberLowerPeriod 
                    => $"{Number.Convert(i, NumberStyle.Roman)}.".ToLower(),
                Bullets.RomanNumberLowerParenthesis 
                    => $"{Number.Convert(i, NumberStyle.Roman)})".ToLower(),
                _ => throw new NotSupportedException(),
            };
        });
        return result;
    }
}