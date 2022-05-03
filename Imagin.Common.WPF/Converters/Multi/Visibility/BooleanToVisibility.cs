using Imagin.Common.Analytics;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class BooleanToVisibilityMultiConverter : MultiConverter<Visibility>
    {
        public static BooleanToVisibilityMultiConverter Default { get; private set; } = new BooleanToVisibilityMultiConverter();
        BooleanToVisibilityMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0)
            {
                if (parameter is object actualParameter)
                {
                    var condition = new StringCondition($"{actualParameter}");

                    var data = new List<bool>();
                    foreach (var i in values)
                    {
                        if (i is bool a)
                            data.Add(a);

                        if (i is Visibility b)
                            data.Add(b.Boolean());
                    }

                    var result = false;
                    if (Try.Invoke(() => result = condition.Evaluate(data), e => Log.Write<BooleanToVisibilityMultiConverter>(e)))
                        return result.Visibility();

                    return Binding.DoNothing;
                }

                foreach (var i in values)
                {
                    if (i is bool a)
                    {
                        if (!a)
                            return Visibility.Collapsed;
                    }
                    if (i is Visibility b)
                    {
                        if (b != Visibility.Visible)
                            return Visibility.Collapsed;
                    }
                }
                return Visibility.Visible;
            }
            return Binding.DoNothing;
        }
    }
}