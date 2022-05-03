using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public class InverseThicknessConverter : Converter<Thickness, Thickness>
    {
        public static InverseThicknessConverter Default { get; private set; } = new InverseThicknessConverter();
        InverseThicknessConverter() { }

        protected override ConverterValue<Thickness> ConvertTo(ConverterData<Thickness> input) => input.Value.Invert();

        protected override ConverterValue<Thickness> ConvertBack(ConverterData<Thickness> input) => input.Value.Invert();
    }
}