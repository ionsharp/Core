using System.Reflection;
using WPFLocalizeExtension.Engine;
using Imagin.Common.Linq;

namespace Imagin.Common.Globalization
{
    /// <summary>
    /// A utility to access localization resources globally.
    /// </summary>
    public static class LocalizationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static T GetValue<T>(string Key)
        {
            var Result = LocalizeDictionary.Instance.GetLocalizedObject(Key, null, LocalizeDictionary.Instance.Culture);

            if (Result is T)
                return (T)Result;

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="Source"></param>
        /// <param name="Dictionary"></param>
        /// <returns></returns>
        public static T GetValue<T>(string Key, string Source, string Dictionary = "Main")
        {
            var Result = LocalizeDictionary.Instance.GetLocalizedObject(Source, Dictionary, Key, LocalizeDictionary.Instance.Culture);

            if (Result is T)
                return (T)Result;

            return default(T);
        }
    }
}
