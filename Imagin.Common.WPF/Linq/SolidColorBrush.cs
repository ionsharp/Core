using System.Windows.Media;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class SolidColorBrushExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Brush"></param>
        /// <returns></returns>
        public static string ToHex(this SolidColorBrush Brush)
        {
            return Brush.Color.ToHex();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Brush"></param>
        /// <returns></returns>
        public static string ToHexWithAlpha(this SolidColorBrush Brush)
        {
            return Brush.Color.ToHexWithAlpha();
        }
    }
}
