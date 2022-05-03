using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class InvertBinding : LocalBinding
    {
        public InvertBinding() : this(".") { }

        public InvertBinding(string path) : base(path) => Converter = InverseBooleanConverter.Default;
    }
}