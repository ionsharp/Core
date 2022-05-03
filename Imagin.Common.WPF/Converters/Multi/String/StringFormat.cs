using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public abstract class StringFormatMultiConverter<T> : MultiConverter<string>
    {
        protected abstract string Convert(T a, string b);

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2)
            {
                if (values[0] is T a)
                {
                    if (values[1] is string b)
                    {
                        var i = b.Between("{", "}");
                        if (!i.NullOrEmpty())
                            return b.ReplaceBetween('{', '}', "0").F(Convert(a, i));

                        return Convert(a, b);
                    }
                }
            }
            return Binding.DoNothing;
        }
    }

    public class ByteStringFormatMultiConverter : StringFormatMultiConverter<byte>
    {
        public static ByteStringFormatMultiConverter Default { get; private set; } = new ByteStringFormatMultiConverter();
        ByteStringFormatMultiConverter() { }

        protected override string Convert(byte a, string b) => a.ToString(b);
    }

    public class DecimalStringFormatMultiConverter : StringFormatMultiConverter<decimal>
    {
        public static DecimalStringFormatMultiConverter Default { get; private set; } = new DecimalStringFormatMultiConverter();
        DecimalStringFormatMultiConverter() { }

        protected override string Convert(decimal a, string b) => a.ToString(b);
    }

    public class DoubleStringFormatMultiConverter : StringFormatMultiConverter<double>
    {
        public static DoubleStringFormatMultiConverter Default { get; private set; } = new DoubleStringFormatMultiConverter();
        DoubleStringFormatMultiConverter() { }

        protected override string Convert(double a, string b) => a.ToString(b);
    }

    public class Int16StringFormatMultiConverter : StringFormatMultiConverter<short>
    {
        public static Int16StringFormatMultiConverter Default { get; private set; } = new Int16StringFormatMultiConverter();
        Int16StringFormatMultiConverter() { }

        protected override string Convert(short a, string b) => a.ToString(b);
    }

    public class Int32StringFormatMultiConverter : StringFormatMultiConverter<int>
    {
        public static Int32StringFormatMultiConverter Default { get; private set; } = new Int32StringFormatMultiConverter();
        Int32StringFormatMultiConverter() { }

        protected override string Convert(int a, string b) => a.ToString(b);
    }

    public class Int64StringFormatMultiConverter : StringFormatMultiConverter<long>
    {
        public static Int64StringFormatMultiConverter Default { get; private set; } = new Int64StringFormatMultiConverter();
        Int64StringFormatMultiConverter() { }

        protected override string Convert(long a, string b) => a.ToString(b);
    }

    public class SingleStringFormatMultiConverter : StringFormatMultiConverter<float>
    {
        public static SingleStringFormatMultiConverter Default { get; private set; } = new SingleStringFormatMultiConverter();
        SingleStringFormatMultiConverter() { }

        protected override string Convert(float a, string b) => a.ToString(b);
    }

    public class UDoubleStringFormatMultiConverter : StringFormatMultiConverter<UDouble>
    {
        public static UDoubleStringFormatMultiConverter Default { get; private set; } = new UDoubleStringFormatMultiConverter();
        UDoubleStringFormatMultiConverter() { }

        protected override string Convert(UDouble a, string b) => a.ToString(b);
    }

    public class UInt16StringFormatMultiConverter : StringFormatMultiConverter<ushort>
    {
        public static UInt16StringFormatMultiConverter Default { get; private set; } = new UInt16StringFormatMultiConverter();
        UInt16StringFormatMultiConverter() { }

        protected override string Convert(ushort a, string b) => a.ToString(b);
    }

    public class UInt32StringFormatMultiConverter : StringFormatMultiConverter<uint>
    {
        public static UInt32StringFormatMultiConverter Default { get; private set; } = new UInt32StringFormatMultiConverter();
        UInt32StringFormatMultiConverter() { }

        protected override string Convert(uint a, string b) => a.ToString(b);
    }

    public class UInt64StringFormatMultiConverter : StringFormatMultiConverter<ulong>
    {
        public static UInt64StringFormatMultiConverter Default { get; private set; } = new UInt64StringFormatMultiConverter();
        UInt64StringFormatMultiConverter() { }

        protected override string Convert(ulong a, string b) => a.ToString(b);
    }
}