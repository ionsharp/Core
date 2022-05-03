using Imagin.Common.Data;
using Imagin.Common.Linq;
using System.ComponentModel;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(SortDirection), typeof(ListSortDirection))]
    public class SortDirectionConverter : Converter<SortDirection, ListSortDirection>
    {
        public static SortDirectionConverter Default { get; private set; } = new SortDirectionConverter();
        SortDirectionConverter() { }

        protected override ConverterValue<ListSortDirection> ConvertTo(ConverterData<SortDirection> input) => input.Value.Convert();

        protected override ConverterValue<SortDirection> ConvertBack(ConverterData<ListSortDirection> input) => input.Value.Convert();
    }
}