using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class IsNullBinding : LocalBinding
    {
        public IsNullBinding() : this(".") { }

        public IsNullBinding(string path) : base(path) => Converter = IsNullConverter.Default;
    }
}