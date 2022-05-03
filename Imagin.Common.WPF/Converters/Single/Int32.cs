using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(double), typeof(int))]
    public class DoubleToInt32Converter : Converter<double, int>
    {
        public static DoubleToInt32Converter Default { get; private set; } = new();
        public DoubleToInt32Converter() { }

        protected override ConverterValue<int> ConvertTo(ConverterData<double> input) => input.Value.Int32();

        protected override ConverterValue<double> ConvertBack(ConverterData<int> input) => input.Value.Double();
    }

    [ValueConversion(typeof(FrameworkElement), typeof(int))]
    public class IndexConverter : Converter<FrameworkElement, int>
    {
        public static IndexConverter Default { get; private set; } = new IndexConverter();
        IndexConverter() { }

        protected override ConverterValue<int> ConvertTo(ConverterData<FrameworkElement> input)
        {
            var item = input.Value;
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(item);

            var index = itemsControl?.ItemContainerGenerator.IndexFromContainer(item) ?? 0;
            return input.Parameter + index;
        }

        protected override ConverterValue<FrameworkElement> ConvertBack(ConverterData<int> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(int))]
    public class StringLengthConverter : Converter<string, int>
    {
        public static StringLengthConverter Default { get; private set; } = new StringLengthConverter();
        StringLengthConverter() { }

        protected override ConverterValue<int> ConvertTo(ConverterData<string> input) => input.Value.Length;

        protected override ConverterValue<string> ConvertBack(ConverterData<int> input) => Nothing.Do;
    }
}