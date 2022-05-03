using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [ValueConversion(typeof(SolidColorBrush), typeof(LinearGradientBrush))]
    public class ProgressForegroundConverter : Converter<SolidColorBrush, LinearGradientBrush>
    {
        public static ProgressForegroundConverter Default { get; private set; } = new ProgressForegroundConverter();
        ProgressForegroundConverter() { }

        LinearGradientBrush Convert(SolidColorBrush input)
        {
            var result = new LinearGradientBrush();

            double offset = 0;
            for (int i = 0; i < 5; i++, offset += 0.25)
            {
                var color = input.Color;

                byte alpha = 0;
                switch (i)
                {
                    case 1:
                    case 3:
                        alpha = ((255.0 / 5.0) * 2.0).Byte();
                        break;
                    case 2:
                        alpha = ((255.0 / 5.0) * 3.0).Byte();
                        break;
                }

                color = System.Windows.Media.Colors.White.A(alpha);
                result.GradientStops.Add(new GradientStop(color, offset));
            }

            return result;
        }

        protected override ConverterValue<SolidColorBrush> ConvertBack(ConverterData<LinearGradientBrush> input) => Nothing.Do;

        protected override ConverterValue<LinearGradientBrush> ConvertTo(ConverterData<SolidColorBrush> input)
            => Convert(input.Value);
    }
}