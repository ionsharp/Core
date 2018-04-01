using System.Windows;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class VisibilityExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Visibility Invert(this Visibility Value)
        {
            return Value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this Visibility Value)
        {
            return Value == Visibility.Visible;
        }
    }
}
