using Imagin.Common.Media;
using System;
using Windows.UI;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class AccentColorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Color ToColor(this AccentColor Value)
        {
            try
            {
                return Value.GetAttribute<KeyAttribute>().Key.ToString().ToColor();
            }
            catch
            {
                return default(Color);
            }
        }
    }
}
