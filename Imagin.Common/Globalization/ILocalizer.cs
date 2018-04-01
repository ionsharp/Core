namespace Imagin.Common.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILocalizer
    {
        /// <summary>
        /// 
        /// </summary>
        string AssemblyName
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        string GetValue(string Key);
    }
}
