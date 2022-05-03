using Imagin.Common.Colors;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class ComponentMultiConverter : MultiConverter<string>
    {
        public static ComponentMultiConverter Default { get; private set; } = new ComponentMultiConverter();
        public ComponentMultiConverter() : base() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2)
            {
                if (values[0] is Components component)
                {
                    if (values[1] is ColorModels model)
                    {
                        var result = model.GetComponent((int)component);
                        return $"({result.Symbol}) {result.Name}";
                    }
                }
            }
            return Binding.DoNothing;
        }
    }
}