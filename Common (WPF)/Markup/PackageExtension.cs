using Imagin.Common.Linq;
using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    /// <summary>
    /// 
    /// </summary>
    public class PackageExtension : MarkupExtension
    {
        string assembly = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Assembly
        {
            get
            {
                return assembly;
            }
        }

        string relativePath = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string RelativePath
        {
            get
            {
                return relativePath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Assembly"></param>
        /// <param name="RelativePath"></param>
        public PackageExtension(string Assembly, string RelativePath) : base()
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
            return new Uri("pack://application:,,,/{0};component/{1}".F(assembly, relativePath), UriKind.RelativeOrAbsolute);
        }
    }
}
