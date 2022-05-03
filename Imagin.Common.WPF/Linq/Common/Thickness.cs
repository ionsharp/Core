using System.Windows;

namespace Imagin.Common.Linq
{
    public static class XThickness
    {
        public static double Height(this Thickness thickness) => thickness.Top + thickness.Bottom;

        public static double Width(this Thickness thickness) => thickness.Left + thickness.Right;

        public static Thickness Invert(this Thickness input) => new(-input.Left, -input.Top, -input.Right, -input.Bottom);
    }
}