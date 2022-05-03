using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class GraphicUnitStringMultiConverter : MultiConverter<string>
    {
        public static GraphicUnitStringMultiConverter Default { get; private set; } = new GraphicUnitStringMultiConverter();
        GraphicUnitStringMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 2 && values[0]?.IsAny(typeof(double), typeof(double?), typeof(int), typeof(int?)) == true && values[1] is GraphicUnit)
            {
                var value = double.Parse(values[0].ToString());

                var funit = parameter is GraphicUnit ? (GraphicUnit)parameter : GraphicUnit.Pixel;
                var tunit = values[1].As<GraphicUnit>();

                var resolution = values.Length > 2 ? (float)values[2] : 72f;
                var places = values.Length > 3 ? (int)values[3] : 3;

                return $"{value.Convert(funit, tunit, resolution).Round(places)} {tunit.GetAttribute<AbbreviationAttribute>().Abbreviation}";
            }
            return string.Empty;
        }
    }
}