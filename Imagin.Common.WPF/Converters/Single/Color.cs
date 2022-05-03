using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Color), typeof(Color))]
    public class ColorWithoutAlphaConverter : Converter<Color, Color>
    {
        public static ColorWithoutAlphaConverter Default { get; private set; } = new ColorWithoutAlphaConverter();
        ColorWithoutAlphaConverter() { }

        protected override ConverterValue<Color> ConvertTo(ConverterData<Color> input) => input.Value.A(255);

        protected override ConverterValue<Color> ConvertBack(ConverterData<Color> input) => Nothing.Do;
    }

    [ValueConversion(typeof(System.Drawing.Color), typeof(Color))]
    public class ColorToColorConverter : Converter<System.Drawing.Color, Color>
    {
        public static ColorToColorConverter Default { get; private set; } = new ColorToColorConverter();
        ColorToColorConverter() { }

        protected override ConverterValue<Color> ConvertTo(ConverterData<System.Drawing.Color> input) => input.Value.Double();

        protected override ConverterValue<System.Drawing.Color> ConvertBack(ConverterData<Color> input) => input.Value.Int32();
    }
    
    [ValueConversion(typeof(Hexadecimal), typeof(Color))]
    public class HexadecimalToColorConverter : Converter<Hexadecimal, Color>
    {
        public static HexadecimalToColorConverter Default { get; private set; } = new HexadecimalToColorConverter();
        HexadecimalToColorConverter() { }

        protected override ConverterValue<Color> ConvertTo(ConverterData<Hexadecimal> input) => input.Value.Color();

        protected override ConverterValue<Hexadecimal> ConvertBack(ConverterData<Color> input) => input.Value.Hexadecimal();
    }

    [ValueConversion(typeof(SolidColorBrush), typeof(Color))]
    public class SolidColorBrushToColorConverter : Converter<SolidColorBrush, Color>
    {
        public static SolidColorBrushToColorConverter Default { get; private set; } = new SolidColorBrushToColorConverter();
        SolidColorBrushToColorConverter() { }

        protected override ConverterValue<Color> ConvertTo(ConverterData<SolidColorBrush> input) => input.Value.Color;

        protected override ConverterValue<SolidColorBrush> ConvertBack(ConverterData<Color> input) => new SolidColorBrush(input.Parameter == 0 ? input.Value : input.Value.A(255));
    }

    [ValueConversion(typeof(StringColor), typeof(Color))]
    public class StringColorConverter : Converter<StringColor, Color>
    {
        public static StringColorConverter Default { get; private set; } = new StringColorConverter();
        StringColorConverter() { }

        protected override ConverterValue<Color> ConvertTo(ConverterData<StringColor> input) => input.Value.Value;

        protected override ConverterValue<StringColor> ConvertBack(ConverterData<Color> input) => new StringColor(input.Value);
    }
}