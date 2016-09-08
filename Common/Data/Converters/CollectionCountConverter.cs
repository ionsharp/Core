using Imagin.Common.Extensions;
using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(IList), typeof(int))]
    public class IListCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.Is<IList>())
                return 0;
            int Result = 0;
            foreach (object i in value.As<IList>())
                Result++;
            return Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

