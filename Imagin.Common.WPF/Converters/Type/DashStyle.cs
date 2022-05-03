using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    public class DashStyleTypeConverter : StringTypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                return input switch
                {
                    nameof(DashStyles.Dash) => DashStyles.Dash,
                    nameof(DashStyles.DashDot) => DashStyles.DashDot,
                    nameof(DashStyles.DashDotDot) => DashStyles.DashDotDot,
                    nameof(DashStyles.Dot) => DashStyles.Dot,
                    nameof(DashStyles.Solid) => DashStyles.Solid,
                    _ => throw new NotSupportedException(),
                };
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}