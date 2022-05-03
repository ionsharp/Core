using Imagin.Common.Linq;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Type), typeof(ObservableCollection<Enum>))]
    public class EnumConverter : Converter<Type, ObservableCollection<Enum>>
    {
        public static EnumConverter Default { get; private set; } = new();
        public EnumConverter() { }

        protected override ConverterValue<ObservableCollection<Enum>> ConvertTo(ConverterData<Type> input) => input.Value.GetEnumCollection(Appearance.Visible);

        protected override ConverterValue<Type> ConvertBack(ConverterData<ObservableCollection<Enum>> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumFlagsToBooleanConverter : Converter<Enum, bool>
    {
        public static EnumFlagsToBooleanConverter Default { get; private set; } = new();
        public EnumFlagsToBooleanConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<Enum> input) => input.ActualParameter is Enum i && input.Value.HasFlag(i);

        protected override ConverterValue<Enum> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }
}