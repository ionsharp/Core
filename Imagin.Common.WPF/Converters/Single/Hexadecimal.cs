using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(SolidColorBrush), typeof(Hexadecimal))]
    public class SolidColorBrushToHexadecimalConverter : Converter<SolidColorBrush, Hexadecimal>
    {
        public static SolidColorBrushToHexadecimalConverter Default { get; private set; } = new SolidColorBrushToHexadecimalConverter();
        SolidColorBrushToHexadecimalConverter() { }

        protected override ConverterValue<Hexadecimal> ConvertTo(ConverterData<SolidColorBrush> input) => input.Value.Color.Hexadecimal();

        protected override ConverterValue<SolidColorBrush> ConvertBack(ConverterData<Hexadecimal> input) => input.Value.SolidColorBrush();
    }
}