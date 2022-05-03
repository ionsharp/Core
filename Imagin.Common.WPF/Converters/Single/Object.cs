using Imagin.Common.Linq;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object), typeof(object))]
    public class NullConverter : Converter<object, object>
    {
        public static NullConverter Default { get; private set; } = new NullConverter();
        NullConverter() { }

        protected override bool AllowNull => true;

        ConverterValue<object> GetResult(object value)
        {
            if (value == null || (value is string i && i.TrimWhitespace().Empty()))
                return Nothing.Do;

            return value;
        }

        protected override ConverterValue<object> ConvertTo(ConverterData<object> input)
            => GetResult(input.ActualValue);

        protected override ConverterValue<object> ConvertBack(ConverterData<object> input)
            => GetResult(input.ActualValue);
    }
}