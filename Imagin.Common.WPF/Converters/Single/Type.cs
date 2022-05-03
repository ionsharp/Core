using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object), typeof(Type))]
    public class ObjectToTypeConverter : Converter<object, Type>
    {
        public static ObjectToTypeConverter Default { get; private set; } = new ObjectToTypeConverter();
        ObjectToTypeConverter() { }

        protected override ConverterValue<Type> ConvertTo(ConverterData<object> input) => input.Value.GetType();

        protected override ConverterValue<object> ConvertBack(ConverterData<Type> input) => Nothing.Do;
    }
}