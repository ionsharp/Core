using Imagin.Common.Drawing;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;
using Imagin.Common.Linq;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class GraphicalUnitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length >= 2 && values[0].Is<double>() && values[1].Is<GraphicalUnit>())
            {
                var Value = values[0].As<double>();
                var From = parameter == null ? GraphicalUnit.Pixel : (parameter.Is<GraphicalUnit>() ? parameter.As<GraphicalUnit>() : Enum.Parse(typeof(GraphicalUnit), parameter.ToString()).As<GraphicalUnit>());
                var To = values[1].As<GraphicalUnit>();
                var Ppi = values.Length <= 2 || values[2] == null ? 72.0 : values[2].As<double>();
                var RoundTo = values.Length <= 3 || values[3] == null ? 3 : values[3].As<int>();
                return Value.ToUnit(From, To, Ppi).Round(RoundTo).ToString();
            }
            return 0.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
