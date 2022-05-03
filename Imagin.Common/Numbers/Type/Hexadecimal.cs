using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Globalization;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public struct Hexadecimal : IEquatable<Hexadecimal>
    {
        public static Hexadecimal Black = new Hexadecimal(0, 0, 0);

        public static Hexadecimal White = new Hexadecimal(255, 255, 255);

        //...

        public const int Length = 8;
        readonly string value;

        //...

        public Hexadecimal(byte r, byte g, byte b, byte a = 255) => value = a.ToString("X2") + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");

        public Hexadecimal(string input)
        {
            if (input.Length > Length)
                throw new ArgumentOutOfRangeException(nameof(input));

            switch (input.Length)
            {
                case 3:
                    input = "{0}{1}{2}{3}".F("FF", new string(input[0], 2), new string(input[1], 2), new string(input[2], 2));
                    break;
                case 6:
                    input = "{0}{1}".F("FF", input);
                    break;
                default:
                    input = string.Concat(input, new string('0', Length - input.Length));
                    break;
            }

            value = input;
        }

        //...

        public static implicit operator Hexadecimal(string input) => new Hexadecimal(input);

        public static implicit operator string(Hexadecimal input) => input.ToString();

        static byte Parse(string input) => int.Parse(input, NumberStyles.HexNumber).Byte();

        public override string ToString() => ToString(true);

        public string ToString(bool withAlpha) => withAlpha ? value : value.Substring(2);

        public Argb Convert()
        {
            var result = ToString(true);
            var a = Parse(result.Substring(0, 2));
            var r = Parse(result.Substring(2, 2));
            var g = Parse(result.Substring(4, 2));
            var b = Parse(result.Substring(6, 2));
            return new Argb(a, r, g, b);
        }

        //...

        public static bool operator ==(Hexadecimal left, Hexadecimal right) => left.EqualsOverload(right);

        public static bool operator !=(Hexadecimal left, Hexadecimal right) => !(left == right);

        public bool Equals(Hexadecimal i) => this.Equals<Hexadecimal>(i) && value == i.value;

        public override bool Equals(object i) => i is Hexadecimal j ? Equals(j) : false;

        public override int GetHashCode() => value.GetHashCode();
    }
}