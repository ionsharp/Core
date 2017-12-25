using System;
using Windows.UI.Xaml;

namespace Imagin.Common.Linq
{
    public static class VisibilityExtensions
    {
        static void AssertInvisibility(Visibility Invisibility)
        {
            if (Invisibility == Visibility.Visible)
                throw new ArgumentException("A value of Visibility.Collapsed or Visibility.Hidden is expected.");
        }

        public static Visibility Invert(this Visibility Visibility, Visibility Invisibility = Visibility.Collapsed)
        {
            AssertInvisibility(Invisibility);
            return Visibility == Visibility.Visible ? Invisibility : Visibility.Visible;
        }

        public static bool ToBoolean(this Visibility Visibility)
        {
            return Visibility == Visibility.Visible;
        }
    }
}
