using WPFLocalizeExtension.Engine;

namespace Imagin.Common.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class Localizer : ILocalizer
    {
        readonly string _assemblyName;
        /// <summary>
        /// 
        /// </summary>
        public string AssemblyName => _assemblyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssemblyName"></param>
        public Localizer(string AssemblyName) => _assemblyName = AssemblyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string GetValue(string Key) => GetValue<string>(Key, AssemblyName);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(string key)
        {
            var Result = LocalizeDictionary.Instance.GetLocalizedObject(key, null, LocalizeDictionary.Instance.Culture);

            if (Result is TValue)
                return (TValue)Result;

            return default(TValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <param name="assemblyName"></param>
        /// <param name="dictionaryName"></param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(string key, string assemblyName, string dictionaryName = "Main")
        {
            var Result = LocalizeDictionary.Instance.GetLocalizedObject(assemblyName, dictionaryName, key, LocalizeDictionary.Instance.Culture);

            if (Result is TValue)
                return (TValue)Result;

            return default(TValue);
        }
    }
}
