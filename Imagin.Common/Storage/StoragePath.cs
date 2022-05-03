using Imagin.Common.Linq;
using System;
using System.IO;
using System.Linq;

namespace Imagin.Common.Storage
{
    public class StoragePath
    {
        public const string Root = @"\";

        public const string RootName = "This PC";

        //...

        public static char[] InvalidFileNameCharacters => Path.GetInvalidFileNameChars();

        //...

        public static string CleanName(string fileName) => Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));

        //...

        public const string DefaultCloneFormat = "{0} [{1}]";

        /// <summary>
        /// Gets a clone of the given path using the given name format.
        /// </summary>
        public static string Clone(string path, string nameFormat, Predicate<string> exists)
        {
            var parent = Path.GetDirectoryName(path);

            var extension = Path.GetExtension(path);
            var name = Path.GetFileNameWithoutExtension(path);

            var n = name;
            string result() => $@"{parent}\{n}{extension}".Replace(@"\\", @"\");

            var i = 0;
            while (exists(result()))
            {
                n = nameFormat.F(name, i);
                i++;
            }

            return result();
        }

        //...
        /// <summary>
        /// Enumerates the given <see cref="string"/> starting with the last <see cref="char"/>; gets everything after first period, if one is found.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetLastExtension(string path)
        {
            if (path.NullOrEmpty())
                return null;

            var f = string.Empty;
            for (var i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '.')
                    return f.Empty() ? null : f;

                f = $"{path[i]}{f}";
            }
            return null;
        }
    }
}