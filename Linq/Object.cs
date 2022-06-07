using System;
using System.Reflection;

namespace Imagin.Core.Linq;

public static partial class XObject
{
    /// <summary>Returns object as specified type.</summary>
    public static Type As<Type>(this object input) => input is Type ? (Type)input : default;

    public static T GetAttribute<T>(this object value) where T : Attribute => value.GetType().GetAttribute<T>();

    /// <summary>Get the value of the given field.</summary>
    public static object GetFieldValue(this object input, string fieldName) => input.GetType().GetField(fieldName).GetValue(input);

    /// <summary>Get the value of the given property.</summary>
    public static object GetPropertyValue(this object input, string propertyName) => input.GetType().GetProperty(propertyName).GetValue(input, null);

    /// <summary>Calls the given action if the object is not <see langword="null"/>.</summary>
    public static void If<T>(this object input, Action<T> action)
    {
        if (input is T i)
            action(i);
    }

    /// <summary>Calls the given action if the object is not <see langword="null"/>.</summary>
    public static void If<T>(this object input, Predicate<T> @if, Action<T> then)
    {
        if (input is T i)
        {
            if (@if(i))
                then.Invoke(i);
        }
    }

    /// <summary>Calls the given action if the object is not <see langword="null"/>.</summary>
    public static Output IfThen<Input, Output>(this object input, Func<Input, Output> action)
    {
        if (input is Input i)
            return action(i);

        return default;
    }

    /// <summary>Checks if given object's type implements interface (T).</summary>
    public static bool Implements<Type>(this object input) where Type : class => (bool)input.GetType()?.GetTypeInfo()?.Implements<Type>();

    /// <summary>Gets if <see cref="object.GetType"/> specifies the <see cref="SerializableAttribute"/>.</summary>
    public static bool IsSerializable(this object input) => input.GetType().GetAttribute<SerializableAttribute>() != null;
        
    /// <summary>Gets whether or not the object is <see cref="System.Nullable"/>.</summary>
    public static bool Nullable(this object input) => input.Nullable<object>();

    /// <summary>Gets "null" if the object is <see langword="null"/> or "not null" if the object isn't.</summary>
    public static string NullString(this object input, Func<object, string> result = null) => input == null ? "null" : result?.Invoke(input) ?? "not null";
    
    /// <summary>Resets all specified fields and properties.</summary>
    public static void Reset(this object input, BindingFlags filter = BindingFlags.Public | BindingFlags.Instance)
    {
        var properties = input.GetType().GetProperties(filter);
        if (properties?.Length > 0)
        {
            foreach (var i in properties)
            {
                Try.Invoke(() =>
                {
                    object defaultValue = null; //Now what?
                    i.SetValue(input, defaultValue);
                }, 
                e => { });
            }
        }
    }

    /// <summary>Set value of given field.</summary>
    public static void SetFieldValue<T>(this object input, string fieldName, T value)
    {
        var field = input.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
        field.SetValue(input, value);
    }

    /// <summary>Set value of given property.</summary>
    public static void SetPropertyValue<T>(this object input, string propertyName, T value)
    {
        var property = input.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (property?.CanWrite == true)
            property.SetValue(input, value, null);
    }

    /// <summary>Cast object to given type.</summary>
    public static T To<T>(this object input) => (T)input;

    //...

    public static decimal Decimal(this object i) => Convert.ToDecimal(i);

    public static double Double(this object i) => Convert.ToDouble(i);

    public static short Int16(this object i) => Convert.ToInt16(i);

    public static int Int32(this object i) => Convert.ToInt32(i);

    public static long Int64(this object i) => Convert.ToInt64(i);

    public static float Single(this object i) => Convert.ToSingle(i);
}