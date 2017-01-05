using System.Reflection;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Check whether or not property is public and can be written to.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool IsPublic(this PropertyInfo Value)
        {
            return Value.CanWrite && Value.GetSetMethod(true).IsPublic;
        }
    }
}
