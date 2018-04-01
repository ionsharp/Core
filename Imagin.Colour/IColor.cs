using Imagin.Common;

namespace Imagin.Colour
{
    /// <summary>
    /// Specifies a color with channels.
    /// </summary>
    public interface IColor
    {
        /// <summary>
        /// Gets the value of all components.
        /// </summary>
        Vector Vector
        {
            get;
        }
    }
}
