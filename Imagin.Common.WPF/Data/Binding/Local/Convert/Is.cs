using Imagin.Common.Converters;
using System;

namespace Imagin.Common.Data
{
    public class IsBinding : LocalBinding
    {
        public Type Type
        {
            set => ConverterParameter = value;
        }

        public IsBinding() : this(".", null) { }

        public IsBinding(Type type) : this(".", type) { }

        public IsBinding(string path, Type type) : base(path)
        {
            Converter = IsConverter.Default;
            Type = type;
        }
    }
}