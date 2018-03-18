using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class OrientationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Orientation Invert(this Orientation input)
            => input == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
    }
}
