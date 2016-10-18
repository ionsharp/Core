using System;
using System.Net;
using System.Windows;
using System.Collections;

namespace Imagin.Controls.Extended
{
    public static class PropertyGridExtensions
    {
        public static Type GetPropertyModelType(this Type Type) 
        {
            if (Type == typeof(bool))
                return typeof(BoolPropertyModel);
            if (Type == typeof(byte))
                return typeof(BytePropertyModel);
            if (typeof(IList).IsAssignableFrom(Type))
                return typeof(CollectionPropertyModel);
            if (Type == typeof(DateTime))
                return typeof(DateTimePropertyModel);
            if (Type == typeof(decimal))
                return typeof(DecimalPropertyModel);
            if (Type == typeof(double))
                return typeof(DoublePropertyModel);
            if (Type.IsEnum)
                return typeof(EnumPropertyModel);
            if (Type == typeof(Guid))
                return typeof(GuidPropertyModel);
            if (Type == typeof(int))
                return typeof(IntPropertyModel);
            if (Type == typeof(long))
                return typeof(LongPropertyModel);
            if (Type == typeof(NetworkCredential))
                return typeof(NetworkCredentialPropertyModel);
            if (Type == typeof(short))
                return typeof(ShortPropertyModel);
            if (Type == typeof(Size))
                return typeof(SizePropertyModel);
            if (Type == typeof(string))
                return typeof(StringPropertyModel);
            return default(Type);
        }
    }
}
