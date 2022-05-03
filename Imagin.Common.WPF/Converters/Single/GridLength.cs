using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(ControlLength), typeof(GridLength))]
    public class GridLengthConverter : Converter<ControlLength, GridLength>
    {
        public static GridLengthConverter Default { get; private set; } = new GridLengthConverter();
        GridLengthConverter() { }

        protected override ConverterValue<GridLength> ConvertTo(ConverterData<ControlLength> input) => (GridLength)input.Value;

        protected override ConverterValue<ControlLength> ConvertBack(ConverterData<GridLength> input) => (ControlLength)input.Value;
    }

    [ValueConversion(typeof(DataGridLength), typeof(GridLength))]
    public class DataGridLengthConverter : Converter<DataGridLength, GridLength>
    {
        public static DataGridLengthConverter Default { get; private set; } = new DataGridLengthConverter();
        DataGridLengthConverter() { }

        protected override ConverterValue<GridLength> ConvertTo(ConverterData<DataGridLength> input) => new GridLength();

        protected override ConverterValue<DataGridLength> ConvertBack(ConverterData<GridLength> input) => new DataGridLength();
    }
}
