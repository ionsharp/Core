using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class FileSizeMultiConverter : MultiConverter<string>
    {
        public static FileSizeMultiConverter Default { get; private set; } = new FileSizeMultiConverter();
        FileSizeMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2)
            {
                if (values[1] is FileSizeFormat format)
                {
                    if (values[0] is long a)
                        return a.FileSize(format);

                    if (values[0] is ulong b)
                        return b.FileSize(format);
                }
            }
            return string.Empty;
        }
    }
}