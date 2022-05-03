using System;
using System.Windows.Data;
using Imagin.Common.Controls;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(SelectionMode), typeof(System.Windows.Controls.SelectionMode))]
    public class SelectionModeConverter : Converter<SelectionMode, System.Windows.Controls.SelectionMode>
    {
        public static SelectionModeConverter Default { get; private set; } = new SelectionModeConverter();
        SelectionModeConverter() { }

        protected override ConverterValue<System.Windows.Controls.SelectionMode> ConvertTo(ConverterData<SelectionMode> input)
        {
            return input.Value switch
            {
                SelectionMode.Multiple => System.Windows.Controls.SelectionMode.Extended,
                SelectionMode.Single or SelectionMode.SingleOrNone => System.Windows.Controls.SelectionMode.Single,
                _ => throw new NotSupportedException(),
            };
        }

        protected override ConverterValue<SelectionMode> ConvertBack(ConverterData<System.Windows.Controls.SelectionMode> input) => Nothing.Do;
    }
}