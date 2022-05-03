using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Linq;
using System.Collections;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object), typeof(StringCollection))]
    public class StringCollectionConverter : Converter<object, StringCollection>
    {
        public static StringCollectionConverter Default { get; private set; } = new();
        public StringCollectionConverter() { }

        protected override ConverterValue<StringCollection> ConvertTo(ConverterData<object> input)
        {
            var result = new StringCollection();
            if (input.Value is IEnumerable a)
                a.ForEach(i => result.Add(i.ToString()));

            else if (input.Value is IList b)
                b.ForEach(i => result.Add(i.ToString()));

            return result;
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<StringCollection> input) => Nothing.Do;
    }
}