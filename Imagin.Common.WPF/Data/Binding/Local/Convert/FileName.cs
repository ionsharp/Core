using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class FileNameBinding : LocalBinding
    {
        public FileNameBinding() : this(".") { }

        public FileNameBinding(string path) : base(path) => Converter = FileNameConverter.Default;
    }
}