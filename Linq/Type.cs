using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Imagin.Core.Linq
{
    public static class XType
    {
        /// <summary>Create a new instance of the given type using <see cref="Activator.CreateInstance()"/>.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Create<T>(this Type input, params object[] parameters) => (T)Activator.CreateInstance(input, parameters);

        public static bool Equals<Type>(this System.Type input) => input == typeof(Type);

        //...

        public static T GetDefault<T>() => default;

        public static object GetDefaultValue(this Type input)
            => typeof(XType).GetMethod(nameof(GetDefault)).MakeGenericMethod(input).Invoke(null, null);

        //...

        public static ObservableCollection<Enum> GetEnumCollection(this Type input, Appearance appearance, bool sort = false)
            => new ObservableCollection<Enum>(input.GetEnumValues(appearance, sort));

        public static ObservableCollection<T> GetEnumCollection<T>(this Type input, Appearance appearance, Func<Enum, T> select, bool sort = false)
        {
            var values = input.GetEnumValues(appearance, sort);
            var result = new ObservableCollection<T>();
            foreach (var i in values)
                result.Add(select(i));

            return result;
        }

        public static ObservableCollection<T> GetEnumCollection<T>(this Type input, Appearance appearance, bool sort = false)
        {
            var values = input.GetEnumValues<T>(appearance, sort);
            var result = new ObservableCollection<T>();
            foreach (var i in values)
                result.Add(i);

            return result;
        }

        //...

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
                var hidden 
                    = i.GetAttribute<HiddenAttribute>()?.Hidden ?? false;
                var visible 
                    = i.GetAttribute<VisibleAttribute>()?.Visible ?? true;

                switch (appearance)
                {
                    case Appearance.Hidden:
                        if (!hidden || visible)
                            continue;

                        goto case Appearance.All;

                    case Appearance.Visible:
                        if (hidden || !visible)
                            continue;

                        goto case Appearance.All;

                    case Appearance.All:
                        yield return i.As<T>();
                        break;
                }
            }

            yield break;
        }

        //...

        static bool IsHidden(this MemberInfo input)
            => input.GetAttribute<System.ComponentModel.BrowsableAttribute>()?.Browsable == false || input.GetAttribute<HiddenAttribute>()?.Hidden == true || input.GetAttribute<VisibleAttribute>()?.Visible == false;

        static bool IsMutable(this MemberInfo input)
        {
            if (input is FieldInfo field)
                return !field.IsInitOnly;

            if (input is PropertyInfo property)
                return property.GetGetMethod(false) != null && property.GetSetMethod(false) != null;

            return false;
        }

        static bool IsSerializable(this MemberInfo input)
            => input.GetAttribute<NonSerializedAttribute>() == null && input.GetAttribute<SerializeAttribute>()?.Serializable != false  && input.GetAttribute<System.Xml.Serialization.XmlIgnoreAttribute>() == null;

        static bool CheckHidden(bool? check, MemberInfo input)
        {
            if (check == null)
                return true;

            if (check.Value)
                return input.IsHidden();

            return !input.IsHidden();
        }

        static bool CheckMutable(bool? check, MemberInfo input)
        {
            if (check == null)
                return true;

            if (check.Value)
                return input.IsMutable();

            return !input.IsMutable();
        }

        static bool CheckSerializable(bool? check, MemberInfo input)
        {
            if (check == null)
                return true;

            if (check.Value)
                return input.IsSerializable();

            return !input.IsSerializable();
        }

        public static IEnumerable<MemberInfo> GetMembers(this Type input, BindingFlags flags, MemberTypes types, bool? hidden = null, bool? mutable = null, bool? serializable = null)
        {
            return input.GetMembers(flags).Where(i => types.HasFlag(i.MemberType) && CheckHidden(hidden, i) && CheckMutable(mutable, i) && CheckSerializable(serializable, i));
        }

        //...

        /// <summary>
        /// Gets whether or not a type implements an interface of <see cref="{T}"/>.
        /// </summary>
        public static bool Implements<T>(this Type input) where T : class
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

        /// <summary>
        /// Gets all types a type derives from.
        /// </summary>
        public static IEnumerable<Type> Inheritance(this Type input)
        {
            Type result = input;
            while (!result.Equals(typeof(object)))
            {
                result = result.BaseType;
                yield return result;
            }
        }

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
            => input.GetAttribute<System.ComponentModel.BrowsableAttribute>()?.Browsable == false || input.GetAttribute<HiddenAttribute>()?.Hidden == true || input.GetAttribute<VisibleAttribute>()?.Visible == false;

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
}