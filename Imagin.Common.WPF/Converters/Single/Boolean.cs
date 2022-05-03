using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(double), typeof(bool))]
    public class DoubleGreaterThanConverter : Converter<double, bool>
    {
        public static DoubleGreaterThanConverter Default { get; private set; } = new DoubleGreaterThanConverter();
        DoubleGreaterThanConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<double> input)
            => input.Value > double.Parse(input.ActualParameter.ToString());
    }

    [ValueConversion(typeof(Enum), typeof(bool))]
    public class HasFlagConverter : Converter<Enum, bool>
    {
        public static HasFlagConverter Default { get; private set; } = new HasFlagConverter();
        HasFlagConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<Enum> input) => input.Value.HasFlag((Enum)input.ActualParameter);

        protected override ConverterValue<Enum> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public class HiddenConverter : Converter<string, bool>
    {
        public static HiddenConverter Default { get; private set; } = new HiddenConverter();
        HiddenConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<string> input) => Computer.Hidden(input.Value);

        protected override ConverterValue<string> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    //...

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : Converter<bool, bool>
    {
        public static InverseBooleanConverter Default { get; private set; } = new InverseBooleanConverter();
        InverseBooleanConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<bool> input) => !input.Value;

        protected override ConverterValue<bool> ConvertBack(ConverterData<bool> input) => !input.Value;
    }

    //...

    [ValueConversion(typeof(object), typeof(bool))]
    public class IsConverter : Converter<object, bool>
    {
        public static IsConverter Default { get; private set; } = new IsConverter();
        IsConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<object> input)
        {
            if (input.ActualParameter is Type i)
                return input.Value.GetType().IsSubclassOf(i) || input.Value.GetType().Equals(i);

            return false;
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : Converter<object, bool>
    {
        public static IsNullConverter Default { get; private set; } = new IsNullConverter();
        IsNullConverter() { }

        protected override bool AllowNull => true;

        protected override ConverterValue<bool> ConvertTo(ConverterData<object> input)
        {
            var result = input.Value == null;
            return input.Parameter == 1 ? !result : result;
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    [ValueConversion(typeof(object), typeof(bool))]
    public class IsStringConverter : Converter<object, bool>
    {
        public static IsStringConverter Default { get; private set; } = new IsStringConverter();
        IsStringConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<object> input) => input.Value is string;

        protected override ConverterValue<object> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    //...

    [ValueConversion(typeof(IEnumerable), typeof(bool))]
    public class IEnumerableToBooleanConverter : Converter<IEnumerable, bool>
    {
        public static IEnumerableToBooleanConverter Default { get; private set; } = new IEnumerableToBooleanConverter();
        IEnumerableToBooleanConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<IEnumerable> input) => !input.Value.Empty();

        protected override ConverterValue<IEnumerable> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    [ValueConversion(typeof(IList), typeof(bool))]
    public class IListToBooleanConverter : Converter<IList, bool>
    {
        public static IListToBooleanConverter Default { get; private set; } = new IListToBooleanConverter();
        IListToBooleanConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<IList> input) => input.Value.Count > 0;

        protected override ConverterValue<IList> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBooleanConverter : Converter<int, bool>
    {
        public static IntToBooleanConverter Default { get; private set; } = new IntToBooleanConverter();
        IntToBooleanConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<int> input) => input.Value > 0;

        protected override ConverterValue<int> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Orientation), typeof(bool))]
    public class OrientationToBooleanConverter : Converter<Orientation, bool>
    {
        public static OrientationToBooleanConverter Default { get; private set; } = new OrientationToBooleanConverter();
        OrientationToBooleanConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<Orientation> input) => input.Value == (Orientation)Enum.Parse(typeof(Orientation), (string)input.ActualParameter);

        protected override ConverterValue<Orientation> ConvertBack(ConverterData<bool> input) => input.Value ? (Orientation)Enum.Parse(typeof(Orientation), (string)input.ActualParameter) : default;
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public class StringToBooleanConverter : Converter<string, bool>
    {
        public static StringToBooleanConverter Default { get; private set; } = new StringToBooleanConverter();
        StringToBooleanConverter() { }

        protected override bool AllowNull => true;

        protected override ConverterValue<bool> ConvertTo(ConverterData<string> input) => input.Value.NullOrEmpty() == false;

        protected override ConverterValue<string> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    //...

    [ValueConversion(typeof(object), typeof(bool))]
    public class ValueEqualsParameterConverter : Converter<object, bool>
    {
        public static ValueEqualsParameterConverter Default { get; private set; } = new ValueEqualsParameterConverter();
        protected ValueEqualsParameterConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<object> input) => input.Value.Equals(input.ActualParameter);

        protected override ConverterValue<object> ConvertBack(ConverterData<bool> input)
        {
            if (!input.Value || input.ActualParameter == null)
                return Nothing.Do;

            return input.ActualParameter;
        }
    }

    [ValueConversion(typeof(object), typeof(bool))]
    public class ValueNotEqualToParameterConverter : Converter<object, bool>
    {
        public static ValueNotEqualToParameterConverter Default { get; private set; } = new ValueNotEqualToParameterConverter();
        ValueNotEqualToParameterConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<object> input) => !input.Value.Equals(input.ActualParameter);

        protected override ConverterValue<object> ConvertBack(ConverterData<bool> input) => input.ActualParameter;
    }
}