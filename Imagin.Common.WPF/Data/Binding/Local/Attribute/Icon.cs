using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class IconBinding : LocalBinding
    {
        public IconBinding() : this(".") { }

        public IconBinding(string path) : base(path) => Converter = IconConverter.Default;
    }
}