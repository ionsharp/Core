using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Core.Linq;

public static class XEnum
{
    public static Enum AddFlag<Enum>(this System.Enum input, Enum value) where Enum : struct, IFormattable, IComparable, IConvertible => (Enum)System.Enum.ToObject(typeof(Enum), input.To<int>() | value.To<int>());

    public static Enum AddFlag(this Enum input, Enum value) => Enum.ToObject(input.GetType(), input.To<int>() | value.To<int>()) as Enum;

    ///

    public static T GetAttribute<T>(this Enum input) where T : Attribute => input.GetAttribute(typeof(T)) as T;

    public static Attribute GetAttribute(this Enum input, Type attribute)
    {
        var info = input.GetType().GetMember($"{input}");
        if (info?.Length > 0)
            return info[0].GetCustomAttributes(true).FirstOrDefault<Attribute>(i => i.As<Attribute>().GetType() is Type j && ((attribute.IsInterface && j.Implements(attribute)) || j.Inherits(attribute, true)));

        return default;
    }

    public static T[] GetAttributes<T>(this Enum input) where T : Attribute
    {
        var info = input.GetType().GetMember(input.ToString());
        if (info?.Length == 1)
        {
            var result = info[0].GetCustomAttributes(true).Where(i => i is T);
            return result.Count() > 0 ? result.Cast<T>().ToArray() : null;
        }
        return default;
    }

    public static IEnumerable<Attribute> GetAttributes(this Enum input)
    {
        var info = input.GetType().GetMember(input.ToString());
        return info[0].GetCustomAttributes(true).Cast<Attribute>() ?? Enumerable.Empty<Attribute>();
    }

    ///

    public static bool HasAttribute<Attribute>(this Enum input) where Attribute : System.Attribute => input.HasAttribute(typeof(Attribute));

    public static bool HasAttribute(this Enum input, Type attribute) => input.GetAttribute(attribute) != null;

    ///

    public static string GetDescription(this Enum input) => input?.GetAttribute<System.ComponentModel.DescriptionAttribute>()?.Description ?? input?.GetAttribute<DescriptionAttribute>()?.Description;

    public static string GetDisplayName(this Enum input) => input?.GetAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName ?? input?.GetAttribute<NameAttribute>()?.Name;

    public static string GetLargeImage(this Enum input) => input?.GetAttribute<ImageAttribute>()?.LargeImage;

    public static string GetSmallImage(this Enum input) => input?.GetAttribute<ImageAttribute>()?.SmallImage;

    ///

    public static bool HasAllFlags(this Enum input, params Enum[] values) 
    {
        foreach (var i in values)
        {
            if (!input.HasFlag(i))
                return false;
        }
        return true;
    }

    public static bool HasAnyFlags(this Enum input, params Enum[] values) 
    {
        foreach (var i in values)
        {
            if (input.HasFlag(i))
                return true;
        }
        return false;
    }

    public static bool HasNoFlags(this Enum input, params Enum[] values) 
    {
        return !input.HasAnyFlags(values);
    }

    ///

    public static bool IsHidden(this Enum input)
        => input.GetAttribute<System.ComponentModel.BrowsableAttribute>()?.Browsable == false || input.HasAttribute<HideAttribute>();

    ///

    public static Enum RemoveFlag<Enum>(this System.Enum input, Enum value) where Enum : struct, IFormattable, IComparable, IConvertible 
        => (Enum)System.Enum.ToObject(typeof(Enum), input.To<int>() & ~value.To<int>());

    public static Enum RemoveFlag(this Enum input, Enum value) 
        => Enum.ToObject(input.GetType(), input.To<int>() & ~value.To<int>()) as Enum;
}