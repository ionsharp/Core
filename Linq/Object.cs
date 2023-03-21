using Imagin.Core.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Imagin.Core.Linq;

public static partial class XObject
{
    /// <summary>Returns object as specified type.</summary>
    public static Type As<Type>(this object input) => input is Type ? (Type)input : default;

    ///

    static T DeepClone<T>(this T input, ICloneHandler handler, BindingFlags flags, MemberTypes types, bool log, List<Type> visited)
    {
        if (log)
        {
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine($"Deep cloning [{input?.GetType()}]...");
        }

        var type = input.GetType();
        if (visited.Contains(type))
            return input;

        visited.Add(type);

        //(a) Null
        if (ReferenceEquals(input, null))
        {
            if (log)
            {
                System.Diagnostics.Debug.WriteLine("The object is null!");
                Analytics.Log.Write<ICloneable>(new ArgumentNullException(nameof(input)));
            }

            return default;
        }

        //(b) Non cloneable
        if (type.HasAttribute<NonCloneableAttribute>())
        {
            if (log)
            {
                System.Diagnostics.Debug.WriteLine("The object can't be cloned!");
                Analytics.Log.Write<ICloneable>(new NotCloneableException<T>());
            }

            return default;
        }

        //(0) CloneHandler
        if (handler?.Clone(input) is T handle)
        {
            if (log)
                System.Diagnostics.Debug.WriteLine("Handler returned nothing!");

            return handle;
        }

        //(1) ICloneable
        /*
        if (input is ICloneable i)
            return (T)i.Clone();
        */

        //(2.a) Value types
        if (input is string || (type.IsValueType && type.IsPrimitive))
        {
            if (log)
                System.Diagnostics.Debug.WriteLine("Not primitive or value type!");

            return input;
        }

        //(2.b) Other (arbitrary) types
        if (input is Assembly || input is Type)
        {
            if (log)
                System.Diagnostics.Debug.WriteLine("Not [System.Reflection.Assembly] or [System.Type]!");

            return input;
        }

        //(3) Binary serialization (unsecure!)
        try
        {
            if (log)
                System.Diagnostics.Debug.WriteLine("Attempting binary serialization...");

            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", nameof(input));

            using Stream stream = new MemoryStream();

            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, input);

            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
        catch (Exception e)
        {
            if (log)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Analytics.Log.Write<ICloneable>(e);
            }
        }

        //(4) Manual (slow, but last resort)
        T result = default;
        Try.Invoke(() =>
        {
            if (log)
                System.Diagnostics.Debug.WriteLine("Attempting manual creation...");

            result = type.Create<T>();
            type.EachMemberValue
            (
                (field, value)
                    => field.If(i => !i.HasAttribute<NonCloneableAttribute>()
                    && i.CanSet(), i => i.SetValue(result, value.DeepClone(handler, flags, types, log, visited))),
                (property, value)
                    => property.If(i => !i.HasAttribute<NonCloneableAttribute>()
                    && i.CanSet(), i => i.SetValue(result, value.DeepClone(handler, flags, types, log, visited))),
                flags, types
            );
        },
        e =>
        {
            if (log)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Analytics.Log.Write<ICloneable>(e);
            }
        });

        //(5) Expression trees...?

        if (log)
            System.Diagnostics.Debug.WriteLine("");

        return result;
    }

    public static T DeepClone<T>(this T input, ICloneHandler handler = null, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, MemberTypes types = MemberTypes.Field | MemberTypes.Property, bool log = false)
        => input.DeepClone(handler, flags, types, log, new());

    public static T DeepClone<T>(this T input, ICloneHandler handler, bool log)
        => input.DeepClone(handler, BindingFlags.Instance | BindingFlags.Public, MemberTypes.Field | MemberTypes.Property, log, new());

    ///

    public static T GetAttribute<T>(this object input) where T : Attribute => input.GetType().GetAttribute<T>();

    public static string GetDescription(this object input) => input?.GetAttribute<System.ComponentModel.DescriptionAttribute>()?.Description ?? input?.GetAttribute<DescriptionAttribute>()?.Description;

    public static string GetName(this object input) => input?.GetAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName ?? input?.GetAttribute<NameAttribute>()?.Name;

    public static string GetLargeImage(this object input) => input?.GetAttribute<ImageAttribute>()?.LargeImage;

    public static string GetSmallImage(this object input) => input?.GetAttribute<ImageAttribute>()?.SmallImage;

    ///

    public static object GetMemberValue(this object input, MemberInfo member) => member is FieldInfo field ? field.GetValue(input) : member is PropertyInfo property ? property.GetValue(input) : null;

    ///

    public static FieldInfo GetField(this object input, string fieldName) => input?.GetType().GetField(fieldName);

    /// <summary>Get the value of the given field.</summary>
    public static object GetFieldValue(this object input, string fieldName) => input?.GetType().GetField(fieldName).GetValue(input);

    ///

    public static PropertyInfo GetProperty(this object input, string propertyName) => input?.GetType().GetProperty(propertyName);

    /// <summary>Get the value of the given property.</summary>
    public static object GetPropertyValue(this object input, string propertyName)
    {
        var result = input?.GetType().GetProperty(propertyName);
        return result?.GetValue(input, null);
    }

    ///

    public static bool HasAttribute<T>(this object input) where T : Attribute => input.GetAttribute<T>() != null;

    ///

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
    public static Output IfReturn<Input, Output>(this object input, Func<Input, Output> action)
    {
        if (input is Input i)
            return action(i);

        return default;
    }

    ///

    /// <summary>Gets "null" if the object is <see langword="null"/> or "not null" if the object isn't.</summary>
    public static string NullString(this object input, Func<object, string> result = null) => input == null ? "null" : result?.Invoke(input) ?? "not null";

    ///

    public static object ResetMembers(this object input, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, MemberTypes types = MemberTypes.Field | MemberTypes.Property)
    {
        if (input == null)
            return null;

        var type = input.GetType();

        //Value
        if (type == typeof(string) || type.IsValueType)
            return input.GetType().GetDefaultValue();

        //Reference
        if (type.IsClass)
        {
            Action<FieldInfo, object> fieldAction
            = (field, value) =>
            {
                if (field.CanSet())
                {
                    var result = field.GetValue(input).ResetMembers(flags, types);
                    field.SetValue(input, result);
                }
            };

            Action<PropertyInfo, object> propertyAction
            = (property, value) =>
            {
                if (property.CanSet())
                {
                    var result = property.GetValue(input).ResetMembers(flags, types);
                    property.SetValue(input, result);
                }
            };

            type.EachMemberValue(fieldAction, propertyAction, flags, types);
            return input;
        }
        return null;
    }

    ///

    public static void SetMemberValue(this object input, MemberInfo member, object value)
    {
        if (member is FieldInfo field)
            field.SetValue(input, value);

        else if (member is PropertyInfo property)
            property.SetValue(input, value);
    }

    ///

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

    ///

    /// <summary>Returns object cast to given type.</summary>
    public static T To<T>(this object input) => (T)input;

    ///

    public static decimal Decimal(this object i) => Convert.ToDecimal(i);

    public static double Double(this object i) => Convert.ToDouble(i);

    public static short Int16(this object i) => Convert.ToInt16(i);

    public static int Int32(this object i) => Convert.ToInt32(i);

    public static long Int64(this object i) => Convert.ToInt64(i);

    public static float Single(this object i) => Convert.ToSingle(i);
}