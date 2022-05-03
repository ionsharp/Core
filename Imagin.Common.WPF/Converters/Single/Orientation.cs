using Imagin.Common.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Orientation), typeof(Orientation))]
    public class InverseOrientationConverter : Converter<Orientation, Orientation>
    {
        public static InverseOrientationConverter Default { get; private set; } = new();
        public InverseOrientationConverter() : base() { }

        protected override ConverterValue<Orientation> ConvertTo(ConverterData<Orientation> input) => input.Value.Invert();

        protected override ConverterValue<Orientation> ConvertBack(ConverterData<Orientation> input) => input.Value.Invert();
    }
}