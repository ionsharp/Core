using Imagin.Colour.Primitives;
using System;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class xyYConverter : ColorConverterBase<xyY>
    {
        public xyY Convert(XYZ input)
        {
            var x = input.X / (input.X + input.Y + input.Z);
            var y = input.Y / (input.X + input.Y + input.Z);

            if (double.IsNaN(x) || double.IsNaN(y))
                return new xyY(0, 0, input.Y);

            var Y = input.Y;
            return new xyY(x, y, Y);
        }
    }
#pragma warning restore 1591
}