using System;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class SpecialFolderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetPath(this Environment.SpecialFolder Value)
        {
            return Environment.GetFolderPath(Value);
        }
    }
}
