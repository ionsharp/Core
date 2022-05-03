using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security;
using System.Text.RegularExpressions;

namespace Imagin.Common.Linq
{
    public static class XString
    {
        static readonly Regex Whitespace = new Regex(@"\s+");

        //...

        public static string After(this string input, string i)
        {
            var pos_a = input.LastIndexOf(i);

            if (pos_a == -1)
                return string.Empty;

            var adjusted = pos_a + i.Length;

            return adjusted >= input.Length ? string.Empty : input.Substring(adjusted);
        }

        public static bool AlphaNumeric(this string input)
            => Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");

        public static string Before(this string input, string i)
        {
            var result = input.IndexOf(i);
            return result == -1 ? string.Empty : input.Substring(0, result);
        }

        public static string Between(this string input, string a, string b)
        {
            var pos_a = input.IndexOf(a);
            var pos_b = input.LastIndexOf(b);

            if (pos_a == -1)
                return string.Empty;

            if (pos_b == -1)
                return string.Empty;

            var adjusted = pos_a + a.Length;
            return adjusted >= pos_b ? string.Empty : input.Substring(adjusted, pos_b - adjusted);
        }

        public static bool? Boolean(this string input)
        {
            switch (input.ToLower())
            {
                case "true":
                case "t":
                case "1":
                    return true;
                case "false":
                case "f":
                case "0":
                    return false;
            }
            return null;
        }

        public static string Capitalize(this string input)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static void EachLine(this string input, Action<string> action)
        {
            foreach (var i in GetLines(input))
                action(i);
        }

        public static bool Empty(this string input)
            => input.Length == 0;

        public static bool EndsWithAny(this string input, params char[] values)
            => input.EndsWithAny(values.Select(i => i.ToString()).ToArray());

        public static bool EndsWithAny(this string input, params object[] values)
            => input.EndsWithAny(values.Select(i => i.ToString()).ToArray());

        public static bool EndsWithAny(this string input, params string[] values)
        {
            foreach (var i in values)
            {
                if (input.EndsWith(i))
                    return true;
            }
            return false;
        }

        public static IEnumerable<string> GetLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (StringReader reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }
        }

        public static string F(this string input, params object[] arguments) => string.Format(input, arguments);

        /// <summary>
        /// Gets the index of the first given character that is found.
        /// </summary>
        public static int FirstIndex(this string input, char first)
        {
            var index = 0;
            foreach (var i in input)
            {
                if (i == first)
                    return index;

                index++;
            }
            return -1;
        }

        public static int Lines(this string input) => input.Split('\n').Length;

        public static bool NullOrEmpty(this string input) => string.IsNullOrEmpty(input);

        public static bool NullOrWhiteSpace(this string input) => string.IsNullOrWhiteSpace(input);

        /// <summary>
        /// Gets if the given character is the only occuring character.
        /// </summary>
        public static bool OnlyContains(this string input, char character)
        {
            if (input.NullOrEmpty())
                return false;

            foreach (var i in input)
            {
                if (!i.Equals(character))
                    return false;
            }
            return true;
        }

        public static string PadLeft(this string input, char i, int repeat) => input.PadLeft(input.Length + repeat, i);

        public static T Parse<T>(this string input, bool ignoreCase = true) where T : struct, IFormattable, IComparable, IConvertible => (T)Enum.Parse(typeof(T), input, ignoreCase);

        public static bool PositiveNumber(this string input) => Regex.IsMatch(input, @"^[0-9]+$");

        /// <summary>
        /// Gets the number of times the given <see cref="char"/> repeats.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static int Repeats(this string input, char character)
        {
            int result = 0;
            foreach (var i in input)
            {
                if (i.Equals(character))
                    result++;
            }
            return result;
        }

        public static string ReplaceBetween(this string input, char a, char b, string replace)
        {
            int? i0 = null;
            int? i1 = null;
            int length = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (i0 == null)
                {
                    if (input[i] == a)
                        i0 = i;
                }
                else
                {
                    if (input[i] == b)
                    {
                        i1 = i;
                        break;
                    }

                    length++;
                }
            }

            if (i0 != null && i1 != null)
                return input.Substring(0, i0.Value + 1) + replace + input.Substring(i1.Value, input.Length - i1.Value);

