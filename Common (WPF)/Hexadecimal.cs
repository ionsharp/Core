using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Media;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct Hexadecimal
    {
        /// <summary>
        /// 
        /// </summary>
        public const int Length = 8;

        string _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public Hexadecimal(Color input) : this(input.R, input.G, input.B, input.A) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Hexadecimal(byte r, byte g, byte b, byte? a)
            => _value = (a ?? 255).ToString("X2") + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
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

            _value = input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Hexadecimal(Color input) => new Hexadecimal(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Color(Hexadecimal input)
        {
            var _input = (string)input;
            var a = Parse(_input.Substring(0, 2));
            var r = Parse(_input.Substring(2, 2));
            var g = Parse(_input.Substring(4, 2));
            var b = Parse(_input.Substring(6, 2));
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Hexadecimal(string input) => new Hexadecimal(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator string(Hexadecimal input) => input.ToString();

        static byte Parse(string input)
            => int.Parse(input, NumberStyles.HexNumber).ToByte();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="withAlpha"></param>
        /// <returns></returns>
        public string ToString(bool withAlpha)
            => withAlpha ? _value : _value.Substring(2);
    }
}
