using System.Windows;

namespace Imagin.Common.Extensions
{
    public static class BoolExtensions
    {
        public static Visibility ToVisibility(this bool ToConvert)
        {
            return ToConvert ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
