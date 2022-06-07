using System;
using System.Text;

namespace Imagin.Core.Numerics;

public static class Number
{
    public static string Convert(in int input, NumberStyle style)
    {
        static string Letter(int index)
        {
            const int columns = 26;
            //ceil(log26(Int32.Max))
            const int digitMaximum = 7;

            const string digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            if (index <= columns)
                return digits[index - 1].ToString();

            var result = new StringBuilder().Append(' ', digitMaximum);

            var current = index;
            var offset = digitMaximum;
            while (current > 0)
            {
                result[--offset] = digits[--current % columns];
                current /= columns;
            }

            return result.ToString(offset, digitMaximum - offset);
        }

        static string Ordinal(int input)
        {
            var result = string.Empty;
            switch (input)
            {
                case 1:
                    result = "st";
                    break;
                case 2:
                    result = "nd";
                    break;
                case 3:
                    result = "rd";
                    break;
                default:
                    result = "th";
                    break;
            }
            return $"{input}{result}";
        }

        static string Roman(int index)
        {
            if ((index < 0) || (index > 3999))
                throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");

            if (index < 1)
                return string.Empty;

            if (index >= 1000)
                return "M" + Roman(index - 1000);

            if (index >= 900)
                return "CM" + Roman(index - 900);

            if (index >= 500)
                return "D" + Roman(index - 500);

            if (index >= 400)
                return "CD" + Roman(index - 400);

            if (index >= 100)
                return "C" + Roman(index - 100);

            if (index >= 90)
                return "XC" + Roman(index - 90);

            if (index >= 50)
                return "L" + Roman(index - 50);

            if (index >= 40)
                return "XL" + Roman(index - 40);

            if (index >= 10)
                return "X" + Roman(index - 10);

            if (index >= 9)
                return "IX" + Roman(index - 9);

            if (index >= 5)
                return "V" + Roman(index - 5);

            if (index >= 4)
                return "IV" + Roman(index - 4);

            if (index >= 1)
                return "I" + Roman(index - 1);

            throw new ArgumentOutOfRangeException();
        }

        switch (style)
        {
            case NumberStyle.Letter: return Letter(input);
            case NumberStyle.Ordinal: return Ordinal(input);
            case NumberStyle.Roman: return Roman(input);
        }
        return default;
    }
}