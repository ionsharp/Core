using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    public class ConverterData<T>
    {
        public readonly int Parameter;

        public T Value => ActualValue is T i ? i : default;

        public readonly object ActualParameter;

        public readonly object ActualValue;

        public ArgumentOutOfRangeException InvalidParameter => new(nameof(Parameter));

        public ConverterData(object value, object parameter)
        {
            ActualValue = value;
            ActualParameter = parameter;
            int.TryParse(ActualParameter?.ToString(), out Parameter);
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    public abstract class Converter<Input, Output> : IValueConverter
    {
        protected virtual bool AllowNull => false;

        protected abstract ConverterValue<Output> ConvertTo(ConverterData<Input> input);

        protected virtual ConverterValue<Input> ConvertBack(ConverterData<Output> input) => Nothing.Do;

        protected virtual bool Is(object input) => input is Input;

        public object Convert(object value, object parameter = null) => Convert(value, null, parameter, null);

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (AllowNull || Is(value))
            {
                var result = ConvertTo(new ConverterData<Input>(value, parameter));
                if (result.ActualValue is not Nothing)
                    return result.ActualValue;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, object parameter = null) => Convert(value, null, parameter, null);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Output i)
            {
                var result = ConvertBack(new ConverterData<Output>(value, parameter));
                if (result.ActualValue is not Nothing)
                    return result.ActualValue;
            }
            return Binding.DoNothing;
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    public class SimpleConverter<Input, Output> : Converter<Input, Output>
    {
        readonly Func<Input, Output> ConvertInput;

        readonly Func<Output, Input> ConvertOutput;

        public SimpleConverter(Func<Input, Output> convertInput, Func<Output, Input> convertOutput = null)
        {
            ConvertInput = convertInput;
            ConvertOutput = convertOutput;
        }

        protected override ConverterValue<Output> ConvertTo(ConverterData<Input> input) => ConvertInput(input.Value);

        protected override ConverterValue<Input> ConvertBack(ConverterData<Output> input) => ConvertOutput != null ? ConvertOutput.Invoke(input.Value) : Nothing.Do;

        public Output ConvertTo(Input input) => ConvertInput(input);

        public Input ConvertBack(Output input) => ConvertOutput != null ? ConvertOutput.Invoke(input) : default;
    }

    [ValueConversion(typeof(object), typeof(object))]
    public class ComplexConverter<Input, Output> : Converter<Input, Output>
    {
        readonly Func<ConverterData<Input>, ConverterValue<Output>> ConvertInput;

        readonly Func<ConverterData<Output>, ConverterValue<Input>> ConvertOutput;

        protected override bool AllowNull => true;

        public ComplexConverter(Func<ConverterData<Input>, ConverterValue<Output>> convertInput, Func<ConverterData<Output>, ConverterValue<Input>> convertOutput = null)
        {
            ConvertInput = convertInput;
            ConvertOutput = convertOutput;
        }

        protected override ConverterValue<Output> ConvertTo(ConverterData<Input> input) => ConvertInput(input);

        protected override ConverterValue<Input> ConvertBack(ConverterData<Output> input) => ConvertOutput != null ? ConvertOutput(input) : Nothing.Do;
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class DoubleConverter : SimpleConverter<double, double>
    {
        public DoubleConverter(Func<double, double> convertTo, Func<double, double> convertBack = null) : base(convertTo, convertBack) { }
    }
}