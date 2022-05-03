using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class TypeBinding : LocalBinding
    {
        public TypeBinding() : this(".") { }

        public TypeBinding(string path) : base(path)
        {
            Converter = ObjectToTypeConverter.Default;
        }
    }
}