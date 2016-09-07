using System.Windows;

namespace Imagin.Common.Extensions
{
    public static class VisibilityExtensions
    {
        public static Visibility GetOpposite(this Visibility ToEvaluate)
        {
            return ToEvaluate == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
