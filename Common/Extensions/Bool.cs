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
        /// <param name="FalseVisibility"></param>
        /// <returns></returns>
        public static Visibility ToVisibility(this bool Value, Visibility FalseVisibility = Visibility.Collapsed)
        {
            return Value ? Visibility.Visible : FalseVisibility;
        }
    }
}
