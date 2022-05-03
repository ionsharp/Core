using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    [ValueConversion(typeof(Cell), typeof(string))]
    public class CellConverter : MultiConverter<string>
    {
        public static CellConverter Default { get; private set; } = new CellConverter();
        CellConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3)
            {
                if (values[0] is string cellText)
                {
                    if (values[1] is CellFormats cellFormat)
                    {
                        if (values[2] is string cellFormatText)
                        {
                            if (cellText.StartsWith("="))
                            {
                                //We have to process the formula before applying the format!
                                cellText = cellText.Substring(0);
                            }

                            switch (cellFormat)
                            {
                                case CellFormats.Custom:
                                    return cellText;

                                case CellFormats.Date:
                                    DateTime? date = null;
                                    Try.Invoke(() => date = DateTime.Parse(cellText));
                                    date = date ?? DateTime.MinValue;
                                    return date.Value.ToString(cellFormatText);

                                case CellFormats.Fraction:
                                    return cellText;

                                case CellFormats.None:
                                    return cellText;

                                case CellFormats.Number:
                                    double? number = null;
                                    Try.Invoke(() => number = double.Parse(cellText));
                                    number = number ?? 0.0;
                                    return number.Value.ToString(cellFormatText);

                                case CellFormats.Percentage:
                                    double? percentage = null;
                                    Try.Invoke(() => percentage = double.Parse(cellText));
                                    percentage = percentage ?? 0.0;
                                    return $"{percentage.Value.ToString(cellFormatText)}%";

                                case CellFormats.Text:
                                    return cellText;

                                case CellFormats.Time:
                                    TimeSpan? time = null;
                                    Try.Invoke(() => time = TimeSpan.Parse(cellText));
                                    time = time ?? TimeSpan.Zero;
                                    return time.Value.ToString(cellFormatText);

                                case CellFormats.TimeRelative:
                                    DateTime? relativeTime = null;
                                    Try.Invoke(() => relativeTime = DateTime.Parse(cellText));
                                    relativeTime = relativeTime ?? DateTime.MinValue;
                                    return cellFormatText.F(relativeTime.Value.Relative());

                                case CellFormats.TimeRelativeDifference:

                                    DateTime? relativeTimeDifference = null;
                                    Try.Invoke(() => relativeTimeDifference = DateTime.Parse(cellText));
                                    relativeTimeDifference = relativeTimeDifference ?? DateTime.MinValue;
                                    return cellFormatText.F(relativeTimeDifference.Value.RelativeDifference(0));
                            }
                            throw new NotSupportedException();
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}