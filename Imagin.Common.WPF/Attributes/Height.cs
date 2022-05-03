using System;

namespace Imagin.Common
{
    public class HeightAttribute : Attribute
    {
        public readonly double MaximumHeight;

        public readonly double MinimumHeight;

        public readonly double Height;

        public HeightAttribute(double height, double heightMinimum = double.NaN, double heightMaximum = double.NaN)
        {
            Height
                = height;
            MinimumHeight
                = heightMinimum;
            MaximumHeight
                = heightMaximum;
        }
    }
}