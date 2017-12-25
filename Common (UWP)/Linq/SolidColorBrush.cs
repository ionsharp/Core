using Imagin.Common.Media;
using Windows.UI.Xaml.Media;

namespace Imagin.Common.Linq
{
    public static class SolidColorBrushExtensions
    {
        public static AccentColor ToAccentColor(this SolidColorBrush Value)
        {
            return Value.Color.ToAccentColor();
        }
    }
}
