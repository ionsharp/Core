using Imagin.Common.Converters;
using Imagin.Common.Storage;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class BrowserGroupConverterSelector : ConverterSelector
    {
        public static readonly BrowserGroupConverterSelector Default = new();
        BrowserGroupConverterSelector() { }

        public override IValueConverter Select(object input)
        {
            return $"{input}" switch
            {
                nameof(ItemProperty.Name) => new SimpleConverter<Item, string>(i => FirstLetterConverter.Default.Convert(i.Name, null, null, null)?.ToString()),
                nameof(ItemProperty.Type) => new SimpleConverter<Item, string>(i => Computer.FriendlyDescription(i.Path)),
                _ => default,
            };
        }
    }
}