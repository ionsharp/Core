using Imagin.Common.Linq;
using System;
using System.Windows.Media;

namespace Imagin.Common.Markup
{
    public class CurrentFont : Font
    {
        public CurrentFont(string fileName) : base(XAssembly.ShortName(), $"Fonts/{fileName}") { }
    }

    public class DefaultFont : Font
    {
        public DefaultFont(string fileName) : base(InternalAssembly.Name, $"Fonts/{fileName}") { }
    }

    public class Font : Uri
    {
        public Font(string relativePath) : base(relativePath) { }

        public Font(string assembly, string relativePath) : base(assembly, relativePath) { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var uri = (System.Uri)base.ProvideValue(serviceProvider);

            FontFamily result = default;
            Try.Invoke(() => result = (FontFamily)new FontFamilyConverter().ConvertFromString(uri.OriginalString));
            return result;
        }
    }
}