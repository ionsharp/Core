using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(string), typeof(FontFamily))]
    public class FontFamilyConverter : Converter<string, FontFamily>
    {
        public static FontFamilyConverter Default { get; private set; } = new FontFamilyConverter();
        FontFamilyConverter() { }

        protected override ConverterValue<FontFamily> ConvertTo(ConverterData<string> input)
        {
            if (input.Value == null)
                return default;

            FontFamily result = null;
            Try.Invoke(() => result = new FontFamily(input.Value));
            return result;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<FontFamily> input)
        {
            return input.Value.Source;
        }
    }
}