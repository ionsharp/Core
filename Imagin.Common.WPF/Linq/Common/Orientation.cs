using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    public static class XOrientation
    {
        public static Orientation Invert(this Orientation input) => input == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
    }
}