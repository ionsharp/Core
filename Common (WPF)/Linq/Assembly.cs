using System.Diagnostics;
using System.Reflection;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Assembly"></param>
        /// <returns></returns>
        public static FileVersionInfo GetVersionInfo(this Assembly Assembly)
        {
            return FileVersionInfo.GetVersionInfo(Assembly.Location);
        }
    }
}
