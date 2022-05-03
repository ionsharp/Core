using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(long), typeof(double))]
    public class BytesToMegaBytesConverter : Converter<long, double>
    {
        public static BytesToMegaBytesConverter Default { get; private set; } = new();
        BytesToMegaBytesConverter() { }

        protected override bool AllowNull => true;

        protected override ConverterValue<double> ConvertTo(ConverterData<long> input)
        {
            if (input.ActualValue == null)
                return 0;

            double.TryParse(input.Value.ToString(), out double result);
            return (result / 1024d / 1024d).Round(3);
        }

        protected override ConverterValue<long> ConvertBack(ConverterData<double> input) => Nothing.Do;
    }
    
    [ValueConversion(typeof(double), typeof(double))]
    public class InverseDoubleConverter : Converter<double, double>
    {
        public static InverseDoubleConverter Default { get; private set; } = new();
        InverseDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<double> input) => 1 - input.Value.Coerce(1);

        protected override ConverterValue<double> ConvertBack(ConverterData<double> input) => 1 - input.Value.Coerce(1);
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class PercentConverter : Converter<double, double>
    {
        public static PercentConverter Default { get; private set; } = new PercentConverter();
        PercentConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<double> input) => input.Value * 100.0;

        protected override ConverterValue<double> ConvertBack(ConverterData<double> input) => input.Value / 100.0;
    }

    //...

    [ValueConversion(typeof(double), typeof(double))]
    public class RadiusToDiameterConverter : Converter<double, double>
    {
        public static RadiusToDiameterConverter Default { get; private set; } = new();
        RadiusToDiameterConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<double> input) => input.Value * 2.0;

        protected override ConverterValue<double> ConvertBack(ConverterData<double> input) => input.Value / 2.0;
    }

    //...

    [ValueConversion(typeof(int), typeof(double))]
    public class SubtractConverter : Converter<int, double>
    {
        public static SubtractConverter Default { get; private set; } = new SubtractConverter();
        SubtractConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<int> input) => input.Value - input.Parameter;

        protected override ConverterValue<int> ConvertBack(ConverterData<double> input) => (input.Value + input.Parameter).Int32();
    }

    //...

    [ValueConversion(typeof(byte), typeof(double))]
    public class ByteToDoubleConverter : Converter<byte, double>
    {
        public static ByteToDoubleConverter Default { get; private set; } = new();
        ByteToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<byte> input) => input.Value.Double() / byte.MaxValue.Double();

        protected override ConverterValue<byte> ConvertBack(ConverterData<double> input) => (input.Value * byte.MaxValue.Double()).Byte();
    }

    [ValueConversion(typeof(DateTime), typeof(double))]
    public class DateTimeToDoubleConverter : Converter<DateTime, double>
    {
        public static DateTimeToDoubleConverter Default { get; private set; } = new();
        DateTimeToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<DateTime> input) => input.Value.Ticks.Double();

        protected override ConverterValue<DateTime> ConvertBack(ConverterData<double> input) => new DateTime(input.Value.Int64());
    }

    [ValueConversion(typeof(decimal), typeof(double))]
    public class DecimalToDoubleConverter : Converter<decimal, double>
    {
        public static DecimalToDoubleConverter Default { get; private set; } = new();
        DecimalToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<decimal> input) => input.Value.Double();

        protected override ConverterValue<decimal> ConvertBack(ConverterData<double> input) => input.Value.Decimal();
    }

    [ValueConversion(typeof(short), typeof(double))]
    public class Int16ToDoubleConverter : Converter<short, double>
    {
        public static Int16ToDoubleConverter Default { get; private set; } = new();
        Int16ToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<short> input) => input.Value.Double();

        protected override ConverterValue<short> ConvertBack(ConverterData<double> input) => input.Value.Int16();
    }

    [ValueConversion(typeof(int), typeof(double))]
    public class Int32ToDoubleConverter : Converter<int, double>
    {
        public static Int32ToDoubleConverter Default { get; private set; } = new();
        Int32ToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<int> input) => input.Value.Double();

        protected override ConverterValue<int> ConvertBack(ConverterData<double> input) => input.Value.Int32();
    }

    [ValueConversion(typeof(long), typeof(double))]
    public class Int64ToDoubleConverter : Converter<long, double>
    {
        public static Int64ToDoubleConverter Default { get; private set; } = new();
        Int64ToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<long> input) => input.Value.Double();

        protected override ConverterValue<long> ConvertBack(ConverterData<double> input) => input.Value.Int64();
    }

    [ValueConversion(typeof(object), typeof(double))]
    public class ObjectToDoubleConverter : Converter<object, double>
    {
        public static ObjectToDoubleConverter Default { get; private set; } = new();
        ObjectToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<object> input) => input.Value.Double() is double result && double.IsNaN(result) ? Nothing.Do : result;

        protected override ConverterValue<object> ConvertBack(ConverterData<double> input) => input.Value;
    }

    [ValueConversion(typeof(One), typeof(double))]
    public class OneToDoubleConverter : Converter<One, double>
    {
        public static OneToDoubleConverter Default { get; private set; } = new();
        public OneToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<One> input) => (double)input.Value;

        protected override ConverterValue<One> ConvertBack(ConverterData<double> input) => (One)input.Value;
    }
    
    [ValueConversion(typeof(float), typeof(double))]
    public class SingleToDoubleConverter : Converter<float, double>
    {
        public static SingleToDoubleConverter Default { get; private set; } = new();
        SingleToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<float> input) => input.Value.Double();

        protected override ConverterValue<float> ConvertBack(ConverterData<double> input) => input.Value.Single();
    }

    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToDoubleConverter : Converter<TimeSpan, double>
    {
        public static TimeSpanToDoubleConverter Default { get; private set; } = new();
        TimeSpanToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<TimeSpan> input) => input.Value.TotalMilliseconds;

        protected override ConverterValue<TimeSpan> ConvertBack(ConverterData<double> input) => TimeSpan.FromMilliseconds(input.Value);
    }

    [ValueConversion(typeof(UDouble), typeof(double))]
    public class UDoubleToDoubleConverter : Converter<UDouble, double>
    {
        public static UDoubleToDoubleConverter Default { get; private set; } = new();
        UDoubleToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<UDouble> input) => (double)input.Value;

        protected override ConverterValue<UDouble> ConvertBack(ConverterData<double> input) => (UDouble)input.Value;
    }

    [ValueConversion(typeof(ushort), typeof(double))]
    public class UInt16ToDoubleConverter : Converter<ushort, double>
    {
        public static UInt16ToDoubleConverter Default { get; private set; } = new();
        UInt16ToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<ushort> input) => input.Value.Double();

        protected override ConverterValue<ushort> ConvertBack(ConverterData<double> input) => input.Value.UInt16();
    }

    [ValueConversion(typeof(uint), typeof(double))]
    public class UInt32ToDoubleConverter : Converter<uint, double>
    {
        public static UInt32ToDoubleConverter Default { get; private set; } = new();
        UInt32ToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<uint> input) => input.Value.Double();

        protected override ConverterValue<uint> ConvertBack(ConverterData<double> input) => input.Value.UInt32();
    }

    [ValueConversion(typeof(ulong), typeof(double))]
    public class UInt64ToDoubleConverter : Converter<ulong, double>
    {
        public static UInt64ToDoubleConverter Default { get; private set; } = new();
        UInt64ToDoubleConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<ulong> input) => input.Value.Double();

        protected override ConverterValue<ulong> ConvertBack(ConverterData<double> input) => input.Value.UInt64();
    }
}