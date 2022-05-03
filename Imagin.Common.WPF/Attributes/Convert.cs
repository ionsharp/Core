using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConvertAttribute : Attribute
    {
        public readonly Type Converter;

        public readonly object ConverterParameter;

        public ConvertAttribute(Type converter, object converterParameter = null)
        {
            Converter
                = converter;
            ConverterParameter
                = converterParameter;
        }
    }
}