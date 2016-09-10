using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string Capitalize(this string Value)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Value.ToLower());
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string GetDirectoryName(this string Path)
        {
            return System.IO.Path.GetDirectoryName(Path);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string GetExtension(this string Path)
        {
            return System.IO.Path.GetExtension(Path);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string GetFileName(this string Path)
        {
            var Name = System.IO.Path.GetFileName(Path);
            return string.IsNullOrEmpty(Name) ? (Path.EndsWith(@":\") ? Path : string.Empty) : Name;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string GetFileNameWithoutExtension(this string Path)
        {
            var Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            return string.IsNullOrEmpty(Name) ? Path.EndsWith(@":\") ? Path : string.Empty : Name;
        }

        /// <summary>
        /// Imagin.Common: Returns a Uri that represents a resource.
        /// </summary>
        public static Uri GetResourceUri(this string AssemblyName, string ResourcePath)
        {
            return new Uri("pack://application:,,,/" + AssemblyName + ";component/" + ResourcePath, UriKind.Absolute);
        }

        /// <summary>
        /// Imagin.Common: Returns a BitmapImage from resource Uri.
        /// </summary>
        public static BitmapImage GetResource(this Uri Uri)
        {
            return new BitmapImage(Uri);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static bool IsAlphaNumeric(this string Text)
        {
            return Regex.IsMatch(Text, @"^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static bool IsDouble(this string ToCheck)
        {
            double n;
            return double.TryParse(ToCheck, out n);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static bool IsInt(this string ToCheck)
        {
            int n;
            return int.TryParse(ToCheck, out n);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static bool IsLong(this string ToCheck)
        {
            long n;
            return long.TryParse(ToCheck, out n);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static bool IsShort(this string ToCheck)
        {
            short n;
            return short.TryParse(ToCheck, out n);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string SplitCamel(this string ToSplit)
        {
            return Regex.Replace(
                Regex.Replace(ToSplit, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        /// <summary>
        /// Imagin.Common: Parses string to boolean.
        /// </summary>
        public static string SplitCamelCase(this string str)
        {
            return str.SplitCamel();
        }

        /// <summary>
        /// Imagin.Common: Parses string to boolean (evaluates "true" and "false"; everything else is parsed to an int).
        /// </summary>
        public static bool ToBool(this string ToConvert)
        {
            string toConvert = ToConvert.ToLower();
            return toConvert == "true" ? true : toConvert == "false" ? false : ToConvert.ToInt() == 0 ? false : true;
        }

        /// <summary>
        /// Imagin.Common: Parses string to byte.
        /// </summary>
        public static byte ToByte(this string ToConvert)
        {
            byte Value = default(byte);
            byte.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Imagin.Common: Parses string to decimal.
        /// </summary>
        public static decimal ToDecimal(this string ToConvert)
        {
            decimal Value = default(decimal);
            decimal.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Imagin.Common: Parses string to double.
        /// </summary>
        public static double ToDouble(this string ToConvert)
        {
            double Value = default(double);
            double.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Imagin.Common: Parses string to int.
        /// </summary>
        public static int ToInt(this string ToConvert)
        {
            int Value = default(int);
            int.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Imagin.Common: Parses string to long.
        /// </summary>
        public static long ToLong(this string ToConvert)
        {
            long Value = default(long);
            long.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Imagin.Common: Parses string to short.
        /// </summary>
        public static short ToShort(this string ToConvert)
        {
            short Value = default(short);
            short.TryParse(ToConvert, out Value);
            return Value;
        }

        public static Version ToVersion(this string raw, char Delimiter = '.')
        {
            int major = 0, minor = 0, build = 0;
            string[] tokens = raw.Split(Delimiter);
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
