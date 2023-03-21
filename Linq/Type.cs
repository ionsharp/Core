using Imagin.Core.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Imagin.Core.Linq;

public static class XType
{
    /// <summary>Create a new instance of the given type using <see cref="Activator.CreateInstance()"/>.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    public static T Create<T>(this Type input, params object[] parameters) => (T)Activator.CreateInstance(input, parameters);

    public static bool Equals<Type>(this System.Type input) => input == typeof(Type);

    ///

    public static T GetDefault<T>() => default;

    public static object GetDefaultValue(this Type input, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, MemberTypes types = MemberTypes.Field | MemberTypes.Property)
    {
        //String: ""
        if (input == typeof(string))
            return "";

        //Value type
        if (input.IsValueType)
            return typeof(XType).GetMethod(nameof(GetDefault)).MakeGenericMethod(input).Invoke(null, null);

        //Reference type
        if (input.IsClass)
            return input.Create<object>();

        return null;
    }

    ///

    public static ObservableCollection<Enum> GetEnumCollection(this Type input, Appearance appearance, bool sort = false)
        => new(input.GetEnumValues(appearance, sort));

    public static ObservableCollection<T> GetEnumCollection<T>(this Type input, Appearance appearance, Func<Enum, T> select, bool sort = false)
    {
        var values = input.GetEnumValues(appearance, sort);
        return new ObservableCollection<T>(values.Select(select));
    }

    public static ObservableCollection<T> GetEnumCollection<T>(this Type input, Appearance appearance, bool sort = false)
    {
        var values = input.GetEnumValues<T>(appearance, sort);
        return new ObservableCollection<T>(values);
    }

    ///

    public static IEnumerable<Enum> GetEnumValues(this Type input, Appearance appearance, bool sort = false)
        => input.GetEnumValues<Enum>(appearance, sort);

    public static IEnumerable<T> GetEnumValues<T>(this Type input, Appearance appearance, bool sort = false)
    {
        if (!input.IsEnum)
            throw new ArgumentOutOfRangeException(nameof(input));

        if (appearance == Appearance.None)
            throw new NotSupportedException();

        var values = input.GetEnumValues().Cast<Enum>();
        values = sort ? values.OrderBy(i => i.ToString()) : values;

        foreach (var i in values)
        {
            switch (appearance)
            {
                case Appearance.Hidden:
                    if (i.IsHidden())
                        goto case Appearance.All;

                    break;

                case Appearance.Visible:
                    if (!i.IsHidden())
                        goto case Appearance.All;

                    break;

                case Appearance.All:
                    yield return i.As<T>();
                    break;
            }
        }

        yield break;
    }

    ///

    public static IEnumerable<MemberInfo> GetMembers(this Type input, BindingFlags flags, MemberTypes types, bool? hidden = null, bool? settable = null, bool? serializable = null, bool? @static = false)
        => input.GetMembers(flags).Where(i => types.HasFlag(i.MemberType) && CheckHidden(hidden, i) && CheckSettable(settable, i) && CheckSerializable(serializable, i) && CheckStatic(@static, i));

    ///

    public static IEnumerable<MethodInfo> Get_Methods(this Type input, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, bool hidden = false)
        => input.GetMethods(flags)?.Where(i => (hidden || !i.HasAttribute<HideAttribute>()) && i.ReturnType == typeof(void) && !(i.GetParameters()?.Length > 0));

    public static IEnumerable<MethodInfo> GetCopyMethods(this Type input, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, bool hidden = false)
        => input.GetMethods(flags)?.Where(i => (hidden || !i.HasAttribute<HideAttribute>()) && i.ReturnType != typeof(void) && !(i.GetParameters()?.Length > 0) && !i.IsGetter());

    public static IEnumerable<MethodInfo> GetPasteMethods(this Type input, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, bool hidden = false)
        => input.GetMethods(flags)?.Where(i => (hidden || !i.HasAttribute<HideAttribute>()) && i.ReturnType == typeof(void) && i.GetParameters()?.Length == 1 && !i.IsEvent() && !i.IsSetter());

