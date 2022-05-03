using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class DoubleBinding : LocalBinding
    {
        public DoubleBinding() : this(".") { }

        public DoubleBinding(string path) : base(path)
        {
            Converter = ObjectToDoubleConverter.Default;
        }
    }
}