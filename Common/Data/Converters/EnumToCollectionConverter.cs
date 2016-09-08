using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Imagin.Common.Extensions;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Enum), typeof(ObservableCollection<Enum>))]
    public class EnumToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            Type EnumType;
            if (value is Enum)
                EnumType = value.GetType();
            else if (value.Is<Type>())
                EnumType = value.As<Type>();
            else return null;

            ObservableCollection<Enum> Result = new ObservableCollection<Enum>();
            foreach (object i in Enum.GetValues(EnumType))
                Result.Add(i.As<Enum>());

            return Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