    ///

    public static string GetRealName(this Type input, bool full)
    {
        if (!input.IsGenericType)
            return input.Name;

        var result = new StringBuilder();
        result.Append(input.Name.Substring(0, input.Name.IndexOf('`')));
        result.Append('<');

        bool appendComma = false;
        foreach (Type arg in input.GetGenericArguments())
        {
            if (appendComma) result.Append(',');
            result.Append(GetRealName(arg, full));
            appendComma = true;
        }

        result.Append('>');

        if (full)
            result.Insert(0, input.FullName.Substring(0, input.FullName.Length - input.Name.Length));

        return result.ToString();
    }

    ///

    static bool CheckHidden(bool? check, MemberInfo input)
        => check == null ? true : check.Value ? input.IsHidden() : !input.IsHidden();

    static bool CheckSerializable(bool? check, MemberInfo input)
        => check == null ? true : check.Value ? input.IsSerializable() : !input.IsSerializable();

    static bool CheckSettable(bool? check, MemberInfo input)
        => check == null ? true : check.Value ? input.CanSet() : !input.CanSet();

    static bool CheckStatic(bool? check, MemberInfo input)
        => check == null ? true : check.Value ? input.IsStatic() : !input.IsStatic();

    ///

    #region static void EachMember(Value)

    static bool CanEnumerate(Type type) => type?.IsClass == true && type != typeof(string) && type != typeof(Type);

    ///

    static void EachMember(this Type input, object source, Action<FieldInfo> fieldAction, Action<PropertyInfo> propertyAction, BindingFlags flags, MemberTypes types, int depth = 0, List<Type> root = null)
    {
        if (input == null) return;

        var type = source?.GetType() ?? input;
        if (depth == 0)
        {
            root = new();
            root.Add(type);
        }
        else
        {
            //Avoid infinite loop
            if (root.Contains(type)) return;
            root.Add(type);
        }

        foreach (var i in type.GetMembers(flags, types, null, true, true))
        {
            object value = null;
            if (i is FieldInfo field)
            {
                fieldAction(field);
                value = source?.GetFieldValue(field.Name);
                if (CanEnumerate(value?.GetType() ?? field.FieldType))
                    value.If(j => j.GetType().EachMember(j, fieldAction, propertyAction, flags, types, depth + 1, root), () => field.FieldType.EachMember(null, fieldAction, propertyAction, flags, types, depth + 1, root));
            }

            else if (i is PropertyInfo property)
            {
                propertyAction(property);
                value = source?.GetPropertyValue(property.Name);
                if (CanEnumerate(value?.GetType() ?? property.PropertyType))
                    value.If(j => j.GetType().EachMember(j, fieldAction, propertyAction, flags, types, depth + 1, root), () => property.PropertyType.EachMember(null, fieldAction, propertyAction, flags, types, depth + 1, root));
            }
        }
        root.Remove(type);
    }

    public static void EachMember(this Type input, Action<FieldInfo> fieldAction, Action<PropertyInfo> propertyAction, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, MemberTypes types = MemberTypes.Field | MemberTypes.Property)
    {
        if (input == null) return;
        input.EachMember(input, fieldAction, propertyAction, flags, types, 0, null);
    }

    ///

    static void EachMemberValue(this Type input, object source, Action<FieldInfo, object> fieldAction, Action<PropertyInfo, object> propertyAction, BindingFlags flags, MemberTypes types, int depth = 0, List<Type> root = null)
    {
        if (input == null) return;

        var type = source?.GetType() ?? input;
        if (depth == 0)
        {
            root = new();
            root.Add(type);
        }
        else
        {
            //Avoid infinite loop
            if (root.Contains(type)) return;
            root.Add(type);
        }

        foreach (var i in type.GetMembers(flags, types, null, true, true))
        {
            object value = null;
            if (i is FieldInfo field)
            {
                value = source?.GetFieldValue(field.Name);
                fieldAction(field, value);
                if (CanEnumerate(value?.GetType() ?? field.FieldType))
                    value.If(j => j.GetType().EachMemberValue(j, fieldAction, propertyAction, flags, types, depth + 1, root), () => field.FieldType.EachMemberValue(null, fieldAction, propertyAction, flags, types, depth + 1, root));
            }

            else if (i is PropertyInfo property)
            {
                value = source?.GetPropertyValue(property.Name);
                propertyAction(property, value);
                if (CanEnumerate(value.GetType() ?? property.PropertyType))
                    value.If(j => j.GetType().EachMemberValue(j, fieldAction, propertyAction, flags, types, depth + 1, root), () => property.PropertyType.EachMemberValue(null, fieldAction, propertyAction, flags, types, depth + 1, root));
            }
        }
        root.Remove(type);
    }

