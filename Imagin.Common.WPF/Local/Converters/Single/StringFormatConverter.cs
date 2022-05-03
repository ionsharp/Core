using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Local.Converters
{
    /// <summary>
    /// Takes the first value as StringFormat and the other values as Parameter for the StringFormat
    /// </summary>
    public class StringFormatConverter : TypeValueConverterBase, IMultiValueConverter
    {
        static MethodInfo miFormat = null;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (miFormat == null)
            {
                try
                {
                    // try to load SmartFormat Assembly
                    var asSmartFormat = Assembly.Load("SmartFormat");
                    var tt = asSmartFormat.GetType("SmartFormat.Smart");
                    miFormat = tt.GetMethod("Format", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string), typeof(object) }, null);
                }
                catch
                {
                    // fallback just take String.Format
                    miFormat = typeof(string).GetMethod("Format", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string), typeof(object) }, null);
                }
            }

            if (targetType != typeof(string))
                throw new Exception("Only string as targettype is allowed");

            if (values == null | values.Length < 1)
                throw new Exception("Not enough parameters");

            if (values[0] == null)
                return null;

            if (values.Length > 1 && values[1] == DependencyProperty.UnsetValue)
                return null;

            return (string)miFormat.Invoke(null, values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        } 
    }
}