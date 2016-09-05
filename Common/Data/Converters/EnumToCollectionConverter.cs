using System;
using System.Collections.ObjectModel;
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

            Type Type;
            if (value is Enum) Type = value.GetType();
            else if (value is Type) Type = (Type)value;
            else return null;

            ObservableCollection<Enum> Items = new ObservableCollection<Enum>();
            Array EnumValues = Enum.GetValues(Type);
            foreach (object Value in EnumValues)
                Items.Add(Value as Enum);

            return Items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
