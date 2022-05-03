using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(DateTime))]
    public class TimeSpanToDateTimeConverter : Converter<TimeSpan, DateTime>
    {
        public static TimeSpanToDateTimeConverter Default { get; private set; } = new TimeSpanToDateTimeConverter();
        TimeSpanToDateTimeConverter() { }

        protected override ConverterValue<DateTime> ConvertTo(ConverterData<TimeSpan> input) => DateTime.Today.AddSeconds(input.Value.TotalSeconds);

        protected override ConverterValue<TimeSpan> ConvertBack(ConverterData<DateTime> input) => Nothing.Do;
    }

    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToDateTimeStringConverter : Converter<TimeSpan, string>
    {
        public static TimeSpanToDateTimeStringConverter Default { get; private set; } = new TimeSpanToDateTimeStringConverter();
        TimeSpanToDateTimeStringConverter() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<TimeSpan> input)
        {
            var result = DateTime.Today.AddSeconds(input.Value.TotalSeconds);
            if (input.ActualParameter is string format)
                return result.ToString(format);

            return $"{result}";
        }

        protected override ConverterValue<TimeSpan> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }
}