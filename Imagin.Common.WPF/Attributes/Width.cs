using System;

namespace Imagin.Common
{
    public class WidthAttribute : Attribute
    {
        public readonly double MaximumWidth;

        public readonly double MinimumWidth;

        public readonly double Width;

        public WidthAttribute(double width, double widthMinimum = double.NaN, double widthMaximum = double.NaN)
        {
            Width
                = width;
            MinimumWidth
                = widthMinimum;
            MaximumWidth
                = widthMaximum;
        }
    }
}