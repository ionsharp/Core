using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace Imagin.Common
{
    public class Resources : ResourceDictionary
    {
        public static ResourceDictionary Load(string filePath)
        {
            var result = default(ResourceDictionary);
            using (var fileStream = File.OpenRead(filePath))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                result = (ResourceDictionary)XamlReader.Load(fileStream);
            }
            return result;
        }

        public static ResourceDictionary Load(Uri fileUri)
        {
            using (var stream = Application.GetResourceStream(fileUri).Stream)
                return (ResourceDictionary)XamlReader.Load(stream);
        }

        public static Result TryLoad(string filePath, out ResourceDictionary result)
        {
            try
            {
                result = Load(filePath);
                return new Success();
            }
            catch (Exception e)
            {
                result = default;
                return new Error(e);
            }
        }

        //...

        public static ResourceDictionary New(string assemblyName, string relativePath) => new() { Source = Uri(assemblyName, relativePath) };

        //...

        public static string Read(string assemblyName, string relativePath)
        {
            Uri uri = Uri(assemblyName, relativePath);

            string result = default;
            using (var stream = Application.GetResourceStream(uri).Stream)
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        result += $"{line}\n";
                }
            }
            return result;
        }

        public static string ReadInternal(string relativePath)
            => Read(InternalAssembly.Name, relativePath);

        public static string ReadProject(string relativePath)
            => Read(XAssembly.ShortName(), relativePath);

        //...

        public static void Save(string assemblyName, string resourcePath, string destinationPath)
        {
            using (var fileStream = File.Create(destinationPath))
            using (var stream = Application.GetResourceStream(Uri(assemblyName, resourcePath)).Stream)
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }

        //...

        public static Stream Stream(Uri uri) => Application.GetResourceStream(uri).Stream;

        //...

        public static Uri InternalImage(Images image) => InternalUri($"Images/{image}.png");

        public static Uri InternalUri(string relativePath) => Uri(InternalAssembly.Name, relativePath);

        //...

        public static Uri ProjectImage(string fileName) => ProjectUri($"Images/{fileName}");

        public static Uri ProjectUri(string relativePath) => Uri(XAssembly.ShortName(), relativePath);

        //...

        public static Uri Uri(string assemblyName, string relativePath) => new($"pack://application:,,,/{assemblyName};component/{relativePath}", UriKind.Absolute);
    }
}