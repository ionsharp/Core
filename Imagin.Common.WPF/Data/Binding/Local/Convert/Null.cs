using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class NullBinding : LocalBinding
    {
        public NullBinding() : this(".") { }

        public NullBinding(string path) : base(path) => Converter = NullConverter.Default;
    }
}