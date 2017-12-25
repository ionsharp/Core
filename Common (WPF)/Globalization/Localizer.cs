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
            return LocalizationProvider.GetValue<string>(Key, AssemblyName);
        }
    }
}
