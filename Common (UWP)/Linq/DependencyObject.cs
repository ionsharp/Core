using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object"></param>
        /// <returns></returns>
        public static T GetChildOfType<T>(this DependencyObject Object) where T : DependencyObject
        {
            if (Object == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Object); i++)
            {
                var Child = VisualTreeHelper.GetChild(Object, i);
                var Result = (Child as T) ?? GetChildOfType<T>(Child);
                if (Result != null) return Result;
            }

            return null;
        }
    }
}
