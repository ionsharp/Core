using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Imagin.Common.Converters
{
    [ContentProperty(nameof(Converters))]
    public class ConverterSelector : Freezable
    {
        public virtual IValueConverter Select(object input) => null;

        protected override Freezable CreateInstanceCore() => new ConverterSelector();

        public ConverterSelector() : base() { }
    }

    public class DefaultConverterSelector : ConverterSelector
    {
        public ObservableCollection<ConverterContainer> Converters { get; private set; } = new();

        public override IValueConverter Select(object input)
        {
            foreach (var i in Converters)
            {
                if (input == i.DataKey)
                    return i.Converter;
            }
            return base.Select(input); ;
        }
    }
}