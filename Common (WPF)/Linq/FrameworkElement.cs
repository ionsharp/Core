using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Helper method to determine if the given framework element has the mouse over it or not.
        /// </summary>
        /// <param name="element">The FrameworkElement to test for mouse containment.</param>
        /// <returns>True, if the mouse is over the FrameworkElement; false, otherwise.</returns>
        public static bool ContainsMouse(this FrameworkElement element)
        {
            var point = Mouse.GetPosition(element);
            return
            (
                point.X >= 0
                &&
                point.X <= element.ActualWidth
                &&
                point.Y >= 0
                &&
                point.Y <= element.ActualHeight
            );
        }
    }
}
