using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(bool), typeof(TextWrapping))]
    public class TextWrappingConverter : Converter<bool, TextWrapping>
    {
        public static TextWrappingConverter Default { get; private set; } = new();
        TextWrappingConverter() { }

        protected override ConverterValue<TextWrapping> ConvertTo(ConverterData<bool> input) 
            => input.Value ? TextWrapping.Wrap : TextWrapping.NoWrap;

        protected override ConverterValue<bool> ConvertBack(ConverterData<TextWrapping> input) 
            => input.Value == TextWrapping.Wrap;
    }
}