            return string.Empty;
        }

        public static string RemoveDigits(this string input) => Regex.Replace(input, @"[\d-]", string.Empty);

        public static string SplitCamel(this string input)
        {
            //New way: Previous character must be lowercase and current character must be uppercase.
            var result = new System.Text.StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                if (result.Length > 0)
                {
                    if (char.IsLetter(input[i - 1]) && char.IsLetter(input[i]))
                    {
                        if (char.IsLower(input[i - 1]) && char.IsUpper(input[i]))
                            result.Append(' ');
                    }
                }
                result.Append(input[i]);
            }
            return result.ToString();
            //Old way: Regex.Replace(Regex.Replace(input, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static int StartRepeats(this string input, char character)
        {
            int result = 0;
            foreach (var i in input)
            {
                if (i.Equals(character))
                    result++;

                else return result;
            }
            return result;
        }

        public static string TrimExtension(this string input)
        {
            var result = string.Empty;
            foreach (var i in input)
            {
                if (char.IsLetterOrDigit(i))
                    result += i;
            }
            return result.ToLower();
        }

        public static string TrimWhitespace(this string input) => Whitespace.Replace(input, string.Empty);

        public static bool TryParse<T>(this string input, out T result, bool ignoreCase = true) where T : struct, IFormattable, IComparable, IConvertible => Enum.TryParse(input, ignoreCase, out result);

        public static int Words(this string input)
        {
            var result = 0;
            Try.Invoke(() => result = input.Split(Array<char>.New(' ', '\r', '\n'), StringSplitOptions.RemoveEmptyEntries).Length);
            return result;
        }

        //...

        public static byte Byte(this string input)
        {
            byte.TryParse(input, out byte result);
            return result;
        }

        public static char Char(this string input)
        {
            char.TryParse(input, out char result);
            return result;
        }

        public static DateTime DateTime(this string input)
        {
            System.DateTime.TryParse(input, out DateTime result);
            return result;
        }

        public static decimal Decimal(this string input)
        {
            decimal.TryParse(input, out decimal result);
            return result;
        }

        public static double Double(this string input)
        {
            double.TryParse(input, out double result);
            return result;
        }

        public static short Int16(this string input)
        {
            short.TryParse(input, out short result);
            return result;
        }

        public static int Int32(this string input)
        {
            int.TryParse(input, out int result);
            return result;
        }

        public static long Int64(this string input)
        {
            long.TryParse(input, out long result);
            return result;
        }

        public static IEnumerable<int> Int32Array(this string input, char separator = ',')
            => input.Int32Array(separator as char?);

        public static IEnumerable<int> Int32Array(this string input, char? separator)
        {
            if (string.IsNullOrEmpty(input))
                yield break;

            if (separator == null)
            {
                foreach (var i in input.ToArray())
                    yield return i.ToString().Int32();
            }
            else
            {
                foreach (var i in input.Split(separator.Value))
                    yield return i.Int32();
            }
        }

        public static SecureString SecureString(this string input)
        {
            var result = new SecureString();
            if (!input.NullOrWhiteSpace())
            {
                foreach (char c in input)
                    result.AppendChar(c);
            }
            return result;
        }

        public static float Single(this string input)
        {
            float.TryParse(input, out float result);
            return result;
        }

        public static TimeSpan TimeSpan(this string input)
        {
            System.TimeSpan.TryParse(input, out TimeSpan result);
            return result;
        }

        public static UDouble UDouble(this string input)
        {
            Common.UDouble.TryParse(input, out UDouble result);
            return result;
        }

        public static ushort UInt16(this string input)
        {
            ushort.TryParse(input, out ushort Result);
            return Result;
        }

        public static uint UInt32(this string input)
        {
            uint.TryParse(input, out uint Result);
            return Result;
        }

        public static ulong UInt64(this string input)
        {
            ulong.TryParse(input, out ulong Result);
            return Result;
        }

        public static Uri Uri(this string input, UriKind kind = UriKind.RelativeOrAbsolute)
        {
            System.Uri.TryCreate(input, kind, out Uri result);
            return result;
        }

        public static Version Version(this string input, char delimiter = '.')
        {
            int major = 0, minor = 0, build = 0;
            string[] tokens = input.Split(delimiter);
            if (tokens.Length > 0)
            {
                int.TryParse(tokens[0], out major);
                if (tokens.Length > 1)
                {
                    int.TryParse(tokens[1], out minor);
                    if (tokens.Length > 2)
                        int.TryParse(tokens[2], out build);
                }
            }
            return new Version(major, minor, build);
        }
    }
}
