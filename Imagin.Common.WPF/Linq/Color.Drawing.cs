using System.Drawing;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ColorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color ToMedia(this Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
