using Imagin.Common.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Storage
{
    public class ValidateConverter : MultiConverter<bool>
    {
        public static ValidateConverter Default { get; private set; } = new ValidateConverter();
        ValidateConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length.EqualsAny(2, 3) == true)
            {
                if (values[0] is string path)
                {
                    if (values[1] is StorageWindowModes mode)
                    {
                        var types = mode.Convert();
                        if (values[2] is IValidate validator)
                            return validator.Validate(types, path);

                        return PathBox.DefaultValidator.Validate(types, path);
                    }
                }
            }
            return Binding.DoNothing;
        }
    }
}