using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Converts <see cref="TimeSpan"/> to a readable label.
    /// </summary>
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan)
            {
                var Value = (TimeSpan)value;

                var Result = string.Empty;

                Func<int, string> Suffix = i => i != 1 ? "s" : "";

                int d = Value.Days, h = Value.Hours, m = Value.Minutes, s = Value.Seconds;

                if (d > 0)
                    Result += "{0} day{1}, ".F(d, Suffix(d));

                if (h > 0)
                    Result += "{0} hour{1}, ".F(h, Suffix(h));

                if (m > 0)
                    Result += "{0} minute{1}, ".F(m, Suffix(m));

                if (s > 0)
                    Result += "{0} second{1}".F(s, Suffix(s));

                return Result.TrimEnd(' ').TrimEnd(',');
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
