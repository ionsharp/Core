using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace Imagin.Common.Extensions
{
    public static class EnumExtensions
    {
        #region Legacy 

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static List<T> GetList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        #endregion

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static List<T> ToList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static ObservableCollection<T> ToObservable<T>()
        {
            return new ObservableCollection<T>(Enum.GetValues(typeof(T)).Cast<T>().ToList());
        }

        /// <summary>
        /// Imagin.Common: Parses string to enum.
        /// </summary>
        public static T Parse<T>(string ToParse)
        {
            return (T)Enum.Parse(typeof(T), ToParse, true);
        }

        /// <summary>
        /// Imagin.Common: Parses string to enum.
        /// </summary>
        public static T ParseEnum<T>(this string ToParse)
        {
            return (T)Enum.Parse(typeof(T), ToParse, true);
        }
    }
}
