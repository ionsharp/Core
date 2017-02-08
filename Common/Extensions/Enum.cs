using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static List<TEnum> GetList<TEnum>() where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return GetValues<TEnum>().ToList<TEnum>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static ObservableCollection<TEnum> GetObservableCollection<TEnum>() where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return new ObservableCollection<TEnum>(GetValues<TEnum>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum Add<TEnum>(this Enum type, TEnum value) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), type.To<int>() | value.To<int>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Enum Add(this Enum type, Enum value)
        {
            return Enum.ToObject(type.GetType(), type.To<int>() | value.To<int>()) as Enum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has(this Enum source, Enum value) 
        {
            return source.HasFlag(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has<TEnum>(this TEnum source, TEnum value) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return source.As<Enum>().HasFlag(value as Enum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool HasAll<TEnum>(this TEnum source, params TEnum[] values) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            foreach (var i in values)
            {
                if (!source.Has(i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool HasAny<TEnum>(this TEnum source, params TEnum[] values) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            foreach (var i in values)
            {
                if (source.Has(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool HasNone<TEnum>(this TEnum source, params TEnum[] values) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return !source.HasAny(values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum Remove<TEnum>(this Enum type, TEnum value) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), type.To<int>() & ~value.To<int>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Enum Remove(this Enum type, Enum value)
        {
            return Enum.ToObject(type.GetType(), type.To<int>() & ~value.To<int>()) as Enum;
        }
    }
}
