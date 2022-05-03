using System.Windows;

namespace Imagin.Common.Linq
{
    public static class XVisibility
    {
        public static Visibility Invert(this Visibility input) => input == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

        public static bool Boolean(this Visibility input) => input == Visibility.Visible;
    }
}