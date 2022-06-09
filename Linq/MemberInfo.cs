using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Core.Linq;

public static class XMemberInfo
{
    public static T GetAttribute<T>(this MemberInfo value) where T : Attribute
    {
        foreach (var i in value.GetCustomAttributes(true))
        {
            if (i is T)
                return (T)i;
        }
        return default;
    }

    public static IEnumerable<T> GetAttributes<T>(this MemberInfo value) where T : Attribute
    {
        foreach (var i in value.GetCustomAttributes(true))
        {
            if (i is T j)
                yield return j;
        }

        yield break;
    }

    public static IEnumerable<Attribute> GetAttributes(this MemberInfo input) => input.GetAttributes<Attribute>();

    public static string GetDisplayName(this MemberInfo input)
        => input.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? input.GetAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName ?? input.Name;

    public static bool HasAttribute<T>(this MemberInfo input) => input.HasAttribute(typeof(T));

    public static bool HasAttribute(this MemberInfo input, Type type)
    {
        foreach (var i in input.GetCustomAttributes(true))
        {
            if (i.GetType() == type)
                return true;
        }
        return false;
    }
}