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
using Imagin.Common.Linq;

namespace Imagin.Common.Linq
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
        /// <param name="Value"></param>
        /// <param name="Append"></param>
        /// <returns></returns>
        public static string Append(this string Value, params string[] Append)
        {
            var Result = Value;
            Append.ForEach(i => Result += i);
            return Result;
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

        /// <summary>
        /// Invokes <see cref="System.IO.Directory.Exists(string)"/>.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool DirectoryExists(this string Path)
        {
            return Directory.Exists(Path);
        }

        /// <summary>
        /// Invokes <see cref="System.IO.File.Exists(string)"/>.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool FileExists(this string Path)
        {
            return File.Exists(Path);
        }

        /// <summary>
        /// Invokes <see cref="System.IO.Path.GetDirectoryName(string)"/>.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Scheme"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Invokes <see cref="System.IO.Path.GetExtension(string)"/>.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="WithoutPeriod"></param>
        /// <returns></returns>
        public static string GetExtension(this string Path, bool WithoutPeriod = false)
        {
            var Result = System.IO.Path.GetExtension(Path);

            if (WithoutPeriod)
                Result = Result.Replace(".", string.Empty);

            return Result;
        }

        /// <summary>
        /// Invokes <see cref="System.IO.Path.GetFileName(string)"/>.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="WithoutExtension"></param>
        /// <param name="Scheme"></param>
        /// <returns></returns>
        public static string GetFileName(this string Path, bool WithoutExtension = false, string Scheme = null)
        {
            var Result = Path;
            if (!Path.IsNullOrEmpty() && !Path.EndsWith(@":\") && !Path.EqualsAny(@"\", "/"))
            {
                switch (Scheme)
                {
                    case "":
                    case null:
                        if (!WithoutExtension)
                        {
                            Result = System.IO.Path.GetFileName(Path);
                        }
                        else Result = System.IO.Path.GetFileNameWithoutExtension(Path);
                        break;
                    default:
                        if (Scheme == Uri.UriSchemeFtp)
                            Result = Path.GetFtpFileName();
                        break;
                }
                
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="GetTypeDescription"></param>
        /// <returns></returns>
        public static string GetFileType(this string Path, Func<string, string> GetTypeDescription)
        {
            var Result = Path.EndsWith(@":\") ? "Drive" : (Path.DirectoryExists() ? "Folder" : (!Path.IsNullOrEmpty() ? GetTypeDescription(Path) : Path));
            return Result.IsNullOrEmpty() ? Path.GetFileName(true) : Result;
        }

        /// <summary>
        /// Returns <see cref="Uri"/> of resource defined in assembly with given path; note, path must NOT begin with slash and slashes must be forward-facing.
        /// </summary>
        public static Uri GetResourceUri(this string AssemblyName, string ResourcePath)
        {
            return new Uri("pack://application:,,,/" + AssemblyName + ";component" + ResourcePath, UriKind.Absolute);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Schemes"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string Value, params string[] Schemes)
        {
            Uri Uri;
            return Uri.TryCreate(Value, UriKind.Absolute, out Uri) && (Schemes.Length > 0 ? Uri.Scheme.EqualsAny(Schemes) : Uri.Scheme.EqualsAny(Uri.UriSchemeFile, Uri.UriSchemeFtp, Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeMailto));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Prepend"></param>
        /// <returns></returns>
        public static string Prepend(this string Value, params string[] Prepend)
        {
            var Result = string.Empty;
            Prepend.ForEach(i => Result += i);
            return Result + Value;
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
        /// Converts alphanumeric hexadecimal to SolidColorBrush.
        /// </summary>
        public static SolidColorBrush ToSolidColorBrush(this string ToConvert)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(ToConvert.StartsWith("#") ? ToConvert : "#" + ToConvert));
        }

        #endregion
    }
}
