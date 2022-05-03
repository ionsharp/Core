namespace Imagin.Common.Colors
{
    /// <summary>
    /// Functions used for conversion to <see cref="XYZ"/> and back.
    /// </summary>
    /// <remarks>https://github.com/tompazourek/Colourful</remarks>
    public interface ICompanding
    {
        /// <summary>
        /// Inverse companding. The input companded channel is made linear with respect to the energy.
        /// </summary>
        double ConvertToLinear(double nonLinearChannel);

        /// <summary>
        /// Companding. The input uncompanded channel (linear) is made nonlinear (depends on the RGB color system).
        /// </summary>
        double ConvertToNonLinear(double linearChannel);
    }
}