    public static void EachMemberValue(this Type input, Action<FieldInfo, object> fieldAction, Action<PropertyInfo, object> propertyAction, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, MemberTypes types = MemberTypes.Field | MemberTypes.Property)
    {
        if (input == null) return;
        input.EachMemberValue(input, fieldAction, propertyAction, flags, types, 0, null);
    }

    ///

    #endregion

    ///

    public static bool HasMemberWithAttribute(this Type input, Type attribute, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public, MemberTypes types = MemberTypes.Event | MemberTypes.Field | MemberTypes.Method | MemberTypes.Property)
    {
        var members = input.GetMembers(flags, types);
        foreach (var i in members)
        {
            if (i.HasAttribute(attribute))
                return true;
        }
        return false;
    }

    ///

    /// <summary>
    /// Gets whether or not a type implements an interface of <see cref="{T}"/>.
    /// </summary>
    public static bool Implements<T>(this Type input)
    {
        return input.Implements(typeof(T));
    }

    /// <summary>
    /// Gets whether or not a type implements an interface of the given type.
    /// </summary>
    public static bool Implements(this Type input, Type b)
    {
        if (!b.GetTypeInfo().IsInterface)
            throw new InvalidCastException("Type is not an interface.");

        return b.IsAssignableFrom(input);
    }

    ///

    /// <summary>
    /// Gets whether or not a type inherits <see cref="{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <param name="include">Whether or not to include the given type.</param>
    /// <returns></returns>
    public static bool Inherits<T>(this Type input, bool include = false) => input.Inherits(typeof(T), include);

    /// <summary>
    /// Gets whether or not a type inherits the given type.
    /// </summary>
    public static bool Inherits(this Type a, Type b, bool include = false, [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
    {
        if (!include)
        {
            if (a.Equals(b))
                return false;
        }
        if (!a.IsInterface)
        {
            while (!a.Equals(typeof(object)))
            {
                if (a.Equals(b))
                    return true;

                a = a.BaseType;
            }
        }
        return false;
    }

    ///

    public static void EachBaseType(this Type input, Action<Type> each) => input.GetBaseTypes().ForEach(i => each(i));

    /// <summary>Gets all types a type derives from.</summary>
    public static IEnumerable<Type> GetBaseTypes(this Type input)
    {
        Type result = input;
        while (!result.Equals(typeof(object)))
        {
            result = result.BaseType;
            yield return result;

            if (result.GetAttribute<BaseAttribute>()?.Type is Type baseType && result == baseType)
                break;
        }
        yield break;
    }

    ///

    public static bool IsGeneric<T>(this Type input)
    {
        foreach (var i in input.GenericTypeArguments)
        {
            if (i == typeof(T))
                return true;
        }
        return false;
    }

    public static bool IsGenericOf<T>(this Type input)
    {
        foreach (var i in input.GenericTypeArguments)
        {
            if (i.IsSubclassOf(typeof(T)))
                return true;
        }
        return false;
    }

    public static bool IsHidden(this Type input)
        => input.GetAttribute<System.ComponentModel.BrowsableAttribute>()?.Browsable == false || input.HasAttribute<HideAttribute>();

    /// <summary>
    /// Gets whether or not the type is <see cref="Nullable{T}"/>.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNullable(this Type input)
    {
        if (!input.GetTypeInfo().IsGenericType)
            return false;

        return input.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}