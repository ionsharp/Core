using Imagin.Colour.Primitives;

namespace Imagin.Colour.Conversion
{
#pragma warning disable 1591
    public class CMYConverter : ColorConverterBase<CMY>
    {
        public CMY Convert(RGB input) => new CMY(1.0 - input.R, 1.0 - input.G, 1.0 - input.B);
    }
#pragma warning restore 1591
}
