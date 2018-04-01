using Imagin.Colour.Primitives;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class RGConverter : ColorConverterBase<RG>
    {
        public RG Convert(RGB input)
        {
            var rgb = input.Vector.Sum();

            var R = input.R / rgb;
            var G = input.G / rgb;
            var g = input.B / rgb;
            return new RG(R, G, g);
        }
    }
#pragma warning restore 1591
}
