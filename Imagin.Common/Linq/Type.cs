using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Create a new instance of the given type using <see cref="Activator.CreateInstance()"/>.
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TCast CreateInstance<TCast>(this Type type)
            => (TCast)Activator.CreateInstance(type);

        /// <summary>
        /// Gets whether or not the type is equal to type, <see langword="TType"/>.
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Equals<TCast>(this Type type)
            => type == typeof(TCast);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetEnumValues<TEnum>(this Type type) where TEnum : struct, IFormattable, IComparable, IConvertible
            => type.GetEnumValues().Cast<TEnum>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appearance"></param>
        /// <returns></returns>
        public static ObservableCollection<Enum> GetEnumValues(this Type type, Appearance appearance)
        {
            if (!type.IsEnum)
                throw new ArgumentOutOfRangeException(nameof(type));

            if (appearance == Appearance.None)
                throw new NotSupportedException();

            var result = new ObservableCollection<Enum>();
            foreach (var i in type.GetEnumValues())
            {
                var field = (Enum)i;
                switch (appearance)
                {
                    case Appearance.Hidden:
                    case Appearance.Visible:
                        var attribute = field.GetAttribute<BrowsableAttribute>();
                        var browsable = attribute == null || attribute.Browsable;

                        if (appearance == Appearance.Hidden && !browsable)
                            goto keep;

                        if (appearance == Appearance.Visible && browsable)
                            goto keep;

                        goto skip;
                    case Appearance.All:
                        goto keep;
                }
                keep: result.Add(field);
                skip: continue;
            }
            return result;
        }

        /// <summary>
        /// Gets whether or not the type specifies a property with the given name.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <param name="propertyName">The name of a property the type may specify.</param>
        /// <returns></returns>
        public static bool HasProperty(this Type type, string propertyName)
            => type.GetProperty(propertyName) != null;

        /// <summary>
        /// Gets whether or not the type implements interface, <see langword="TType"/> (or whether <see langword="TType"/> is assignable from the type).
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Implements<TType>(this Type type) where TType : class
        {
            if (!typeof(TType).GetTypeInfo().IsInterface)
                throw new InvalidCastException("Type is not an interface.");

            return typeof(TType).IsAssignableFrom(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
                return false;

            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Attempts to create a new instance of the given type using <see cref="Activator.CreateInstance()"/>.
        /// </summary>
        /// <typeparam name="TCast"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TCast TryCreateInstance<TCast>(this Type type)
        {
            try
            {
                return (TCast)Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                return default(TCast);
            }
        }
    }
}