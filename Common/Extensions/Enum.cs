using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Imagin.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static IEnumerable<TEnum> ToEnumerable<TEnum>() where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static IEnumerable<TEnum> ToEnumerable<TEnum>(this TEnum ToEvaluate) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// Imagin.Common: Converts from string to enum.
        /// </summary>
        public static TEnum ParseEnum<TEnum>(this string ToParse, bool IgnoreCase = true) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return (TEnum)Enum.Parse(typeof(TEnum), ToParse, IgnoreCase);
        }

        /// <summary>
        /// Imagin.Common: Attempts to convert from string to enum.
        /// </summary>
        public static TEnum TryParseEnum<TEnum>(this string ToParse, bool IgnoreCase = true) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            var Result = default(TEnum);
            Enum.TryParse<TEnum>(ToParse, IgnoreCase, out Result);
            return Result;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static List<TEnum> ToList<TEnum>() where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return default(TEnum).ToEnumerable<TEnum>().ToList<TEnum>();
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static List<TEnum> ToList<TEnum>(this TEnum ToConvert) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return default(TEnum).ToEnumerable<TEnum>().ToList<TEnum>();
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static ObservableCollection<TEnum> ToObservableCollection<TEnum>() where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return new ObservableCollection<TEnum>(default(TEnum).ToList<TEnum>());
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static ObservableCollection<TEnum> ToObservableCollection<TEnum>(this TEnum ToConvert) where TEnum : struct, IFormattable, IComparable, IConvertible
        {
            return new ObservableCollection<TEnum>(default(TEnum).ToList<TEnum>());
        }
    }
}
