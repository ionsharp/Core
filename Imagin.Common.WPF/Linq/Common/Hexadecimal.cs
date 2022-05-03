using Imagin.Common.Numbers;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    public static class XHexadecimal
    {
        public static Color Color(this Hexadecimal input)
        {
            var result = input.Convert();
            return System.Windows.Media.Color.FromArgb(result.A, result.R, result.G, result.B);
        }

        public static SolidColorBrush SolidColorBrush(this Hexadecimal input) => new BrushConverter().ConvertFrom($"#{input}") as SolidColorBrush;
    }
}