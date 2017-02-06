using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Imagin.Common.Extensions
{
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
        /// Converts from string to enum.
        /// </summary>
        public static TEnum ParseEnum<TEnum>(this string ToParse, bool IgnoreCase = true) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return (TEnum)Enum.Parse(typeof(TEnum), ToParse, IgnoreCase);
        }

        /// <summary>
        /// Attempts to convert from string to enum.
        /// </summary>
        public static TEnum TryParseEnum<TEnum>(this string ToParse, bool IgnoreCase = true) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            var Result = default(TEnum);
            Enum.TryParse<TEnum>(ToParse, IgnoreCase, out Result);
            return Result;
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
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="type"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool Has<TEnum>(this Enum type, TEnum Value) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return type.HasFlag(Value as Enum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has(this Enum type, Enum value)
        {
            return type.HasFlag(value);
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
