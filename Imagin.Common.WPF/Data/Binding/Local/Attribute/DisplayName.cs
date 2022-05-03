using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class DisplayNameBinding : LocalBinding
    {
        public DisplayNameBinding() : this(".") { }

        public DisplayNameBinding(string path) : base(path) => Converter = DisplayNameConverter.Default;
    }
}