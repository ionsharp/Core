using Imagin.Common.Drawing;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(object[]), typeof(string))]
    public class GraphicalUnitConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 2 && values[0] is double && values[1] is GraphicalUnit)
            {
                var value = (double)values[0];

                var funit = parameter is GraphicalUnit ? (GraphicalUnit)parameter : GraphicalUnit.Pixel;
                var tunit = values[1].As<GraphicalUnit>();

                var resolution = values.Length > 2 ? (float)values[2] : 72f;

                var places = values.Length > 3 ? (int)values[3] : 3;

                return value.ToUnit(funit, tunit, resolution).Round(places).ToString();
            }
            return 0.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
