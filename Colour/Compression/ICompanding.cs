using Imagin.Colour.Primitives;

namespace Imagin.Colour.Compression
{
    /// <summary>
    /// Pair of companding functions for <see cref="WorkingSpace"/>; used for conversion to <see cref="XYZ"/> and back. See also, <seealso cref="WorkingSpace.Companding"/>.
    /// </summary>
    public interface ICompanding
    {
        /// <summary>
        /// Companded channel (nonlinear) is made linear with respect to energy.
        /// </summary>
        double InverseCompanding(double channel);

        /// <summary>
        /// Uncompanded channel (linear) is made nonlinear.
        /// </summary>
        double Companding(double channel);
    }
}
