using WPFLocalizeExtension.Engine;

namespace Imagin.Common.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class Localizer : ILocalizer
    {
        readonly string assemblyName;
        /// <summary>
        /// 
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return assemblyName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssemblyName"></param>
        public Localizer(string AssemblyName)
        {
            assemblyName = AssemblyName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string GetValue(string Key)
        {
            return GetValue<string>(Key, AssemblyName);
        }

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
