using Imagin.Common.Converters;

namespace Imagin.Common.Data
{
    public class EqualsBinding : LocalBinding
    {
        public object Parameter
        {
            set => ConverterParameter = value;
        }

        public EqualsBinding() : this(".", null) { }

        public EqualsBinding(object parameter) : this(".", parameter) { }

        public EqualsBinding(string path, object parameter) : base(path)
        {
            Converter = ValueEqualsParameterConverter.Default;
            Parameter = parameter;
        }
    }
}