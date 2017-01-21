using Imagin.Common.Debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        #region Private

        /// <summary>
        /// Gets the directory name of a path formatted for a FTP server.
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>The parent directory path</returns>
        static string GetFtpDirectoryName(this string Path)
        {
            var tpath = (Path == null ? "" : Path.GetFtpPath());
            int lastslash = -1;

            if (tpath.Length == 0 || tpath == "/")
                return "/";

            lastslash = tpath.LastIndexOf('/');
            if (lastslash < 0)
                return ".";

            return tpath.Substring(0, lastslash);
        }

        /// <summary>
        /// Gets the file name from the path.
        /// </summary>
        /// <param name="path">The full path to the file</param>
        /// <returns>The file name</returns>
        static string GetFtpFileName(this string Path)
        {
            var Result = (Path == null ? null : Path);
            var LastSlash = -1;

            if (Result != null)
            {
                LastSlash = Result.LastIndexOf('/');
                if (LastSlash < 0) return Result;

                LastSlash += 1;
                if (LastSlash >= Result.Length) return Result;

                return Result.Substring(LastSlash, Result.Length - LastSlash);
            }
            return null;
        }

        /// <summary>
        /// Converts the specified path into a valid FTP file system path.
        /// </summary>
        /// <param name="path">The file system path</param>
        /// <returns>A path formatted for FTP</returns>
        static string GetFtpPath(this string Path)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                Path = Regex.Replace(Path.Replace('\\', '/'), "[/]+", "/").TrimEnd('/');
                return Path.Length == 0 ? "/" : Path;
            }
            return "./";
        }

        #endregion

        #region Public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToAppend"></param>
        /// <param name="Append"></param>
        /// <returns></returns>
        public static string Append(this string ToAppend, params string[] Append)
        {
            string Result = ToAppend;
            foreach (string i in Append)
                Result += i;
            return Result;
        }

        /// <summary>
        /// Get string after a.
        /// </summary>
        public static string After(this string value, string a)
        {
            var pos_a = value.LastIndexOf(a);

            if (pos_a == -1)
                return string.Empty;

            var adjusted = pos_a + a.Length;

            return adjusted >= value.Length ? string.Empty : value.Substring(adjusted);
        }

        /// <summary>
        /// Get string between a and b.
        /// </summary>
        public static string Between(this string value, string a, string b)
        {
            var pos_a = value.IndexOf(a);
            var pos_b = value.LastIndexOf(b);

            if (pos_a == -1)
                return string.Empty;

            if (pos_b == -1)
                return string.Empty;

            var adjusted = pos_a + a.Length;

            return adjusted >= pos_b ? string.Empty : value.Substring(adjusted, pos_b - adjusted);
        }

        /// <summary>
        /// Get string before a a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            var pos_a = value.IndexOf(a);
            return pos_a == -1 ? string.Empty : value.Substring(0, pos_a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string Capitalize(this string Value)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Value.ToLower());
        }

        public static bool DirectoryExists(this string DirectoryPath)
        {
            return Directory.Exists(DirectoryPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        /// <param name="Args"></param>
        /// <returns></returns>
        public static string F(this string Format, params object[] Args)
        {
            return string.Format(Format, Args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Format"></param>
        /// <param name="Args"></param>
        /// <returns></returns>
        public static string Format(this string Format, params object[] Args)
        {
            return Format.F(Args);
        }

        public static bool FileExists(this string FilePath)
        {
            return File.Exists(FilePath);
        }

        public static string GetDirectoryName(this string Path, string Scheme = null)
        {
            var Result = Path;
            switch (Scheme)
            {
                case "":
                case null:
                    Result = System.IO.Path.GetDirectoryName(Result);
                    break;
                default:
                    if (Scheme == Uri.UriSchemeFtp)
                        Result = Path.GetFtpDirectoryName();
                    break;
            }
            return Result;
        }

        public static string GetExtension(this string Path)
        {
            return System.IO.Path.GetExtension(Path);
        }

        public static string GetFileName(this string Path, string Scheme = null)
        {
            var Result = Path;
            if (!Result.EndsWith(@":\"))
            {
                switch (Scheme)
                {
                    case "":
                    case null:
                        Result = System.IO.Path.GetFileName(Path);
                        break;
                    default:
                        if (Scheme == Uri.UriSchemeFtp)
                            Result = Path.GetFtpFileName();
                        break;
                }
            }
            return Result;
        }

        public static string GetFileNameWithoutExtension(this string Path)
        {
            var Name = System.IO.Path.GetFileNameWithoutExtension(Path);
            return string.IsNullOrEmpty(Name) ? Path.EndsWith(@":\") ? Path : string.Empty : Name;
        }

        public static string GetFileType(this string Path, Func<string, string> GetTypeDescription)
        {
            var Result = Path.EndsWith(@":\") ? "Drive" : (Path.DirectoryExists() ? "Folder" : (!Path.IsNullOrEmpty() ? GetTypeDescription(Path) : Path));
            return Result.IsNullOrEmpty() ? Path.GetFileNameWithoutExtension() : Result;
        }

        /// <summary>
        /// Returns a Uri that represents a resource.
        /// </summary>
        public static Uri GetResourceUri(this string AssemblyName, string ResourcePath)
        {
            return new Uri("pack://application:,,,/" + AssemblyName + ";component/" + ResourcePath, UriKind.Absolute);
        }

        /// <summary>
        /// Returns a BitmapImage from resource Uri.
        /// </summary>
        public static BitmapImage GetResource(this Uri Uri)
        {
            return new BitmapImage(Uri);
        }

        public static Type FindType(this string TypeName)
        {
            var type = Type.GetType(TypeName);

            if (type != null)
                return type;

            foreach (var i in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = i.GetType(TypeName);
                if (type != null)
                    return type;
            }

            return null;
        }

        public static bool IsAlphaNumeric(this string Text)
        {
            return Regex.IsMatch(Text, @"^[a-zA-Z0-9]+$");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCheck"></param>
        /// <returns></returns>
        public static bool IsDouble(this string ToCheck)
        {
            double n;
            return double.TryParse(ToCheck, out n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToEvaluate"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string ToEvaluate)
        {
            return ToEvaluate.Length == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCheck"></param>
        /// <returns></returns>
        public static bool IsInt(this string ToCheck)
        {
            int n;
            return int.TryParse(ToCheck, out n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCheck"></param>
        /// <returns></returns>
        public static bool IsLong(this string ToCheck)
        {
            long n;
            return long.TryParse(ToCheck, out n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToEvaluate"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string ToEvaluate)
        {
            return string.IsNullOrEmpty(ToEvaluate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToEvaluate"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string ToEvaluate)
        {
            return string.IsNullOrWhiteSpace(ToEvaluate) || ToEvaluate.All(char.IsWhiteSpace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCheck"></param>
        /// <returns></returns>
        public static bool IsShort(this string ToCheck)
        {
            short n;
            return short.TryParse(ToCheck, out n);
        }

        public static bool IsValidUrl(this string ToEvaluate, params string[] Schemes)
        {
            Uri Uri;
            return Uri.TryCreate(ToEvaluate, UriKind.Absolute, out Uri) && (Schemes.Length > 0 ? Uri.Scheme.EqualsAny(Schemes) : Uri.Scheme.EqualsAny(Uri.UriSchemeFile, Uri.UriSchemeFtp, Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeMailto));
        }

        public static string Prepend(this string ToPrepend, params string[] Prepend)
        {
            string Result = string.Empty;
            foreach (string i in Prepend)
                Result += i;
            Result += ToPrepend;
            return Result;
        }

        /// <summary>
        /// Parses string to boolean.
        /// </summary>
        public static string SplitCamelCase(this string ToConvert)
        {
            return Regex.Replace(Regex.Replace(ToConvert, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        /// <summary>
        /// Attempt to run program at path.
        /// </summary>
        public static Result TryRun(this string Value, string Parameter = null)
        {
            try
            {
                System.Diagnostics.Process.Start(Value, Parameter);
                return new Success();
            }
            catch (Exception e)
            {
                return new Error(e);
            }
        }

        /// <summary>
        /// Parses string to boolean (evaluates "true" and "false"; everything else is parsed to an int).
        /// </summary>
        public static bool? ToBool(this string ToConvert)
        {
            switch (ToConvert.ToLower())
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

        /// <summary>
        /// Parses string to byte.
        /// </summary>
        public static byte ToByte(this string ToConvert)
        {
            byte Value = default(byte);
            byte.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static char ToChar(this string Value)
        {
            var Result = default(char);
            char.TryParse(Value, out Result);
            return Result;
        }

        /// <summary>
        /// Parses string to short.
        /// </summary>
        public static short ToInt16(this string ToConvert)
        {
            short Value = default(short);
            short.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Parses string to int.
        /// </summary>
        public static int ToInt32(this string ToConvert)
        {
            int Value = default(int);
            int.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Parses string to long.
        /// </summary>
        public static long ToInt64(this string ToConvert)
        {
            long Value = default(long);
            long.TryParse(ToConvert, out Value);
            return Value;
        }

        public static IEnumerable<int> ToInt32Array(this string Value, char Separator = ',')
        {
            if (String.IsNullOrEmpty(Value))
                yield break;

            foreach (var s in Value.Split(Separator))
            {
                int n;
                if (int.TryParse(s, out n))
                    yield return n;
            }
        }

        /// <summary>
        /// Parses string to DateTime.
        /// </summary>
        public static DateTime ToDateTime(this string ToConvert)
                {
                    DateTime Value = default(DateTime);
                    DateTime.TryParse(ToConvert, out Value);
                    return Value;
                }

        /// <summary>
        /// Parses string to decimal.
        /// </summary>
        public static decimal ToDecimal(this string ToConvert)
        {
            decimal Value = default(decimal);
            decimal.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Parses string to double.
        /// </summary>
        public static double ToDouble(this string ToConvert)
        {
            double Value = default(double);
            double.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Parses string to int.
        /// </summary>
        [Obsolete("Use ToInt16 instead.")]
        public static int ToInt(this string ToConvert)
        {
            int Value = default(int);
            int.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Parses string to long.
        /// </summary>
        [Obsolete("Use ToInt32 instead.")]
        public static long ToLong(this string ToConvert)
        {
            long Value = default(long);
            long.TryParse(ToConvert, out Value);
            return Value;
        }

        public static SecureString ToSecureString(this string ToConvert)
        {
            var Result = new SecureString();
            if (!ToConvert.IsNullOrWhiteSpace())
            {
                foreach (char c in ToConvert)
                    Result.AppendChar(c);
            }
            return Result;
        }

        /// <summary>
        /// Parses string to short.
        /// </summary>
        [Obsolete("Use ToInt16 instead.")]
        public static short ToShort(this string ToConvert)
        {
            short Value = default(short);
            short.TryParse(ToConvert, out Value);
            return Value;
        }

        /// <summary>
        /// Converts alphanumeric hexadecimal to SolidColorBrush.
        /// </summary>
        public static SolidColorBrush ToSolidColorBrush(this string ToConvert)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(ToConvert.StartsWith("#") ? ToConvert : "#" + ToConvert));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="Delimiter"></param>
        /// <returns></returns>
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

        #endregion
    }
}
