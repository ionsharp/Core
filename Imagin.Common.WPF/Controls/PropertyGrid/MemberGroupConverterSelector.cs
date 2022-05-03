using Imagin.Common.Converters;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class MemberGroupConverterSelector : ConverterSelector
    {
        public static readonly MemberGroupConverterSelector Default = new();

        public override IValueConverter Select(object input)
        {
            return $"{input}" switch
            {
                nameof(MemberGroupName.Category) => new SimpleConverter<MemberModel, string>(i => i.Category),
                nameof(MemberGroupName.DisplayName) => new SimpleConverter<MemberModel, string>(i => FirstLetterConverter.Default.Convert(i.DisplayName, null, null, null)?.ToString()),
                nameof(MemberGroupName.DeclaringType) => new SimpleConverter<MemberModel, string>(i => i.DeclaringType.Name),
                nameof(MemberGroupName.Type) => new SimpleConverter<MemberModel, string>(i => i.Type.Name),
                _ => default,
            };
        }
    }
}