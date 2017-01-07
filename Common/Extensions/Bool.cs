using System.Windows;

namespace Imagin.Common.Extensions
{
    public static class BoolExtensions
    {
        public static Visibility ToVisibility(this bool ToConvert, Visibility FalseVisibility = Visibility.Collapsed)
        {
            return ToConvert ? Visibility.Visible : FalseVisibility;
        }
    }
}
