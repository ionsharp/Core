using Imagin.Core.Conversion;
using Imagin.Core.Input;
using Imagin.Core.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Imagin.Core;

public static class XPropertyChanged
{
    static A Get<A, B>(IPropertyChanged input, A defaultValue, bool serialize, string propertyName, Func<A, B> convertTo, Func<B, A> convertBack)
    {
        B result = default;

        var properties = serialize ? input.SerializedProperties : input.NonSerializedProperties;
        if (!properties.ContainsKey(propertyName))
        {
            result = convertTo(defaultValue);
            properties.Add(propertyName, result);
        }
        else result = properties[propertyName].As<B>();

        var e = new GetPropertyEventArgs(propertyName, convertBack(result));
        input.OnPropertyGet(e);

        return (A)e.Value;
    }

    public static T Get<T>(this IPropertyChanged input, T defaultValue = default, bool serialize = true, [CallerMemberName] string propertyName = "")
        => Get(input, defaultValue, serialize, propertyName, i => i, i => i);

    ///

    public static A GetFrom<A, B>(this IPropertyChanged input, A defaultValue, IConvert<A, B> convert, bool serialize = true, [CallerMemberName] string propertyName = "")
        => Get(input, defaultValue, serialize, propertyName, convert.ConvertTo, convert.ConvertBack);

    public static T GetFromString<T>(this IPropertyChanged input, T defaultValue, bool serialize = true, [CallerMemberName] string propertyName = "") where T : Enum
        => Get(input, defaultValue, serialize, propertyName, i => $"{i}", i => (T)Enum.Parse(typeof(T), (string)i));

    ///

    static bool Set<T>(IPropertyChanged input, T newValue, bool serialize, bool handle, string propertyName, Func<T, object> convertTo, Func<object, T> convertBack)
    {
        var properties = serialize ? input.SerializedProperties : input.NonSerializedProperties;

        if (!properties.ContainsKey(propertyName))
            properties.Add(propertyName, convertTo(default));

        var oldValue = convertBack(properties[propertyName]);
        if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            return false;

        var e = new PropertyChangingEventArgs(propertyName, oldValue, newValue);
        if (!handle)
            input.OnPropertyChanging(e);

        if (!e.Cancel)
        {
            newValue = (T)e.NewValue;
            properties[propertyName] = convertTo(newValue);

            if (!handle)
            {
                input.OnPropertyChanged(new(propertyName, oldValue, newValue));
                Modify(input, propertyName, oldValue, newValue);
            }
            return true;
        }
        return false;
    }

    public static bool Set<T>(this IPropertyChanged input, T newValue, bool serialize = true, bool handle = false, [CallerMemberName] string propertyName = "")
        => Set(input, newValue, serialize, handle, propertyName, i => i, i => (T)i);
    
    public static bool Set<T>(this IPropertyChanged input, Expression<Func<T>> propertyName, T value, bool serialize = true, bool handle = false)
        => input.Set(value, serialize, handle, propertyName.Body.As<MemberExpression>().Member.Name);

    ///

    public static bool SetFrom<A, B>(this IPropertyChanged input, A newValue, IConvert<A, B> convert, bool serialize = true, bool handle = false, [CallerMemberName] string propertyName = "")
        => Set(input, newValue, serialize, handle, propertyName, i => convert.ConvertTo(i), i => convert.ConvertBack((B)i));

    public static bool SetFromString<T>(this IPropertyChanged input, T newValue, bool serialize = true, bool handle = false, [CallerMemberName] string propertyName = "") where T : Enum
        => Set(input, newValue, serialize, handle, propertyName, i => $"{i}", i => (T)Enum.Parse(typeof(T), (string)i));

    ///

    static void Modify(IPropertyChanged input, string propertyName, object oldValue, object newValue)
    {
        if (propertyName != nameof(Base.IsModified))
        {
            if (input is Base i)
            {
                Try.Invoke(() =>
                {
                    if (i.GetProperty(propertyName)?.HasAttribute<ModifyAttribute>() == true)
                        i.OnModified(new ModifiedEventArgs(i, propertyName, oldValue, newValue));
                });
            }
        }
    }

    public static void Update<T>(this IPropertyChanged input, Expression<Func<T>> propertyName)
    {
        if (propertyName == null)
            throw new ArgumentNullException(nameof(propertyName));

        var body = propertyName.Body as MemberExpression;
        if (body == null)
            throw new ArgumentNullException($"{nameof(propertyName)}.{nameof(propertyName.Body)}");

        input.Update(body.Member.Name);
    }

    public static void Update(this IPropertyChanged input, string propertyName)
        => input.OnPropertyChanged(new(propertyName, null, null));
}