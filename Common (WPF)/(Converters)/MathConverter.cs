using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Imagin.Common.Linq;
using Imagin.Common.Linq;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(object), typeof(object))]
    public class MathConverter : IValueConverter
    {
        object Value = null;

        MathOperation Type { get; set; } = MathOperation.Unknown;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Supported = new Type[]
            {
                typeof(short), typeof(int), typeof(long), typeof(double), typeof(decimal)
            };

            if 
            (
                value != null && 
                value.IsAny(Supported) && 
                parameter != null &&
                parameter.IsAny(Supported)
            )
            {
                var a = System.Convert.ToDouble(value);
                var b = System.Convert.ToDouble(parameter);

                switch (Type)
                {
                    case MathOperation.Add:
                        return a + b;
                    case MathOperation.Divide:
                        return a / b;
                    case MathOperation.Multiply:
                        return a * b;
                    case MathOperation.Subtract:
                        return a - b;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public MathConverter(MathOperation type, object value) : base()
        {
            Type = type;
            Value = value;
        }
    }
}
