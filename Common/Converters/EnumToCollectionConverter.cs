using Imagin.Common.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

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

            var Result = new ObservableCollection<Enum>();

            if (EnumType.IsEnum)
            {
                foreach (var i in Enum.GetValues(EnumType))
                {
                    var Field = i as Enum;

                    var Attribute = Field.GetAttribute<BrowsableAttribute>();
                    if (Attribute == null || Attribute.Browsable)
                        Result.Add(i.As<Enum>());
                }
            }

            return Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
