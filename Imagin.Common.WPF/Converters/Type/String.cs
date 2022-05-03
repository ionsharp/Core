using System;
using System.ComponentModel;
using System.Globalization;

namespace Imagin.Common.Converters
{
    public abstract class StringTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
    }

    public abstract class StringTypeConverter<T> : StringTypeConverter
    {
        protected abstract int? Length { get; }

        protected virtual char Separator { get; } = ',';

        protected abstract T Convert(string input);

        protected abstract object Convert(T[] input);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string input)
            {
                var items = input.Split(Array<char>.New(Separator), StringSplitOptions.RemoveEmptyEntries);
                var result = new T[items.Length];

                if (Length != null && Length != result.Length)
                    throw new ArgumentOutOfRangeException(nameof(value));

                for (var i = 0; i < result.Length; i++)
                    result[i] = Convert(items[i]);

                return Convert(result);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}