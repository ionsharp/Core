using Imagin.Colour.Primitives;

namespace Imagin.Colour.Adaptation
{
    /// <summary>
    /// A linear transformation of a source color <see langword="XS, YS, ZS"/> into a destination color <see langword="XD, YD, ZD"/> by a linear transformation <see langword="[M]"/>, which is dependent on the source reference white <see langword="XWS, YWS, ZWS"/> and the destination reference white <see langword="XWD, YWD, ZWD"/>.
    /// </summary>
    public interface IChromaticAdaptation
    {
        /// <remarks>Doesn't crop the resulting color space coordinates (e. g. allows negative values for XYZ coordinates).</remarks>
        XYZ Transform(XYZ sourceColor, XYZ sourceIlluminant, XYZ targetIlluminant);
    }
}
