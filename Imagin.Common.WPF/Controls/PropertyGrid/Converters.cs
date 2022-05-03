using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class AnyMemberHasAttributeConverter : Converter<object, bool>
    {
        public static AnyMemberHasAttributeConverter Default { get; private set; } = new();
        AnyMemberHasAttributeConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<object> input)
        {
            if (input.ActualParameter is System.Type type)
            {
                foreach (var i in MemberCollection.GetMembers(input.Value.GetType()))
                {
                    if (i.HasAttribute(type))
                        return true;
                }
            }
            return false;
        }
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class AnyMemberHasAttributeVisibilityConverter : Converter<object, Visibility>
    {
        public static AnyMemberHasAttributeVisibilityConverter Default { get; private set; } = new();
        AnyMemberHasAttributeVisibilityConverter() { }

        protected override ConverterValue<Visibility> ConvertTo(ConverterData<object> input)
        {
            if (input.ActualParameter is System.Type type)
            {
                foreach (var i in MemberCollection.GetMembers(input.Value.GetType()))
                {
                    if (i.HasAttribute(type))
                        return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }
    }
}