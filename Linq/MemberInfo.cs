using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Core.Linq;

public static class XMemberInfo
{
    public static bool CanGet(this MemberInfo input) => input is FieldInfo field ? field.CanGet() : input is PropertyInfo property ? property.CanGet() : false;

    public static bool CanGet(this FieldInfo input) => input.IsPublic;

    public static bool CanGet(this PropertyInfo input) => input.CanRead && input.GetGetMethod(true) != null;

    ///

    public static bool CanSet(this MemberInfo input) => input is FieldInfo field ? field.CanSet() : input is PropertyInfo property ? property.CanSet() : false;

    public static bool CanSet(this FieldInfo input) => !input.IsInitOnly && input.IsPublic;

    public static bool CanSet(this PropertyInfo input) => input.CanWrite && input.GetSetMethod(true) != null;
    
    ///

    public static T GetAttribute<T>(this MemberInfo input) where T : Attribute
    {
        foreach (var i in input.GetCustomAttributes(true))
        {
            if (i is T j)
                return j;
        }
        return default;
    }

    public static IEnumerable<T> GetAttributes<T>(this MemberInfo input) where T : Attribute
    {
        foreach (var i in input.GetCustomAttributes(true))
        {
            if (i is T j)
                yield return j;
        }
        yield break;
    }

    public static IEnumerable<Attribute> GetAttributes(this MemberInfo input) => input.GetAttributes<Attribute>();

    ///

    public static string GetCategory(this MemberInfo input, string empty = "Other")
        => input.GetAttribute<CategoryAttribute>()?.Category?.ToString() ?? input.GetAttribute<System.ComponentModel.CategoryAttribute>()?.Category ?? empty;

    public static string GetDescription(this MemberInfo input)
        => input.GetAttribute<DescriptionAttribute>()?.Description ?? input.GetAttribute<System.ComponentModel.DescriptionAttribute>()?.Description ?? "";

    public static string GetDisplayName(this MemberInfo input)
        => input.GetAttribute<NameAttribute>()?.Name ?? input.GetAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName ?? input.Name;

    ///

    public static Type GetMemberType(this MemberInfo input) => input is FieldInfo field ? field.FieldType : input is PropertyInfo property ? property.PropertyType : null;

    ///

    public static bool HasAttribute<T>(this MemberInfo input) => input.HasAttribute(typeof(T));

    public static bool HasAttribute(this MemberInfo input, Type type)
    {
        foreach (var i in input.GetCustomAttributes(true))
        {
            if (i.GetType().Inherits(type, true))
                return true;
        }
        return false;
    }

    ///

    public static bool IsHidden(this MemberInfo input)
        => input.GetAttribute<System.ComponentModel.BrowsableAttribute>()?.Browsable == false || input.HasAttribute<HideAttribute>();

    public static bool IsIndexor(this MemberInfo input)
        => input is PropertyInfo property && property.GetIndexParameters()?.Length > 0;
    
    public static bool IsSerializable(this MemberInfo input)
        => !input.HasAttribute<NonSerializedAttribute>() && !input.HasAttribute<NonSerializableAttribute>() && !input.HasAttribute<System.Xml.Serialization.XmlIgnoreAttribute>();

    public static bool IsStatic(this MemberInfo input)
        => (input is FieldInfo field && field.IsStatic) || (input is PropertyInfo property && property.GetAccessors(true)[0].IsStatic);
}