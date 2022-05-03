using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    public class MultiConverterData
    {
        public readonly CultureInfo Culture;

        public readonly object[] Values;

        public readonly object Parameter;

        public readonly Type TargetType;

        public MultiConverterData(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Values = values;
            TargetType = targetType;
            Parameter = parameter;
            Culture = culture;
        }
    }

    [ValueConversion(typeof(object[]), typeof(object))]
    public class MultiConverter<Result> : IMultiValueConverter
    {
        readonly Func<MultiConverterData, Result> To;

        public MultiConverter() : base() { }

        public MultiConverter(Func<MultiConverterData, Result> to) => To = to;

        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => To.Invoke(new MultiConverterData(values, targetType, parameter, culture));

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class DefaultMultiConverter : MultiConverter<object[]>
    {
        public static DefaultMultiConverter Default { get; private set; } = new();
        DefaultMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values;
    }
}