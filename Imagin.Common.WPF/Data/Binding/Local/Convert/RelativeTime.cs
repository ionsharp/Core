using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class RelativeTimeBinding : LocalBinding
    {
        public RelativeTimeBinding() : this(".") { }

        public RelativeTimeBinding(string path) : base(path)
        {
            Converter = RelativeTimeConverter.Default;
        }
    }
}