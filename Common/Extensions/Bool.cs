using System.Windows;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static short ToInt16(this bool Value)
        {
            return Value ? (short)1 : (short)0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static int ToInt32(this bool Value)
        {
            return Value ? 1 : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="FalseVisibility"></param>
        /// <returns></returns>
        public static Visibility ToVisibility(this bool Value, Visibility FalseVisibility = Visibility.Collapsed)
        {
            return Value ? Visibility.Visible : FalseVisibility;
        }
    }
}
