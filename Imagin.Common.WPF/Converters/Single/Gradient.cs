using Imagin.Common.Media;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Gradient), typeof(LinearGradientBrush))]
    public class GradientConverter : Converter<Gradient, LinearGradientBrush>
    {
        public static GradientConverter Default { get; private set; } = new GradientConverter();
        GradientConverter() { }

        protected override ConverterValue<LinearGradientBrush> ConvertTo(ConverterData<Gradient> input) => input.Value.LinearBrush();

        protected override ConverterValue<Gradient> ConvertBack(ConverterData<LinearGradientBrush> input) => new Gradient(input.Value);
    }

    [ValueConversion(typeof(LinearGradientBrush), typeof(Gradient))]
    public class LinearGradientBrushConverter : Converter<LinearGradientBrush, Gradient>
    {
        public static LinearGradientBrushConverter Default { get; private set; } = new LinearGradientBrushConverter();
        LinearGradientBrushConverter() { }

        protected override ConverterValue<Gradient> ConvertTo(ConverterData<LinearGradientBrush> input) => new Gradient(input.Value);

        protected override ConverterValue<LinearGradientBrush> ConvertBack(ConverterData<Gradient> input) => input.Value.LinearBrush();
    }

    [ValueConversion(typeof(RadialGradientBrush), typeof(Gradient))]
    public class RadialGradientBrushConverter : Converter<RadialGradientBrush, Gradient>
    {
        public static RadialGradientBrushConverter Default { get; private set; } = new RadialGradientBrushConverter();
        RadialGradientBrushConverter() { }

        protected override ConverterValue<Gradient> ConvertTo(ConverterData<RadialGradientBrush> input) => new Gradient(input.Value);

        protected override ConverterValue<RadialGradientBrush> ConvertBack(ConverterData<Gradient> input) => input.Value.RadialBrush();
    }
}