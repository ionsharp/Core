using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class AbbreviationBinding : LocalBinding
    {
        public AbbreviationBinding() : this(".") { }

        public AbbreviationBinding(string path) : base(path) => Converter = AbbreviationConverter.Default;
    }
}