using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(double))]
    public class MathMultiConverter : MultiConverter<double>
    {
        public static MathMultiConverter Default { get; private set; } = new MathMultiConverter();
        MathMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3)
            {
                if (values[0] is double a && values[2] is double b)
                {
                    var operation = (NumberOperation)values[1];
                    switch (operation)
                    {
                        case NumberOperation.Add:
                            return a + b;
                        case NumberOperation.Divide:
                            return a / b;
                        case NumberOperation.Multiply:
                            return a * b;
                        case NumberOperation.Subtract:
                            return a - b;
                    }
                }
                else Log.Write<MathMultiConverter>(new Warning($"a = {values[0].NullString()}, b = {values[2].NullString()}"));
            }
            else if (values?.Length > 0)
            {
                if (values[0] is double firstValue)
                {
                    var result = firstValue;
                    NumberOperation? m = null;
                    for (var i = 1; i < values.Length; i++)
                    {
                        if (values[i] is NumberOperation operation)
                        {
                            m = operation;
                        }
                        else if (m != null && values[i] is double nextValue)
                        {
                            switch (m)
                            {
                                case NumberOperation.Add:
                                    result += nextValue;
                                    break;
                                case NumberOperation.Divide:
                                    result /= nextValue;
                                    break;
                                case NumberOperation.Multiply:
                                    result *= nextValue;
                                    break;
                                case NumberOperation.Subtract:
                                    result -= nextValue;
                                    break;
                            }
                            m = null;
                        }
                    }
                    return result;
                }
            }
            return default(double);
        }
    }
}