using Imagin.Common.Linq;
using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace Imagin.Common.Markup
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceExtension : MarkupExtension
    {
        readonly string assembly = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Assembly
        {
            get => assembly;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get => "pack://application:,,,/{0};component/{1}".F(assembly, relativePath);
        }

        readonly string relativePath = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string RelativePath
        {
            get => relativePath;
        }

        readonly Type targetType = typeof(ImageSource);
        /// <summary>
        /// 
        /// </summary>
        public Type TargetType
        {
            get => targetType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Assembly"></param>
        /// <param name="RelativePath"></param>
        public ResourceExtension(string Assembly, string RelativePath) : base()
        {
            assembly = Assembly;
            relativePath = RelativePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");

            if (targetType == typeof(ImageSource))
                return new ImageSourceConverter().ConvertFromString(Path);

            throw new NotSupportedException("The type '{0}' is not supported.".F(targetType?.FullName));
        }
    }
}
