using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class DescriptionBinding : LocalBinding
    {
        public DescriptionBinding() : this(".") { }

        public DescriptionBinding(string path) : base(path) => Converter = DescriptionConverter.Default;
    }
}