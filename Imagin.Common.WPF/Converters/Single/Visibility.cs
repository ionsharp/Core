using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Visibility), typeof(Visibility))]
    public class InverseVisibilityConverter : Converter<Visibility, Visibility>
    {
        public static InverseVisibilityConverter Default { get; private set; } = new InverseVisibilityConverter();
        InverseVisibilityConverter() { }

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<Visibility> input) => input.Value.Invert();

        protected override ConverterValue<Visibility> ConvertBack(ConverterData<Visibility> input) => input.Value.Invert();
    }

    //...

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : Converter<object, object>
    {
        public static BooleanToVisibilityConverter Default { get; private set; } = new BooleanToVisibilityConverter();
        BooleanToVisibilityConverter() { }

        protected override bool Is(object input) => input is bool || input is bool? || input is Handle || input is Visibility;

        protected override ConverterValue<object> ConvertTo(ConverterData<object> input)
        {
            if (input.ActualValue is bool || input.ActualValue is bool?)
            {
                var i = input.ActualValue is bool ? (bool)input.ActualValue : input.ActualValue is bool? ? ((bool?)input.ActualValue).Value : throw new NotSupportedException();
                var result = i.Visibility(input.ActualParameter is Visibility ? (Visibility)input.ActualParameter : Visibility.Collapsed);

                return input.Parameter == 0
                    ? result
                    : input.Parameter == 1
                        ? result.Invert()
                        : throw input.InvalidParameter;
            }
            return ConvertBack(input);
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<object> input)
        {
            if (input.ActualValue is Visibility visibility)
            {
                var result = ((Visibility)input.ActualValue).Boolean();
                return input.Parameter == 0
                    ? result
                    : input.Parameter == 1
                        ? !result
                        : throw input.InvalidParameter;
            }

            return ConvertTo(input);
        }
    }

    [ValueConversion(typeof(double), typeof(Visibility))]
    public class DoubleToVisibilityConverter : Converter<double, Visibility>
    {
        public static DoubleToVisibilityConverter Default { get; private set; } = new DoubleToVisibilityConverter();
        DoubleToVisibilityConverter() { }

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<double> input)
        {
            var result = (input.Value > 0).Visibility();
            return input.Parameter == 0 ? result : input.Parameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override ConverterValue<double> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    public class Int32ToVisibilityConverter : Converter<int, Visibility>
    {
        public static Int32ToVisibilityConverter Default { get; private set; } = new Int32ToVisibilityConverter();
        Int32ToVisibilityConverter() { }

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<int> input)
        {
            var result = (input.Value > 0).Visibility();
            return input.Parameter == 0 ? result : input.Parameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override ConverterValue<int> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(long), typeof(Visibility))]
    public class Int64ToVisibilityConverter : Converter<long, Visibility>
    {
        public static Int64ToVisibilityConverter Default { get; private set; } = new Int64ToVisibilityConverter();
        Int64ToVisibilityConverter() { }

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<long> input)
        {
            var result = (input.Value > 0).Visibility();
            return input.Parameter == 0 ? result : input.Parameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override ConverterValue<long> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ObjectToVisibilityConverter : Converter<object, Visibility>
    {
        public static ObjectToVisibilityConverter Default { get; private set; } = new ObjectToVisibilityConverter();
        ObjectToVisibilityConverter() { }

        protected override bool AllowNull => true;

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<object> input)
        {
            var result = (input.ActualValue != null).Visibility();
            return input.Parameter == 0 ? result : input.Parameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ObjectIsToVisibilityConverter : Converter<object, Visibility>
    {
        public static ObjectIsToVisibilityConverter Default { get; private set; } = new ObjectIsToVisibilityConverter();
        ObjectIsToVisibilityConverter() { }

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<object> input)
        {
            if (input.ActualParameter is Type i)
                return input.Value.GetType().IsSubclassOf(i).Visibility();

            return Nothing.Do;
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Orientation), typeof(Visibility))]
    public class OrientationToVisibilityConverter : Converter<Orientation, Visibility>
    {
        public static OrientationToVisibilityConverter Default { get; private set; } = new OrientationToVisibilityConverter();
        OrientationToVisibilityConverter() { }

        Visibility Convert(Orientation input) => input == Orientation.Horizontal ? Visibility.Visible : Visibility.Collapsed;

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<Orientation> input)
        {
            return input.Parameter == 0 ? Convert(input.Value) : input.Parameter == 1 ? Convert(input.Value).Invert() : throw input.InvalidParameter;
        }

        protected override ConverterValue<Orientation> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(Visibility))]
    public class StringToVisibilityConverter : Converter<string, Visibility>
    {
        public static StringToVisibilityConverter Default { get; private set; } = new StringToVisibilityConverter();
        StringToVisibilityConverter() { }

        protected override bool AllowNull => true;

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<string> input)
        {
            var result = input.Value.NullOrEmpty().Invert().Visibility();
            return input.Parameter == 0 ? result : input.Parameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    //...

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class ValueEqualsParameterVisibilityConverter : Converter<object, Visibility>
    {
        public static ValueEqualsParameterVisibilityConverter Default { get; private set; } = new ValueEqualsParameterVisibilityConverter();
        protected ValueEqualsParameterVisibilityConverter() { }

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<object> input) => input.Value.Equals(input.ActualParameter).Visibility();

        protected override ConverterValue<object> ConvertBack(ConverterData<Visibility> input) => input.ActualParameter;
    }
}