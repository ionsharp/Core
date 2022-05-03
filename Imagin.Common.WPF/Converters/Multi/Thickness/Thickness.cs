using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(Thickness))]
    public class ThicknessMultiConverter : MultiConverter<Thickness>
    {
        public static ThicknessMultiConverter Default { get; private set; } = new();
        public ThicknessMultiConverter() : base() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0)
            {
                if (values[0] is double l0)
                {
                    if (values.Length > 1)
                    {
                        if (values[1] is double t0)
                        {
                            if (values.Length > 2)
                            {
                                if (values[2] is double r0)
                                {
                                    if (values.Length > 3)
                                    {
                                        if (values[3] is double b0)
                                            return new Thickness(l0, t0, r0, b0);
                                    }
                                    return new Thickness(l0, t0, r0, 0);
                                }
                            }
                            return new Thickness(l0, t0, 0, 0);
                        }
                    }
                    return new Thickness(l0, 0, 0, 0);
                }
                else if (values[0] is int l1)
                {
                    if (values.Length > 1)
                    {
                        if (values[1] is int t1)
                        {
                            if (values.Length > 2)
                            {
                                if (values[2] is int r1)
                                {
                                    if (values.Length > 3)
                                    {
                                        if (values[3] is int b1)
                                            return new Thickness(l1, t1, r1, b1);
                                    }
                                    return new Thickness(l1, t1, r1, 0);
                                }
                            }
                            return new Thickness(l1, t1, 0, 0);
                        }
                    }
                    return new Thickness(l1, 0, 0, 0);
                }
            }
            return new Thickness(0);
        }
    }
}