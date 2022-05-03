using Imagin.Common.Storage;

namespace Imagin.Common.Converters
{
    public class ExtensionsTypeConverter : StringTypeConverter<string>
    {
        protected override int? Length => null;

        protected override char Separator => ';';

        protected override string Convert(string input) => input;

        protected override object Convert(string[] input) => new Extensions(input);
    }
}