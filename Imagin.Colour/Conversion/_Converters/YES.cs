using Imagin.Colour.Primitives;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class YESConverter : ColorConverterBase<YES>
    {
        public YES Convert(RGB input)
        {
            return new YES
            (
                input.R * .253 + input.G *  .684 + input.B *  .063,
                input.R * .500 + input.G * -.500 + input.B *  .000,
                input.R * .250 + input.G *  .250 + input.B * -.500
            );
        }
    }
#pragma warning restore 1591
}